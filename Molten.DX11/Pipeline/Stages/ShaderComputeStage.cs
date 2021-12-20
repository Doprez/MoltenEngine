﻿using Silk.NET.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Molten.Graphics
{
    internal unsafe class ShaderComputeStage : PipeShaderStage<ID3D11ComputeShader>
    {
        public ShaderComputeStage(PipeDX11 pipe) :
            base(pipe, ShaderType.DomainShader)
        {
            uint uavSlots = pipe.Device.Features.MaxUnorderedAccessViews;
            UAResources = DefineSlotGroup<PipeBindableResource>(uavSlots, PipeBindTypeFlags.Input, "UAV");
            Task = DefineSlot<ComputeTask>(0, PipeBindTypeFlags.Input, "Compute Task");
        }

        protected override unsafe void OnBindConstants(PipeSlotGroup<ShaderConstantBuffer> grp,
            ID3D11Buffer** buffers, uint* firstConstants, uint* numConstants)
        {
            Pipe.Context->CSSetConstantBuffers1(grp.FirstChanged, grp.NumSlotsChanged, buffers, firstConstants, numConstants);
        }

        protected override unsafe void OnBindResources(PipeSlotGroup<PipeBindableResource> grp,
            ID3D11ShaderResourceView** srvs)
        {
            Pipe.Context->CSSetShaderResources(grp.FirstChanged, grp.NumSlotsChanged, srvs);

            // Set unordered access resources
            if (UAResources.BindAll())
            {
                int numChanged = (int)Resources.NumSlotsChanged;
                ID3D11UnorderedAccessView** uavs = stackalloc ID3D11UnorderedAccessView*[numChanged];
                uint* pUAVInitialCounts = stackalloc uint[numChanged];

                uint sid = Resources.FirstChanged;
                for (int i = 0; i < numChanged; i++)
                {
                    srvs[i] = Resources[sid].BoundValue ?? null;
                    pUAVInitialCounts[i] = 0; // TODO set initial counts. Research this more.
                }

                Pipe.Context->CSSetUnorderedAccessViews(UAResources.FirstChanged, UAResources.NumSlotsChanged, uavs, pUAVInitialCounts);
            }
        }

        protected override unsafe void OnBindSamplers(PipeSlotGroup<ShaderSampler> grp, ID3D11SamplerState** resources)
        {
            Pipe.Context->CSSetSamplers(grp.FirstChanged, grp.NumSlotsChanged, resources);
        }

        protected override unsafe void OnBindShader(PipeSlot<ShaderComposition<ID3D11ComputeShader>> slot)
        {
            if (slot.BoundValue != null)
                Pipe.Context->CSSetShader(slot.BoundValue.RawShader, null, 0);
            else
                Pipe.Context->CSSetShader(null, null, 0);
        }

        internal new void Bind()
        {
            throw new NotSupportedException("ShaderComputeStage does not support Bind(). Call Dispatch instead");
        }
        internal void Dispatch(uint groupsX, uint groupsY, uint groupsZ)
        {
            Task.Bind();
            Shader.Value = Task.BoundValue.Composition;

            // Call base.Bind() to bind all resources, samplers and buffers on device compute stage.
            base.Bind();

            if (Shader.BoundValue == null)
            {
                return;
            }
            else
            {
                // Ensure dispatch is within supported range.
                int maxZ = Device.Features.Compute.MaxDispatchZDimension;
                int maxXY = Device.Features.Compute.MaxDispatchXYDimension;

                if (groupsZ > maxZ)
                {
#if DEBUG
                    Pipe.Log.Write("Unable to dispatch compute shader. Z dimension (" + groupsZ + ") is greater than supported (" + maxZ + ").");
#endif
                    return;
                }
                else if (groupsX > maxXY)
                {
#if DEBUG
                    Pipe.Log.Write("Unable to dispatch compute shader. X dimension (" + groupsX + ") is greater than supported (" + maxXY + ").");
#endif
                    return;
                }
                else if (groupsY > maxXY)
                {
#if DEBUG
                    Pipe.Log.Write("Unable to dispatch compute shader. Y dimension (" + groupsY + ") is greater than supported (" + maxXY + ").");
#endif
                    return;
                }

                // TODO have this processed during the presentation call of each graphics pipe.

                Pipe.Context->Dispatch(groupsX, groupsY, groupsZ);
            }
        }

        public PipeSlotGroup<PipeBindableResource> UAResources { get; }

        public PipeSlot<ComputeTask> Task { get; }
    }
}