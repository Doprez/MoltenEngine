﻿using Silk.NET.Direct3D11;

namespace Molten.Graphics
{
    /// <summary>Stores a blend state for use with a <see cref="CommandQueueDX11"/>.</summary>
    public unsafe class BlendStateDX11 : GraphicsObject<ID3D11BlendState1>
    {
        internal StructKey<BlendDesc1> Desc { get; }

        ID3D11BlendState1* _native;
        Color4 _blendFactor;
        uint _sampleMask;

        public BlendStateDX11(GraphicsDevice device, StructKey<BlendDesc1> desc, Color4 blendFactor, uint blendMask) : 
            base(device, GraphicsBindTypeFlags.Input)
        {
            Desc = new StructKey<BlendDesc1>(desc);
            _blendFactor = blendFactor;
            _sampleMask = blendMask;
        }

        protected override void OnApply(GraphicsCommandQueue cmd)
        {
            if (_native == null)
            {
                (cmd as CommandQueueDX11).DXDevice.Ptr->CreateBlendState1(Desc, ref _native);
                Version++;
            }
        }

        public override void GraphicsRelease()
        {
            SilkUtil.ReleasePtr(ref _native);
            Desc.Dispose();
        }

        public static implicit operator ID3D11BlendState1*(BlendStateDX11 state)
        {
            return state._native;
        }

        public static implicit operator ID3D11BlendState*(BlendStateDX11 state)
        {
            return (ID3D11BlendState*)state._native;
        }

        public override unsafe ID3D11BlendState1* NativePtr => _native;

        internal Color4 BlendFactor
        {
            get => _blendFactor;
            set
            {
                if(_blendFactor != value)
                {
                    _blendFactor = value;
                    Version++;
                }
            }
        }

        internal uint BlendSampleMask
        {
            get => _sampleMask;
            set
            {
                if(_sampleMask != value)
                {
                    _sampleMask = value;
                    Version++;
                }
            }
        }
    }
}
