﻿using Molten.Graphics;

namespace Molten.Examples;

[Example("Texture Arrays - 1D", "Demonstrates how 1D texture arrays are used")]
public class Texture1DArray : MoltenExample
{
    ContentLoadHandle _hShader;
    ContentLoadHandle _hTexture;

    protected override void OnLoadContent(ContentLoadBatch loader)
    {
        base.OnLoadContent(loader);

        _hShader = loader.Load<Shader>("assets/BasicTexture1D.json");
        _hTexture = loader.Load<ITexture1D>("assets/1d_1.png");
        loader.OnCompleted += Loader_OnCompleted;
    }

    private void Loader_OnCompleted(ContentLoadBatch loader)
    {
        if (!_hShader.HasAsset())
        {
            Close();
            return;
        }

        Shader shader = _hShader.Get<Shader>();
        ITexture1D texture = _hTexture.Get<ITexture1D>();
        shader[ShaderBindType.Resource, 0] = texture;
        TestMesh.Shader = shader;
    }

    protected override Mesh GetTestCubeMesh()
    {
        return Engine.Renderer.Device.Resources.CreateMesh(SampleVertexData.TextureArrayCubeVertices);
    }
}
