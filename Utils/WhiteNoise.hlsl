#ifndef WHITE_NOISE_PARIAL
#define WHITE_NOISE_PARIAL

void WhiteNoise_float(float2 UV, out float Out){
	Out = frac(sin(dot(UV, float2(12.9898, 78.233))) * 43758.5453);
}

//get a scalar random value from a 3d value
float rand3dTo1d(float3 value, float3 dotDir = float3(12.9898, 78.233, 37.719)) {
	//make value smaller to avoid artefacts
	float3 smallValue = sin(value);
	//get scalar value from 3d vector
	float random = dot(smallValue, dotDir);
	//make value more random by making it bigger and then taking the factional part
	random = frac(sin(random) * 143758.5453);
	return random;
}

float rand2dTo1d(float2 value, float2 dotDir = float2(12.9898, 78.233)) {
	float2 smallValue = sin(value);
	float random = dot(smallValue, dotDir);
	random = frac(sin(random) * 143758.5453);
	return random;
}

float rand1dTo1d(float value, float mutator = 0.546) {
	float random = frac(sin(value + mutator) * 143758.5453);
	return random;
}

//to 2d functions

float2 rand3dTo2d(float3 value) {
	return float2(
		rand3dTo1d(value, float3(12.989, 78.233, 37.719)),
		rand3dTo1d(value, float3(39.346, 11.135, 83.155))
	);
}

float2 rand2dTo2d(float2 value) {
	return float2(
		rand2dTo1d(value, float2(12.989, 78.233)),
		rand2dTo1d(value, float2(39.346, 11.135))
	);
}

float2 rand1dTo2d(float value) {
	return float2(
		rand2dTo1d(value, 3.9812),
		rand2dTo1d(value, 7.1536)
	);
}

//to 3d functions

float3 rand3dTo3d(float3 value) {
	return float3(
		rand3dTo1d(value, float3(12.989, 78.233, 37.719)),
		rand3dTo1d(value, float3(39.346, 11.135, 83.155)),
		rand3dTo1d(value, float3(73.156, 52.235, 09.151))
	);
}

float3 rand2dTo3d(float2 value) {
	return float3(
		rand2dTo1d(value, float2(12.989, 78.233)),
		rand2dTo1d(value, float2(39.346, 11.135)),
		rand2dTo1d(value, float2(73.156, 52.235))
	);
}

float3 rand1dTo3d(float value) {
	return float3(
		rand1dTo1d(value, 3.9812),
		rand1dTo1d(value, 7.1536),
		rand1dTo1d(value, 5.7241)
	);
}

//get a scalar random value from a 3d value
void Rand3dTo1d_float(float3 Value, out float Out) {
	Out = rand3dTo1d(Value);
}

void Rand2dTo1d_float(float2 Value, out float Out) {
	Out = rand2dTo1d(Value);
}

void Rand1dTo1d_float(float3 Value, out float Out) {
	Out = rand1dTo1d(Value);
}

//to 2d functions

void Rand3dTo2d_float(float3 Value, out float2 Out) {
	Out = rand3dTo2d(Value);
}

void Rand2dTo2d_float(float2 Value, out float2 Out) {
	Out = rand2dTo2d(Value);
}

void Rand1dTo2d_float(float Value, out float2 Out) {
	Out = rand1dTo2d(Value);
}

//to 3d functions

void Rand3dTo3d_float(float3 Value, out float3 Out) {
	Out = rand3dTo3d(Value);
}

void Rand2dTo3d_float(float2 Value, out float3 Out) {
	Out = rand2dTo3d(Value);
}

void Rand1dTo3d_float(float Value, out float3 Out) {
	Out = rand1dTo3d(Value);
}



//ALTRES NOISES
/*float2 modulo(float2 divident, float2 divisor, out float2 Out){
    float2 positiveDivident = divident % divisor + divisor;
    Out = positiveDivident % divisor;
}*/
float3 modulo(float3 divident, float3 divisor, out float3 Out){
	float3 positiveDivident = divident % divisor + divisor;
	Out = positiveDivident % divisor;
}



float3 voronoiNoise(float3 value, float3 period, float output){
    float3 baseCell = floor(value);

    //first pass to find the closest cell
    float minDistToCell = 10;
    float3 toClosestCell;
    float3 closestCell;
    [unroll]
    for(int x1=-1; x1<=1; x1++){
        [unroll]
        for(int y1=-1; y1<=1; y1++){
            [unroll]
            for(int z1=-1; z1<=1; z1++){
                float3 cell = baseCell + float3(x1, y1, z1);
                float3 tiledCell = modulo(cell, period);
                float3 cellPosition = cell + rand3dTo3d(tiledCell);
                float3 toCell = cellPosition - value;
                float distToCell = length(toCell);
                if(distToCell < minDistToCell){
                    minDistToCell = distToCell;
                    closestCell = cell;
                    toClosestCell = toCell;
                }
            }
        }
    }

    //second pass to find the distance to the closest edge
    float minEdgeDistance = 10;
    [unroll]
    for(int x2=-1; x2<=1; x2++){
        [unroll]
        for(int y2=-1; y2<=1; y2++){
            [unroll]
            for(int z2=-1; z2<=1; z2++){
                float3 cell = baseCell + float3(x2, y2, z2);
                float3 tiledCell = modulo(cell, period);
                float3 cellPosition = cell + rand3dTo3d(tiledCell);
                float3 toCell = cellPosition - value;

                float3 diffToClosestCell = abs(closestCell - cell);
                bool isClosestCell = diffToClosestCell.x + diffToClosestCell.y + diffToClosestCell.z < 0.1;
                if(!isClosestCell){
                    float3 toCenter = (toClosestCell + toCell) * 0.5;
                    float3 cellDifference = normalize(toCell - toClosestCell);
                    float edgeDistance = dot(toCenter, cellDifference);
                    minEdgeDistance = min(minEdgeDistance, edgeDistance);
                }
            }
        }
    }

    float random = rand3dTo1d(closestCell);
    return float3(minDistToCell, random, minEdgeDistance);
}



/*void WhiteNoise_half(half2 UV, out half Out){
	Out = frac(sin(dot(UV, half2(12.9898, 78.233))) * 43758.5453);
}*/
#endif