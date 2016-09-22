
#include "macros.fxh"

//Parameters for normal rendering.
float4x4 World;
float4x4 WorldLight;
float4x4 View;
float4x4 Projection;
float2 TexcoordOffset;
float2 TexcoordScale;
float4 MaterialColor = float4(1,1,1,1);

//Parameters for shadow mapping.
float4x4 LightViewProjection;
texture DiffuseTexture;
texture ShadowMap;
float ShadowScale = 1.0f / 1024.0f;

//parameters for lighting
float3 LightDirection = float3(1, -1,  -1);
float4 AmbientLight;
float AmbientIntensity;
float DiffuseIntensity;

sampler2D DiffuseSampler = sampler_state {
	Texture = (DiffuseTexture);
	MagFilter = Point;
	MinFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D ShadowMapSampler = sampler_state {
	Texture = (ShadowMap);
	MagFilter = Point;
	MinFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler2D LinearSampler = sampler_state {
	Texture = (DiffuseTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Clamp;
	AddressV = Clamp;
};

struct VertexShaderInput
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : TEXCOORD1;
};

struct VertexShaderShadowReceiverInput
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : NORMAL0;
};

struct VertexShaderShadowReceiverOutput
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : TEXCOORD1;
	float4 LightPosition : TEXCOORD2;
};

//Calculates light equatation for diffuse and ambient with shadow factor.
float4 CalculateAmbientDiffuseLighting(float3 normal, float3 lightDir, float shadowFactor)
{
	float diffuseFactor = saturate(dot(normalize(normal), normalize(-lightDir)));
	float4 diffuseColor = float4(1, 1, 1, 1) * diffuseFactor * DiffuseIntensity;
	float4 ambientColor = AmbientLight * AmbientIntensity;

	return ambientColor + diffuseColor * shadowFactor;
}

//calculates a shadow factor based on light position.
float CalculateShadowFactor(float4 lightPosition)
{
	float2 projectedTexCoords;
	projectedTexCoords[0] = lightPosition.x / lightPosition.w / 2.0f + 0.5f;
	projectedTexCoords[1] = -lightPosition.y / lightPosition.w / 2.0f + 0.5f;

	float shadowFactor = 1.0f;

	if ((saturate(projectedTexCoords).x == projectedTexCoords.x) && (saturate(projectedTexCoords).y == projectedTexCoords.y))
	{
		float depthStoredInShadowMap = tex2D(ShadowMapSampler, projectedTexCoords).r;
		float realDistance = lightPosition.z / lightPosition.w;
		if ((realDistance - 2.0f*ShadowScale) > depthStoredInShadowMap)
		{
			shadowFactor = 0.5f;
		}
	}

	return shadowFactor;
}

//calculates a shadow factor based on light position.
float CalculateShadowFactorPCF(float4 lightPosition)
{
	float2 projectedTexCoords;
	projectedTexCoords[0] = lightPosition.x / lightPosition.w / 2.0f + 0.5f;
	projectedTexCoords[1] = -lightPosition.y / lightPosition.w / 2.0f + 0.5f;

	const int range = 2;
	const float samples = (range*2.0f + 1.0f)*(range*2.0f + 1.0f);

	float inShadowCount = samples;

	if ((saturate(projectedTexCoords).x == projectedTexCoords.x) && (saturate(projectedTexCoords).y == projectedTexCoords.y))
	{
		float realDistance = lightPosition.z / lightPosition.w;
		float2 currentTexcoords = projectedTexCoords;
		float depthStoredInShadowMap;

		if ((realDistance - 2.0f*ShadowScale) > tex2D(ShadowMapSampler, currentTexcoords).x)
		{
			[unroll]
			for (int i = -range; i <= range; i++)
			{
				[unroll]
				for (int j = -range; j <= range; j++)
				{
					currentTexcoords = projectedTexCoords;
					currentTexcoords[0] += (i * ShadowScale);
					currentTexcoords[1] += (j * ShadowScale);

					if ((realDistance - 2.0f*ShadowScale) > tex2D(ShadowMapSampler, currentTexcoords).x)
					{
						inShadowCount--;
					}
				}
			}
		}
	}

	return 0.5f + (inShadowCount / samples) * 0.5f;
}

