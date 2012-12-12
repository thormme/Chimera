struct ColorPair
{
    float3 Diffuse;
    float3 Specular;
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
	float4 Diffuse        : COLOR0;
	float4 Specular       : COLOR1;
	float2 TexCoord       : TEXCOORD0;
	float4 ShadowPosition : TEXCOORD1;
	float4 HiResShadowPosition : TEXCOORD2;
	float  LightAmount    : TEXCOORD3;
	float4 PositionPS     : SV_Position;
};

struct OutlineVSOutput
{
	float4 PositionPS : SV_Position;
};

