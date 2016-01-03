texture DepthMap;
sampler2D DepthMapSampler = sampler_state {
	Texture = (DepthMap);
	MagFilter = Point;
	MinFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

float4 DebugShadowMapPixelShader(float4 position : SV_Position, float4 color : COLOR0, float2 tex_coord : TEXCOORD0) : SV_Target0
{
	float4 depth = tex2D(DepthMapSampler, tex_coord);
	return float4(depth.r, depth.r, depth.r, 1.0f);
}

technique Default
{
	pass Pass1
	{
		PixelShader = compile ps_4_0 DebugShadowMapPixelShader();
	}
}

float4x4 WorldViewProjection;

struct VertexShaderInput
{
	float4 Position : SV_Position0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float4 Color : COLOR0;
};

VertexShaderOutput LineDrawVS(VertexShaderInput input)
{
	VertexShaderOutput output;
	output.Position = mul(input.Position, WorldViewProjection);
	output.Color = input.Color;

	return output;
}

float4 LineDrawPS(VertexShaderOutput input) : COLOR0
{
	return input.Color;
}

technique LineDraw
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 LineDrawVS();
		PixelShader = compile ps_4_0 LineDrawPS();
	}
}