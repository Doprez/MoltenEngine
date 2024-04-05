﻿namespace Molten.Graphics.Vulkan;

public class RenderSurface2DVK : Texture2DVK, IRenderSurface2D, IRenderSurfaceVK
{
    /// <summary>
    /// Creates a new instance of <see cref="RenderSurface2DVK"/>.
    /// </summary>
    /// <param name="device">The parent <see cref="GpuDevice"/>.</param>
    /// <param name="dimensions">The image dimensions.</param>
    /// <param name="aaLevel">The anti-aliasing/multi-sample level.</param>
    /// <param name="sampleQuality">The sample quality.</param>
    /// <param name="format">The graphics format.</param>
    /// <param name="flags">Resource flags.</param>
    /// <param name="allowMipMapGen">If true, the generation of mip-maps will be allowed on the current <see cref="RenderSurface2DVK"/> instance.</param>
    /// <param name="name"></param>
    public RenderSurface2DVK(DeviceVK device, uint width, uint height, uint mipCount, uint arraySize, 
        AntiAliasLevel aaLevel, MSAAQuality sampleQuality, 
        GpuResourceFormat format, GpuResourceFlags flags, string name) : 
        base(device, 
            width, height, mipCount, arraySize,
            aaLevel, 
            sampleQuality, 
            format, 
            flags, 
            name)
    {
        Viewport = new ViewportF(0, 0, Width, Height);
    }

    /// <inheritdoc/>
    public void Clear(GpuPriority priority, GpuCommandList cmd, Color color)
    {
        SurfaceClearTaskVK task = Device.Tasks.Get<SurfaceClearTaskVK>();
        task.Color = color;
        Device.Tasks.Push<TextureVK, SurfaceClearTaskVK>(priority, this, task);
    }

    /// <inheritdoc/>
    public ViewportF Viewport { get; protected set; }

    /// <inheritdoc/>
    public Color? ClearColor { get; set; }
}
