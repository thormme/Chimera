#include "Structures.fxh"
#include "Common.fxh"
#include "Skinning.fxh"
#include "Lighting.fxh"
#include "VertexShaders.fxh"
#include "PixelShaders.fxh"

Technique SkinnedShadowCast
{
	Pass
	{
		VertexShader = compile vs_3_0 SkinnedShadowCastVS();
		PixelShader  = compile ps_3_0 ShadowCastPS();
	}
}

Technique SkinnedCelShade
{
	Pass
	{
		VertexShader = compile vs_3_0 SkinnedVS();
		PixelShader  = compile ps_3_0 CelShadePS();
	}
}

Technique SkinnedOutline
{
	Pass
	{
		VertexShader = compile vs_3_0 SkinnedOutlineVS();
		PixelShader  = compile ps_3_0 OutlinePS();
	}
}

Technique SkinnedPhong
{
	Pass
	{
		VertexShader = compile vs_3_0 SkinnedVS();
		PixelShader  = compile ps_3_0 PhongPS();
	}
}

Technique SkinnedNormalDepthShade
{
	Pass
	{
		VertexShader = compile vs_3_0 SkinnedNormalDepthVS();
		PixelShader  = compile ps_3_0 NormalDepthPS();
	}
}

Technique TerrainCelShade
{
	Pass
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader  = compile ps_3_0 TerrainCelShadePS();
	}
}

Technique TerrainPhong
{
	Pass
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader  = compile ps_3_0 TerrainPhongPS();
	}
}

Technique ShadowCast
{
	Pass
	{
		VertexShader = compile vs_3_0 ShadowCastVS();
		PixelShader  = compile ps_3_0 ShadowCastPS();
	}
}

Technique CelShade
{
	Pass
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader  = compile ps_3_0 CelShadePS();
	}
}

Technique NoShade
{
	Pass
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader  = compile ps_3_0 NoShadePS();
	}
}

Technique Outline
{
	Pass
	{
		VertexShader = compile vs_3_0 OutlineVS();
		PixelShader  = compile ps_3_0 OutlinePS();
	}
}

Technique Phong
{
	Pass
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader  = compile ps_3_0 PhongPS();
	}
}

Technique NormalDepthShade
{
	Pass
	{
		VertexShader = compile vs_3_0 NormalDepthVS();
		PixelShader  = compile ps_3_0 NormalDepthPS();
	}
}
