﻿using Silk.NET.Core.Native;
using Silk.NET.Direct3D12;
using Silk.NET.DXGI;

namespace Molten.Graphics.DX12;

public abstract class TextureDX12 : GpuTexture, ITexture
{
    ResourceHandleDX12 _handle;
    ResourceBarrier _barrier;
    ResourceStates _barrierState;
    ResourceDesc1 _desc;
    ProtectedSessionDX12 _protectedSession;

    protected TextureDX12(DeviceDX12 device,
        ResourceDimension resourceDimension,
        TextureDimensions dimensions,
        GpuResourceFormat format, GpuResourceFlags flags, string name,
        ProtectedSessionDX12 protectedSession = null) : 
        base(device, ref dimensions, format, flags, name)
    {
        Device = device;
        _protectedSession = protectedSession;

        if (IsBlockCompressed)
            throw new NotSupportedException("1D textures do not supports block-compressed formats.");

        if (dimensions.ArraySize > ushort.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(dimensions.ArraySize), "Array size cannot exceed 65535.");

        if (dimensions.MipMapCount > ushort.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(dimensions.MipMapCount), "Mip-map level count cannot exceed 65535.");

        if (resourceDimension == ResourceDimension.Buffer)
            throw new InvalidOperationException("Textures cannot use a buffer resource dimension.");

        Desc = new ResourceDesc1()
        {
            Width = dimensions.Width,
            Height = dimensions.Height,
            DepthOrArraySize = (ushort)Math.Max(1, dimensions.ArraySize),
            MipLevels = (ushort)dimensions.MipMapCount,
            Format = format.ToApi(),
            Dimension = resourceDimension,
            Flags = Flags.ToResourceFlags(),
            Alignment = 0,
            Layout = TextureLayout.LayoutUnknown,
            SampleDesc = new SampleDesc()
            {
                Count = (uint)dimensions.MultiSampleLevel,
                Quality = (uint)dimensions.SampleQuality,
            },
            SamplerFeedbackMipRegion = new MipRegion() // Sampler feedback info: https://microsoft.github.io/DirectX-Specs/d3d/SamplerFeedback.html
        };


        if (this is IRenderSurface)
            Desc.Flags |= ResourceFlags.AllowRenderTarget;

        if (this is IDepthStencilSurface)
            Desc.Flags |= ResourceFlags.AllowDepthStencil;
    }

    protected override void OnApply(GpuCommandList cmd)
    {
        if (_handle == null)
            OnCreateResource();
    }

    protected unsafe void OnCreateResource()
    {
        ID3D12Resource1* ptr = OnCreateTexture();
        _handle = OnCreateHandle(ptr);
        ShaderResourceViewDesc srvDesc = new ShaderResourceViewDesc
        {
            Format = DxgiFormat,
            Shader4ComponentMapping = ResourceInterop.EncodeShader4ComponentMapping(ShaderComponentMapping.FromMemoryComponent0,
                    ShaderComponentMapping.FromMemoryComponent1,
                    ShaderComponentMapping.FromMemoryComponent2,
                    ShaderComponentMapping.FromMemoryComponent3),
        };

        if (!Flags.Has(GpuResourceFlags.DenyShaderAccess))
        {
            SetSRVDescription(ref srvDesc);
            _handle.SRV.Initialize(ref srvDesc);
        }

        if (Flags.Has(GpuResourceFlags.UnorderedAccess))
        {
            UnorderedAccessViewDesc uavDesc = default;
            SetUAVDescription(ref srvDesc, ref uavDesc);
            _handle.UAV.Initialize(ref uavDesc);
        }
    }

    protected virtual ClearValue GetClearValue() => default;

    protected override void OnGpuRelease()
    {
        _handle?.Dispose();
    }

    protected unsafe virtual ID3D12Resource1* OnCreateTexture()
    {
        HeapFlags heapFlags = HeapFlags.None;
        ResourceFlags flags = Flags.ToResourceFlags();
        HeapType heapType = HeapType.Default; // Flags.ToHeapType(); // TODO Properly set heap properties based on access flags and UMA support.
        ResourceStates stateFlags = Flags.ToResourceState();

        HeapProperties heapProp = new HeapProperties()
        {
            Type = heapType,
            CPUPageProperty = CpuPageProperty.Unknown,
            CreationNodeMask = 1,
            MemoryPoolPreference = MemoryPool.Unknown,
            VisibleNodeMask = 1,
        };

        // TODO Adjust for GPU memory architecture based on UMA support.
        // See for differences: https://microsoft.github.io/DirectX-Specs/d3d/D3D12GPUUploadHeaps.html
        if (heapType == HeapType.Custom)
        {
            // TODO Set CPUPageProperty and MemoryPoolPreference based on UMA support.
        }

        if (Flags.Has(GpuResourceFlags.DenyShaderAccess))
            flags |= ResourceFlags.DenyShaderResource;

        if (Flags.Has(GpuResourceFlags.UnorderedAccess))
            flags |= ResourceFlags.AllowUnorderedAccess;

        ClearValue clearValue = GetClearValue();
        ClearValue* ptrClearValue = clearValue.Format != Format.FormatUnknown ? &clearValue : null;
        Guid guid = ID3D12Resource1.Guid;
        void* ptr = null;

        fixed (ResourceDesc1* ptrDesc = &_desc)
        {
            HResult hr = Device.Handle->CreateCommittedResource2(&heapProp, heapFlags, ptrDesc, stateFlags, ptrClearValue, _protectedSession, &guid, &ptr);
            if (!Device.Log.CheckResult(hr, () => $"Failed to create {_desc.Dimension} resource"))
                return null;
        }

        return (ID3D12Resource1*)ptr;
    }

    protected unsafe virtual ResourceHandleDX12 OnCreateHandle(ID3D12Resource1* ptr)
    {
        if (_handle == null)
            return new ResourceHandleDX12(this, ptr);
        else
            _handle[0] = ptr;

        return _handle;
    }

    protected abstract void SetUAVDescription(ref ShaderResourceViewDesc srvDesc, ref UnorderedAccessViewDesc desc);

    protected abstract void SetSRVDescription(ref ShaderResourceViewDesc desc);

    //protected override void OnResizeTextureImmediate(GpuCommandList cmd, ref readonly TextureDimensions dimensions, GpuResourceFormat format)
    //{
    //    _desc.Width = dimensions.Width;
    //    _desc.MipLevels = (ushort)dimensions.MipMapCount;
    //    _desc.Format = format.ToApi();

    //    if (Desc.Dimension != ResourceDimension.Texture1D)
    //        _desc.Height = dimensions.Height;
    //    else
    //        _desc.Height = 1;

    //    if (Desc.Dimension == ResourceDimension.Texture3D)
    //        _desc.DepthOrArraySize = (ushort)dimensions.Depth;
    //    else
    //        _desc.DepthOrArraySize = (ushort)Math.Max(1, dimensions.ArraySize);

    //    Dimensions = dimensions;

    //    OnCreateResource();
    //}

    /// <summary>Gets the DXGI format of the texture.</summary>
    public Format DxgiFormat => ResourceFormat.ToApi();

    public new DeviceDX12 Device { get; }

    public override ResourceHandleDX12 Handle => _handle;

    /// <summary>
    /// Gets the internal resource barrier state of the current <see cref="BufferDX12"/>.
    /// </summary>
    internal ResourceStates BarrierState { get; set; }

    internal ref ResourceDesc1 Desc => ref _desc;
}
