//-----------------------------------------------------------------------------
// Using code inspired by PostprocessEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#include "macros.fxh"

// Settings of outline edges.
float EdgeWidth = 1;
float EdgeIntensity = 1;

// Epsilon values for face normals.
float NormalThreshold = 0.1;
float DepthThreshold = 1.25;

// How dark the edges get in response to changes in data.
float NormalSensitivity = 0.3;
float DepthSensitivity = 8;

// Resolution of scene rendered to texture.
float2 ScreenResolution;

// Current scene rendered to texture without any post processing.
DECLARE_TEXTURE(SceneTexture, 0);

// Texture containing normal vector to face at each pixel in RGB and depth in alpha.
DECLARE_TEXTURE(NormalDepthTexture, 1);

// Texture containing outline of scene.
DECLARE_TEXTURE(OutlineTexture, 2);

float4 EdgeDetectPS(float2 texCoord : TEXCOORD0) : COLOR0
{
	// Look up the original color from the main scene.
    float3 scene = SAMPLE_TEXTURE(SceneTexture, texCoord);

	// Grab four values adjacent to texCoord and offset along the four diagonals.
	float2 edgeOffset = EdgeWidth / ScreenResolution;
        
    float4 n1 = SAMPLE_TEXTURE(NormalDepthTexture, texCoord + float2(-1, -1) * edgeOffset);
    float4 n2 = SAMPLE_TEXTURE(NormalDepthTexture, texCoord + float2( 1,  1) * edgeOffset);
    float4 n3 = SAMPLE_TEXTURE(NormalDepthTexture, texCoord + float2(-1,  1) * edgeOffset);
    float4 n4 = SAMPLE_TEXTURE(NormalDepthTexture, texCoord + float2( 1, -1) * edgeOffset);

    // Work out how much the normal and depth values are changing.
    float4 diagonalDelta = abs(n1 - n2) + abs(n3 - n4);

    float normalDelta = dot(diagonalDelta.xyz, 1);
    float depthDelta = diagonalDelta.w;
        
    // Filter out very small changes, in order to produce nice clean results.
    normalDelta = saturate((normalDelta - NormalThreshold) * NormalSensitivity);
    depthDelta = saturate((depthDelta - DepthThreshold) * DepthSensitivity);

    // Does this pixel lie on an edge?
    float edgeAmount = saturate(normalDelta + depthDelta) * EdgeIntensity;
        
    // Apply the edge detection result to the main scene color.
    scene *= (1 - edgeAmount);

	return float4(edgeAmount, edgeAmount, edgeAmount, 1.0);
}

float4 CompositePS(float2 texCoord : TEXCOORD0) : COLOR0
{
	float3 sceneColor = SAMPLE_TEXTURE(SceneTexture, texCoord);

	float3 outline = SAMPLE_TEXTURE(OutlineTexture, texCoord);

	return float4(sceneColor * (1 - outline.r), 1.0);
}

technique EdgeDetect
{
	Pass
	{
		PixelShader = compile ps_2_0 EdgeDetectPS();
	}
}

technique Composite
{
	Pass
	{
		PixelShader = compile ps_2_0 CompositePS();
	}
}