﻿using Silk.NET.Core.Native;
using Silk.NET.Direct3D12;

namespace Molten.Graphics.DX12;

public unsafe class ShaderIOLayoutDX12 : ShaderIOLayout
{
    internal InputElementDesc[] VertexElements { get; private set; }

    public ShaderIOLayoutDX12(uint elementCount) : base(elementCount) { }

    public ShaderIOLayoutDX12(ShaderCodeResult result, ShaderType sType, ShaderIOLayoutType type) :
        base(result, sType, type)
    { }

    protected override void Initialize(uint numVertexElements)
    {
        if (numVertexElements > 0)
            VertexElements = new InputElementDesc[numVertexElements];
    }

    protected override void BuildVertexElement(ShaderCodeResult result, ShaderIOLayoutType type, ShaderParameterInfo pInfo, GraphicsFormat format, int index)
    {
        // Elements is null if the IO is not for a vertex shader input.
        if (VertexElements == null)
            return;

        VertexElements[index] = new InputElementDesc()
        {
            SemanticName = (byte*)pInfo.SemanticNamePtr,
            SemanticIndex = pInfo.SemanticIndex,
            InputSlot = 0, // This does not need to be set. A shader has a single layout, 
            InstanceDataStepRate = 0, // This does not need to be set. The data is set via Context.DrawInstanced + vertex data/layout.
            AlignedByteOffset = 16 * pInfo.Register,
            InputSlotClass = InputClassification.PerVertexData,
            Format = format.ToApi(),
        };
    }

    protected override void OnDispose()
    {
        // Dispose of element string pointers, since they were statically-allocated by Silk.NET
        for (uint i = 0; i < VertexElements.Length; i++)
            SilkMarshal.Free((nint)VertexElements[i].SemanticName);
    }
}
