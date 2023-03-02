﻿
namespace Molten.Graphics
{
    public class Material : HlslShader
    {
        MaterialPass[] _passes = new MaterialPass[0];

        /// <summary>
        /// The bytecode of the first pass' vertex shader.
        /// </summary>
        public unsafe void* InputStructureByteCode;

        Dictionary<string, MaterialPass> _passesByName;

        public Material(GraphicsDevice device, string filename) : base(device, filename)
        {
            _passesByName = new Dictionary<string, MaterialPass>();
        }

        public void AddPass(MaterialPass pass)
        {
            int id = 0;
            if (_passes == null)
            {
                _passes = new MaterialPass[1];
            }
            else
            {
                id = _passes.Length;
                Array.Resize(ref _passes, _passes.Length + 1);
            }

            _passes[id] = pass;
        }

        public MaterialPass GetPass(uint index)
        {
            return _passes[index];
        }

        public MaterialPass GetPass(string name)
        {
            return _passesByName[name];
        }

        public override void GraphicsRelease()
        {
            for (int i = 0; i < _passes.Length; i++)
                _passes[i].Dispose();

            base.OnDispose();
        }

        public ObjectMaterialProperties Object { get; set; }

        public LightMaterialProperties Light { get; set; }

        public SceneMaterialProperties Scene { get; set; }

        public GBufferTextureProperties Textures { get; set; }

        public SpriteBatchMaterialProperties SpriteBatch { get; set; }

        public int PassCount => _passes.Length;

        public MaterialPass[] Passes => _passes;

        /// <summary>
        /// The default state to use for passes that were not provided a state.
        /// </summary>
        public GraphicsState DefaultState { get; set; }
    }
}
