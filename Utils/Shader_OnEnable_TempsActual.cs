using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shader_OnEnable_TempsActual : MonoBehaviour
{
    [SerializeField] Image image;
    private void OnEnable()
    {
        image.material.SetFloat("_TempsActual", Time.time);
    }
}
