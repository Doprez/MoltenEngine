float4x4 wvp;

TextureCube mapSky;
SamplerState skySampler;

struct PS_IN
{
	float4 Pos : SV_POSITION;
	float3 uv : TEXCOORD;
};

PS_IN VS(float3 pos : POSITION)
{
  PS_IN output = (PS_IN)0;

  //Output xyww instead of xyzw to ensure Z is always 1, the most distant from the camera.
  output.Pos = mul(float4(pos, 1.0f), wvp);
  output.uv = pos;

  return output;
}


float4 PS(PS_IN input) : SV_Target
{
  return mapSky.Sample(skySampler, input.uv);
}