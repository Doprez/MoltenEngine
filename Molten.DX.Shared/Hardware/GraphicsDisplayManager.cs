﻿using SharpDX.Direct3D;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Molten.Graphics
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="F">The DXGI Factory type</typeparam>
    /// <typeparam name="A">The DXGI Adapter type</typeparam>
    /// <typeparam name="D">The type of adapter description</typeparam>
    /// <seealso cref="Molten.Graphics.IDisplayManager" />
    public abstract class GraphicsDisplayManager<F, A, D, O> : IDisplayManager 
        where F : Factory, new()
        where A : Adapter
        where D : struct
        where O : Output
    {
        F _dxgiFactory;
        List<int> _usable;
        List<GraphicsAdapterDX<A, D, O>> _adapters;
        Logger _log;
        int _defaultID = -1;
        int _selectedID = -1;

        /// <summary>
        /// Initializes the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Initialize(Logger logger, GraphicsSettings settings)
        {
            _dxgiFactory = new F();
            _usable = new List<int>();
            _adapters = new List<GraphicsAdapterDX<A, D, O>>();
            _log = logger;

            // Detect adapters.
            A[] detected = GetDxgiAdapters(_dxgiFactory);
            for (int i = 0; i < detected.Length; i++)
            {
                D desc = GetAdapterDescription(detected[i]);
                GraphicsAdapterDX<A, D, O> adapter = InstanciateAdapter(detected[i], desc, i);
                _adapters.Add(adapter);

                if (adapter.OutputCount > 0)
                {
                    _usable.Add(i);

                    // Set default if needed
                    if (_defaultID == -1)
                    {
                        _defaultID = i;
                        _selectedID = i;
                    }
                }
            }

            // Output detection info into renderer log.
            _log.WriteLine($"Detected {_adapters.Count} adapters:");
            List<IDisplayOutput> displays = new List<IDisplayOutput>();
            for(int i = 0; i < _adapters.Count; i++)
            {
                _log.WriteLine($"   Adapter {i}: {_adapters[i].Name}{(_usable.Contains(i) ? " (usable)" : "")}");
                _adapters[i].GetAttachedOutputs(displays);
                for(int d = 0; d < displays.Count; d++)
                    _log.WriteLine($"       Display {d}: {displays[d].Name}");
                displays.Clear();
            }

            // Validate the provided adapter ID from settings.
            _selectedID = settings.GraphicsAdapterID;
            if(_selectedID < 0 || _selectedID >= _adapters.Count)
            {
                _selectedID = _defaultID;
                settings.GraphicsAdapterID.Value = _selectedID;
                settings.DisplayOutputIds.Values.Clear();
            }

            // Validate display count.
            displays.Clear();
            IDisplayAdapter preferredAdapter = _adapters[_selectedID];
            preferredAdapter.GetAttachedOutputs(displays);
            if (settings.DisplayOutputIds.Values.Count == 0 || settings.DisplayOutputIds.Values.Count > displays.Count)
            {
                settings.DisplayOutputIds.Values.Clear();
                settings.DisplayOutputIds.Values.Add(0);
            }

            settings.Apply();

            // Add all preferred displays to active list
            foreach(int id in settings.DisplayOutputIds.Values)
                preferredAdapter.AddActiveOutput(preferredAdapter.GetOutput(id));

            // Log preferred adapter stats
            _log.WriteLine($"Chosen {preferredAdapter.Name.Replace("\0", "")}");
            _log.WriteLine($"    Dedicated VRAM: {preferredAdapter.DedicatedVideoMemory:N2} MB");
            _log.WriteLine($"    System RAM dedicated to video: {preferredAdapter.DedicatedSystemMemory:N2} MB");
            _log.WriteLine($"    Shared system RAM: {preferredAdapter.SharedSystemMemory:N2} MB");
        }

        protected abstract A[] GetDxgiAdapters(F factory);

        protected abstract GraphicsAdapterDX<A, D, O> InstanciateAdapter(A adapter, D desc, int id);

        protected abstract D GetAdapterDescription(A adapter);

        /// <summary>
        /// Gets the adapters.
        /// </summary>
        /// <param name="output">The output list.</param>
        public void GetAdapters(List<IDisplayAdapter> output)
        {
            output.AddRange(_adapters);
        }
        
        /// <summary>
        /// Adds all adapters with at least one output attached, to the provided output list.
        /// </summary>
        /// <param name="output">The list in which to add the results.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void GetAdaptersWithOutputs(List<IDisplayAdapter> output)
        {
            for (int i = 0; i < _usable.Count; i++)
                output.Add(_adapters[_usable[i]]);
        }

        /// <summary>
        /// Gets the adapter at the specified listing ID. This may change if the physical hardware is altered or swapped around.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns></returns>
        public IDisplayAdapter GetAdapter(int id)
        {
            if (id >= _adapters.Count)
                throw new IndexOutOfRangeException($"ID was {id} while there are only {_adapters.Count} adapters.");

            if (id < 0)
                throw new IndexOutOfRangeException("ID cannot be less than 0");

            return _adapters[id];
        }

        public void Dispose()
        {
            _dxgiFactory.Dispose();
        }

        /// <summary>
        /// Gets the number of display adapters attached to the system.
        /// </summary>
        public int AdapterCount => _adapters.Count;

        /// <summary>
        /// Gets the system's default display adapter.
        /// </summary>
        public IDisplayAdapter DefaultAdapter => _adapters[_defaultID];

        /// <summary>
        /// Gets or sets the adapter currently selected for use by the engine.
        /// </summary>
        public IDisplayAdapter SelectedAdapter
        {
            get => _adapters[_selectedID];
            set
            {
                if (value.Manager != this)
                    throw new AdapterException(value, "The adapter not owned by the current display manager.");

                _selectedID = value.ID;
            }
        }

        /// <summary>Gets the DXGI factory.</summary>
        public F DxgiFactory => _dxgiFactory;
    }
}
