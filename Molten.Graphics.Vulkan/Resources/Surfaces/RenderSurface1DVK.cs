﻿namespace Molten.Graphics.Vulkan;

public class RenderSurface1DVK : Texture1DVK, IRenderSurface1D, IRenderSurfaceVK
{
    /// <summary>
    /// Creates a new instance of <see cref="RenderSurface2DVK"/>.
    /// </summary>
    /// <param name="device">The parent <see cref="GpuDevice"/>.</param>
    /// <param name="arraySize">The number of array slices (textures) within the texture array.</param>
    /// <param name="mipCount">The number of mip-map levels.</param>
    /// <param name="width">The width of the 1D texture.</param>
    /// <param name="format">The graphics format.</param>
    /// <param name="flags">Resource flags.</param>
    /// <param name="allowMipMapGen">If true, the generation of mip-maps will be allowed on the current <see cref="RenderSurface2DVK"/> instance.</param>
    /// <param name="name"></param>
    public RenderSurface1DVK(DeviceVK device, uint width, uint mipCount, uint arraySize, 
        GpuResourceFormat format, GpuResourceFlags flags, string name) : 
        base(device, width, mipCount, arraySize, format, flags, name)
    {
        Viewport = new ViewportF(0, 0, Width, 1);
    }

    /// <inheritdoc/>
    public void Clear(GpuPriority priority, GpuCommandList cmd, Color color)
    {
        SurfaceClearTaskVK task = new();
        task.Surface = this;
        task.Color = color;
        Device.Tasks.Push(priority, ref task, cmd);
    }

    /// <inheritdoc/>
    public ViewportF Viewport { get; protected set; }
    
    /// <inheritdoc/>
    public Color? ClearColor { get; set; }
}
