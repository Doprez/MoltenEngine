#include "gbuffer_common.hlsl"
#include "light_common.hlsl"

struct HS_OUTPUT
{
	float3 HemiDir : POSITION;
	float LightID : TEXCOORD0;
};

struct DS_OUTPUT
{
	float4 Position : SV_POSITION;
	float3 PositionXYW : TEXCOORD0;
	uint LightID : BLENDINDICES;
};

/////////////////////////////////////////////////////////////////////////////
// Hull shader
/////////////////////////////////////////////////////////////////////////////

// Custom point light constant function
HS_CONSTANT_DATA_OUTPUT PointConstantHS(uint PatchID : SV_PrimitiveID)
{
	HS_CONSTANT_DATA_OUTPUT Output;

	uint id = floor(0.5 * PatchID); // Light ID (2 patches per light)
	Output.Edges[0] = Output.Edges[1] = Output.Edges[2] = Output.Edges[3] = LightData[id].Tess;
	Output.Inside[0] = Output.Inside[1] = LightData[id].Tess;

	return Output;
}


static const float3 HemilDir[2] = {
	float3(1.0, 1.0, 1.0),
	float3(-1.0, 1.0, -1.0)
};

[domain("quad")]
[partitioning("integer")]
[outputtopology("triangle_ccw")]
[outputcontrolpoints(4)]
[patchconstantfunc("PointConstantHS")]
HS_OUTPUT HS(uint PatchID : SV_PrimitiveID)
{
	HS_OUTPUT Output;

	uint hemiID = min(PatchID, fmod(PatchID, 2)); // use the remainder as the hemiDir ID (or the patchID if it's less than 2).

	Output.LightID = floor(0.5 * PatchID);
	Output.HemiDir = HemilDir[hemiID];
	return Output;

}

/////////////////////////////////////////////////////////////////////////////
// Domain Shader shader
/////////////////////////////////////////////////////////////////////////////
[domain("quad")]
DS_OUTPUT DS(HS_CONSTANT_DATA_OUTPUT input, float2 UV : SV_DomainLocation, const OutputPatch<HS_OUTPUT, 4> quad)
{
	// Transform the UV's into clip-space
	float2 posClipSpace = UV.xy * 2.0 - 1.0;

	// Find the absulate maximum distance from the center
	float2 posClipSpaceAbs = abs(posClipSpace.xy);
	float maxLen = max(posClipSpaceAbs.x, posClipSpaceAbs.y);

	// Generate the final position in clip-space
	float3 normDir = normalize(float3(posClipSpace.xy, (maxLen - 1.0)) * quad[0].HemiDir);
	float4 posLS = float4(normDir.xyz, 1.0);

	// Transform all the way to projected space
	DS_OUTPUT Output;
	Output.LightID = quad[0].LightID;
	Output.Position = mul(posLS, LightData[Output.LightID].Transform);

	// Store the clip space position
	Output.PositionXYW = Output.Position.xyw;

	return Output;
}

/////////////////////////////////////////////////////////////////////////////
// Pixel shader
/////////////////////////////////////////////////////////////////////////////
cbuffer cbLight : register(b4)
{
	float3 cameraPosition			: packoffset(c0);
}

float4 PS(DS_OUTPUT input) : SV_TARGET
{
	MATERIAL g = UnpackGBuffer(input.PositionXYW);
  uint lightID = input.LightID;
  
  float3 lightDir = normalize(LightData[lightID].Position - g.worldPos.xyz);
  float3 diffuse = max(0, dot(lightDir, g.normal)) * LightData[lightID].Color;
  
  float dist = distance(LightData[lightID].Position, g.worldPos.xyz);
  float quadAtten = 1.0 / (dist * dist);
  float atten = max(0.0, quadAtten - LightData[lightID].RangeRcp);
  
  return float4(diffuse * atten, 1);
}

float4 PS_Debug() : SV_TARGET
{
	return float4(1.0, 1.0, 1.0, 1.0);
}