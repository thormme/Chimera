//-----------------------------------------------------------------------------
// SkinnedEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#include "Macros.fxh"

#define SKINNED_EFFECT_MAX_BONES   64
#define MAX_CASCADE_COUNT          4

DECLARE_TEXTURE(Texture,      0);
DECLARE_TEXTURE(ShadowMap,    1);
DECLARE_TEXTURE(AlphaMap,     2);
DECLARE_TEXTURE(RedTexture,   3);
DECLARE_TEXTURE(GreenTexture, 4);
DECLARE_TEXTURE(BlueTexture,  5);
DECLARE_TEXTURE(AlphaTexture, 6);

BEGIN_CONSTANTS

    float4 DiffuseColor                         _vs(c0)  _ps(c1)  _cb(c0);
    float3 EmissiveColor                        _vs(c1)  _ps(c2)  _cb(c1);
    float3 SpecularColor                        _vs(c2)  _ps(c3)  _cb(c2);
    float  SpecularPower                        _vs(c3)  _ps(c4)  _cb(c2.w);

    float3 EyePosition                          _vs(c13) _ps(c14) _cb(c12);

    float3 FogColor                                      _ps(c0)  _cb(c13);
    float4 FogVector                            _vs(c14)          _cb(c14);

    float4x4 World                              _vs(c19)          _cb(c15);
    float3x3 WorldInverseTranspose              _vs(c23)          _cb(c19);
    
    float4x3 Bones[SKINNED_EFFECT_MAX_BONES]    _vs(c26)          _cb(c22);

	float3 xOverlayColor;
	float  xOverlayColorWeight;

	float2 xTextureOffset;
	float4x4 xTextureTransformation;

	float  xNumShadowBands;

	bool  xVisualizeCascades;
	float xCascadeCount;
	float4 xCascadeBufferBounds[MAX_CASCADE_COUNT];

	bool   xIsBeingSpaghettified;
	float3 xWormholePosition;
	float  xMaxWormholeDistance;
	float  xModelWormholeDistance;

	uint xDrawCursor;
	float3 xCursorPosition;
	float xCursorInnerRadius;
	float xCursorOuterRadius;

	float4 xPickingIndex;

	float2 Texture_uvOffset;
	float2 Texture_uvScale;

	float2 RedTexture_uvOffset;
	float2 RedTexture_uvScale;

	float2 GreenTexture_uvOffset;
	float2 GreenTexture_uvScale;

	float2 BlueTexture_uvOffset;
	float2 BlueTexture_uvScale;

	float2 AlphaTexture_uvOffset;
	float2 AlphaTexture_uvScale;

	float4 xTextureMask;

MATRIX_CONSTANTS

    float4x4 ViewProj                      _vs(c15)          _cb(c0);
	float4x4 LightWorldViewProj;
	float4x4 AnimateLightWorldViewProj;
	float4x4 xLightView;
	float4x4 xLightProjection;
	float4x4 xLightProjections[MAX_CASCADE_COUNT];
	float4   xCascadeColors[MAX_CASCADE_COUNT];

END_CONSTANTS

//-----------------------------------------------------------------------------
// Common.fxh
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------


float ComputeFogFactor(float4 position)
{
    return saturate(dot(position, FogVector));
}

void ApplyFog(inout float4 color, float fogFactor)
{
    color.rgb = lerp(color.rgb, FogColor * color.a, fogFactor);
}

void AddSpecular(inout float4 color, float3 specular)
{
    color.rgb += specular * color.a;
}
