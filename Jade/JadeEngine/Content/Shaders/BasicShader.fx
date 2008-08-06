float4x4 World;
float4x4 View;
float4x4 Project; 
float3 AmbientLightColor;
float3 EyePosition;
float3 DiffuseColor; 
float3 LightDirection; 
float3 LightDiffuseColor;
float SpecularPower;
float3 LightSpecularColor;

struct VS_INPUT 
{ 
	float4 Position : POSITION0;
	float3 Normal	: NORMAL0;
}; 

struct VS_OUTPUT 
{ 
	float4 Position			: POSITION0; 
	float3 Normal			: TEXCOORD1;
	float3 ViewDirection	: TEXCOORD2;
}; 

struct PS_INPUT
{
	float3 Normal			: TEXCOORD1;
	float3 ViewDirection	: TEXCOORD2;
};

VS_OUTPUT Transform(VS_INPUT Input)
{ 
	float4x4 WorldViewProject = mul(mul(World, View), Project); 
	float3 ObjectPosition = mul(Input.Position, World);
	
	VS_OUTPUT Output; 
	Output.Position			= mul(Input.Position, WorldViewProject); 
	Output.Normal			= mul(Input.Normal, World);
	Output.ViewDirection	= EyePosition - ObjectPosition;
	
	return Output; 
} 

float4 BasicShader(PS_INPUT Input) : COLOR0
{ 
	float3 Normal			= normalize(Input.Normal);
	float3 ViewDirection	= normalize(Input.ViewDirection);
	
	float EdgeDirection		= dot(Normal, ViewDirection);
	float3 TotalAmbient		= saturate(AmbientLightColor * EdgeDirection);
	
	float NDotL				= dot(Normal, LightDirection);
	float3 DiffuseAverage	= (DiffuseColor + LightDiffuseColor) * 0.5f;
	float3 TotalDiffuse		= saturate(DiffuseAverage * NDotL);
	
	float3 Reflection		= normalize(2.0f * NDotL * Normal - LightDirection);
	float RDotV				= max(0.0f, dot(Reflection, ViewDirection));
	float3 TotalSpecular	= saturate(LightSpecularColor * pow(RDotV, SpecularPower));
	
	float4 FinalColor = float4(saturate(TotalAmbient + TotalDiffuse + TotalSpecular), 1.0f); 
	return FinalColor;
}
 
technique BasicShader 
{ 
	pass P0 
	{ 
		VertexShader = compile vs_2_0 Transform(); 
		PixelShader = compile ps_2_0 BasicShader();
	}
}