using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XS_Utils;
public class MaterialAutoGenerator
{
    const string RGB = "_RGB";
    const string RGBA = "_RGBA";
    const string N = "_N";
    const string HOR = "_HOR";
    const string M = "_M";
    const string E = "_E";
    const string ME = "_ME";
    const string XS_OBJECTE = "Shader Graphs/XS_Objecte";
    const string XS_ENTORN = "Shader Graphs/XS_Entorn";

    public enum Toon
    {
        _,
        Alpha,
        Emision,
        EmisionAlpha,
        Metal,
        MetalAlpha,
        MetalEmision,
        MetalEmisionAlpha,
        Simple,
        SimpleAlpha,
        SimpleNormal,
        SimpleNormalAlpha
    }

    [MenuItem("Assets/XidoStudio/Material/Objecte")]
    static void CreateObjectMaterial() => CreateMaterial(true);
    [MenuItem("Assets/XidoStudio/Material/Entorn")]
    static void CreateEntornMaterial() => CreateMaterial(false);

    static void CreateMaterial(bool _objecte)
    {
        //Chech what i selected to stop the process if needded.
        if(Selection.activeObject == null)
        {
            Debugar.LogError("You didn't select a texture");
            return;
        }
        if(Selection.activeObject.GetType() != typeof(Texture2D))
        {
            Debugar.LogError("This object is not a Texture");
            return;
        }

        bool objecte = _objecte;
        Toon toonSelected = Toon._;

        //Get all the strings needed to find the textures
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        string selectedObject = Selection.activeObject.name;
        string folder = path.Split(selectedObject)[0];
        string name = selectedObject.Split('_')[0];

        //Check if there is an already created material in the folder with the same name and delete it if needded. 
        object alreadyCreated = AssetDatabase.LoadAssetAtPath($"{folder}{name}.asset", typeof(object));
        if(alreadyCreated != null)
        {
            Debugar.Log("¡¡¡Destroy the old material with the same name to aboud conflicts!!!");
            AssetDatabase.DeleteAsset($"{folder}{name}.asset");
        }

        //Try to find all textures in the same folder
        string rgbJPG = $"{folder}{name}{RGB}.jpg";
        string rgbPNG = $"{folder}{name}{RGB}.png";
        Texture2D rgb = AssetDatabase.LoadAssetAtPath<Texture2D>(rgbJPG);
        if(rgb == null) rgb = AssetDatabase.LoadAssetAtPath<Texture2D>(rgbPNG);

        string rgbaPNG = $"{folder}{name}{RGBA}.png";
        Texture2D rgba = AssetDatabase.LoadAssetAtPath<Texture2D>(rgbaPNG);

        if(rgb == null && rgba == null){
            Debugar.Log("There is not the basic texture RGB o RGBA. I'll find one without sufix...");

            rgb = AssetDatabase.LoadAssetAtPath<Texture2D>($"{folder}{name}.jpg");
            if (rgb == null) rgba = AssetDatabase.LoadAssetAtPath<Texture2D>($"{folder}{name}.png");
        }

        string nJPG = $"{folder}{name}{N}.jpg";
        string nPNG = $"{folder}{name}{N}.png";
        Texture2D n = AssetDatabase.LoadAssetAtPath<Texture2D>(nJPG);
        if (n == null) n = AssetDatabase.LoadAssetAtPath<Texture2D>(nPNG);

        string horJPG = $"{folder}{name}{HOR}.jpg";
        string horPNG = $"{folder}{name}{HOR}.png";
        Texture2D hor = AssetDatabase.LoadAssetAtPath<Texture2D>(horJPG);
        if (hor == null) hor = AssetDatabase.LoadAssetAtPath<Texture2D>(horPNG);

        string mJPG = $"{folder}{name}{M}.jpg";
        string mPNG = $"{folder}{name}{M}.png";
        Texture2D m = AssetDatabase.LoadAssetAtPath<Texture2D>(mJPG);
        if (m == null) m = AssetDatabase.LoadAssetAtPath<Texture2D>(mPNG);

        string eJPG = $"{folder}{name}{E}.jpg";
        string ePNG = $"{folder}{name}{E}.png";
        Texture2D e = AssetDatabase.LoadAssetAtPath<Texture2D>(eJPG);
        if (e == null) e = AssetDatabase.LoadAssetAtPath<Texture2D>(ePNG);

        string meJPG = $"{folder}{name}{ME}.jpg";
        string mePNG = $"{folder}{name}{ME}.png";
        Texture2D me = AssetDatabase.LoadAssetAtPath<Texture2D>(meJPG);
        if (me == null) me = AssetDatabase.LoadAssetAtPath<Texture2D>(mePNG);


        //Analize wicth textures found
        if (rgb != null & rgba == null & n != null & hor != null & m == null & e == null & me == null) toonSelected = Toon._;
        else if (rgb == null & rgba != null & n != null & hor != null & m == null & e == null & me == null) toonSelected = Toon.Alpha;
        else if (rgb != null & rgba == null & n != null & hor != null & m == null & e != null & me == null) toonSelected = Toon.Emision;
        else if (rgb == null & rgba != null & n != null & hor != null & m == null & e != null & me == null) toonSelected = Toon.EmisionAlpha;
        else if (rgb != null & rgba == null & n != null & hor != null & m != null & e == null & me == null) toonSelected = Toon.Metal;
        else if (rgb == null & rgba != null & n != null & hor != null & m != null & e == null & me == null) toonSelected = Toon.MetalAlpha;
        else if (rgb != null & rgba == null & n != null & hor != null & m == null & e == null & me != null) toonSelected = Toon.MetalEmision;
        else if (rgb == null & rgba != null & n != null & hor != null & m == null & e == null & me != null) toonSelected = Toon.MetalEmisionAlpha;
        else if (rgb != null & rgba == null & n == null & hor == null & m == null & e == null & me == null) toonSelected = Toon.Simple;
        else if (rgb == null & rgba != null & n == null & hor == null & m == null & e == null & me == null) toonSelected = Toon.SimpleAlpha;
        else if (rgb != null & rgba == null & n != null & hor == null & m == null & e == null & me == null) toonSelected = Toon.SimpleNormal;
        else if (rgb == null & rgba != null & n != null & hor == null & m == null & e == null & me == null) toonSelected = Toon.SimpleNormalAlpha;


        //Fill up the objects textures
        string tipus = objecte ? XS_OBJECTE : XS_ENTORN;
        Material material = new Material(Shader.Find($"{tipus}{toonSelected}"));
        switch (toonSelected)
        {
            case Toon._:
                material.SetTexture(RGB, rgb);
                material.SetTexture(N, n);
                material.SetTexture(HOR, hor);
                break;
            case Toon.Alpha:
                material.SetTexture(RGBA, rgba);
                material.SetTexture(N, n);
                material.SetTexture(HOR, hor);
                break;
            case Toon.Emision:
                material.SetTexture(RGB, rgb);
                material.SetTexture(N, n);
                material.SetTexture(HOR, hor);
                material.SetTexture(E, e);
                break;
            case Toon.EmisionAlpha:
                material.SetTexture(RGBA, rgba);
                material.SetTexture(N, n);
                material.SetTexture(HOR, hor);
                material.SetTexture(E, e);
                break;
            case Toon.Metal:
                material.SetTexture(RGB, rgb);
                material.SetTexture(N, n);
                material.SetTexture(HOR, hor);
                material.SetTexture(M, m);
                break;
            case Toon.MetalAlpha:
                material.SetTexture(RGBA, rgba);
                material.SetTexture(N, n);
                material.SetTexture(HOR, hor);
                material.SetTexture(M, m);
                break;
            case Toon.MetalEmision:
                material.SetTexture(RGB, rgb);
                material.SetTexture(N, n);
                material.SetTexture(HOR, hor);
                material.SetTexture(ME, me);
                break;
            case Toon.MetalEmisionAlpha:
                material.SetTexture(RGBA, rgba);
                material.SetTexture(N, n);
                material.SetTexture(HOR, hor);
                material.SetTexture(ME, me);
                break;
            case Toon.Simple:
                material.SetTexture(RGB, rgb);
                break;
            case Toon.SimpleAlpha:
                material.SetTexture(RGBA, rgba);
                break;
            case Toon.SimpleNormal:
                material.SetTexture(RGB, rgb);
                material.SetTexture(N, n);
                break;
            case Toon.SimpleNormalAlpha:
                material.SetTexture(RGBA, rgba);
                material.SetTexture(N, n);
                break;
            default:
                break;
        }

        //Create the actual objecte
        AssetDatabase.CreateAsset(material, $"{folder}{name}.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = material;
    }
}
