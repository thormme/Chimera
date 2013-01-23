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
	float3 halfVectors = normalize(eyeVector + xDirLightDirection);

    float3 dotL = mul(xDirLightDirection, worldNormal);
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

ShadowData GetShadowData(float4 worldPosition)
{
	ShadowData result;

	float4 texCoords[MAX_CASCADE_COUNT];
	float lightSpaceDepth[MAX_CASCADE_COUNT];

	for (int i = 0; i < xCascadeCount; i++)
	{
		float4 lightSpaceView = mul(worldPosition, xLightView);
		float4 lightSpacePosition = mul(lightSpaceView, xLightProjections[i]);
		texCoords[i] = lightSpacePosition / lightSpacePosition.w;
		lightSpaceDepth[i] = texCoords[i].z;
	}

	result.TexCoords_0_1 = float4(texCoords[0].xy, texCoords[1].xy);
	result.TexCoords_2_3 = float4(texCoords[2].xy, texCoords[3].xy);
	result.LightSpaceDepth_0_3 = float4(lightSpaceDepth[0], lightSpaceDepth[1], lightSpaceDepth[2], lightSpaceDepth[3]);

	return result;
}

ShadowSplitData GetSplitData(ShadowData shadowData)
{
	float2 shadowTexCoords[MAX_CASCADE_COUNT+1] =
	{
		shadowData.TexCoords_0_1.xy,
		shadowData.TexCoords_0_1.zw,
		shadowData.TexCoords_2_3.xy,
		shadowData.TexCoords_2_3.zw,
		float2(0,0)
	};

	float lightSpaceDepths[MAX_CASCADE_COUNT] =
	{
		shadowData.LightSpaceDepth_0_3.x,
		shadowData.LightSpaceDepth_0_3.y,
		shadowData.LightSpaceDepth_0_3.z,
		shadowData.LightSpaceDepth_0_3.w
	};

	for (int i = 0; i < xCascadeCount; i++)
	{
		if (shadowTexCoords[i].x >= xCascadeBufferBounds[i].x &&
			shadowTexCoords[i].x <= xCascadeBufferBounds[i].y &&
			shadowTexCoords[i].y >= xCascadeBufferBounds[i].z &&
			shadowTexCoords[i].y <= xCascadeBufferBounds[i].w)
		{
			ShadowSplitData result;
			result.TexCoords = shadowTexCoords[i];
			result.LightSpaceDepth = lightSpaceDepths[i];
			result.SplitIndex = i;
			result.Color = xCascadeColors[i];

			return result;
		}
	}

	ShadowSplitData result = { float2(0,0), 0, xCascadeCount, float4(0,0,0,1) };
	return result;
}

bool IsInShadow(float4 shadowMapPosition)
{
	float2 shadowTexCoord = 0.5 * shadowMapPosition.xy / shadowMapPosition.w + float2(0.5f, 0.5f);
	shadowTexCoord.y = 1.0f - shadowTexCoord.y;

	if (shadowTexCoord.x < 0.0 || shadowTexCoord.y > 1.0 || shadowTexCoord.y < 0.0 || shadowTexCoord.y > 1.0)
	{
		return false;
	}

	float shadowDepth = SAMPLE_TEXTURE(ShadowMap, shadowTexCoord).r;
	float ourDepth = (shadowMapPosition.z / shadowMapPosition.w) - DepthBias;

	return (shadowDepth < ourDepth);

	//ShadowSplitData splitData = GetSplitData(shadowData);
	//float shadowDepth = SAMPLE_TEXTURE(ShadowMap, splitData.TexCoords).r;

	//return splitData.Color;
	//if (splitData.SplitIndex == xCascadeCount)
	//{
	//	return false;
	//}
	//return (shadowDepth < splitData.LightSpaceDepth);
}