﻿using Molten.Collections;
using System.Collections.Concurrent;

namespace Molten.Graphics;

public class GpuTaskManager : IDisposable
{
    private class TaskQueue
    {
        internal ThreadedQueue<GpuTask> Tasks = new();
        internal GpuFrameBuffer<GpuCommandList> Cmd;
    }

    TaskQueue[] _queues;
    ConcurrentDictionary<Type, ObjectPool<GpuTask>> _taskPool;
    GpuDevice _device;

    internal GpuTaskManager(GpuDevice device)
    {
        _device = device;
        GpuPriority[] priorities = Enum.GetValues<GpuPriority>();
        _queues = new TaskQueue[priorities.Length];

        for(int i = 0; i < priorities.Length; i++)
        {
            _queues[i] = new TaskQueue();
            _queues[i].Cmd = new GpuFrameBuffer<GpuCommandList>(_device, (gpu) => gpu.GetCommandList());
        }

        _taskPool = new ConcurrentDictionary<Type, ObjectPool<GpuTask>>();
    }

    public T Get<T>() 
        where T : GpuTask, new()
    {
        if(!_taskPool.TryGetValue(typeof(T), out ObjectPool<GpuTask> pool))
        {
            pool = new ObjectPool<GpuTask>(() => new T());
            if(!_taskPool.TryAdd(typeof(T), pool))
            {
                _device.Log.Error($"Failed to create render task pool for '{typeof(T).Name}'");
                return new T();
            }
        }

        T task = (T)pool.GetInstance();
        task.Pool = pool;
        return task;
    }

    /// <summary>
    /// Pushes a <see cref="GpuTask"/> to the specified priority queue in the current <see cref="GpuTaskManager"/>.
    /// </summary>
    /// <param name="priority">The priority of the task.</param>
    /// <param name="task"></param>
    public void Push(GpuPriority priority, GpuTask task)
    {
        if (task.Validate())
        {
            TaskQueue queue = _queues[(int)priority];
            queue.Tasks.Enqueue(task);
        }
    }

    /// <summary>
    /// Pushes a compute-based shader as a task.
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="shader">The compute shader to be run inside the task.</param>
    /// <param name="groupsX">The number of X compute thread groups.</param>
    /// <param name="groupsY">The number of Y compute thread groups.</param>
    /// <param name="groupsZ">The number of Z compute thread groups.</param>
    /// <param name="callback">A callback to run once the task is completed.</param>
    public void Push(GpuPriority priority, Shader shader, uint groupsX, uint groupsY, uint groupsZ, GpuTask.EventHandler callback = null)
    {
        Push(priority, shader, new Vector3UI(groupsX, groupsY, groupsZ), callback);
    }

    public void Push(GpuPriority priority, Shader shader, Vector3UI groups, GpuTask.EventHandler callback = null)
    {
        ComputeTask task = Get<ComputeTask>();
        task.Shader = shader;
        task.Groups = groups;
        task.OnCompleted += callback;
        Push(priority, task);
    }

    public void Dispose()
    {
        foreach (ObjectPool<GpuTask> pool in _taskPool.Values)
            pool.Dispose();

        foreach (TaskQueue queue in _queues)
            queue.Cmd.Dispose();

        _taskPool.Clear();
    }

    /// <summary>
    /// Processes all tasks held in the manager for the specified priority queue, for the current <see cref="GpuTaskManager"/>.
    /// </summary>
    /// <param name="priority">The priority of the task.</param>
    /// <param name="cmdQueue">The GPU queue that will execute the command list.</param>
    internal void Process(GpuPriority priority)
    {
        // TODO Implement "AllowBatching" property on RenderTask to allow multiple tasks to be processed in a single Begin()-End() command block
        //      Tasks that don't allow batching will:
        //       - Be executed in individual Begin()-End() command blocks
        //       - Be executed on the next available compute device queue
        //       - May not finish in the order they were requested due to task size, queue size and device performance.

        TaskQueue queue = _queues[(int)priority];
        GpuCommandList cmd = queue.Cmd.Prepare();

        cmd.Begin();
        cmd.BeginEvent($"Process '{priority}' tasks");
        while (queue.Tasks.TryDequeue(out GpuTask task))
            task.Process(cmd);

        cmd.EndEvent();
        cmd.End();
        _device.Execute(cmd);
    }
}
