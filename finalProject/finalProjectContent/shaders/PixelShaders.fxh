float4 ShadowCastPS(ShadowVSOutput pin) : COLOR
{
	return float4(pin.Depth, 0, 0, 0);
}

float4 NormalDepthPS(float4 color : COLOR0) : COLOR0
{
	return color;
}

float discretePallete = 0.1f;

float4 CelShadePS(VSOutput pin) : SV_Target0
{
	float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord);

	float textureWeight = 1.0f - xOverlayColorWeight;
	color.rgb *= textureWeight;
	color.rgb += xOverlayColorWeight * xOverlayColor;

	color.rgba -= (color.rgba % discretePallete);

	if (xNumShadowBands < 2.9f)
	{
		if (pin.LightAmount <= 0.0f)
		{
			color *= 0.5f;
		}
	}
	else
	{
		if (pin.LightAmount <= 0.66f && pin.LightAmount > 0.0f)
		{
			color *= 0.75f;
		}
		else if (pin.LightAmount <= 0.0f)
		{
			color *= 0.5f;
		}
	}

	if (IsInShadow(pin.ShadowCoord))
	{
		color *= 0.5f;
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

float4 PhongPS(VSOutput pin) : SV_Target0
{
	float4 diffuseColor = SAMPLE_TEXTURE(Texture, pin.TexCoord);
		
	float diffuseIntensity = saturate(pin.LightAmount);
		
	float4 diffuse = diffuseIntensity * diffuseColor;

	float4 color = float4(xDirLightAmbientColor, 1.0) + diffuse;

	float textureWeight = 1.0f - xOverlayColorWeight;
	color.rgb *= textureWeight;
	color.rgb += xOverlayColorWeight * xOverlayColor;

	if (IsInShadow(pin.ShadowCoord))
	{	
		color *= 0.5f;
	}
	 
    return color;
}

float4 TerrainCelShadePS(VSOutput pin) : SV_Target0
{
	float4 color = PhongPS(pin);

	float textureWeight = 1.0f - xOverlayColorWeight;
	color.rgb *= textureWeight;
	color.rgb += xOverlayColorWeight * xOverlayColor;
	//color.rgba -= (color.rgba % discretePallete);

	return color;
}