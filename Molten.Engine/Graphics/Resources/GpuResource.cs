﻿namespace Molten.Graphics;

public abstract class GpuResource : GpuObject, IGpuResource
{
    protected GpuResource(GpuDevice device, GpuResourceFlags flags) :
        base(device)
    {
        Flags = flags;
        LastUsedFrameID = Device.Renderer.FrameID;
    }

    protected virtual void ValidateFlags()
    {
        // Only staging resources have CPU-write access.
        if (Flags.Has(GpuResourceFlags.UploadMemory))
        {
            if (!Flags.Has(GpuResourceFlags.DenyShaderAccess))
                throw new GpuResourceException(this, "Staging textures cannot allow shader access. Add GraphicsResourceFlags.NoShaderAccess flag.");
        }
    }

    /// <summary>
    /// Invoked when the current <see cref="GpuObject"/> should apply any changes before being bound to a GPU context.
    /// </summary>
    /// <param name="cmd">The <see cref="GpuCommandList"/> that the current <see cref="GpuObject"/> is to be bound to.</param>
    public void Apply(GpuCommandList cmd)
    {
        if (IsDisposed)
            return;

        LastUsedFrameID = Device.Renderer.FrameID;

        OnApply(cmd);
    }

    protected abstract void OnApply(GpuCommandList cmd);

    public void CopyTo(GpuPriority priority, GpuResource destination, GpuTask.EventHandler completeCallback = null)
    {
        if (Flags.IsGpuReadable())
            throw new ResourceCopyException(this, destination, "Source resource must have the GraphicsResourceFlags.GpuRead flag set.");

        if (!destination.Flags.IsGpuWritable())
            throw new ResourceCopyException(this, destination, "Destination resource must have the GraphicsResourceFlags.GpuWrite flag set.");

        // If copying between two images, do a format and bounds check
        if (this is GpuTexture srcTex)
        {
            if (destination is GpuTexture destTex)
            {
                if (ResourceFormat != destination.ResourceFormat)
                    throw new ResourceCopyException(this, destination, "The source and destination texture formats do not match.");

                // Validate dimensions.
                if (destTex.Width != srcTex.Width ||
                    destTex.Height != srcTex.Height ||
                    destTex.Depth != srcTex.Depth)
                    throw new ResourceCopyException(this, destination, "The source and destination textures must have the same dimensions.");
            }
            else
            {
                throw new NotImplementedException("Copying a texture to a non-texture is currently unsupported.");
            }
        }
        else if (this is GpuBuffer && destination is GpuBuffer)
        {
            if (destination.SizeInBytes < SizeInBytes)
                throw new GpuResourceException(this, "The destination buffer is not large enough.");
        }

        ResourceCopyTask task = Device.Tasks.Get<ResourceCopyTask>();
        task.Destination = destination;
        task.OnCompleted += completeCallback;
        task.Resource = this;
        Device.Tasks.Push(priority, task);
    }

