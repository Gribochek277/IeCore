﻿#version 330
out vec4 FragColor;
in vec2 f_texcoord;
in vec3 f_pos;
in vec3 v_norm;

// material parameters
uniform sampler2D maintexture;
uniform sampler2D normaltexture;
uniform float metallic;
uniform float roughness;
uniform float ambientStr;

// lights
uniform int numberOfLights;
uniform vec3 lightPos[64];
uniform vec3 lightColor[64];

// IBL
uniform samplerCube irradianceMap;

uniform vec3 cameraPosition;

const float PI = 3.14159265359;
  
float DistributionGGX(vec3 N, vec3 H, float roughness)
{
    float a = roughness*roughness;
    float a2 = a*a;
    float NdotH = max(dot(N, H), 0.0);
    float NdotH2 = NdotH*NdotH;

    float nom   = a2;
    float denom = (NdotH2 * (a2 - 1.0) + 1.0);
    denom = PI * denom * denom;

    return nom / max(denom, 0.001); // prevent divide by zero for roughness=0.0 and NdotH=1.0
}
// ----------------------------------------------------------------------------
float GeometrySchlickGGX(float NdotV, float roughness)
{
    float r = (roughness + 1.0);
    float k = (r*r) / 8.0;

    float nom   = NdotV;
    float denom = NdotV * (1.0 - k) + k;

    return nom / denom;
}
// ----------------------------------------------------------------------------
float GeometrySmith(vec3 N, vec3 V, vec3 L, float roughness)
{
    float NdotV = max(dot(N, V), 0.0);
    float NdotL = max(dot(N, L), 0.0);
    float ggx2 = GeometrySchlickGGX(NdotV, roughness);
    float ggx1 = GeometrySchlickGGX(NdotL, roughness);

    return ggx1 * ggx2;
}

vec3 fresnelSchlick(float cosTheta, vec3 F0)
{
    return F0 + (1.0 - F0) * pow(1.0 - cosTheta, 5.0);
}

vec3 fresnelSchlickRoughness(float cosTheta, vec3 F0, float roughness)
{
    return F0 + (max(vec3(1.0 - roughness), F0) - F0) * pow(1.0 - cosTheta, 5.0);
} 

vec3 getNormalFromMap(vec2 texcoord)
{
    vec3 tangentNormal = texture(normaltexture, texcoord).xyz * 2.0 - 1.0;

    vec3 Q1  = dFdx(f_pos);
    vec3 Q2  = dFdy(f_pos);
    vec2 st1 = dFdx(texcoord);
    vec2 st2 = dFdy(texcoord);

    vec3 N   = normalize(v_norm);
    vec3 T  = normalize(Q1*st2.t - Q2*st1.t);
    vec3 B  = -normalize(cross(N, T));
    mat3 TBN = mat3(T, B, N);

    return normalize(TBN * tangentNormal);
}

void main()
{		
	vec3 N = getNormalFromMap(f_texcoord);
    vec3 V = normalize(cameraPosition - f_pos);
	vec3 albedo = texture(maintexture, f_texcoord).rgb;

    // calculate reflectance at normal incidence; if dia-electric (like plastic) use F0 
    // of 0.04 and if it's a metal, use the albedo color as F0 (metallic workflow)    
    vec3 F0 = vec3(0.04); 
    F0 = mix(F0, albedo, metallic);

    // reflectance equation
    vec3 Lo = vec3(0.0);
    for(int i = 0; i < numberOfLights; ++i) 
    {
        // calculate per-light radiance
        vec3 L = normalize(lightPos[i] - f_pos);
        vec3 H = normalize(V + L);
        float distance = length(lightPos[i] - f_pos);
        float attenuation = 1.0 / (distance * distance);
        vec3 radiance = lightColor[i] * attenuation;

        // Cook-Torrance BRDF
        float NDF = DistributionGGX(N, H, roughness);   
        float G   = GeometrySmith(N, V, L, roughness);      
        vec3 F    = fresnelSchlick(clamp(dot(H, V), 0.0, 1.0), F0);
           
        vec3 nominator    = NDF * G * F; 
        float denominator = 4 * max(dot(N, V), 0.0) * max(dot(N, L), 0.0);
        vec3 specular = nominator / max(denominator, 0.001); // prevent divide by zero for NdotV=0.0 or NdotL=0.0
        
        // kS is equal to Fresnel
        vec3 kS = F;
        // for energy conservation, the diffuse and specular light can't
        // be above 1.0 (unless the surface emits light); to preserve this
        // relationship the diffuse component (kD) should equal 1.0 - kS.
        vec3 kD = vec3(1.0) - kS;
        // multiply kD by the inverse metalness such that only non-metals 
        // have diffuse lighting, or a linear blend if partly metal (pure metals
        // have no diffuse light).
        kD *= 1.0 - metallic;	  

        // scale light by NdotL
        float NdotL = max(dot(N, L), 0.0);        

        // add to outgoing radiance Lo
        Lo += (kD * albedo / PI + specular) * radiance * NdotL;  // note that we already multiplied the BRDF by the Fresnel (kS) so we won't multiply by kS again
    }   
    

	//vec3 ambient = vec3(0.03) * albedo * ambientStr;
    vec3 kS = fresnelSchlickRoughness(max(dot(N, V), 0.0), F0, roughness); 
	vec3 kD = 1.0 - kS;
	vec3 irradiance = texture(irradianceMap, N).rgb;
	vec3 diffuse    = irradiance * albedo;
	vec3 ambient    = (kD * diffuse) * ambientStr; 

    vec3 color = ambient + Lo;

    // HDR tonemapping
    color = color / (color + vec3(1.0));
    // gamma correct
    color = pow(color, vec3(1.0/2.2)); 

    FragColor = vec4(color, 1.0);
}  