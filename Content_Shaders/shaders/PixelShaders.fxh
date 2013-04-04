///////////////// Helper Methods //////////////////

float4 CompositeTerrainTexture(float2 texCoord)
{
	float4 textureWeights    = SAMPLE_TEXTURE(AlphaMap,     texCoord       );

	float2 baseUV = Texture_uvOffset + float2(texCoord.r * Texture_uvScale.r, texCoord.g * Texture_uvScale.g);
	float4 baseColor         = SAMPLE_TEXTURE(Texture,      baseUV       );

	float2 redUV = RedTexture_uvOffset + float2(texCoord.r * RedTexture_uvScale.r, texCoord.g * RedTexture_uvScale.g);
	float4 redTextureColor   = SAMPLE_TEXTURE(RedTexture,   redUV);

	float2 greenUV = GreenTexture_uvOffset + float2(texCoord.r * GreenTexture_uvScale.r, texCoord.g * GreenTexture_uvScale.g);
	float4 greenTextureColor = SAMPLE_TEXTURE(GreenTexture, greenUV);

	float2 blueUV = BlueTexture_uvOffset + float2(texCoord.r * BlueTexture_uvScale.r, texCoord.g * BlueTexture_uvScale.g);
	float4 blueTextureColor  = SAMPLE_TEXTURE(BlueTexture,  blueUV);

	float2 alphaUV = AlphaTexture_uvOffset + float2(texCoord.r * AlphaTexture_uvScale.r, texCoord.g * AlphaTexture_uvScale.g);
	float4 alphaTextureColor = SAMPLE_TEXTURE(AlphaTexture, alphaUV);

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

bool DrawCursor(float4 positionWS)
{
	switch (xDrawCursor)
	{
		case 1:
		{
			float radius = length(float2(xCursorPosition.r - positionWS.r, xCursorPosition.b - positionWS.b));
			return radius <= xCursorOuterRadius && radius >= xCursorInnerRadius;
		}
		case 2:
		{
			float minXOut = xCursorPosition.r - xCursorOuterRadius, maxXOut = xCursorPosition.r + xCursorOuterRadius;
			float minZOut = xCursorPosition.b - xCursorOuterRadius, maxZOut = xCursorPosition.b + xCursorOuterRadius;

			float minXIn = xCursorPosition.r - xCursorInnerRadius, maxXIn = xCursorPosition.r + xCursorInnerRadius;
			float minZIn = xCursorPosition.b - xCursorInnerRadius, maxZIn = xCursorPosition.b + xCursorInnerRadius;

			bool insideOuter = positionWS.r >= minXOut &&
							   positionWS.r <= maxXOut &&
							   positionWS.b >= minZOut &&
							   positionWS.b <= maxZOut;

			bool insideInner = positionWS.r >= minXIn &&
							   positionWS.r <= maxXIn &&
							   positionWS.b >= minZIn &&
							   positionWS.b <= maxZIn;

			return insideOuter && !insideInner;
		}
	}

	return false;
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
	float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord + xTextureOffset);

	return CelShadePSHelper(pin, color);
}

float4 TerrainCelShadePS(VSOutput pin) : SV_Target0
{
	if (DrawCursor(pin.PositionWS))
	{
		return float4(0, 1, 0, 1);
	}

	float4 color = CompositeTerrainTexture(pin.TexCoord + xTextureOffset);

	return CelShadePSHelper(pin, color);
}

float4 PhongPS(VSOutput pin) : SV_Target0
{
	float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord + xTextureOffset);
		
	return PhongPSHelper(pin, color);
}

float4 TerrainPhongPS(VSOutput pin) : SV_Target0
{
	if (DrawCursor(pin.PositionWS))
	{
		return float4(0, 1, 0, 1);
	}

	float4 color = CompositeTerrainTexture(pin.TexCoord + xTextureOffset);

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
	float4 color = tex2D(NoShade_Sampler, pin.TexCoord + xTextureOffset);

	float textureWeight = 1.0f - xOverlayColorWeight;
	color.rgb *= textureWeight;
	color.rgb += xOverlayColorWeight * xOverlayColor;

	return color;
}

float4 OutlinePS(OutlineVSOutput pin) : SV_Target0
{
	return float4(0.0, 0.0, 0.0, 1.0);
}
