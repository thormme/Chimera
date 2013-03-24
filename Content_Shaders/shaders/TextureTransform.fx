#include "macros.fxh"

DECLARE_TEXTURE(Texture, 0);

float2 UVOffset;
float2 UVScale;

float4 ScalePS(float2 texCoord : TEXCOORD0) : COLOR0
{
	float2 transformedTexCoord = UVOffset + float2(texCoord.r * UVScale.r, texCoord.g * UVScale.g);
	float3 textureColor = SAMPLE_TEXTURE(Texture, transformedTexCoord);

	return float4(textureColor, 1.0);
}

technique Scale
{
	Pass
	{
		PixelShader = compile ps_2_0 ScalePS();
	}
}