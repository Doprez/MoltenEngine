﻿using Silk.NET.Core.Native;
using Silk.NET.Direct3D12;
using Silk.NET.DXGI;

namespace Molten.Graphics.DX12;

public abstract class TextureDX12 : GraphicsTexture, ITexture
{
    ResourceHandleDX12 _handle;
    ResourceBarrier _barrier;
    ResourceStates _barrierState;
    ResourceDesc1 _desc;
    ProtectedSessionDX12 _protectedSession;

    protected TextureDX12(DeviceDX12 device,
        ResourceDimension resourceDimension,
        TextureDimensions dimensions,
        AntiAliasLevel aaLevel, 
        MSAAQuality sampleQuality, 
        GraphicsFormat format, GraphicsResourceFlags flags, string name,
        ProtectedSessionDX12 protectedSession = null) : 
        base(device, dimensions, aaLevel, sampleQuality, format, flags, name)
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
                Count = (uint)aaLevel,
                Quality = (uint)sampleQuality,
            },
            SamplerFeedbackMipRegion = new MipRegion() // Sampler feedback info: https://microsoft.github.io/DirectX-Specs/d3d/SamplerFeedback.html
        };
    }

    protected ResourceFlags GetResourceFlags()
    {
        ResourceFlags result = Flags.ToResourceFlags();

        /*if (this is RenderSurface2DDX12)
            result |= ResourceFlags.AllowRenderTarget;

        if (this is DepthSurfaceDX12)
            result |= ResourceFlags.AllowDepthStencil;*/

        return result;
    }

    protected unsafe override void OnCreateResource()
    {
        _handle?.Dispose();

        HeapFlags heapFlags = HeapFlags.None;
        ResourceFlags flags = Flags.ToResourceFlags();
        HeapType heapType = Flags.ToHeapType();
        ResourceStates stateFlags = Flags.ToResourceState();

        HeapProperties heapProp = new HeapProperties()
        {
            Type = heapType,
            CPUPageProperty = CpuPageProperty.Unknown,
            CreationNodeMask = 1,
            MemoryPoolPreference = MemoryPool.Unknown,
            VisibleNodeMask = 1,
        };

        if (Flags.Has(GraphicsResourceFlags.DenyShaderAccess))
            flags |= ResourceFlags.DenyShaderResource;

        if (Flags.Has(GraphicsResourceFlags.UnorderedAccess))
            flags |= ResourceFlags.AllowUnorderedAccess;

        ClearValue clearValue = GetClearValue();
        ClearValue* ptrClearValue = clearValue.Format != Format.FormatUnknown ? &clearValue : null;
        Guid guid = ID3D12Resource1.Guid;
        void* ptr = null;

        fixed (ResourceDesc1* ptrDesc = &_desc)
        {
            HResult hr = Device.Ptr->CreateCommittedResource2(&heapProp, heapFlags, ptrDesc, stateFlags, ptrClearValue, _protectedSession, &guid, &ptr);
            if (!Device.Log.CheckResult(hr, () => $"Failed to create {_desc.Dimension} resource"))
                return;
        }

        _handle = new ResourceHandleDX12(this, (ID3D12Resource1*)ptr);

        if (!Flags.Has(GraphicsResourceFlags.DenyShaderAccess))
            SetSRVDescription(ref _handle.SRV.Desc);

        if (Flags.Has(GraphicsResourceFlags.UnorderedAccess))
            SetUAVDescription(ref _handle.SRV.Desc, ref _handle.UAV.Desc);

        //SetDebugName(_handle.NativePtr, $"{Name}");
    }

    protected virtual ClearValue GetClearValue() => default;

    protected override void OnGraphicsRelease()
    {
        _handle?.Dispose();
    }

    protected unsafe virtual ResourceHandleDX12 OnCreateHandle(ID3D12Resource1* ptr)
    {
        return new ResourceHandleDX12(this, ptr);
    }

    protected abstract void SetUAVDescription(ref ShaderResourceViewDesc srvDesc, ref UnorderedAccessViewDesc desc);

    protected abstract void SetSRVDescription(ref ShaderResourceViewDesc desc);

    protected override void OnResizeTexture(ref readonly TextureDimensions dimensions, GraphicsFormat format)
    {
        _desc.Width = dimensions.Width;
        _desc.MipLevels = (ushort)dimensions.MipMapCount;
        _desc.Format = format.ToApi();

        if (Desc.Dimension != ResourceDimension.Texture1D)
            _desc.Height = dimensions.Height;
        else
            _desc.Height = 1;

        if (Desc.Dimension == ResourceDimension.Texture3D)
            _desc.DepthOrArraySize = (ushort)dimensions.Depth;
        else
            _desc.DepthOrArraySize = (ushort)Math.Max(1, dimensions.ArraySize);

        Dimensions = dimensions;

        OnCreateResource();
    }

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