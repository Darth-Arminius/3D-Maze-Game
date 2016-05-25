float4x4 World;
float4x4 View;
float4x4 Projection;

uniform extern texture UserTexture;
float3 lDir;
float innerC;
float outerC;

float3 LightPosition;
float3 LightPosition2;
float3 LightPosition3;

float3 LightDiffuseColor;
float3 LightDiffuseColor2;
float3 LightDiffuseColor3;

float LightDistanceSquared;
float LightDistanceSquared2;
 
sampler texsampler = sampler_state
{
	Texture = <UserTexture>;
	mipfilter = LINEAR; 
};
 
struct VertexShaderInput
{
  float4 Position : POSITION0;
  float4 TexCoords : TEXCOORD0;
  float3 Normal : NORMAL0;
};
 
struct VertexShaderOutput
{
  float4 Position : POSITION0;
  float2 TexCoords : TEXCOORD0;
  float3 Normal : TEXCOORD1;
  float3 WorldPos : TEXCOORD2;
};
 
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
  VertexShaderOutput output;
 
  float4x4 viewprojection = mul(View, Projection);
  float4 posWorld = mul(input.Position, World);
  
  
  output.Position = mul(posWorld, viewprojection);
  output.TexCoords = input.TexCoords;
  output.Normal = mul(input.Normal, (float3x3)World);
  output.WorldPos = posWorld;
 
  return output;
}
 
float4 PixelShaderFunctionWithTex(VertexShaderOutput input) : COLOR0
{
	float4 tex = tex2D(texsampler, input.TexCoords);
	float4 ga = {0.2f, 0.2f, 0.2f, 1.0f};
	float2 cosAngles = cos(float2(outerC, innerC) * 0.5f);
	float3 n = normalize(input.Normal);
	
	//player Lantern
	float3 lightDir3 = normalize(input.WorldPos - LightPosition); 
	float diffuseLighting = saturate(dot(input.Normal, -lightDir3));
    diffuseLighting *= (LightDistanceSquared / dot(LightPosition - input.WorldPos, LightPosition - input.WorldPos));
	float4 PL = float4(saturate(tex.xyz * LightDiffuseColor * diffuseLighting * 0.8), 1.0) + (tex*0.05);
	
	//yellow Spotlight
    float3 lightDir = (LightPosition2 - input.WorldPos) / LightDistanceSquared2;
    float atten = saturate(1.0f - dot(lightDir, lightDir));
	float3 l = normalize(lightDir);
    float spotDot = dot(-l, normalize(lDir));
    float spotEffect = smoothstep(cosAngles[0], cosAngles[1], spotDot);	
	atten *= spotEffect;   
    float nDotL = saturate(dot(n, l));
	float4 YS = (float4(LightDiffuseColor2,1.0) * nDotL * atten);
                   
	//blue Spotlight
	float3 lightDir2 = (LightPosition3 - input.WorldPos) / LightDistanceSquared2;
    float atten2 = saturate(1.0f - dot(lightDir2, lightDir2));
	float3 l2 = normalize(lightDir2);
    float spotDot2 = dot(-l2, normalize(lDir));
    float spotEffect2 = smoothstep(cosAngles[0], cosAngles[1], spotDot2);	
	atten2 *= spotEffect2;   
    float nDotL2 = saturate(dot(n, l2));
	float4 BS = (float4(LightDiffuseColor3,1.0) * nDotL2 * atten2);
	
	return PL + YS + BS * tex;
}
 
technique TechniqueWithTexture
{
  pass Pass1
  {
    VertexShader = compile vs_3_0 VertexShaderFunction();
    PixelShader = compile ps_3_0 PixelShaderFunctionWithTex();
  }
}











