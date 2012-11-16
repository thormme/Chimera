float4x4 lightViewProjection;

ShadowVSOutput ShadowCastVS(float4 Position: POSITION)
{
	ShadowVSOutput output;

	output.PositionPS = mul(Position, LightWorldViewProj);
	output.Depth = output.PositionPS.z / output.PositionPS.w;

	return output;
}

NormalDepthVSOutput NormalDepthVS(VSInput vin)
{
	NormalDepthVSOutput output;

	output.PositionPS = mul(vin.Position, WorldViewProj);

	float3 worldNormal = normalize(mul(vin.Normal, WorldInverseTranspose));

	// Output color holds the normalized normal vector in rgb and the normalized depth in alpha.
	output.Color.rgb = (worldNormal + 1) / 2;
	output.Color.a   = output.PositionPS.z / output.PositionPS.w;

	return output;
}

VSOutput VS(VSInput vin)
{
	VSOutput output;

	float4 pos_ws = mul(vin.Position, World);
    float3 eyeVector = normalize(EyePosition - pos_ws.xyz);
    float3 worldNormal = normalize(mul(vin.Normal, WorldInverseTranspose));

    ColorPair lightResult = ComputeLights(eyeVector, worldNormal);
    
    output.PositionPS = mul(vin.Position, WorldViewProj);
    output.Diffuse = float4(lightResult.Diffuse, DiffuseColor.a);
	output.Specular = float4(lightResult.Specular, ComputeFogFactor(vin.Position));

	output.LightAmount = dot(worldNormal, xDirLightDirection);

	output.ShadowPosition = mul(vin.Position, LightWorldViewProj);
	output.TexCoord = vin.TexCoord;

	return output;
}

float outlineThickness = 0.25f;

OutlineVSOutput OutlineVS(VSInput vin)
{
	OutlineVSOutput output;

	float3 worldNormal = normalize(mul(vin.Normal, WorldInverseTranspose));
	float4 pos_ws = mul(vin.Position, World);
	float3 eyeVector = normalize(EyePosition - pos_ws.xyz);

	output.PositionPS = vin.Position;

	if (dot(worldNormal, eyeVector) <= 0.0f)
		output.PositionPS += normalize(float4(vin.Normal, 0.0)) * outlineThickness;

	output.PositionPS = mul(output.PositionPS, WorldViewProj);

	return output;
}

ShadowVSOutput SkinnedShadowCastVS(SkinnedVSInput svin)
{
	Skin(svin);
	return ShadowCastVS(svin.Position);
}

VSOutput SkinnedVS(SkinnedVSInput svin)
{
	Skin(svin);
	VSInput vin;
	vin.Position = svin.Position;
	vin.Normal   = svin.Normal;
	vin.TexCoord = svin.TexCoord;
	return VS(vin);
}

OutlineVSOutput SkinnedOutlineVS(SkinnedVSInput svin)
{
	Skin(svin);
	VSInput vin;
	vin.Position = svin.Position;
	vin.Normal   = svin.Normal;
	vin.TexCoord = svin.TexCoord;
	return OutlineVS(vin);
}

NormalDepthVSOutput SkinnedNormalDepthVS(SkinnedVSInput svin)
{
	Skin(svin);
	VSInput vin;
	vin.Position = svin.Position;
	vin.Normal   = svin.Normal;
	vin.TexCoord = svin.TexCoord;
	return NormalDepthVS(vin);
}