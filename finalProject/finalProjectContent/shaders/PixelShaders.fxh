float4 ShadowCastPS(ShadowVSOutput pin) : COLOR
{
	return float4(pin.Depth, 0, 0, 0);
}

float discretePallete = 0.1f;

float4 CelShadePS(CelVSOutput pin) : SV_Target0
{
	float4 color = SAMPLE_TEXTURE(Texture, pin.TexCoord);
	color.rgba -= (color.rgba % discretePallete);

	if (pin.LightAmount <= 0.0f || ComputeShadow(pin.ShadowPosition))
		color -= float4(0.25, 0.25, 0.25, 0.0);

	return color;
}

float4 OutlinePS(OutlineVSOutput pin) : SV_Target0
{
	return float4(0.0, 0.0, 0.0, 1.0);
}

float4 PhongPS(PhongVSOutput pin) : SV_Target0
{
	float4 color = float4(0.0, 0.0, 0.0, 1.0);
	if (!ComputeShadow(pin.ShadowPosition))
	{
		color = SAMPLE_TEXTURE(Texture, pin.TexCoord) * pin.Diffuse;
	}

	AddSpecular(color, pin.Specular.rgb);
    ApplyFog(color, pin.Specular.w);
    
    return color;
}
