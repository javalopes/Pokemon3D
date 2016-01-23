texture SourceMap;
float2 InvScreenSize;

sampler2D SourceMapSampler = sampler_state {
	Texture = (SourceMap);
	MagFilter = Point;
	MinFilter = Point;
	AddressU = Clamp;
	AddressV = Clamp;
};

float4 HorizontalBlurPS(float4 position : SV_Position, float4 color : COLOR0, float2 tex_coord : TEXCOORD0) : SV_Target0
{
	return (tex2D(SourceMapSampler, tex_coord) 
		+ tex2D(SourceMapSampler, tex_coord + float2(InvScreenSize.x,0.0f) * 2.0f)
		+ tex2D(SourceMapSampler, tex_coord + float2(InvScreenSize.x, 0.0f) * 4.0f)
		+ tex2D(SourceMapSampler, tex_coord + float2(InvScreenSize.x, 0.0f) * 6.0f)
		+ tex2D(SourceMapSampler, tex_coord + float2(InvScreenSize.x, 0.0f) * -2.0f)
		+ tex2D(SourceMapSampler, tex_coord + float2(InvScreenSize.x, 0.0f) * -4.0f)
		+ tex2D(SourceMapSampler, tex_coord + float2(InvScreenSize.x, 0.0f) * -6.0f)) / 7.0f;
}

float4 VerticalBlurPS(float4 position : SV_Position, float4 color : COLOR0, float2 tex_coord : TEXCOORD0) : SV_Target0
{
	return (tex2D(SourceMapSampler, tex_coord)
		+ tex2D(SourceMapSampler, tex_coord + float2(0.0f, InvScreenSize.y) * 2.0f)
		+ tex2D(SourceMapSampler, tex_coord + float2(0.0f, InvScreenSize.y) * 4.0f)
		+ tex2D(SourceMapSampler, tex_coord + float2(0.0f, InvScreenSize.y) * 6.0f)
		+ tex2D(SourceMapSampler, tex_coord + float2(0.0f, InvScreenSize.y) * -2.0f)
		+ tex2D(SourceMapSampler, tex_coord + float2(0.0f, InvScreenSize.y) * -4.0f)
		+ tex2D(SourceMapSampler, tex_coord + float2(0.0f, InvScreenSize.y) * -6.0f)) / 7.0f;
}

technique HorizontalBlur
{
	pass Pass1
	{
		PixelShader = compile ps_4_0 HorizontalBlurPS();
	}
}

technique VerticalBlur
{
	pass Pass1
	{
		PixelShader = compile ps_4_0 VerticalBlurPS();
	}
}