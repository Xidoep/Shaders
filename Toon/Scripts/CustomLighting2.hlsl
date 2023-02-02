#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl"


struct CustomLightingData {
    float3 normalWS;
    float3 albedo;
};

float3 CustomLightHandling(CustomLightingData d, Light light) {

    float3 radiance = light.color;

    float diffuse = saturate(dot(d.normalWS, light.direction));

    float3 color = d.albedo * radiance * diffuse;

    return color;
}

float3 CalculateCustomLighting(CustomLightingData d) {
    Light mainLight = GetMainLight();
    float3 color = 0;
    color += CustomLightHandling(d, mainLight);

    return color;
}
void CalculateCustomLighting_float(float3 Normal, float3 Albedo,
    out float3 Color) {

    CustomLightingData d;
    d.normalWS = Normal;
    d.albedo = Albedo;

    Color = CalculateCustomLighting(d);
}

#endif