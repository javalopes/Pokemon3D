
#include "Macros.fxh"

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

TECHNIQUE_PSONLY(Default, DebugShadowMapPixelShader)

float4x4 WorldViewProjection;
float4 Color = float4(0.0f, 1.0f, 0.0f, 1.0f);

struct VertexShaderInput
{
	float4 Position : SV_Position0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
};

VertexShaderOutput LineDrawVS(VertexShaderInput input)
{
	VertexShaderOutput output;
	output.Position = mul(input.Position, WorldViewProjection);

	return output;
}

float4 LineDrawPS(VertexShaderOutput input) : COLOR0
{
	return Color;
}

TECHNIQUE(LineDraw, LineDrawVS, LineDrawPS)