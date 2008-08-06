// Shared variables - available in all effects
// These don't need to be set manually
shared float2 PixelSize;
shared float4x4 InvViewProj;
shared float NearClipPlane;
shared float FarClipPlane;
shared float3 CameraPosition;

// Effect variables
float sampleWeights[7];
float2 sampleOffsets[7];

float focalDistance;
float focalLength;

// Samplers
sampler inputSampler : register(s0);
sampler depthSampler : register(s1);
sampler blurSampler : register(s2);

// Blur filter - blurs with samples that are <= current depth
float4 DofBlurShader(float2 texCoord : TEXCOORD0) : COLOR
{
	// Get current depth
	float currentDepth = tex2D(depthSampler, texCoord).x;
	
	// Start building color
	float4 c = 0;
	
	// Add weighted texture taps
	for (int i = 0; i < 7; i++)
	{
		c += tex2D(inputSampler, texCoord + sampleOffsets[i]) * sampleWeights[i];
	}
	
	return c;
}

// Depth of field shader - combines blur with color based on DOF params
float4 DofShader(float2 texCoord : TEXCOORD0) : COLOR
{
	// Get the depth
	float depth = tex2D(depthSampler, texCoord).x;

	// Clip if depth is out of range
	//clip(depth * 2.0f - 1.0f);

	// Get the color and blur color
	float4 baseColor = tex2D(inputSampler, texCoord);
	float4 blurColor = tex2D(blurSampler, texCoord);

	// Get world position
	float4 screenPos = float4(
		texCoord.x * 2.0f - 1.0f,
		(1.0f - texCoord.y) * 2.0f - 1.0f,
		depth,
		1.0f);
	float4 worldPos = mul(screenPos, InvViewProj);
	
	// Calculate focal amount
	float distanceFromFocus = distance(
		focalDistance,
		distance((worldPos.xyz / worldPos.w), CameraPosition)
		);
	float focalValue = smoothstep(0.0f, focalLength, distanceFromFocus);
	
	// Get color	
	float4 returnColor = lerp(baseColor, blurColor, focalValue);
	
	// Return the color
	return returnColor;
}

// Techniques
technique Blur
{
    pass Pass0
    {
		pixelShader = compile ps_2_0 DofBlurShader();
    }
}

technique DepthOfField
{
    pass Pass0
    {
		pixelShader = compile ps_2_0 DofShader();
    }
}