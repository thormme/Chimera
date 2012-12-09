//-----------------------------------------------------------------------------
// Lighting.fxh
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

float3 xDirLightDirection;
float3 xDirLightDiffuseColor;
float3 xDirLightSpecularColor;
float3 xDirLightAmbientColor;

ColorPair ComputeLights(float3 eyeVector, float3 worldNormal)
{
	float3 halfVectors = normalize(eyeVector - xDirLightDirection);

    float3 dotL = mul(-xDirLightDirection, worldNormal);
    float3 dotH = mul(halfVectors, worldNormal);
    
    float3 zeroL = step(0, dotL);

    float3 diffuse  = zeroL * dotL;
    float3 specular = pow(max(dotH, 0) * zeroL, SpecularPower);

    ColorPair result;
    
    result.Diffuse  = mul(diffuse,  xDirLightDiffuseColor) * DiffuseColor.rgb + EmissiveColor;
    result.Specular = mul(specular, xDirLightSpecularColor) * SpecularColor;

    return result;
}

float DepthBias = 0.001f;

bool ComputeShadow(float4 shadowMapPosition)
{
	float2 shadowTexCoord = 0.5 * shadowMapPosition.xy / shadowMapPosition.w + float2(0.5f, 0.5f);
	shadowTexCoord.y = 1.0f - shadowTexCoord.y;

	if (shadowTexCoord.x < 0.0 || shadowTexCoord.y > 1.0 || shadowTexCoord.y < 0.0 || shadowTexCoord.y > 1.0)
	{
		return false;
	}

	float shadowDepth = SAMPLE_TEXTURE(ShadowMap, shadowTexCoord).r;

	float ourDepth = (shadowMapPosition.z / shadowMapPosition.w) - DepthBias;

	if (shadowDepth < ourDepth)
	{
		return true;
	}

	return false;
}