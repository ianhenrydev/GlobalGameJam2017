using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class EnableCameraDepth : MonoBehaviour
{
    private void OnEnable()
    {
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
    }
}
