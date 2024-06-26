﻿namespace Molten.Graphics;

[Flags]
public enum GpuCommandListFlags
{
    /// <summary>
    /// No flags. The command list should only be submitted once per frame and will not provide any synchronization objects.
    /// </summary>
    None = 0,

    /// <summary>
    /// A short-lived/transient command list, intended for successive submissions within the same frame.
    /// </summary>
    Short = 1,

    /// <summary>
    /// The next command list will provide a fence for syncing.
    /// </summary>
    CpuSyncable = 1 << 1,

    /// <summary>
    /// The current command list will wait for all previously-submitted command lists in the current frame, which had the <see cref="CpuSyncable"/> flag set.
    /// </summary>
    CpuWait = 1 << 2,

    /// <summary>
    /// The current command list list can only be submitted once.
    /// </summary>
    SingleSubmit = 1 << 3,
}

public static class GraphicsCommandListFlagsExtensions
{
    public static bool Has(this GpuCommandListFlags flags, GpuCommandListFlags flag)
    {
        return (flags & flag) == flag;
    }
}
