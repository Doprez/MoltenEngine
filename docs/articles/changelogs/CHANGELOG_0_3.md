### [6278c84b]
Updated documentation data 

### [ccd9fa5c]
[Android] Updated assembly and package info 

### [82652947]
Updated assembly and package info 

### [188981d2]
[Graphics] Fixed missing blend state parameters 
[Graphics] Better handling of default material/pass states

### [1819b922]
[Graphics] Fixed null default states 

### [0172e7cd]
[Graphics] Final refactor of render state management 
[Graphics] Renamed ShaderSampler to GraphicsSampler for consistency
[Graphics] Renamed GraphicsPipelineState to GraphicsState
[Graphics] Added GraphicsStateParameters struct
[Graphics] Added GraphicsSamplerParameters struct
[Graphics] GraphicsState and GraphicsSampler can now be created from parameter structs
[Graphics] Removed obsolete GraphicsDevice.SamplerBank property
[Graphics] Removed obsolete SamplerBank class
[Graphics] Removed obsolete PipelineStateBank class
[Graphics] Removed obsolete GraphicsStateBank abstract class
[Graphics] Removed conditional state system from materials/shaders

### [b7d2466e]
[Vulkan] Started implementing rasterizer state 

### [c27710a9]
Merge branch 'NewStateSystem' 

### [91681204]
Reapplied all previous changes in a buildable and stable state 

### [109be888]
Updated all shaders to user the new state structure 

### [fde9891d]
[Graphics] Cleanup of GraphicsPipelineState 

### [6fed2131]
[DX11] Fixed issues with applying blend/depth/raster state changes 

### [8b442018]
[Graphics] Improvements to XML shader node parsers 
[Graphics] ShaderHeaderNode can now store multiple values

### [5eb6bef6]
[Util] Fixed StructKey memory allocation 
[Graphics] Fixed various runtime exceptions

### [d99d6492]
[Graphics] Fixed remaining build errors 

### [90a92eb8]
Revert "[Graphics] Merged Blend, Depth and Rasterizer states into GraphicsPipelineState" 
This reverts commit ae2e3a0fc1a2619d1f3f8958ca427164909bdf1a.

### [eedc24b7]
Revert "[Graphics] on render state merger" 
This reverts commit 1bb478a1668eb94bea7dc660259fb2e117afbd15.

### [da1d1a7f]
Revert "[Vulkan] Started implementation of PipelineStateVK" 
This reverts commit 6431bcb53b6af8fe110ad8b6dc5bb99b6516c56f.

### [6431bcb5]
[Vulkan] Started implementation of PipelineStateVK 
[DX11] Fixes to PipelineStateDX11

[1bb478a1]
[Graphics] on render state merger 
[Graphics] Merged all state node parsers

[ae2e3a0f]
[Graphics] Merged Blend, Depth and Rasterizer states into GraphicsPipelineState 
[Graphics] Removed GraphicsBlendState
[Graphics] Removed GraphicsDepthState
[Graphics] Removed GraphicsRasterizaterState
[Graphics] Added GraphicsPipelineState
[Graphics] Replaced blend, depth and rasterizer preset banks with PipelineStateBank
[Graphics] Added key-based caching system to GraphicsDevice

[3eafad74]
[Graphics] Refactored shader state node parsers 
[Graphics] Automated property mapping for blend, depth, sampler and rasterizer states in XML material definitions
[Graphics] Skip XML "#text" nodes in ShaderNodeParser
[Graphics] Fixed ShaderHeaderNode.Name not being set
[Graphics] Added StateNodeParser base class
[Util] Added non-generic overload of EngineUtil.TryParseEnum()

### [a59d9c1e]
[Graphics] Removed obsolete GraphicsState class 

### [6173658d]
[Graphics] Removed obsolete CommandStateStack 

### [9c30ab83]
[Vulkan] First pass on depth-stencil state implementation 
[Graphics] Exposed support for depth bounds testing
Added null check in EngineUtil.Free()
[Graphics] Added GraphicsCapabilities.DepthBoundsTesting property
[Graphics] Added missing summaries to DepthStencilOperation enum

### [69fc7a04]
[Graphics] Renamed GraphicsDepthState.DepthWriteEnabled for consistency 

### [de5fc6a6]
[Graphics] Refactored depth write-enable properties 
[Graphics] Removed DepthWriteFlags enum
[Graphics] Renamed GraphicsDepthState.WriteFlags to DepthWriteEnable
[Graphics] GraphicsDepthState.DepthWriteEnable is now a bool
[Graphics] Renamed "<writemask>" material tag to "<writeenable>"

### [49653505]
[Graphics] Simplified indexed mesh usage 
[Graphics] Merged indexed-mesh functionality into base Mesh class
[Graphics] Removed IndexedMesh<T>
[Graphics] Removed StandardIndexedMesh<T>
[Graphics] Removed IndexedInstancedMesh<T>
[Graphics] Removed IndexedMesh creation methods from ResourceFactory
[Graphics] Fixed SkyboxStep.MakeSphere() using Int32 index format instead of UInt32

### [417aa921]
Update README.md 

### [01bd1b34]
Merge pull request #178 from Syncaidius/dependabot/nuget/BenchmarkDotNet-0.13.5 
Bump BenchmarkDotNet from 0.13.4 to 0.13.5

