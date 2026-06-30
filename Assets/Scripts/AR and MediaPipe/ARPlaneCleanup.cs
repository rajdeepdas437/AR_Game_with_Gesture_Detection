using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARPlaneCleanup : MonoBehaviour
{
    private ARPlaneManager planeManager;

    void Start()
    {
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void Update()
    {
        foreach (var plane in planeManager.trackables)
        {
            if (plane.subsumedBy != null)
            {
                plane.gameObject.SetActive(false);
            }
        }
    }
}
