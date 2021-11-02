void LlumPrincipal_float(float3 WorldPos, out float3 Direction, out float3 Color, out float DistanceAtten, out float ShadowAtten)
{
	#ifdef SHADERGRAPH_PREVIEW
	   Direction = float3(1, 1, -0.4);
	   Color = float4(1,1,1,1);
	   DistanceAtten = 1;
	   ShadowAtten = 1;
	#else
	   Light mainLight = GetMainLight();
	   Direction = mainLight.direction;
	   Color = mainLight.color;
	   DistanceAtten = mainLight.distanceAttenuation;
	   
	   float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);

		ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	   float shadowStrength = GetMainLightShadowStrength() * 2;
	   ShadowAtten = SampleShadowmap(shadowCoord,TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture),shadowSamplingData, shadowStrength, false);
	#endif
}