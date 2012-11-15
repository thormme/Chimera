float4x4 lightViewProjection;

ShadowVSOutput ShadowCastVS(float4 Position: POSITION)
{
	ShadowVSOutput output;

	output.PositionPS = mul(Position, LightWorldViewProj);
	output.Depth = output.PositionPS.z / output.PositionPS.w;

	return output;
}

CelVSOutput CelShadeVS(VSInput vin)
{
	CelVSOutput output;

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

PhongVSOutput PhongVS(VSInput vin)
{
	PhongVSOutput output;

	float4 pos_ws = mul(vin.Position, World);
    float3 eyeVector = normalize(EyePosition - pos_ws.xyz);
    float3 worldNormal = normalize(mul(vin.Normal, WorldInverseTranspose));

    ColorPair lightResult = ComputeLights(eyeVector, worldNormal);
    
    output.PositionPS = mul(vin.Position, WorldViewProj);
    output.Diffuse = float4(lightResult.Diffuse, DiffuseColor.a);
    output.Specular = float4(lightResult.Specular, ComputeFogFactor(vin.Position));
    
	output.ShadowPosition = mul(vin.Position, LightWorldViewProj);
    output.TexCoord = vin.TexCoord;

	return output;
}

ShadowVSOutput SkinnedShadowCastVS(SkinnedVSInput svin)
{
	Skin(svin);
	return ShadowCastVS(svin.Position);
}

CelVSOutput SkinnedCelShadeVS(SkinnedVSInput svin)
{
	Skin(svin);
	VSInput vin;
	vin.Position = svin.Position;
	vin.Normal   = svin.Normal;
	vin.TexCoord = svin.TexCoord;
	return CelShadeVS(vin);
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

PhongVSOutput SkinnedPhongVS(SkinnedVSInput svin)
{
	Skin(svin);
	VSInput vin;
	vin.Position = svin.Position;
	vin.Normal   = svin.Normal;
	vin.TexCoord = svin.TexCoord;
	return PhongVS(vin);
}
