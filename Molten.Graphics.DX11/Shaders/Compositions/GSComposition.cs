﻿using Silk.NET.Direct3D11;

namespace Molten.Graphics
{
    internal class GSComposition : ShaderComposition<ID3D11GeometryShader>
    {
        public GSComposition(HlslShader parentShader, bool optional) : 
            base(parentShader, optional, ShaderType.GeometryShader)
        {
        }

        protected override unsafe ID3D11GeometryShader* CreateShader(void* ptrBytecode, nuint numBytes)
        {
            ID3D11GeometryShader* ppShader = null;
            Parent.Device.NativeDevice->CreateGeometryShader(ptrBytecode, numBytes, null, &ppShader);
            return ppShader;
        }
    }
}