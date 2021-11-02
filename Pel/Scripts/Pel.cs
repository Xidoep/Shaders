using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Pel : MonoBehaviour
    {
        const string KEY_CAPA = "_capa";
        const string KEY_OFFSET = "_offset";

        Camera cam;

        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;

        [Range(0, 31)] public int horizontalStackSize = 20;
        [Range(0.01f, 20)] public float altura;
        public bool sombra;

        //float offset;
        //int layer = 1;

        private Matrix4x4 matrix;
        MaterialPropertyBlock mpb;


        private void Awake() 
        {
            mpb = new MaterialPropertyBlock();
            if (meshFilter == null) meshFilter.GetComponent<MeshFilter>();
            if (meshRenderer == null) meshRenderer.GetComponent<MeshRenderer>();
        } 
        private void Start() => Init();
        private void Init() => cam = Camera.main;


        private void OnEnable()
        {
            if (meshFilter == null) meshFilter.GetComponent<MeshFilter>();
            if (meshRenderer == null) meshRenderer.GetComponent<MeshRenderer>();
        }

        void Update()
        {
            if (cam == null)
                Init();

            matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

            if (mpb == null) mpb = new MaterialPropertyBlock();

            for (int i = 0; i < horizontalStackSize; i++)
            {
                mpb.SetFloat("_altura",  altura);
                mpb.SetFloat(KEY_OFFSET, altura - ((altura / (float)horizontalStackSize) * i));
                Graphics.DrawMesh(meshFilter.mesh, matrix, meshRenderer.material, 1, cam, 0, mpb, sombra, false, false);
            }
        }
    }


