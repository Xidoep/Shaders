
#define UI0 1597334673U
#define UI1 3812015801U
#define UI2 ufloat2(UI0, UI1)
#define UI3 ufloat3(UI0, UI1, 2798796415U)
#define UIF (1.0 / float(0xffffffffU))


void mainImage_float(
	float2 uv,
	float4 _Offset, 
	float time, 
	float _Scale, 
	float3 _Speed, 
	float _Octavem, 
	float _OctaveScale, 
	float _Attenuation, 
	float _Color, 
	float _Intensity, 
	out float4 output )
{
		//float2 uv = i.globalTexcoord;

		output = _Offset;

		//float time = _IsTimeControlled == 1.0f ? _ControlledTime : _Time.y;

		output += SimplexNoise_Octaves(float3(uv, 0,0), _Scale, _Speed.xyz, uint(_Octave), _OctaveScale, _Attenuation, time);

		output = output * _Color * _Intensity;
}
