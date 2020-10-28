using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Value : MonoBehaviour
{
    public Color originalColor;
    public Color idColor;
    Material material;
    void Start()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer)
            material = meshRenderer.material;
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
            material = spriteRenderer.material;

        if (material != null && material.HasProperty("_IDColor"))
            material.SetColor("_IDColor", idColor);
    }
    
    public void ToggleOutline(bool IsOutline)
    {
        if (material != null)
            material.SetInt("_IsOutline", IsOutline ? 1 : 0);
    }
}
