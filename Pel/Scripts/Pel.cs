using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pel : MonoBehaviour
{
    const string KEY_ALTURA = "_altura";
    const string KEY_OFFSET = "_offset";

    Camera cam;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;

    [Range(0, 31)] [SerializeField] int horizontalStackSize = 20;
    [Range(0.01f, 20)] [SerializeField] float altura = 3;
    [SerializeField] bool sombra;

    Matrix4x4[] matrix;
    MaterialPropertyBlock mpb;



    private void Start() => Init();
    private void Init() 
    {
        cam = Camera.main;
    } 


    private void OnEnable()
    {
        mpb = new MaterialPropertyBlock();
        if (meshFilter == null) meshFilter.GetComponent<MeshFilter>();
        if (meshRenderer == null) meshRenderer.GetComponent<MeshRenderer>();

        matrix = new Matrix4x4[1];

        if (cam == null)
            Init();
    }

    void LateUpdate()
    {
        matrix[0] = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        for (int i = 0; i < horizontalStackSize; i++)
        {
            mpb.SetFloat(KEY_ALTURA,  altura);
            mpb.SetFloat(KEY_OFFSET, altura - ((altura / (float)horizontalStackSize) * i));
            Graphics.DrawMeshInstanced(meshFilter.mesh, 0, meshRenderer.material, matrix, 1, mpb);
        }
    }
}


