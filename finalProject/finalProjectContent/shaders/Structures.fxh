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

struct CelVSOutput
{
	float4 Diffuse        : COLOR0;
	float4 Specular       : COLOR1;
	float2 TexCoord       : TEXCOORD0;
	float4 ShadowPosition : TEXCOORD1;
	float  LightAmount    : TEXCOORD2;
	float4 PositionPS     : SV_Position;
};

struct OutlineVSOutput
{
	float4 PositionPS : SV_Position;
};

struct PhongVSOutput
{
	float4 Diffuse        : COLOR0;
	float4 Specular       : COLOR1;
	float2 TexCoord       : TEXCOORD0;
	float4 ShadowPosition : TEXCOORD1;
	float4 PositionPS     : SV_Position;
};
