//-----------------------------------------------------------------------------
// SkinnedEffect.fx
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#include "Macros.fxh"

#define SKINNED_EFFECT_MAX_BONES   72

DECLARE_TEXTURE(Texture, 0);

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

MATRIX_CONSTANTS

    float4x4 WorldViewProj                      _vs(c15)          _cb(c0);

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
