﻿namespace Molten.Graphics
{
    internal unsafe class InputLayoutBinder : ContextSlotBinder<VertexInputLayout>
    {
        internal override void Bind(ContextSlot<VertexInputLayout> slot, VertexInputLayout value)
        {
            slot.Context.Native->IASetInputLayout(slot.BoundValue);
        }

        internal override void Unbind(ContextSlot<VertexInputLayout> slot, VertexInputLayout value)
        {
            slot.Context.Native->IASetInputLayout(null);
        }
    }
}