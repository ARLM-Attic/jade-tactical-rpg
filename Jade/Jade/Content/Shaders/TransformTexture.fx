float4x4 World; 
float4x4 View; 
float4x4 Project; 

sampler TextureSampler; 
struct VS_INPUT 
{ 
	float4 Position : POSITION0; 
	float2 Texcoord : TEXCOORD0; 
}; 

struct VS_OUTPUT 
{ 
	float4 Position : POSITION0; 
	float2 Texcoord : TEXCOORD0;
}; 

VS_OUTPUT Transform(VS_INPUT Input) 
{ 
	VS_OUTPUT Output; 
	float4 worldPosition = mul(Input.Position, World); 
	float4 viewPosition = mul(worldPosition, View); 
	Output.Position = mul(viewPosition, Project); 
	Output.Texcoord = Input.Texcoord; 
	return Output; 
} 

float4 Texture(VS_OUTPUT Input) : COLOR0
{
	return tex2D(TextureSampler, Input.Texcoord); 
} 

technique TransformTexture 
{ 
	pass P0
	{ 
		VertexShader = compile vs_2_0 Transform(); 
		PixelShader = compile ps_2_0 Texture();
	}
}