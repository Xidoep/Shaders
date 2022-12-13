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
        int size = 32;
        TextureFormat format = TextureFormat.RGBA32;
        TextureWrapMode wrapMode = TextureWrapMode.Repeat;

        Texture2D noise = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/XidoStudio/Shaders/Utils/noiseTexture (1).png");
        // Create the texture and apply the configuration
        Texture3D texture = new Texture3D(size, size, size, format, false);
        texture.wrapMode = wrapMode;

        // Create a 3-dimensional array to store color data
        Color[] colors = new Color[size * size * size];
        string debug = "";

        // Populate the array so that the x, y, and z values of the texture will map to red, blue, and green colors
        float inverseResolution = 1.0f / (size - 1.0f);
        for (int z = 0; z < size; z++)
        {
            int zOffset = z * size * size;
            for (int y = 0; y < size; y++)
            {
                int yOffset = y * size;
                for (int x = 0; x < size; x++)
                {
                    //colors[x + yOffset + zOffset] = new Color(x * inverseResolution, y * inverseResolution, z * inverseResolution, 1.0f);
                    //colors[x + yOffset + zOffset] = new Color((x + y + z) * (inverseResolution / 3f), 0, 0, 1.0f);
                    //colors[x + yOffset + zOffset] = (Color.white - (Color.white * 0.5f)) * ((x + y + z) * (inverseResolution / 3f));
                    //colors[x + yOffset + zOffset] = (Color.white * -0.5f) + (Color.white * ((x + y + z) * (inverseResolution / 3f))) * 2f;
                    //colors[x + yOffset + zOffset] = new Color( -1f + (((x + y + z) * (inverseResolution / 3f))) * 2f,0,0,1);
                    //debug += new Vector2Int(x + z, y + z).ToString() + "\n";
                    float color = -1f + noise.GetPixel(x + z, y).r * 2;
                    colors[x + yOffset + zOffset] = new Color(color, 0,0, color);
                }
            }
        }
        Debug.Log(debug);

        // Copy the color values to the texture
        texture.SetPixels(colors);

        // Apply the changes to the texture and upload the updated texture to the GPU
        texture.Apply();

        // Save the texture to your Unity Project
        AssetDatabase.DeleteAsset("Assets/XidoStudio/Shaders/Nuvols/Example3DTexture.asset");
        AssetDatabase.CreateAsset(texture, "Assets/XidoStudio/Shaders/Nuvols/Example3DTexture.asset");
    }
}
#endif