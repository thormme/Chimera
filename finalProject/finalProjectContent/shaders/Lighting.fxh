//-----------------------------------------------------------------------------
// Lighting.fxh
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

float3 xDirLightDirection;
float3 xDirLightDiffuseColor;
float3 xDirLightSpecularColor;

ColorPair ComputeLights(float3 eyeVector, float3 worldNormal)
{
	float3 halfVectors = normalize(eyeVector - xDirLightDirection);

    float3 dotL = mul(-xDirLightDirection, worldNormal);
    float3 dotH = mul(halfVectors, worldNormal);
    
    float3 zeroL = step(0, dotL);

    float3 diffuse  = zeroL * dotL;
    float3 specular = pow(max(dotH, 0) * zeroL, SpecularPower);

    ColorPair result;
    
    result.Diffuse  = mul(diffuse,  xDirLightDiffuseColor)  * DiffuseColor.rgb + EmissiveColor;
    result.Specular = mul(specular, xDirLightSpecularColor) * SpecularColor;

    return result;
}