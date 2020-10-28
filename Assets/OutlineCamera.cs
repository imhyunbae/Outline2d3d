using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OutlineCamera : MonoBehaviour
{
    Camera thisCamera;
    public RenderTexture outlineMap;
    public List<Value> values;
    void Start()
    {
        thisCamera = GetComponent<Camera>();
        if (outlineMap == null)
            outlineMap = new RenderTexture(thisCamera.pixelWidth, thisCamera.pixelHeight, 0);
    }

    void Update()
    {
        SetOutlineCamera();
        thisCamera.Render();
        SetRenderingCamera();
    }

    void SetOutlineCamera()
    {
        thisCamera.clearFlags = CameraClearFlags.SolidColor;
        thisCamera.backgroundColor = Color.black;
        // no shadow
        thisCamera.useOcclusionCulling = false;
        thisCamera.targetTexture = outlineMap;

        foreach (Value value in values)
            value.ToggleOutline(true);
    }

    void SetRenderingCamera()
    {
        thisCamera.clearFlags = CameraClearFlags.Skybox;
        // shadow
        thisCamera.useOcclusionCulling = true;
        thisCamera.targetTexture = null;
        foreach (Value value in values)
            value.ToggleOutline(false);
    }
}