//Modulates two colors and preserves alpha value from source color. this is important for transparent objects.
float4 ModulatePreserveAlpha(float4 perserveColor, float4 modulate)
{
	float alpha = perserveColor.a;
	float4 resultColor = perserveColor * modulate;
	resultColor.a = alpha;
	return resultColor;
}

float2 TransformTexcoord(float2 texcoord)
{
	return TexcoordOffset + float2(texcoord.x * TexcoordScale.x, texcoord.y * TexcoordScale.y);
}

VertexShaderOutput LitVS(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, (float3x3)World);
	output.TexCoord = TransformTexcoord(input.TexCoord);

	return output;
}

float4 LitPS(VertexShaderOutput input) : COLOR0
{
	float4 colorFromTexture = tex2D(DiffuseSampler, input.TexCoord);
	float4 lightColor = CalculateAmbientDiffuseLighting(input.Normal, LightDirection, 1.0f);

	return ModulatePreserveAlpha(colorFromTexture, lightColor);
}

VertexShaderOutput LitNoTextureVS(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, (float3x3)World);
	output.TexCoord = float2(0,0);

	return output;
}

float4 LitNoTexturePS(VertexShaderOutput input) : COLOR0
{
	float4 lightColor = CalculateAmbientDiffuseLighting(input.Normal, LightDirection, 1.0f);

	return ModulatePreserveAlpha(MaterialColor, lightColor);
}

VertexShaderShadowReceiverOutput LitShadowReceiverVS(VertexShaderShadowReceiverInput input)
{
	VertexShaderShadowReceiverOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, (float3x3)World);
	output.TexCoord = TransformTexcoord(input.TexCoord);

	float4 lightWorldPos = mul(input.Position, WorldLight);
	output.LightPosition = mul(lightWorldPos, LightViewProjection);

	return output;
}

VertexShaderShadowReceiverOutput LitNoTextureShadowReceiverVS(VertexShaderShadowReceiverInput input)
{
	VertexShaderShadowReceiverOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, (float3x3)World);
	output.TexCoord = float2(0, 0);

	float4 lightWorldPos = mul(input.Position, WorldLight);
	output.LightPosition = mul(lightWorldPos, LightViewProjection);

	return output;
}

float4 LitShadowReceiverPS(VertexShaderShadowReceiverOutput input) : COLOR0
{
	float shadowFactor = CalculateShadowFactor(input.LightPosition);
	float4 lightColor = CalculateAmbientDiffuseLighting(input.Normal, LightDirection, shadowFactor);
	float4 colorFromTexture = tex2D(DiffuseSampler, input.TexCoord);

	return ModulatePreserveAlpha(colorFromTexture, lightColor);
}

float4 LitNoTextureShadowReceiverPS(VertexShaderShadowReceiverOutput input) : COLOR0
{
	float shadowFactor = CalculateShadowFactor(input.LightPosition);
	float4 lightColor = CalculateAmbientDiffuseLighting(input.Normal, LightDirection, shadowFactor);

	return ModulatePreserveAlpha(MaterialColor, lightColor);
}

float4 LitShadowReceiverPCFPS(VertexShaderShadowReceiverOutput input) : COLOR0
{
	float shadowFactor = CalculateShadowFactorPCF(input.LightPosition);
	float4 lightColor = CalculateAmbientDiffuseLighting(input.Normal, LightDirection, shadowFactor);
	float4 colorFromTexture = tex2D(DiffuseSampler, input.TexCoord);

	return ModulatePreserveAlpha(colorFromTexture, lightColor);
}

float4 LitNoTextureShadowReceiverPCFPS(VertexShaderShadowReceiverOutput input) : COLOR0
{
	float shadowFactor = CalculateShadowFactorPCF(input.LightPosition);
	float4 lightColor = CalculateAmbientDiffuseLighting(input.Normal, LightDirection, shadowFactor);

	return ModulatePreserveAlpha(MaterialColor, lightColor);
}

