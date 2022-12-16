void Raymarch_float(float3 rayOrigin, float3 rayDirection, float numSteps, float stepSize, float4 Sphere, float densityScale, out float density)
{
	density = -1;

	for(int i =0; i<numSteps; i++){
		rayOrigin += (rayDirection*stepSize);

		//Get Sphere Density
		float sphereDist = distance(rayOrigin, Sphere.xyz);
		if(sphereDist < Sphere.w){
			density += 0.1*densityScale*2;
		}
		else{

		}
	}
	density = clamp( exp(-density),0,1);
}

void RaymarchNoise_float(float3 rayOrigin, float3 rayDirection, float numSteps, float stepSize, float4 Sphere, float densityScale, UnityTexture3D Volume, float3 offset, out float density)
{
	density = -1;

	for(int i =0; i<numSteps; i++){
		rayOrigin += (rayDirection*stepSize);

		float3 samplePos = rayOrigin + offset;

		float sampledDensity = tex3D(Volume, samplePos).r;
		//density += sampledDensity*densityScale;

		//Get Sphere Density
		float sphereDist = distance(rayOrigin, Sphere.xyz + offset);
		if(sphereDist < Sphere.w - sampledDensity * 0.5){
			density += sampledDensity;
		}
		else{

		}
	}
	density =  clamp( exp(-density),0,1);
}

void Raymarch3DTexture_float(float3 rayOrigin, float3 rayDirection, float numSteps, float stepSize, float4 Sphere, float densityScale, UnityTexture3D Volume, float3 offset, out float density)
{
	density = 0;

	for(int i =0; i<numSteps; i++){
		rayOrigin += (rayDirection*stepSize);

		float sampledDensity = tex3D(Volume, rayOrigin+offset).r;
		density += sampledDensity*densityScale;
	}
	density = exp(-density);
}

void Raymarch3DTextureLight_float(
	float3 rayOrigin, 
	float3 rayDirection, 
	float numSteps, 
	float stepSize, 
	float4 Sphere, 
	float densityScale, 
	UnityTexture3D Volume, 
	float3 offset, 
	float numLightSteps, 
	float lightStepSize, 
	float3 lightDir, 
	float lightAbsorb, 
	float darknessThreshold, 
	float transmittance, 
	out float3 output)
{
	float density = 0;
	float transmission = 0;
	float lightAccumulation = 0;
	float finalLight = 1;

	for(int i = 0; i < numSteps; i++){
		rayOrigin += (rayDirection*stepSize);

		float3 samplePos = rayOrigin * .1 + offset;

		float sampledDensity = tex3D(Volume, samplePos).r;
		density += sampledDensity*densityScale;

		//light loop
		float3 lightRayOrigin = samplePos;
		for(int j =0; j<numLightSteps; j++){
			lightRayOrigin += lightDir*lightStepSize;
			float lightDensity = tex3D(Volume, lightRayOrigin).r;
			lightAccumulation += lightDensity*densityScale;
		}

		float lightTransmission = exp(-lightAccumulation);
		float shadow = darknessThreshold + lightTransmission * (1.0 -darknessThreshold);
		finalLight += density*transmittance*shadow;
		transmittance *= exp(-density*lightAbsorb);
	}

	transmission = exp(-density);

	output = float3(finalLight, transmission, transmittance);
}