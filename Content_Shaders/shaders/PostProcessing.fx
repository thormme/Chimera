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
float NormalThreshold = 0.7;
float DepthThreshold = 0.1;

// How dark the edges get in response to changes in data.
float NormalSensitivity = 1;
float DepthSensitivity = 10;

// Given 3x3 sobel kernels.
const float SobelXKernel[9] = { -1, 0, 1, -2, 0, 2,  -1,  0, 1 };
const float SobelYKernel[9] = { 1, 2,  1, 0, 0,  0, -1, -2, -1 };

// Resolution of scene rendered to texture.
float2 ScreenResolution;

// Parameters for selection box rendering.
float2 xSelectionStart;
float2 xSelectionEnd;
float2 xSelectionLineWidth;
float4 xSelectionColor;

// Current scene rendered to texture without any post processing.
DECLARE_TEXTURE(SceneTexture, 0);

// Texture containing normal vector to face at each pixel in RGB and depth in alpha.
DECLARE_TEXTURE(NormalDepthTexture, 1);

// Texture containing outline of scene.
DECLARE_TEXTURE(OutlineTexture, 2);

float4 EdgeDetectPS(float2 texCoord : TEXCOORD0) : COLOR0
{
	// Grab all values adjacent to texCoord and offset along each cardinal direction.
	float2 edgeOffset = EdgeWidth / ScreenResolution;
        
	float4 pixels[9];

	for (int row = -1; row <= 1; row++)
	{
		for (int col = -1; col <= 1; col++)
		{
			pixels[(col + 1) + (row + 1) * 3] = SAMPLE_TEXTURE(NormalDepthTexture, texCoord + float2(col, row) * edgeOffset);
		}
	}

	float4 sobelXGradient = float4(0,0,0,0);
	float4 sobelYGradient = float4(0,0,0,0);
	for (int i = 0; i < 9; i++)
	{
		sobelXGradient += pixels[i] * SobelXKernel[i];
		sobelYGradient += pixels[i] * SobelYKernel[i];
	}

	float depthXGradient = sobelXGradient.w;
	float depthYGradient = sobelYGradient.w;

	float normalGradient = length(sobelXGradient.rgb) + length(sobelYGradient.rgb);
	float depthGradient = sqrt(depthXGradient * depthXGradient + depthYGradient * depthYGradient);

    // Filter out very small changes, in order to produce nice clean results.
    normalGradient = saturate((normalGradient - NormalThreshold) * NormalSensitivity);
    depthGradient = saturate((depthGradient - DepthThreshold) * DepthSensitivity);

    // Does this pixel lie on an edge?
    float edgeAmount = saturate(normalGradient + depthGradient) * EdgeIntensity;

	return float4(edgeAmount, edgeAmount, edgeAmount, 1);
}

float4 CompositePS(float2 texCoord : TEXCOORD0) : COLOR0
{
	float3 outline = SAMPLE_TEXTURE(OutlineTexture, texCoord);

	float3 sceneColor = SAMPLE_TEXTURE(SceneTexture, texCoord);

	return float4(sceneColor * (1.0 - 0.75 * outline.r), 1.0);
}

float4 SelectionBoxPS(float2 texCoord : TEXCOORD0) : COLOR0
{
	float3 sceneColor = SAMPLE_TEXTURE(SceneTexture, texCoord);

	if ((texCoord.r >= xSelectionStart.r && texCoord.r <= xSelectionStart.r + xSelectionLineWidth.r ||
		 texCoord.r <= xSelectionEnd.r   && texCoord.r >= xSelectionEnd.r   - xSelectionLineWidth.r) &&
		(texCoord.g >= xSelectionStart.g && texCoord.g <= xSelectionStart.g + xSelectionLineWidth.g ||
		 texCoord.g <= xSelectionEnd.g   && texCoord.g >= xSelectionEnd.g   - xSelectionLineWidth.g))
	{
		sceneColor = xSelectionColor.rgb;
	}

	return float4(sceneColor, 1.0);
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

technique SelectionBox
{
	Pass
	{
		PixelShader = compile ps_2_0 SelectionBoxPS();
	}
}