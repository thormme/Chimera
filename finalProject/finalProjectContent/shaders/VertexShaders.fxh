float4x4 lightWorld;
float4x4 lightViewProjection;

ShadowVSOutput ShadowCastVS(VSInput vin)
{
	ShadowVSOutput output;

	output.PositionPS = mul(vin.Position, mul(lightWorld, lightViewProjection));
	output.Depth = output.PositionPS.zw;

	return output;
}

ShadowVSOutput TerrainShadowCastVS(TerrainVSInput vin)
{
	ShadowVSOutput output;

	output.PositionPS = mul(vin.Position, mul(lightWorld, lightViewProjection));
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
    
    output.TexCoord = vin.TexCoord;

	return output;
}

ShadowVSOutput SkinnedShadowCastVS(VSInput vin)
{
	Skin(vin);
	return ShadowCastVS(vin);
}

CelVSOutput SkinnedCelShadeVS(VSInput vin)
{
	Skin(vin);
	return CelShadeVS(vin);
}

OutlineVSOutput SkinnedOutlineVS(VSInput vin)
{
	Skin(vin);
	return OutlineVS(vin);
}

PhongVSOutput SkinnedPhongVS(VSInput vin)
{
	Skin(vin);
	return PhongVS(vin);
}

CelVSOutput TerrainCelShadeVS(TerrainVSInput vin)
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

	output.TexCoord = vin.TexCoord;

	return output;
}

OutlineVSOutput TerrainOutlineVS(TerrainVSInput vin)
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

PhongVSOutput TerrainPhongVS(TerrainVSInput vin)
{
	PhongVSOutput output;

	float4 pos_ws = mul(vin.Position, World);
    float3 eyeVector = normalize(EyePosition - pos_ws.xyz);
    float3 worldNormal = normalize(mul(vin.Normal, WorldInverseTranspose));

    ColorPair lightResult = ComputeLights(eyeVector, worldNormal);
    
    output.PositionPS = mul(vin.Position, WorldViewProj);
    output.Diffuse = float4(lightResult.Diffuse, DiffuseColor.a);
    output.Specular = float4(lightResult.Specular, ComputeFogFactor(vin.Position));
    
    output.TexCoord = vin.TexCoord;

	return output;
}