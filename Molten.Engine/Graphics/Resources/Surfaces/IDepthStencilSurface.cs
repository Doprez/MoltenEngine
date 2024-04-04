﻿namespace Molten.Graphics;

/// <summary>
/// Represents the implementation of a depth-stencil reneder surface.
/// </summary>
public interface IDepthStencilSurface : ITexture2D
{
    /// <summary>Clears the provided <see cref="IDepthStencilSurface"/> to the specified depth and stencil values.</summary>
    /// <param name="priority">The priority of the clear command.</param>
    /// <param name="cmd">The command list that will perform the clear operation.</param>
    /// <param name="flags">The depth-stencil clearing flags.</param>
    /// <param name="depthValue">The value to clear the depth to. Only applies if <see cref="DepthClearFlags.Depth"/> flag was provided.</param>
    /// <param name="stencilValue">The value to clear the stencil to. Only applies if <see cref="DepthClearFlags.Stencil"/> flag was provided.</param>
    void Clear(GpuPriority priority, GpuCommandList cmd, DepthClearFlags flags, float depthValue = 1.0f, byte stencilValue = 0);

    /// <summary>Gets the depth-specific format of the surface.</summary>
    DepthFormat DepthFormat { get; }

    /// <summary>Gets the viewport that defines the renderable area of the render target.</summary>
    ViewportF Viewport { get; }
}
