﻿using Silk.NET.Direct3D12;
using System.Runtime.InteropServices;

namespace Molten.Graphics.DX12;
internal class RootSigPopulator1_1 : RootSignaturePopulatorDX12
{
    internal override unsafe void Populate(ref VersionedRootSignatureDesc versionedDesc, 
        ref readonly GraphicsPipelineStateDesc psoDesc, 
        ShaderPassDX12 pass)
    {
        ref RootSignatureDesc1 desc = ref versionedDesc.Desc11;
        PopulateStaticSamplers(ref desc.PStaticSamplers, ref desc.NumStaticSamplers, pass);

        desc.NumParameters = (uint)pass.Parent.Resources.Length;
        desc.PParameters = EngineUtil.AllocArray<RootParameter1>(desc.NumParameters);
        desc.Flags = GetFlags(in psoDesc, pass);

        List<DescriptorRange1> ranges = new();
        PopulateRanges(DescriptorRangeType.Srv, ranges, pass.Parent.Resources);
        PopulateRanges(DescriptorRangeType.Uav, ranges, pass.Parent.UAVs);
        PopulateRanges(DescriptorRangeType.Cbv, ranges, pass.Parent.ConstBuffers);

        // TODO Add support for heap-based samplers.
        // TODO Add support for static CBV (which require their own root parameter with the data_static flag set.

        desc.NumParameters = 1;
        desc.PParameters = EngineUtil.AllocArray<RootParameter1>(desc.NumParameters);
        ref RootParameter1 param = ref desc.PParameters[0];

        param.ParameterType = RootParameterType.TypeDescriptorTable;
        param.DescriptorTable.NumDescriptorRanges = (uint)ranges.Count;
        param.DescriptorTable.PDescriptorRanges = EngineUtil.AllocArray<DescriptorRange1>((uint)ranges.Count);
        param.ShaderVisibility = ShaderVisibility.All; // TODO If a parameter is only used on 1 stage, set this to that stage.

        Span<DescriptorRange1> rangeSpan = CollectionsMarshal.AsSpan(ranges);
        Span<DescriptorRange1> tableRanges = new(param.DescriptorTable.PDescriptorRanges, ranges.Count);
        rangeSpan.CopyTo(tableRanges);
    }

    internal override unsafe void Free(ref VersionedRootSignatureDesc versionedDesc)
    {
        ref RootSignatureDesc1 desc = ref versionedDesc.Desc11;

        for (int i = 0; i < desc.NumParameters; i++)
            EngineUtil.Free(ref desc.PParameters[i].DescriptorTable.PDescriptorRanges);

        EngineUtil.Free(ref desc.PParameters);
        EngineUtil.Free(ref desc.PStaticSamplers);
    }

    private void PopulateRanges(DescriptorRangeType type, List<DescriptorRange1> ranges, Array variables)
    {
        uint last = 0;
        uint i = 0;
        DescriptorRange1 r = new();

        for (; i < variables.Length; i++)
        {
            if (variables.GetValue(i) == null)
                continue;

            // Create a new range if there was a gap.
            uint prev = i - 1; // What the previous should be.
            if (last == i || last == prev)
            {
                // Finalize previous range
                if (last != i)
                    r.NumDescriptors = i - r.BaseShaderRegister;

                // Start new range.
                r = new DescriptorRange1();
                r.BaseShaderRegister = i;
                r.RangeType = type;
                r.RegisterSpace = 0; // TODO Populate - Requires reflection enhancements to support new HLSL register syntax: register(t0, space1);
                r.OffsetInDescriptorsFromTableStart = 0;
                r.Flags = DescriptorRangeFlags.None;
                ranges.Add(r);
            }

            last = i;
        }

        // Finalize last range to the list
        r.NumDescriptors = i - r.BaseShaderRegister;
    }
}
