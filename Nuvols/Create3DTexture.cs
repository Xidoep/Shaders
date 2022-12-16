using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class Create3DTexture : MonoBehaviour
{
    [MenuItem("CreateExamples/3DTexture")]
    static void CreateTexture3D()
    {
        // Configure the texture
        int width = 128;
        int height = 128;
        int depth = 128;
        float scale = 0.012f;
        TextureFormat format = TextureFormat.RGBA32;
        TextureWrapMode wrapMode = TextureWrapMode.Repeat;

        Texture3D texture = new Texture3D(width, height, depth, format, false);
        texture.wrapMode = wrapMode;









        // Create a color array to store the 3D noise values
        Color[] colors = new Color[width * height * depth];

        // Generate the 3D noise values
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {

                    // Warp the noise values using a sine function
                    float noiseValue = PerlinNoise3D(x * scale * 1, y * scale * 1, z * scale * 1) + 0.2f;
                    float noiseValue2 = PerlinNoise3D(x * scale * 2, y * scale * 2, z * scale * 2) - 0.16f;
                    float noiseValue3 = PerlinNoise3D(x * scale * 3, y * scale * 3, z * scale * 3) - 0.16f;
                    float noiseValue4 = PerlinNoise3D(x * scale * 4, y * scale * 4, z * scale * 4) - 0.16f;
                    float noiseValue5 = PerlinNoise3D(x * scale * 5, y * scale * 5, z * scale * 5) - 0.16f;

                    float maxim = Mathf.Clamp(Mathf.Min(((y * 5) / (float)height) -1, (((height - y) * 5) / (float)height)) -1, 0, 3);
                    float noise = (noiseValue - noiseValue2 - noiseValue3 - noiseValue4 - noiseValue5) * maxim;

                    colors[x + y * width + z * width * height] = (new Color(noise, noise, noise, noise) * 5) - (Color.white * 0.1f);
                }
            }
        }

        // Set the texture data
        texture.SetPixels(colors);
        texture.Apply();






        // Save the texture to your Unity Project
        AssetDatabase.DeleteAsset("Assets/XidoStudio/Shaders/Nuvols/Example3DTexture.asset");
        AssetDatabase.CreateAsset(texture, "Assets/XidoStudio/Shaders/Nuvols/Example3DTexture.asset");
    }

    static float PerlinNoise3D(float x, float y, float z)
    {
        y += 1;
        z += 2;
        float xy = _perlin3DFixed(x, y);
        float xz = _perlin3DFixed(x, z);
        float yz = _perlin3DFixed(y, z);
        float yx = _perlin3DFixed(y, x);
        float zx = _perlin3DFixed(z, x);
        float zy = _perlin3DFixed(z, y);
        return xy * xz * yz * yx * zx * zy;
    }
    static float _perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
        //return Mathf.PI * Mathf.PerlinNoise(a, b);
        //return Mathf.PerlinNoise(a, b);
    }
}
#endif
