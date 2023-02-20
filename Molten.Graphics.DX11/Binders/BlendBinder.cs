﻿namespace Molten.Graphics
{
    internal unsafe class BlendBinder : GraphicsSlotBinder<BlendStateDX11>
    {
        public override void Bind(GraphicsSlot<BlendStateDX11> slot, BlendStateDX11 value)
        {
            CommandQueueDX11 cmd = slot.Cmd as CommandQueueDX11;

            value = value ?? cmd.DXDevice.BlendBank.GetPreset(BlendPreset.Default) as BlendStateDX11;
            Color4 tmp = value.BlendFactor;
            cmd.Native->OMSetBlendState(value as BlendStateDX11, (float*)&tmp, value.BlendSampleMask);
        }

        public override void Unbind(GraphicsSlot<BlendStateDX11> slot, BlendStateDX11 value)
        {
            (slot.Cmd as CommandQueueDX11).Native->OMSetBlendState(null, null, 0);
        }
    }
}
