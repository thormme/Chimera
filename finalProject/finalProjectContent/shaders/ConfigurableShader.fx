#include "Structures.fxh"
#include "Common.fxh"
#include "Skinning.fxh"
#include "Lighting.fxh"
#include "VertexShaders.fxh"
#include "PixelShaders.fxh"

Technique SkinnedCelShade
{
	Pass
	{
		VertexShader = compile vs_2_0 SkinnedCelShadeVS();
		PixelShader  = compile ps_2_0 CelShadePS();
	}
}

Technique SkinnedOutline
{
	Pass
	{
		VertexShader = compile vs_2_0 SkinnedOutlineVS();
		PixelShader  = compile ps_2_0 OutlinePS();
	}
}

Technique SkinnedPhong
{
	Pass
	{
		VertexShader = compile vs_2_0 SkinnedPhongVS();
		PixelShader  = compile ps_2_0 PhongPS();
	}
}

Technique CelShade
{
	Pass
	{
		VertexShader = compile vs_2_0 CelShadeVS();
		PixelShader  = compile ps_2_0 CelShadePS();
	}
}

Technique Outline
{
	Pass
	{
		VertexShader = compile vs_2_0 OutlineVS();
		PixelShader  = compile ps_2_0 OutlinePS();
	}
}

Technique Phong
{
	Pass
	{
		VertexShader = compile vs_2_0 PhongVS();
		PixelShader  = compile ps_2_0 PhongPS();
	}
}

Technique TerrainCelShade
{
	Pass
	{
		VertexShader = compile vs_2_0 TerrainCelShadeVS();
		PixelShader  = compile ps_2_0 CelShadePS();
	}
}

Technique TerrainOutline
{
	Pass
	{
		VertexShader = compile vs_2_0 TerrainOutlineVS();
		PixelShader  = compile ps_2_0 OutlinePS();
	}
}

Technique TerrainPhong
{
	Pass
	{
		VertexShader = compile vs_2_0 TerrainPhongVS();
		PixelShader  = compile ps_2_0 PhongPS();
	}
}
