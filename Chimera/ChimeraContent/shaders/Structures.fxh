struct ColorPair
{
    float3 Diffuse;
    float3 Specular;
};

struct ShadowData
{
	float4 TexCoords_0_1;
	float4 TexCoords_2_3;
	float4 LightSpaceDepth_0_3;
};

struct ShadowPixel
{
	bool   InShadow;
	float4 Color;
};

struct ShadowSplitData
{
	float2 TexCoords;
	float  LightSpaceDepth;
	int    SplitIndex;
	float4 Color;
};

struct SkinnedVSInput
{
	float4 Position : SV_Position;
	float3 Normal   : NORMAL;
	float2 TexCoord : TEXCOORD0;
	int4   Indices  : BLENDINDICES0;
	float4 Weights  : BLENDWEIGHT0;
};

struct VSInput
{
	float4 Position : SV_Position;
	float3 Normal   : NORMAL;
	float2 TexCoord : TEXCOORD0;
};

struct ShadowVSOutput
{
	float Depth      : TEXCOORD0;
	float4 PositionPS : SV_Position;
};

struct NormalDepthVSOutput
{
	float4 PositionPS : SV_Position;
	float4 Color      : COLOR0;
};

struct VSOutput
{
	float4     Diffuse     : COLOR0;
	float4     Specular    : COLOR1;
	float2     TexCoord    : TEXCOORD0;
	float      LightAmount : TEXCOORD1;
	float4     PositionPS  : SV_Position;
	//float4     ShadowCoord : TEXCOORD2;
	ShadowData Shadow      : TEXCOORD2;
};

struct OutlineVSOutput
{
	float4 PositionPS : SV_Position;
};