TECHNIQUE(Lit, LitVS, LitPS)
TECHNIQUE(LitNoTexture, LitNoTextureVS, LitNoTexturePS)
TECHNIQUE(LitShadowReceiver, LitShadowReceiverVS, LitShadowReceiverPS)
TECHNIQUE(LitNoTextureShadowReceiver, LitNoTextureShadowReceiverVS, LitNoTextureShadowReceiverPS)
TECHNIQUE(LitShadowReceiverPCF, LitShadowReceiverVS, LitShadowReceiverPCFPS)
TECHNIQUE(LitNoTextureShadowReceiverPCF, LitNoTextureShadowReceiverVS, LitNoTextureShadowReceiverPCFPS)

//----------------------------------------------------------------------------------------
//Shadow Caster Shader Code. Creates Shadow map.
//----------------------------------------------------------------------------------------

struct VSInputShadowCaster
{
	float4 Position : POSITION;
};

struct VSOutputShadowReceiver
{
	float4 Position : POSITION;
	float4 DepthPosition : TEXCOORD0;
};

VSOutputShadowReceiver ShadowCasterVS(VSInputShadowCaster input)
{
	VSOutputShadowReceiver output;

	input.Position.w = 1.0f;

	float4 worldPosition = mul(input.Position, WorldLight);
	output.Position = mul(worldPosition, LightViewProjection);
	output.DepthPosition = output.Position;

	return output;
}

float4 ShadowCasterPS(VSOutputShadowReceiver input) : SV_TARGET
{
	float depthValue = input.DepthPosition.z / input.DepthPosition.w;
	return float4(depthValue, depthValue, depthValue, 1.0f);
}

struct VSInputShadowCasterTransparent
{
	float4 Position : POSITION;
	float2 Texcoord: TEXCOORD0;
};

struct VSOutputShadowReceiverTransparent
{
	float4 Position : POSITION;
	float4 DepthPosition : TEXCOORD0;
	float2 Texcoord : TEXCOORD1;
};


VSOutputShadowReceiverTransparent ShadowCasterTransparentVS(VSInputShadowCasterTransparent input)
{
	VSOutputShadowReceiverTransparent output;

	input.Position.w = 1.0f;

	float4 worldPosition = mul(input.Position, WorldLight);
	output.Position = mul(worldPosition, LightViewProjection);
	output.DepthPosition = output.Position;
	output.Texcoord = TransformTexcoord(input.Texcoord);

	return output;
}

float4 ShadowCasterTransparentPS(VSOutputShadowReceiverTransparent input) : SV_TARGET
{
	float alpha = tex2D(DiffuseSampler, input.Texcoord).a;

	if (alpha < 0.001f)
	{
		discard;
		return float4(0, 0, 0, 0);
	}
	float depthValue = input.DepthPosition.z / input.DepthPosition.w;
	return float4(depthValue, depthValue, depthValue, 1.0f);
}

TECHNIQUE(ShadowCaster, ShadowCasterVS, ShadowCasterPS)
TECHNIQUE(ShadowCasterTransparent, ShadowCasterTransparentVS, ShadowCasterTransparentPS)

//-----------------------------------------------------------------------------
// Unlit
//-----------------------------------------------------------------------------

struct UnlitInputVS
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : NORMAL0;
};

struct UnlitOutputVS
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
	float3 Normal   : TEXCOORD1;
};

UnlitOutputVS UnlitVS(UnlitInputVS input)
{
	UnlitOutputVS output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.Normal = mul(input.Normal, (float3x3)World);
	output.TexCoord = TransformTexcoord(input.TexCoord);

	return output;
}

float4 UnlitPS(UnlitOutputVS input) : COLOR0
{
	return tex2D(DiffuseSampler, input.TexCoord);
}

float4 UnlitNoTexturePS(UnlitOutputVS input) : COLOR0
{
	return MaterialColor;
}

float4 UnlitPSLinearSampled(UnlitOutputVS input) : COLOR0
{
	return tex2D(LinearSampler, input.TexCoord);
}

TECHNIQUE(Unlit, UnlitVS, UnlitPS)
TECHNIQUE(UnlitNoTexture, UnlitVS, UnlitNoTexturePS)
TECHNIQUE(UnlitLinearSampled, UnlitVS, UnlitPSLinearSampled)