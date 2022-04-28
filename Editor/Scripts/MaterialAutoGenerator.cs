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
    const string NE = "_NE";
    const string HOR = "_HOR";
    const string HORM = "_HORM";
    const string HORE = "_HORE";
    const string RGBE = "_RGBE";

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
        string name = $"_{selectedObject.Split('_')[1]}";

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
        string nTIF = $"{folder}{name}{N}.tif";
        Texture2D n = AssetDatabase.LoadAssetAtPath<Texture2D>(nJPG);
        if (n == null) n = AssetDatabase.LoadAssetAtPath<Texture2D>(nPNG);
        if (n == null) n = AssetDatabase.LoadAssetAtPath<Texture2D>(nTIF);

        string horJPG = $"{folder}{name}{HOR}.jpg";
        string horPNG = $"{folder}{name}{HOR}.png";
        string horTIF = $"{folder}{name}{HOR}.tif";
        Texture2D hor = AssetDatabase.LoadAssetAtPath<Texture2D>(horJPG);
        if (hor == null) hor = AssetDatabase.LoadAssetAtPath<Texture2D>(horPNG);
        if (hor == null) hor = AssetDatabase.LoadAssetAtPath<Texture2D>(horTIF);




        string hormJPG = $"{folder}{name}{HORM}.jpg";
        string hormPNG = $"{folder}{name}{HORM}.png";
        string hormTIF = $"{folder}{name}{HORM}.tif";
        Texture2D horm = AssetDatabase.LoadAssetAtPath<Texture2D>(hormJPG);
        if (horm == null) horm = AssetDatabase.LoadAssetAtPath<Texture2D>(hormPNG);
        if (horm == null) horm = AssetDatabase.LoadAssetAtPath<Texture2D>(hormTIF);

        string horeJPG = $"{folder}{name}{HORE}.jpg";
        string horePNG = $"{folder}{name}{HORE}.png";
        string horeTIF = $"{folder}{name}{HORE}.tif";
        Texture2D hore = AssetDatabase.LoadAssetAtPath<Texture2D>(horeJPG);
        if (hore == null) hore = AssetDatabase.LoadAssetAtPath<Texture2D>(horePNG);
        if (hore == null) hore = AssetDatabase.LoadAssetAtPath<Texture2D>(horeTIF);

        string neJPG = $"{folder}{name}{NE}.jpg";
        string nePNG = $"{folder}{name}{NE}.png";
        string neTIF = $"{folder}{name}{NE}.tif";
        Texture2D ne = AssetDatabase.LoadAssetAtPath<Texture2D>(neJPG);
        if (ne == null) ne = AssetDatabase.LoadAssetAtPath<Texture2D>(nePNG);
        if (ne == null) ne = AssetDatabase.LoadAssetAtPath<Texture2D>(neTIF);
        /*
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
        */

        //Analize wicth textures found
        /*if (rgb != null & rgba == null & n != null & hor != null & m == null & e == null & me == null) toonSelected = Toon._;
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
        */

        if      (rgb != null & rgba == null & n != null & hor != null & horm == null & hore == null & ne == null) toonSelected = Toon._;
        else if (rgb == null & rgba != null & n != null & hor != null & horm == null & hore == null & ne == null) toonSelected = Toon.Alpha;
        else if (rgb != null & rgba == null & n != null & hor == null & horm == null & hore != null & ne == null) toonSelected = Toon.Emision;
        else if (rgb == null & rgba != null & n != null & hor == null & horm == null & hore != null & ne == null) toonSelected = Toon.EmisionAlpha;
        else if (rgb != null & rgba == null & n != null & hor == null & horm != null & hore == null & ne == null) toonSelected = Toon.Metal;
        else if (rgb == null & rgba != null & n != null & hor == null & horm != null & hore == null & ne == null) toonSelected = Toon.MetalAlpha;
        else if (rgb != null & rgba == null & n != null & hor == null & horm != null & hore == null & ne != null) toonSelected = Toon.MetalEmision;
        else if (rgb == null & rgba != null & n != null & hor == null & horm != null & hore == null & ne != null) toonSelected = Toon.MetalEmisionAlpha;
        else if (rgb != null & rgba == null & n == null & hor == null & horm == null & hore == null & ne == null) toonSelected = Toon.Simple;
        else if (rgb == null & rgba != null & n == null & hor == null & horm == null & hore == null & ne == null) toonSelected = Toon.SimpleAlpha;
        else if (rgb != null & rgba == null & n != null & hor == null & horm == null & hore == null & ne == null) toonSelected = Toon.SimpleNormal;
        else if (rgb == null & rgba != null & n != null & hor == null & horm == null & hore == null & ne == null) toonSelected = Toon.SimpleNormalAlpha;


        //Fill up the objects textures
        string tipus = objecte ? XS_OBJECTE : XS_ENTORN;
        Material material = new Material(Shader.Find($"{tipus}{toonSelected}"));

        Debugar.Log(toonSelected.ToString());
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
                material.SetTexture(HORE, hore);
                break;
            case Toon.EmisionAlpha:
                material.SetTexture(RGBA, rgba);
                material.SetTexture(N, n);
                material.SetTexture(HORE, hore);
                break;
            case Toon.Metal:
                material.SetTexture(RGB, rgb);
                material.SetTexture(N, n);
                material.SetTexture(HORM, horm);
                break;
            case Toon.MetalAlpha:
                material.SetTexture(RGBA, rgba);
                material.SetTexture(N, n);
                material.SetTexture(HORM, horm);
                break;
            case Toon.MetalEmision:
                material.SetTexture(RGB, rgb);
                material.SetTexture(NE, ne);
                material.SetTexture(HORM, horm);
                break;
            case Toon.MetalEmisionAlpha:
                material.SetTexture(RGBA, rgba);
                material.SetTexture(NE, ne);
                material.SetTexture(HORM, horm);
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
