using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurRenderer : MonoBehaviour
{
    private Camera _blurCam;
    public Material blurMaterial;
    private static readonly int RenTex = Shader.PropertyToID("_RenTex");

    void Start()
    {
        _blurCam = GetComponent<Camera>();
        
        if (_blurCam.targetTexture != null)
        {
            _blurCam.targetTexture.Release();
        }
        
        _blurCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, 1);
        
        blurMaterial.SetTexture(RenTex, _blurCam.targetTexture);
    }
}
