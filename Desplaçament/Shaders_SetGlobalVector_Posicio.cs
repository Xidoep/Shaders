using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Shaders_SetGlobalVector_Posicio : MonoBehaviour
{
    const string POSICIO = "_Posicio";

    void Update()
    {
        XS_Utils.Shaders.SetColor(POSICIO, transform.position);
    }

}
