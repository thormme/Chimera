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
	color.rgba -= (color.rgba % discretePallete);

	if (pin.LightAmount <= 0.0f || ComputeShadow(pin.ShadowPosition))
	{
		color -= float4(0.35, 0.35, 0.35, 0.0);
	}

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

	if (ComputeShadow(pin.ShadowPosition))
	{	
		color -= float4(0.35, 0.35, 0.35, 0.0);
	}
	else
	{
		AddSpecular(color, pin.Specular.rgb);
    
		ApplyFog(color, pin.Specular.w);
	}
    
    return color;
}

float4 TerrainCelShadePS(VSOutput pin) : SV_Target0
{
	float4 color = PhongPS(pin);
	color.rgba -= (color.rgba % discretePallete);

	return color;
}