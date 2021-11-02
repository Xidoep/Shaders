using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class PelSkinned : MonoBehaviour
    {
        const string KEY_ALTURA = "_altura";
        const string KEY_OFFSET = "_offset";

        Camera cam;

        public SkinnedMeshRenderer skinnedMeshRenderer;

        [Range(0, 31)] public int horizontalStackSize = 20;
        [Range(0.01f, 40)] public float altura;


        //float offset;
        //int layer = 1;

        private Matrix4x4 matrix;
        Mesh baseMesh;
        MaterialPropertyBlock mpb;

        private void Awake()
        {
            baseMesh = new Mesh();
            mpb = new MaterialPropertyBlock();
        }

        private void Start() => Init();
        private void Init() => cam = Camera.main;


        void Update()
        {
            if (cam == null)
                Init();

            matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

            if (skinnedMeshRenderer != null) skinnedMeshRenderer.BakeMesh(baseMesh);

            for (int i = 0; i < horizontalStackSize; i++)
            {
                mpb.SetFloat(KEY_ALTURA, altura);
                mpb.SetFloat(KEY_OFFSET, altura - ((altura / (float)horizontalStackSize) * i));
                Graphics.DrawMesh(baseMesh, matrix, skinnedMeshRenderer.material, 1, cam, 0, mpb, true, false, false);
            }
        }
    }


