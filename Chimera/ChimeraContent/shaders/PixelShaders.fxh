///////////////// Helper Methods //////////////////

float4 CompositeTerrainTexture(float2 texCoord)
{
	float4 baseColor         = SAMPLE_TEXTURE(Texture,      texCoord       );
	float4 textureWeights    = SAMPLE_TEXTURE(AlphaMap,     texCoord       );
	float4 redTextureColor   = SAMPLE_TEXTURE(RedTexture,   texCoord * 8.0f);
	float4 greenTextureColor = SAMPLE_TEXTURE(GreenTexture, texCoord * 8.0f);
	float4 blueTextureColor  = SAMPLE_TEXTURE(BlueTexture,  texCoord * 8.0f);
	float4 alphaTextureColor = SAMPLE_TEXTURE(AlphaTexture, texCoord * 8.0f);

	textureWeights.r *= redTextureColor.a;
	textureWeights.g *= greenTextureColor.a;
	textureWeights.b *= blueTextureColor.a;
	textureWeights.a *= alphaTextureColor.a;

	float4 baseRedComp			     = textureWeights.r * redTextureColor   + (1.0 - textureWeights.r) * baseColor;
	float4 baseRedGreenComp			 = textureWeights.g * greenTextureColor + (1.0 - textureWeights.g) * baseRedComp;
	float4 baseRedGreenBlueComp		 = textureWeights.b * blueTextureColor  + (1.0 - textureWeights.b) * baseRedGreenComp;
	float4 baseRedGreenBlueAlphaComp = textureWeights.a * alphaTextureColor + (1.0 - textureWeights.a) * baseRedGreenBlueComp;

	return float4(baseRedGreenBlueAlphaComp.rgb, 1.0f);
}

float4 CelShadePSHelper(VSOutput pin, float4 textureColor)
{
	float4 color = textureColor;

	float textureWeight = 1.0f - xOverlayColorWeight;
	color.rgb *= textureWeight;
	color.rgb += xOverlayColorWeight * xOverlayColor;

	const float A = 0.3f;
	const float B = 0.6f;
	const float C = 1.0f;

	ShadowPixel shadowPixel = ComputeShadow(pin.Shadow, pin.LightAmount);

	float lightAmount;
	if (shadowPixel.InShadow || pin.LightAmount < A)
	{
		lightAmount = A;
	}
	else if (pin.LightAmount < B)
	{
		lightAmount = B;
	}
	else
	{
		lightAmount = C;
	}

	color *= lightAmount;

	if (xVisualizeCascades == true)
	{
		color = (color + shadowPixel.Color) / 2.0f;
	}

	return color;
}

float4 PhongPSHelper(VSOutput pin, float4 textureColor)
{
	float4 diffuseColor = textureColor;

	float diffuseIntensity = saturate(pin.LightAmount);
		
	float4 diffuse = diffuseIntensity * diffuseColor;

	float4 color = float4(xDirLightAmbientColor, 1.0) + diffuse;

	float textureWeight = 1.0f - xOverlayColorWeight;
	color.rgb *= textureWeight;
	color.rgb += xOverlayColorWeight * xOverlayColor;

	ShadowPixel shadowPixel = ComputeShadow(pin.Shadow, pin.LightAmount);

	if (shadowPixel.InShadow == true)
	{
		color *= 0.3f;
	}

	if (xVisualizeCascades == true)
	{
		color = (color + shadowPixel.Color) / 2.0f;
	}
	 
    return color;
}

sampler NoShade_Sampler = sampler_state
{
	texture = <Texture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = wrap;
	AddressV = wrap;
};

/////////////// Pixel Shaders /////////////////

float4 CelShadePS(VSOutput pin) : SV_Target0
{
	float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord);

	return CelShadePSHelper(pin, color);
}

float4 TerrainCelShadePS(VSOutput pin) : SV_Target0
{
	float4 color = CompositeTerrainTexture(pin.TexCoord);

	return CelShadePSHelper(pin, color);
}

float4 PhongPS(VSOutput pin) : SV_Target0
{
	float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord);
		
	return PhongPSHelper(pin, color);
}

float4 TerrainPhongPS(VSOutput pin) : SV_Target0
{
	float4 color = CompositeTerrainTexture(pin.TexCoord);

	return PhongPSHelper(pin, color);
}

float4 ShadowCastPS(ShadowVSOutput pin) : COLOR
{
	return float4(pin.Depth, 0, 0, 0);
}

float4 NormalDepthPS(float4 color : COLOR0) : COLOR0
{
	return color;
}

float4 NoShadePS(VSOutput pin) : SV_Target0
{
	float4 color = tex2D(NoShade_Sampler, pin.TexCoord);

	float textureWeight = 1.0f - xOverlayColorWeight;
	color.rgb *= textureWeight;
	color.rgb += xOverlayColorWeight * xOverlayColor;

	return color;
}

float4 OutlinePS(OutlineVSOutput pin) : SV_Target0
{
	return float4(0.0, 0.0, 0.0, 1.0);
}
