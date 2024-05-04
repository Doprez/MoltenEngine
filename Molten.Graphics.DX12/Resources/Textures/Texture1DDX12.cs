﻿using Silk.NET.Direct3D12;

namespace Molten.Graphics.DX12;

public class Texture1DDX12 : TextureDX12, ITexture1D
{
    public Texture1DDX12(
        DeviceDX12 device,
        uint width,
        GpuResourceFlags flags,
        GpuResourceFormat format = GpuResourceFormat.R8G8B8A8_UNorm,
        uint mipCount = 1,
        uint arraySize = 1,
        string name = null,
        ProtectedSessionDX12 protectedSession = null)
        : base(device, ResourceDimension.Texture1D, new TextureDimensions(width, 1, 1, mipCount, arraySize),
            format, flags, name, protectedSession)
    { }

    public void Resize(GpuPriority priority, GpuCommandList cmd,
        uint newWidth,
        uint newMipMapCount = 0,
        uint newArraySize = 0,
        GpuResourceFormat newFormat = GpuResourceFormat.Unknown,
        GpuTaskCallback completeCallback = null)
    {
        Resize(priority, cmd, newWidth, Dimensions.Height, newMipMapCount, newArraySize, newFormat, completeCallback);
    }

    protected override void SetSRVDescription(ref ShaderResourceViewDesc desc)
    {
        desc.ViewDimension = SrvDimension.Texture1Darray;
        desc.Format = DxgiFormat;
        desc.Texture1DArray = new Tex1DArraySrv()
        {
            MipLevels = Desc.MipLevels,
            ArraySize = Desc.DepthOrArraySize,
            MostDetailedMip = 0,
            FirstArraySlice = 0,
        };
    }

    protected override void SetUAVDescription(ref ShaderResourceViewDesc srvDesc, ref UnorderedAccessViewDesc desc)
    {
        desc.Format = srvDesc.Format;
        desc.ViewDimension = UavDimension.Texture1Darray;
        desc.Texture2DArray = new Tex2DArrayUav()
        {
            ArraySize = Desc.DepthOrArraySize,
            FirstArraySlice = srvDesc.Texture2DArray.FirstArraySlice,
            MipSlice = 0,
            PlaneSlice = 0
        };
    }
}
