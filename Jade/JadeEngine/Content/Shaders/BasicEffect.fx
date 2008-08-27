float4x4 World				: WORLD;
float4x4 WorldIT			: WORLDINVERSETRANSPOSE;
float4x4 View				: VIEW;
float4x4 Project			: PROJECTION; 
float4x4 WorldViewProject	: WORLDVIEWPROJECTION;

float3 EyePosition;

float3	AmbientLightColor		: AMBIENT;
float3	AmbientLightPosition	: LIGHTDIR0_DIRECTION;

float4	MaterialDiffuseColor	: DIFFUSE; 
float4	MaterialSpecularColor	: SPECULAR;
float	MaterialSpecularPower	: SPECULARPOWER;

sampler TextureSampler; 

struct VS_INPUT 
{ 
	float4 Position : POSITION0;
	float3 Normal	: NORMAL0;
	float2 TexCoord : TEXCOORD0;
}; 

struct VS_OUTPUT 
{ 
	float4 Position			: POSITION0; 
	float3 Normal			: TEXCOORD1;
	float3 ViewDirection	: TEXCOORD2;
	float2 TexCoord			: TEXCOORD0;
}; 

VS_OUTPUT Transform(VS_INPUT Input)
{ 
	WorldViewProject = mul(mul(World, View), Project); 
	float3 ObjectPosition = mul(Input.Position, World).xyz;
	
	VS_OUTPUT Output; 
    Output.Position			= mul(Input.Position, WorldViewProject);
	Output.TexCoord			= Input.TexCoord; 
	Output.ViewDirection	= EyePosition - ObjectPosition;
	Output.Normal			= mul(Input.Normal, WorldIT);
	
	return Output; 
} 

float4 BasicShader(VS_OUTPUT Input) : COLOR0
{ 
	float2 TexCoord	= Input.TexCoord;
	float3 Light	= normalize(-AmbientLightPosition);
	float3 View		= normalize(Input.ViewDirection);
	float3 Normal	= normalize(Input.Normal);
	float3 Half		= normalize(Light + View);
	float3 NDotL	= dot(Normal, Light);
	float3 NDotH	= dot(Normal, Half);
	
	float3 Diffuse	= saturate(NDotL) * MaterialDiffuseColor.rgb;
	float3 Specular	= pow(saturate(NDotH), MaterialSpecularPower) * MaterialSpecularColor.rgb;
	float4 Texture	= tex2D(TextureSampler, TexCoord);
	
	float3 Color	= saturate(Diffuse + AmbientLightColor + Specular + Texture.xyz);
	float Alpha		= MaterialDiffuseColor.a * Texture.a;

	return float4(Color, Alpha);
}
 
technique tech
{ 
	pass P0 
	{ 
		VertexShader = compile vs_2_0 Transform(); 
		PixelShader = compile ps_2_0 BasicShader();
	}
}