﻿namespace Molten.Graphics;

/// <summary>A <see cref="RenderSceneChange"/> for adding a <see cref="SceneObject"/> to the root of a scene.</summary>
internal class RenderAddScene : GpuTask
{
    public SceneRenderData Data;

    public override void ClearForPool()
    {
        Data = null;
    }

    public override bool Validate()
    {
        return true;
    }

    protected override bool OnProcess(RenderService renderer, GpuCommandList cmd)
    {
        renderer.Scenes.Add(Data);
        return true;
    }
}
