
void GetNeigbourWithLargestAlpha_float(UnityTexture2D baseTexture, float2 baseTextureUV, float2 baseTextureTexelSize, float currentAlpha, int searchWidth, float outlineWidth, out float alpha){
    alpha = currentAlpha;
    float2 texelSize = (outlineWidth / searchWidth) * baseTextureTexelSize.xy;

    for (int x = -searchWidth; x <= searchWidth; x++){
        for (int y = -searchWidth; y <= searchWidth; y++){
            if(x == 0 && y == 0)
                continue;

            float2 offset = float2(x,y) * texelSize;
            float4 neighbour = tex2D(baseTexture, baseTextureUV + offset);
            alpha = max(alpha, neighbour.a);
        }
    }
    //alpha = alpha;
    //return alpha;
}

bool IsOutline(float currentAlpha, float largestNeighbourAlpha, out bool outline){
    if(currentAlpha < 0.5 && largestNeighbourAlpha >= 0.5){
        return true;
    }
    return false;
}

