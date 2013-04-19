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

Technique SkinnedNormalDepthShade
{
	Pass
	{
		VertexShader = compile vs_3_0 SkinnedNormalDepthVS();
		PixelShader  = compile ps_3_0 NormalDepthPS();
	}
}

Technique SkinnedCelShadeWithoutShadows
{
	Pass
	{
		VertexShader = compile vs_3_0 SkinnedVS();
		PixelShader  = compile ps_3_0 CelShadePS();
	}
}

Technique SkinnedCelShadeWithShadows
{
	Pass
	{
		VertexShader = compile vs_3_0 SkinnedVSWithShadows();
		PixelShader  = compile ps_3_0 CelShadePSWithShadows();
	}
}

Technique TerrainCelShadeWithoutShadows
{
	Pass
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader  = compile ps_3_0 TerrainCelShadePS();
	}
}

Technique TerrainCelShadeWithShadows
{
	Pass
	{
		VertexShader = compile vs_3_0 VSWithShadows();
		PixelShader  = compile ps_3_0 TerrainCelShadePSWithShadows();
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

Technique CelShadeWithoutShadows
{
	Pass
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader  = compile ps_3_0 CelShadePS();
	}
}

Technique CelShadeWithShadows
{
	Pass
	{
		VertexShader = compile vs_3_0 VSWithShadows();
		PixelShader  = compile ps_3_0 CelShadePSWithShadows();
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

Technique NormalDepthShade
{
	Pass
	{
		VertexShader = compile vs_3_0 NormalDepthVS();
		PixelShader  = compile ps_3_0 NormalDepthPS();
	}
}

Technique PickingShade
{
	Pass
	{
		VertexShader = compile vs_3_0 VS();
		PixelShader  = compile ps_3_0 PickingPS();
	}
}
