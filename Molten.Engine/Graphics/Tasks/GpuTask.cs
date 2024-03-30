﻿using Molten.Collections;

namespace Molten.Graphics;

public abstract class GpuTask : IPoolable
{
    public delegate void EventHandler(GpuTask task, bool success);

    /// <summary>
    /// Gets or sets the pool that owns the current <see cref="GpuTask"/>.
    /// </summary>
    internal ObjectPool<GpuTask> Pool { get; set; }

    /// <summary>
    /// Invoked when the task has been completed.
    /// </summary>
    public event EventHandler OnCompleted;

    public abstract void ClearForPool();

    public abstract bool Validate();

    /// <summary>
    /// Invoked when the current <see cref="GpuTask"/> needs to be processed.
    /// </summary>
    /// <param name="cmd">The <see cref="GpuCommandList"/> that should process the task.</param>
    public void Process(GpuCommandList cmd)
    {
        if(OnProcess(cmd.Device.Renderer, cmd))
            OnCompleted?.Invoke(this, true);
        else
            OnCompleted?.Invoke(this, false);

        OnCompleted = null;

        // Recycle the completed/failed task.
        Pool.Recycle(this);
    }

    /// <summary>
    /// Invoked when the task should be processed by the specified <see cref="GpuCommandList"/>.
    /// </summary>
    /// <param name="renderer">The renderer that the task is bound to.</param>
    /// <param name="cmd">The <see cref="GpuCommandList"/> that should process the current <see cref="GpuTask"/>.</param>
    /// <returns></returns>
    protected abstract bool OnProcess(RenderService renderer, GpuCommandList cmd);
}