### [6169f05b]
[Graphics] Added indexed instanced mesh support 

### [a9d7c14f]
[Graphics] Further improvements to meshes 
[Graphics] Added IVertexInstanceType for specifying batching parameters
[Graphics] Removed need to provide batching callback for InstancedMesh

### [3fd65d80]
Bump BenchmarkDotNet from 0.13.4 to 0.13.5 
Bumps [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) from 0.13.4 to 0.13.5.
- [Release notes](https://github.com/dotnet/BenchmarkDotNet/releases)
- [Commits](https://github.com/dotnet/BenchmarkDotNet/compare/v0.13.4...v0.13.5)

Signed-off-by: dependabot[bot] <support@github.com>

### [b749e3bb]
[Graphics] Optimised InstancedMesh batching 

### [4e7d59ba]
[DX11] Hardware instancing now functional 
[DX11] Fixed instanceStepRate not being set on new VertexFormat instances

### [eba5be48]
[DX11] Moved CommandQueueDX11.Bind() into ApplyState() 

### [39237a89]
[Graphics] Removed unnecessary BasicInstanceData.InstanceID field 

### [073d890e]
[Graphics] Simplified renderable batching system 
[Graphics] Meshes now unbind any vertex/index buffers they use after drawing
[DX11] Fixed FXC compiler not capitalizing semantic names
[DX11] Fixed FXC not setting ShaderIOStructure metadata semantic index
[DX11] Fixes to VertexInputLayout mapping
Fixed ContentWatcher causing assets to load twice

### [274f5953]
[Graphics] Further work on instanced/batched mesh support 

### [86200e85]
[Graphics] First pass on mesh batching support 
[Graphics] First pass on InstancedMesh implementation
[Graphics] Fixed IndexedMesh not releasing index buffer on dispose
[Graphics] Fixed non-functional IndexedMesh.IndexCount property
[Graphics] Fixed IndexedMesh.MaxIndices not being assigned
[Graphics] Added RenderDataBatch class

### [2ff626d6]
[Graphics] Simplified Mesh OnRender() 

### [0999ca29]
Fixed non-indexed stress test example 

### [63168921]
[Graphics] Removed obsolete MeshComponent class 
[Graphics] RenderableComponent is no longer abstract
[Graphics] Optimised and cleaned up RenderableComponent.RemoveFromScene()
[Graphics] Added missing summaries to Mesh properties
[Graphics] Removed Mesh<T>.ApplyBuffers()

### [0a76af5c]
Cameras now have their own BackgroundColor property 
Removed Scene.BackgroundColor in favour of using CameraComponent.BackgroundColor
[Graphics] Fixed name typo of GraphicsCommandQueue.SetMarker()
[Graphics] Fixed nested scenes not rendering. e.g. example windows

### [045bb1f2]
[Graphics] Removed StartStep from RenderChain.BuildRender() 

### [30f97e16]
[Graphics] Removed remnants of depth-override system 

[3e410fc1]
Reorganised scene components 

### [c428a393]
[Graphics] Improved naming of GraphicsPriority values 

### [50610e59]
[DX11] Fixed invalid VB count when unbinding 

### [f38d454a]
[Graphics] Improved disposal/release of shader reflection data 

### [9126b2ee]
[DX11] Fixed incorrect setup of constant buffer default values 

### [ab272d44]
[Graphics] Removed unused DepthStencilPreset.Sprite2D 

### [a3a026b2]
[Graphics] Removed obsolete depth-write override system 

### [7d85b2d8]
[DX11] Fixed incorrect conditions for setting null input layout 

### [6287b263]
[Android] Fixed DXC missing from build config 

### [e4c1c064]
[Android] Fix build errors 

### [6769635f]
Merge pull request #177 from Syncaidius/dependabot/nuget/Magick.NET-Q8-AnyCPU-12.3.0 
Bump Magick.NET-Q8-AnyCPU from 12.2.2 to 12.3.0
### [9644712d]
[Graphics] Fixed ignored RenderCameraFlags.DoNotClear 

### [39be427a]
Bump Magick.NET-Q8-AnyCPU from 12.2.2 to 12.3.0 
Bumps [Magick.NET-Q8-AnyCPU](https://github.com/dlemstra/Magick.NET) from 12.2.2 to 12.3.0.
- [Release notes](https://github.com/dlemstra/Magick.NET/releases)
- [Commits](https://github.com/dlemstra/Magick.NET/compare/12.2.2...12.3.0)

---
updated-dependencies:
- dependency-name: Magick.NET-Q8-AnyCPU
  dependency-type: direct:production
  update-type: version-update:semver-minor
...

Signed-off-by: dependabot[bot] <support@github.com>
### [eac3dc2e]
[Graphics] Fixed SpriteBatcher clip stack not correctly resetting 
[Graphics] Re-internalized RenderService.SpriteBatch

### [959594c3]
[Graphics] Use correct depth state for SDF text rendering 

### [7f6fca0a]
[DX11] Correctly set WinformsSurface.Name base property 

### [19f80c22]
Made EngineObject.Name a virtual property 
[DX11] WinformsSurface now correctly overrides Name property

### [8589b49c]
[Graphics] Removed RenderService.ClearIfFirstUse() 
[Graphics] Optimised how render surfaces are cleared during render
[Graphics] StartStep clean-up

### [981f4347]
[Graphics] Fixed 'Name' conflict in IRenderSurface and IGraphicsObject 
[Graphics] IRenderSurface now inherits IGraphicsObject
[Graphics] Start/end render scene event for each scene

### [86a002da]
[Graphics] Use the correct depth state in sprite shader 

### [26cb63c4]
[Graphics] FontManager initialization now handled by RenderService 
[Graphics] Added Name property to Texture1DProperties and derivatives
[Graphics] Added check in TextureProcessor for lack of a renderer or font manager
[Graphics] Added RenderService.Fonts property
Removed Engine.Fonts property

### [baba8710]
[DX11] Improved usage of debug annotations 
[[Graphics] Added debug annotation methods to GraphicsCommandQueue

### [25f42779]
[DX11] TextureBase derivatives now properly use debug naming 
[DX11] Fixed Texture2D template constructor not passing MSAA flags

### [0a85b424]
[DX11] Fixed unitialized IASetInputLayout when VBs are null 

### [da586393]
[DX11] Include category and severity in debug layer output 

### [8d515bd2]
[DX11] Hooked up debug layer to log output 

### [d5412e1e]
[Graphics] More debug logging 

### [a7e12dff]
[Graphics] RenderSurfaceBlend.BlendEnable is now a bool property 
[Graphics] Improvements to graphics state debug/logging

### [ef4c3fe9]
[DX11] Removed obsolete RasterizerStateDX11.Equals() method 

### [f9af4a16]
[Graphics] Standardized default blend-state initialization 

### [beef84ba]
[Graphics] Cleanup of SpriteBatch.Flush() 

### [31d41e27]
[DX11] Fixed DeviceDX11.CreateBuffer() passing incorrect BufferMode 

### [8c999bab]
[Graphics] Fixed slot not re-binding resource if version has changed 
[DX11] Fix lack of checks in rasterizer and depth state properties
[Graphics] Removed obsolete IShaderSampler interface

### [9e470e10]
[Graphics] Removed obsolete RenderSerice abstract methods 

### [f6940602]
[Graphics] Added pre-render and post-render stages to renderer 
[Graphics] Moved core render logic into RenderChain
[Graphics] Moved RenderService.GetRenderStep() into RenderChain
[Graphics] Renamed RenderStepBase to RenderStep
[Graphics] Merged SceneRenderData<R> into SceneRenderData base
[Graphics] Merged LayerRenderData<R> into LayerRenderData base
[Graphics] Removed abstract RenderService.OnCreateRenderData()
[Graphics] Removed IRenderable

### [faa15a7b]
[Graphics] Fixed unset default sampler border color 

### [7dd41bc3]
[DX11] Fixed blend/depth/rasterizer states not updating their version counter 

### [42b5b8ec]
[Graphics] Standardized sprite-batcher 
[DX11] Merged SpriteBatcherDX11 into SpriteBatcher
[Graphics] Fixed missing GraphicsBufferFlags in LightingStep
[Graphics] Removed redundant MappedBufferException
[Graphics] Removed redundant UnsupportedFeatureException
[DX11] Renamed BufferSegment.Map() to .GetStream()
[DX11] Improvements to BufferSegment.GetStream()
[DX11] Renamed TexureApply task to ApplyObjectTask
[Graphics] Standardized ApplyObjectTask
Fixed WorkerThread not waiting-on-reset once the task queue is empty
Fixed WorkerGroup.Dispose() hanging during thread disposal
Fixed incorrect blend-state preset usage in BlendNodeParser

### [ccf4f6a4]
[DX11] Fixed incorrect use of GraphicsBufferFlags.ShaderResource 

### [f2bdc424]
[DX11] Fixed GraphicsBufferFlags.UnorderedAccess being ignored 

### [d92321e7]
[Graphics] Standardized mesh creation and usage 
[Graphics] Standardized buffer and buffer segment system
[Graphics] Added IGraphicsBuffer and IGraphicsBufferSegment interfaces
[Graphics] Added IStagingBuffer interface
[DX11] StagingBuffer now inherits IStagingBuffer
[Graphics] Removed IMesh and IIndexedMesh
[Graphics] Removed IBonedMesh
[Graphics] Standardized remaining render steps
[Graphics] Re-internalized many render step methods
[Graphics] ResourceFactory mesh methods are no longer abstract

### [9228b768]
Code cleanup 

### [2af6d900]
[Graphics] Commonized render chain system 
[Graphics] RenderService.Chain is no longer abstract
[Graphics] Simplified render chain initialization

### [d89e0f07]
[Graphics] Commonized several render steps 

### [4206cc74]
[Graphics] Commonized slot-binding system 

### [1b03c438]
[Graphics] Renamed ContextBindableResource to GraphicsResourceDX11 

### [0c6873f4]
[Graphics] Further work on commonizing the renderer 
[Graphics] Moved StartStep into Molten.Engine
[Graphics] Moved RenderStepBase into Molten.Engine

### [c9727e37]
[Graphics] Fixed build errors 

### [508984b8]
[Graphics] Began commonizing the renderer 
[Graphics] Removed ISpriteRenderer
[Graphics] Improved sprite batch abstraction
[Graphics] Removed ResourceFactory.CreateSpriteRenderer()
[Graphics] Added RenderService.SpriteBatch property

### [37ab286a]
[Graphics] Abstracted compute task system 
[Graphics] Moved remaining shader node parsers to Molten.Engine
[Android] Fixed build error in Molten.Engine.Android

### [5aff8f5b]
[Graphics] Abstracted material system 
[Graphics] Simplified shader compiler system
[Graphics] Removed redundant ShaderCompilerContext.Renderer property
[DX11] Removed obsolete FxcNodeParser class
[DXC] Removed obsolete DxcFoundation class
[Vulkan] Removed obsolete SpirVShader class
[Vulkan] Removed obsolete SpirVNodeParser class
[Graphics] IMaterialPass now inherits IShaderElement
[Graphics] Moved several ShaderNodeParsers from DX11 into Molten.Engine
[Graphics] Removed redundant IShader.Metadata property
[DX11] Removed obsolete HlslInputBindDescription class
[Graphics] Removed IMaterial
[Graphics] Removed IMaterialPass

### [73c9d4e8]
[Graphics] Fixed remaining build errors 

### [e40ef928]
[Graphics] Common default blend, depth and rasterizer state configs 
[Graphics] Abstracted sampler state management
[Graphics] Added GraphicsDevice.SamplerBank property

### [d92d830f]
[Graphics] Abstracted rasterizer state management 
[Graphics] Added abstract GraphicsDevice.CreateRasterizerState()
[Graphics] Added RasterizerFillingMode enum
[Graphics] Added RasterizerCullingMode enum
[Graphics] Moved RasterizerStateBank into Molten.Engine and GraphicsDevice

### [132d6e81]
[Graphics] Fixed several build errors 

### [79224481]
[Graphics] Simplified surface blend configuration 

### [ecfd0374]
[Graphics] Further refinements to blend and depth state management 

### [5d7b0808]
[Graphics] Abstracted the blend and depth states 
[Graphics] Abstracted Depth-stencil state
[Graphics] Added GraphicsDevice.CreateDepthState()
[Graphics] Added GraphicsDevice.CreateBlendState()
[Graphics] Added GraphicsDevice.CreateDefaultSurfaceBlend()
[Graphics] Abstracted State-bank classes
[Graphics] Abstracted slot binding system

### [d77a1787]
[DX11] Removed obsolete/unused HlslFoundation.Parent property 

### [3c2c9eae]
[DX11] Moved CommandQueueDX11.DepthWriteOverride to base class 

### [7cbdf30f]
[Graphics] Further work on making renderer cross-API/platform 
[DX11] Merged DeviceContextState into CommandQueueDX11
[DX11] Renamed GraphicsPipeState to GraphicsState
[Graphics] Added several key abstract methods to GraphicsCommandQueue
[DX11] Removed CommandQueueDX11.AllSlots property - Never used
[Graphics] Added GraphicsPriority enum
[Graphics] Texture/Surface Clear() and Resize() now expect a GraphicsPriority parameter
[DX11] Moved and abstracted SurfaceManager into Molten.Engine
[DX11] Moved SurfaceManager to Molten.Engine
[DX11] Moved SurfaceSizeMode and MainSurfaceType to Molten.Engine
[DX11] Moved and abstracted DepthSurfaceTracker and SurfaceTracker into Molten.Engine

### [4fca75b0]
[DX11] Removed redundant ContextSlot.ParentState property 
[DX11] Removed redundant ContextSlotGroup.ParentState property
[DX11] Replaced ParentState property with Cmd property

### [71225d02]
[Graphics] Renamed ContextBindTypeFlags to GraphicsBindTypeFlags 
[Graphics] Moved GraphicsBindTypeFlags into main engine project

### [48411b2e]
[Content] Improved loading of different content types from same file 
Fixed 'Save Texture' example

### [54396fe3]
[Content] Refactored ContentLoadBatch callbacks 
[Content] This fixes #126

### [92e41853]
[Graphics] Abstracted VertexFormat and Shader IO 
[Graphics] Moved VertexFormat from DX11 into Molten.Engine
[Graphics] ShaderIOStructure is now abstract
[DX11] Implemented API-specific shader IO in ShaderIOStructureDX11
[Graphics] Added VertexElementType to GraphicsFormat extension method ToGraphicsFormat()
[Graphics] Abstracted vertex format cache

### [45750786]
[DX11] Flattened InputElementData into ShaderIOStructure 

### [796de66c]
[Graphics]  More work on refactoring shader reflection 
[Graphics] Fixed DXC build errors
[DXC] Removed DxcCompileResult
[DXC] Implemented DxcCompiler.BuildReflection()
[DXC] Removed DxcReflection
[Graphics] Added ShaderClassResult.DebugData pointer property
[Graphics] Renamed ShaderInputInfo to ShaderResourceInfo
[Graphics] Added ShaderReflection.InputParameters
[Graphics] Added ShaderReflection.OutputParameters
[Graphics] Added several reflection-related enum types
[Graphics] Added ShaderParameterInfo to store input/output parameter reflection info

### [e5af85fd]
[DX11] Fixed remaining build errors in FxcClassCompiler 

### [fe67b40b]
[Graphics] First pass on abstracting shader reflection 
[Graphics] Added ShaderReflection class
[Graphics] IShaderClassResult is now ShaderClassResult class
[DX11] Removed FxcCompileResult
[DX11] Moved FxcCompileResult functionality into ShaderClassResult
[Graphics] Added PrimitiveTopology enum
[Graphics] Added ShaderReturnType and ShaderInputType enums
[Graphics] Added ShaderInputInfo reflection class
[Graphics] Added ShaderInputFlags enum
[Graphics] Added ShaderResourceDimension enum
[Graphics] Added ShaderVariableClass enum
[Graphics] Added ShaderVariableType enum
[Graphics] Added ShaderVariableFlags enum
[Graphics] Added ConstantBufferFlags enum
[Graphics] Added ConstantBufferInfo class and ConstantBufferType enum
[Graphics] Added ConstantBufferVariableInfo

### [addb5e6e]
[Graphics] Simplified shader node-parser initialization 
[Graphics] Removed abstract ShaderCompiler.GetNodeParserList()
[Util] Fixed ReflectionHelper.FindType() including abstract types
[Util] Fixed ReflectionHelper.FindTypeInParentAssembly() including generic base types

### [b966dfa8]
[Graphics] Further work on device and render abstraction 
[Graphics] Added GraphicsObject base class
[Graphics] Renamed RenderService.OnInitializeApi() to OnInitializeDisplayManager()
[Graphics] Added RenderService.OnInitializeDevice()
[Graphics] Added RenderService.Device property
[Graphics] Renamed DisplayManager base to GraphicsDisplayManager
[DX11] Moved ContextObject funtionality into GraphicsObject
[DX11] Removed ContextObject
[DX11] Moved DeviceDX11 disposal tracking into GraphicsDevice base class
[DX11] Removed DeviceDX11.MarkForRelease()
[DX11] Removed DeviceDX11.DisposeMarkedObjects()

### [0cdf53b1]
[Graphics] First pass on device and command-queue abstraction 
[DX11] Renamed DeviceContext to CommandQueueDX11
[DX11] Separated DeviceDX11 from CommandQueueDX11
[Vulkan] Separated DeviceVK extension management into DeviceManagerVK
[DXC] Added support for Spir-V arg
[Vulkan] DeviceVK now inherits GraphicsDevice
[DX11] DeviceDX11 now inherits GraphicsDevice
[DX12] DeviceDX12 now inherits GraphicsDevice
[Graphics] Moved VRAM allocation methods to GraphicsDevice
[Graphics] Moved device Log and Settings properties to GraphicsDevice
[DX11] Removed DeviceDX11.Contexts property
[Vulkan] Fixed VK_KHR_surface extension only loading if debug layer is enabled
[Vulkan] Renamed ExtensionManager to ExtensionLoaderVK
[Vulkan] Renamed InstanceManager to InstanceLoaderVK
[Vulkan] Renamed DeviceManagerVK to DeviceLoaderVK
[DX11] Fixed incorrect validation mode being used in CommandQueueDX11.DrawIndexedInstanced()
[DX11] Removed unused StateConditions parameter from CommandQueueDX11.DrawIndexedInstanced()
[Graphics] Added abstract class GraphicsCommandQueue
[DX11] CommandQueueDX11 now inherits GraphicsCommandQueue

### [472df14b]
[Vulkan] Initialize DXC shader compiler 
[DXC] Main DxcCompiler class is now public

### [dc54c194]
[Vulkan] Retrieve WindowSurfaceVK back-buffer images/views 
[Vulkan] Added implicit Device cast to DeviceVK

### [327ee968]
[Vulkan] Added Swap-chain initialization to WindowSurfaceVK 
[Vulkan] Added helper DeviceVK.GetSharingMode()
[Vulkan] Store main graphics queue in DeviceVK
[Vulkan] Added DeviceVK.GraphicsQueue internal property
[Vulkan] Added helper CommandQueueVK.HasFlags()

### [ee446c15]
Added missing DataMember attribue on GraphicsSettings.BackBufferSize 

### [142aaaa9]
[DX11] Fixed errors related to GraphicsSetting.BackBufferSize changes 
[Graphics] Added GraphicsSettings.GetBackBufferSize() helper method

### [44e403ca]
[Vulkan] Added back-buffer size validation to WindowSurfaceVK 
[Graphics] Added BackBufferMode enum
[Graphics] GraphicsSettings.BackBufferSize is now a BackBufferMode enum value

### [dc95327c]
[Vulkan] Added GraphicsFormat to WindowSurfaceVK constructor 
[Vulkan] Added format, color-space and present mode validation to WindowSurfaceVK

### [7c74aa40]
Merge pull request #176 from Syncaidius/dependabot/nuget/BenchmarkDotNet-0.13.4 
Bump BenchmarkDotNet from 0.13.3 to 0.13.4

### [7c188dde]
Bump BenchmarkDotNet from 0.13.3 to 0.13.4 
Bumps [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) from 0.13.3 to 0.13.4.
- [Release notes](https://github.com/dotnet/BenchmarkDotNet/releases)
- [Commits](https://github.com/dotnet/BenchmarkDotNet/compare/v0.13.3...v0.13.4)

### [6fc57012]
[Vulkan] Simplified ExtensionManager GetLayer() and GetExtension() overrides 

### [f952976a]
[Vulkan] Fixed DeviceVK enumerating extensions as instance extensions 

### [0f964f4d]
[Vulkan] Fixes to window initialization 
[Vulkan] Don't set bind to null in ExtensionManager.Build()
[Vulkan] Fixed ExtensionManager.GetExtension()
[Vulkan] Correctly set RendererVK.Device property
[Vulkan] Correctly dispose of surface and window in WindowSurfaceVK

### [3dd5e59c]
[Vulkan] Implemented RendererVK.Resources property 
[Vulkan] Added ResourceFactoryVK
[Vulkan] WindowSurfaceVK can now be created via ResourceFactoryVK

### [f161a152]
[DXGI] Abstracted SwapChain creation for DX11/12 
[DXGI] Added DisplayManagerDXGI.CreateSwapChain()
[DX11] Fixed swap-chain resize not updating back-buffer size to match settings

### [ed82cd3d]
[Vulkan] Groundwork for swap-chain/present functionality 
[DX12] Implemented command queue initialization
[DX12] Improvements to error loggings
[Vulkan] Implemented command queue initialization
[Vulkan] Optimised error logging
[Vulkan] Added PresentationSurfaceVK
[Vulkan] Added DeviceVK.FindPresentQueue()
[Vulkan] Added ExtensionManager.GetExtension<E>()
[Vulkan] Added helper classes AllocatedObjectVK<T> and NativeObjectVK<T>
[Vulkan] ExtensionManager now inherits AllocatedObjectVK<T>
[Vulkan] DisplayAdapterVK now inherits NativeObjectVK<T>
[Vulkan] CommandQueueVK now inherits NativeObjectVK<T>
[Graphics] Renamed SupportedCommandSet.MaxCount to MaxQueueCount

### [df316e29]
Update README.md 

### [62e1a8d4]
[DX12] First pass on DeviceDX12 instantiation 
Disable nullable compilation in Molten.Graphics.DX12 project
[DX11] Renamed DeviceDX11.NativeDevice  field to Ptr
[Vulkan] Added VRAM allocation tracking methods to DeviceVK
[DX12] Added VRAM allocation tracking methods to DeviceDX12

### [ebbf20ee]
[DX12] Added detection of ROVs and conservative rasterization 
[Graphics] Added ConservativeRasterizationLevel enum
[Graphics] Added GraphicsCapabilities.RasterizerOrderViews property
[Graphics] Added GraphicsCapabilities.ConservativeRasterization property

### [820f2c57]
[DX12] Additional work on capability detection 

### [b618b434]
[DX12] Hooked up DXGI device detection 
[Graphics] Added ShaderModel.Model6_7 value for SM 6.7
[Graphics] Added GraphicsApi.DirectX12_1 and .DirectX12_2 values
[DX12] Initial pass on device capability detection

### [44e38c7c]
[DX12] Initial project setup 

### [31f93cb0]
Update README.md 

### [4dcb4e3c]
[Vulkan] Initial pass on display mode detection/retrieval 
[Vulkan] Added DisplayModeVK
[Graphics] Updated IDisplayOutput.GetSupportedModes() to .GetModes()

### [d46dea08]
[DXGI] Several improvements to device detection/initialization 
[DXGI] Improved detection and avoidance of unsupported GPUs
[DX11] Merged DeviceBuilderDX11.CreateDevice() and CreateHeadlessDevice()
[DX11] Initialize intel GPUs with hardware device type flag
[DXGI] Initialize DisplayAdapterDXGI devices before detecting capabilities

### [27cc3933]
[Graphics] Added IDisplayOutput.GetSupportedModes(GraphicsFormat) 
[DXGI] Fixed DisplayOutputDXGI.GetSupportedModes() implementation
[DXGI] Renamed DisplayMode to DisplayModeDXGI
[Graphics] Added DisplayScalingMode enum
[Graphics] Added exposed output display modes via IDisplayMode interface

### [621e7bce]
[Vulkan] DisplayOutputVK now implements IDisplayOutput.DesktopBounds property 

### [38f03212]
[Graphics] Made IDisplayManager an abstract class 
[Graphics] Moved Adapter logging from RenderService to DisplayManager class
[Graphics] DisplayManager indexer no longer abstract
[Vulkan] Moved DisplayManagerVK indexer into DisplayManager base
[DXGI] Moved DisplayManagerDXGI indexer into DisplayManager base

### [328e63d3]
[Vulkan] First pass on display/monitor detection 
[Graphics] Fixed typo in RenderService detected output logging
[Graphics] Removed IDisplayManager.AdaptersWithOutputs property
[DXGI] If all adapters have no outputs, set default to first detected adapter

### [c212e0da]
[Vulkan] Initialize GLFW and its required extensions 

[38aa8618]
[Graphics] Fixed RenderService not setting IDisplayManager.SelectedAdapter 
[Vulkan] Improvements to extension/layer logging
[Vulkan] Hooked up device initialization. Closes #110

### [fd12e48d]
Use DX11 renderer to maintain usability 

### [e4f54881]
[Vulkan] Instantiate DeviceVK in renderer 
[Vulkan] Implemented DisplayManagerVK DeviceID indexer
[Vulkan] Implemented DisplayManagerVK.DefaultAdapter
[Vulkan] Implemented DisplayManagerVK.SelectedAdapter
[Vulkan] Fixed IndexOutOfRange exception when saved DisplayOutputID in GraphicsSettings is invalid

### [2cfd73da]
[Vulkan] Include number of supported timestamp bits in CommandSet detection 
[DX11] Set CommandSet.TimestampBits where appropriate

### [cf675f4f]
[Vulkan] Merged DeviceManager into DeviceVK 
[Vulkan] ExtensionManager now inherits EngineObject
[Vulkan] DeviceVK now inherits ExtensionManager

### [faf2b4f3]
Unsaved changes for ExtensionManager.HasExtension()... Oops! 

### [c7552f9a]
[Vulkan] Simplified ExtensionManager<D> to manage only a single instance 
[Vulkan] Added implicit cast operator for ExtensionManager
[Vulkan] Added ExtensionManager.HasExtension() method

### [b49dca21]
[Vulkan] Abstracted most of InstanceManager into ExtensionManager 
[Vulkan] Fixed VulkanExtension.Unload() not disposing of extension
[Vulkan] Added DeviceManager for managing device-level layers/extensions

### [d3a4d68a]
[Vulkan] Added operator for casting a DisplayAdapterVK to PhysicalDevice 
[Vulkan] Added VulkanVK.Native property

### [10ab12b4]
[Graphics] Refactored how outputs/monitors are accessed via IDisplayAdapter 
[Graphics] Added IDisplayAdapter.Outputs and ActiveOutputs list properties
[Graphics] Removed IDisplayAdapter.GetOutput()
[Graphics] Removed IDisplayAdapter.OutputCount property
[Graphics] Removed IDisplayAdapter.GetActiveOutputs() and GetAttachedOutputs()
[Graphics] Cleaned up adapter logging in RenderService

### [852668d4]
[Graphics] Add logging for detected command sets 

### [d973446a]
[Vulkan] Added dectection of command list/queue support 
[DX11] re-enabled detection of command list and concurrent resource support
[Graphics] Added CommandSetCapabilityFlags
[Graphics] Added GraphicsCapabilities.CommandSets list property
[Graphics] Added GraphicsCapabilities.DeferredCommandLists and ConcurrentResourceCreation properties

### [4f1fed75]
[Vulkan] InstanceManager.Build() now allows the API version to be set 
[Vulkan] Further improvements to layer/extension logging

### [1b9a89f8]
[Vulkan] Refactored layer/extension enumeration 
[Vulkan] Added RendererVK.Enumerate() helper method

### [20e06cc2]
[Vulkan] Cleanup 

### [2c992dd1]
[Vulkan] Refactored layer and extension detection/init 
[Vulkan] Added InstanceManager
[Vulkan] Instance creation starts with a InstanceManager.BeginNew() call
[Vulkan] Extensions are added via InstanceManager.AddExtension() after a BeginNew() call
[Vulkan] Layers are added via InstanceManager.AddLayer() after a BeginNew() call
[Vulkan] Instances are now created via InstanceManager.Build()
[Vulkan] Improvements to layer/extension validation and logging
[Math] Added ByteMath.MinBitValue() and MaxBitValue()
[Vulkan] Added VersionVK helper struct for dealing with UInt32-based version values
[Graphics] Fixed typos in RenderService detected adapter log

### [e07b4b0e]
[Graphics] Refactored display adapter RAM/VRAM detection 
[Graphics] Removed memory properties from IDisplayAdapter
[Graphics] Added memory properties to GraphicsCapabilties
[Graphics] Render service now lists detected adapter type and memory info in logs
[Vulkan] DisplayAdapterVK now implements Adapters and AdaptersWithOutputs properties
[Vulkan] Adapter memory capabilities now detected

### [a0751dec]
[DXGI] Remove obsolete 'id' parameter from DisplayAdapterDXGI constructor 

### [61760102]
[Graphics] Display adapter type is now detected 
[Graphics] Added IDisplayAdapter.Type
[Vulkan] DisplayAdapterVK now populates adapter type
[DXGI] DisplayAdapterDXGI now populates adapter type

### [d7c8f4e8]
[Vulkan] DisplayAdapterVK now populates Name and Vendor properties 
[Graphics] Renamed GraphicsAdapterVendor to DeviceVendor
[Util] Moved DeviceVendor from Molten.Engine to Molten.Utility
[Util] Added EngineUtil.VendorFromPCI() helper method
[Util] Added vulkan vendor IDs to DeviceVendor enum

### [4cff111d]
[Vulkan] Fixed memory error in DisplayManagerVK.Initialize() 
[Vulkan] Fixed invalid pointer in CapabilityBuilder.LogAdditionalProperties()
[Vulkan] Fixed GraphicsCapabilities.SetShaderCap() when setting a Bool32 on a bool property

### [2698893f]
[Vulkan] Fix for crash during DisplayManagerVK initialization 

### [ad21d7df]
[Vulkan] Implemented hook for validation/debug layer messages 

[9e29aa32]
[Graphics] Further improvements to graphics device initialization 
[Graphics] Detected adapter logging moved from DisplayManagerDXGI to RenderService
[Graphics] Moved adapter settings validation into RenderService
[Graphics] Removed obsolete adapter settings validation from DisplayAdapterDXGI
[Graphics] Added IDisplayManager.GetCompatibleAdapters()
[Graphics] Removed IDisplayManager.GetAdapter()
[Graphics] Added IDisplayManager DeviceID indexer
[Graphics] Renamed GraphicsSettings.GraphicsAdapterID to AdapterID
[Graphics] Removed IDisplayManager.GetAdapters()
[Graphics] Removed IDisplayManager.GetAdaptersWithOutputs();
[Graphics] Removed IDisplayManager.AdapterCount property
[Graphics] Added IDisplayManager.Adapters property
[Graphics] Added IDisplayManager.AdaptersWithOutputs property
[Graphics] IDisplayManager.SelectedAdapter and .DefaultAdapter now refer to a IDisplayAdapter
[Graphics] IDisplayAdapter.ID is now a DeviceID value
[Util] Added DeviceID struct with operators for converting to Silk.NET Luid
[Vulkan] Uses PhysicalDeviceProperties2 from Vulkan 1.1 API
[Graphics] Fixed RenderService initialization calling EngineSettings.Apply()
[Graphics] RenderService.Initialize() now only calls Apply() on settings it modifies
Improved handling of non-primitive SettingValue types in Json SettingValueConverter
Engine now calls EngineSettings.Apply() after calling EngineSettings.Load()
SettingBank now tracks nested SettingBank objects
SettingBank.Apply() and .Cancel() now propagate the call to nested SettingBank objects
EngineSettings.Apply() will now apply Graphics, Input, Network, Audio and UI settings too

### [df3e25c6]
[Vulkan] Set API capability to Unsupported if below API 1.0 

### [a123da8e]
[Vulkan] Properly detect max supported API version 

### [837fb2c5]
[Graphics] Complete overhaul of GPU capability/feature detection 
[Graphics] Added IDisplayAdapter.Capabilities property
[DX11] Removed DeviceFeaturesDX11
[DX11] Moved D3D11 device creation to DeviceBuilderDX11
[DX11] Removed DeviceDX11.Features property
[DX11] Added Device.DX11.Adapter property
[DX11] Fixed D3D11 API getting disposed twice - DeviceDX11 and RendererDX11
[Graphics] Merged SamplerCapabilities into GraphicsCapabilities
[Graphics] Added ComputeCapabilities - Inherits ShaderStageCapabilities
[DX11] Improved detection of device buffer limits
[DX11] DirectX 11.1 UAV limit now correctly determined (64)
[DX11] Added helper method DeviceBuilderDX11.CreateHeadlessDevice() - no context init
[DX11] Removed GraphicsComputeFeatures
[Vulkan] Extended functionality of CapabilityBuilder

### [3b942fec]
[DX11] Updated to DirectX 11.4 API 
[DX11] Added GraphicsRasterizerState.ConservativeRaster
[DX11] Added GraphicsRasterizerState.ForcedSampleCount

### [563f9a17]
[DXGI] Upgraded Molten.Graphics.DXGI from DXGI 1.1 to 1.6 API 
[Vulkan] Added CapabilityBuilder class
[Vulkan] Moved GraphicsCapabilities build from DisplayManagerVK to CapabilityBuilder

### [da68c715]
[Graphics] Added missing summaries to ShaderStageCapabilities 
[Vulkan] Added detection of shader precision features

### [1a234b73]
[Graphics] Added precision properties to ShaderStageCapabilities 
[DX11] Improved detection of 10-bit and 16-bit min-precision support

### [4c2c71c1]
[Graphics] Started implementing GPU capability validation 
[Graphics] Added protected method RenderService.OnInitializeApi()
[Graphics] Added GraphicsSettings.MinimumCapabilities
[Graphics] Added GraphicsCapabilities class
[DX11] DeviceFeaturesDX11 now inherits GraphicsCapabilities

### [7fe4ac48]
[DX11] Fixed missing allocation in DeviceDX11.GetDefferedContext() 

### [9295c54e]
[DX11] Renamed nested RenderChain.Link class to standalone RenderChainLink 

### [f6e46e70]
[DX11] Renamed nested RenderChain.Context class to standalone RenderChainContext 

### [77cf641f]
[DX11] Renamed Device to DeviceDX11 

### [ba05e285]
Revert "[DX11] Separated DeviceContext from DeviceDX11" 
This reverts commit c9543b8574bf0382579cf5c1f07cdf7dc6888b63.

### [c9543b85]
[DX11] Separated DeviceContext from DeviceDX11 
[DX11] Renamed Device to DeviceDX11
[DX11] Renamed RenderChain.Context to RenderChainContext

### [a2b10d4f]
[Vulkan] Cleaned up RendererVK.EnableValidationLayers() 

### [70fc8f3e]
[Graphics] Fixed RenderService calling settings.Log() instead of settings.Graphics.Log() 
[Vulkan] Log when a validation layer is successfully enabled

