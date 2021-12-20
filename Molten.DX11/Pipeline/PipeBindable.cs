﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Molten.Graphics
{
    /// <summary>
    /// Represents a DX11 bindable pipeline object.
    /// </summary>
    public abstract class PipeBindable : PipeObject
    {
        internal PipeBindable(DeviceDX11 device) : base(device)
        {
            BoundTo = new HashSet<PipeSlot>();
        }

        /// <summary>Invoked when the current <see cref="PipeBindable"/> is to be bound to a <see cref="PipeSlot"/>.</summary>
        /// <param name="slot">The <see cref="PipeSlot"/> to bind to.</param>
        /// <returns>True if the binding succeeded.</returns>
        internal bool BindTo(PipeSlot slot)
        {
            // TODO validate binding. Allow bindable to do it's own validation too.
            // If validation fails, return false here.
            // E.g. is the object bound to both input and outout in an invalid way?

            BoundTo.Add(slot);
            Refresh(slot, slot.Stage.Pipe);
            return true;
        }

        internal void UnbindFrom(PipeSlot slot)
        {
            BoundTo.Remove(slot);
        }

        /// <summary>
        /// Invoked right before the current <see cref="PipeBindable"/> is due to be bound to a <see cref="PipeDX11"/>.
        /// </summary>
        /// <param name="slot">The <see cref="PipeSlot"/> which contains the current <see cref="PipeBindable"/>.</param>
        /// <param name="pipe">The <see cref="PipeDX11"/> that the current <see cref="PipeBindable"/> is to be bound to.</param>
        protected internal abstract void Refresh(PipeSlot slot, PipeDX11 pipe);

        /// <summary>
        /// Gets the instance-specific version of the current <see cref="PipeBindable"/>. Any change which will require a device
        /// update should increase this value. E.g. Resizing a texture, recompiling a shader/material, etc.
        /// </summary>
        internal protected uint Version { get; protected set; }

        /// <summary>
        /// Gets a list of slots that the current <see cref="PipeBindable"/> is bound to.
        /// </summary>
        internal HashSet<PipeSlot> BoundTo { get; }
    }

    public unsafe abstract class PipeBindable<T> : PipeBindable
        where T : unmanaged
    {
        internal PipeBindable(DeviceDX11 device) : base(device)
        {

        }

        /// <summary>
        /// Gets the native pointer of the current <see cref="PipeBindable{T}"/>, as a <typeparamref name="T"/> pointer.
        /// </summary>
        internal abstract T* NativePtr { get; }

        /// <summary>
        /// Gets the native pointer of the current <see cref="PipeBindable{T}"/>,as a <see cref="void"/> pointer.
        /// </summary>
        internal void* RawNative => NativePtr;

        public static implicit operator T*(PipeBindable<T> bindable)
        {
            return bindable.NativePtr;
        }
    }
}