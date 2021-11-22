using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FovCamera : MonoBehaviour
{
    [SerializeField] private Camera camera;

    private void Awake()
    {
        camera.clearFlags = CameraClearFlags.Nothing;
        
        Debug.Log(camera.clearFlags);
    }
}
