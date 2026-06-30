using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameScript : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.5f;

    private Renderer rend;
    private Material[] mats;

    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        mats = rend.materials;
    }

    void Update()
    {
        foreach (Material mat in mats)
        {
            Vector2 offset = mat.GetTextureOffset("_EmissionMap");
            offset.y += scrollSpeed * Time.deltaTime;
            mat.SetTextureOffset("_EmissionMap", offset);
        }
    }
}
