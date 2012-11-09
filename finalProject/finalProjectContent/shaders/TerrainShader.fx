float4x4 xWorld;
float4x4 xView;
float4x4 xProjection;

float3   xLightDirection;

struct VertexShaderInput
{
    float4 Position : SV_Position;
	float3 Normal   : NORMAL;
	float4 Color    : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : SV_Position;
	float4 Color    : COLOR0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, xWorld);
    float4 viewPosition = mul(worldPosition, xView);
    output.Position = mul(viewPosition, xProjection);

	float4 worldNormal = mul(input.Normal, xWorld);

    output.Color = input.Color;
	if (dot(xLightDirection, worldNormal) > 0.0f)
	{
		output.Color -= float4(0.25, 0.25, 0.25, 0.0);
	}

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    return input.Color;
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