    /// <summary>
    /// Copies a sub-resource from the current <see cref="GpuResource"/> to the sub-resource of the destination <see cref="GpuResource"/>.
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="sourceLevel"></param>
    /// <param name="sourceSlice"></param>
    /// <param name="destination"></param>
    /// <param name="destLevel"></param>
    /// <param name="destSlice"></param>
    /// <param name="completeCallback"></param>
    /// <exception cref="ResourceCopyException"></exception>
    public void CopyTo(GpuPriority priority,
    uint sourceLevel, uint sourceSlice,
    GpuResource destination, uint destLevel, uint destSlice,
    GpuTask.EventHandler completeCallback = null)
    {
        if (!Flags.Has(GpuResourceFlags.UploadMemory))
            throw new ResourceCopyException(this, destination, "The current texture cannot be copied from because the GpuResourceFlags.UploadMemory flag was not set.");

        if (!destination.Flags.Has(GpuResourceFlags.DownloadMemory))
            throw new ResourceCopyException(this, destination, "The destination texture cannot be copied to because the GpuResourceFlags.DownloadMemory flag was not set.");

        // Validate dimensions.
        // TODO this should only test the source and destination level dimensions, not the textures themselves.
        if (this is GpuTexture srcTex)
        {
            if (destination is GpuTexture destTex)
            {
                if (ResourceFormat != destination.ResourceFormat)
                    throw new ResourceCopyException(this, destination, "The source and destination texture formats do not match.");

                if (destTex.Width != srcTex.Width ||
                    destTex.Height != srcTex.Height ||
                    destTex.Depth != srcTex.Depth)
                    throw new ResourceCopyException(this, destination, "The source and destination textures must have the same dimensions.");

                if (sourceLevel >= srcTex.MipMapCount)
                    throw new ResourceCopyException(this, destination, "The source mip-map level exceeds the total number of levels in the source texture.");

                if (sourceSlice >= srcTex.ArraySize)
                    throw new ResourceCopyException(this, destination, "The source array slice exceeds the total number of slices in the source texture.");

                if (destLevel >= destTex.MipMapCount)
                    throw new ResourceCopyException(this, destination, "The destination mip-map level exceeds the total number of levels in the destination texture.");

                if (destSlice >= destTex.ArraySize)
                    throw new ResourceCopyException(this, destination, "The destination array slice exceeds the total number of slices in the destination texture.");

                SubResourceCopyTask task = Device.Tasks.Get<SubResourceCopyTask>();
                task.SrcRegion = null;
                task.Resource = this;
                task.SrcSubResource = (sourceSlice * srcTex.MipMapCount) + sourceLevel;
                task.DestResource = destination;
                task.DestStart = Vector3UI.Zero;
                task.DestSubResource = (destSlice * destTex.MipMapCount) + destLevel;
                task.OnCompleted += completeCallback;
                Device.Tasks.Push(priority, task);
            }
            else
            {
                throw new NotImplementedException("Copying a texture to a non-texture is currently unsupported.");
            }
        }
    }

    /// <summary>Copies all the data in the current <see cref="GpuResource"/> to the destination <see cref="GpuResource"/>.</summary>
    /// <param name="priority">The priority of the operation</param>
    /// <param name="destination">The <see cref="GpuResource"/> to copy to.</param>
    /// <param name="sourceRegion"></param>
    /// <param name="destByteOffset"></param>
    /// <param name="completionCallback">A callback to invoke once the operation is completed.</param>
    public void CopyTo(GpuPriority priority, GpuResource destination, ResourceRegion sourceRegion, uint destByteOffset = 0,
        GpuTask.EventHandler completionCallback = null)
    {
        SubResourceCopyTask task = Device.Tasks.Get<SubResourceCopyTask>();
        task.DestResource = destination;
        task.Resource = this;
        task.DestStart = new Vector3UI(destByteOffset, 0, 0);
        task.SrcRegion = sourceRegion;
        task.OnCompleted += completionCallback;
        Device.Tasks.Push(priority, task);
    }

    /// <summary>
    /// Gets the size of the resource, in bytes. 
    /// <para>This is the total size of all sub-resources within the resource, such as mip-map levels and array slices.</para>
    /// </summary>
    public abstract ulong SizeInBytes { get; protected set; }

    /// <summary>
    /// Gets the resource flags that provided given when the current <see cref="GpuResource"/> was created.
    /// </summary>
    public GpuResourceFlags Flags { get; protected set; }

    /// <summary>
    /// Gets or [protected] sets the <see cref="GpuResourceFormat"/> of the resource.
    /// </summary>
    public abstract GpuResourceFormat ResourceFormat { get; protected set; }

    /// <summary>
    /// Gets the ID of the frame that the current <see cref="GpuResource"/> was applied.
    /// </summary>
    public ulong LastUsedFrameID { get; private set; }

    /// <summary>
    /// Gets the ID of the frame that the current <see cref="GpuTexture"/> was resized. 
    /// If the texture was never resized then the frame ID will be the ID of the frame that the texture was created.
    /// </summary>
    public ulong LastFrameResizedID { get; internal set; }
}
