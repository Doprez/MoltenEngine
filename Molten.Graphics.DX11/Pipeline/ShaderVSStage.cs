﻿using Silk.NET.Direct3D11;

namespace Molten.Graphics
{
    internal class ShaderVSStage : ContextShaderStage<ID3D11VertexShader>
    {
        public ShaderVSStage(CommandQueueDX11 queue) : base(queue, ShaderType.Vertex)
        {
        }

        internal override unsafe void SetConstantBuffers(uint startSlot, uint numBuffers, ID3D11Buffer** buffers)
        {
            Context.Native->VSSetConstantBuffers(startSlot, numBuffers, buffers);
        }

        internal override unsafe void SetResources(uint startSlot, uint numViews, ID3D11ShaderResourceView1** views)
        {
            Context.Native->VSSetShaderResources(startSlot, numViews, (ID3D11ShaderResourceView**)views);
        }

        internal override unsafe void SetSamplers(uint startSlot, uint numSamplers, ID3D11SamplerState** states)
        {
            Context.Native->VSSetSamplers(startSlot, numSamplers, states);
        }

        internal override unsafe void SetShader(void* shader, ID3D11ClassInstance** classInstances, uint numClassInstances)
        {
            Context.Native->VSSetShader((ID3D11VertexShader*)shader, classInstances, numClassInstances);
        }
    }
}
