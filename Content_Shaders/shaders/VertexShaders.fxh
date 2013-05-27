float4x4 lightViewProjection;

float4 Spaghettify(float4 Position)
{
	float4 offset = float4(xWormholePosition, 1.0) - Position;
	float offsetLength = length(offset);

	if (offsetLength >= xModelWormholeDistance)
	{
		return Position;
	}

	float stretchMagnitude = 1.0f - offsetLength / xMaxWormholeDistance;

	return Position + normalize(offset) * stretchMagnitude * 20.0f;
}

ShadowVSOutput ShadowCastVS(float4 Position: POSITION)
{
	ShadowVSOutput output;

	float4 worldPosition = mul(Position, World);

	if (xIsBeingSpaghettified)
	{
		worldPosition = Spaghettify(worldPosition);
	}

	float4 worldPositionView = mul(worldPosition, xLightView);
	output.PositionPS = mul(worldPositionView, xLightProjection);
	output.Depth = output.PositionPS.z / output.PositionPS.w;

	return output;
}

NormalDepthVSOutput NormalDepthVS(VSInput vin)
{
	NormalDepthVSOutput output;

	float4 positionWS = mul(vin.Position, World);

	if (xIsBeingSpaghettified)
	{
		positionWS = Spaghettify(positionWS);
	}

	output.PositionPS = mul(positionWS, ViewProj);

	float3 worldNormal = normalize(mul(vin.Normal, WorldInverseTranspose));

	// Output color holds the normalized normal vector in rgb and the normalized depth in alpha.
	output.Color.rgb = worldNormal;
	output.Color.a   = output.PositionPS.z / output.PositionPS.w;

	if (output.Color.a < 1)
	{
		output.Color.a = 1;
	}

	return output;
}

VSOutput VS(VSInput vin)
{
	VSOutput output;

	output.PositionWS = mul(vin.Position, World);

	if (xIsBeingSpaghettified)
	{
		output.PositionWS = Spaghettify(output.PositionWS);
	}

    float3 eyeVector = normalize(EyePosition - output.PositionWS.xyz);
    float3 worldNormal = normalize(mul(vin.Normal, WorldInverseTranspose));

    ColorPair lightResult = ComputeLights(eyeVector, worldNormal);
    
    output.PositionPS = mul(output.PositionWS, ViewProj);
    output.Diffuse = float4(lightResult.Diffuse, DiffuseColor.a);
	output.Specular = float4(lightResult.Specular, ComputeFogFactor(vin.Position));

	output.LightAmount = dot(worldNormal, -xDirLightDirection);

	output.TexCoord = vin.TexCoord + xTextureOffset;

	return output;
}

VSOutputWithShadows VSWithShadows(VSInput vin)
{
	VSOutputWithShadows output;
	VSOutput vout = VS(vin);

	output.PositionWS  = vout.PositionWS;
	output.PositionPS  = vout.PositionPS;
	output.Diffuse     = vout.Diffuse;
	output.Specular    = vout.Specular;
	output.LightAmount = vout.LightAmount;
	output.TexCoord    = vout.TexCoord;

	output.Shadow = GetShadowData(output.PositionWS);

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

	output.PositionPS = mul(pos_ws, ViewProj);

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

VSOutputWithShadows SkinnedVSWithShadows(SkinnedVSInput svin)
{
	Skin(svin);
	VSInput vin;
	vin.Position = svin.Position;
	vin.Normal   = svin.Normal;
	vin.TexCoord = svin.TexCoord;
	return VSWithShadows(vin);
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