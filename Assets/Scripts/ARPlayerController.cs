using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using UnityEngine;

public class ARPlayerController : MonoBehaviour
{
    [SerializeField] GameObject Cube;
    [SerializeField] Camera arCamera;
    private GestureDetector gestureDetector;
    private bool canInstantiate;
    [SerializeField] float Cooldown = 2f;
    void Start()
    {
        canInstantiate = true;
        gestureDetector = GetComponent<GestureDetector>();
    }

    void Update()
    {
        if(gestureDetector.isPinching && canInstantiate)
        {
           StartCoroutine(CreateCube()); 
        }
        if(gestureDetector.SpecialGesture)
        {
            StartCoroutine(CreateCube());
        }
        
    }

    public void SpecialAttack()
    {
        
        
    }

    IEnumerator CreateCube()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
        canInstantiate=false;
        GameObject spawnedCube = Instantiate(Cube, ray.origin, arCamera.transform.rotation);
        spawnedCube.GetComponentInChildren<Rigidbody>().velocity = ray.direction*10f;
        yield return new WaitForSeconds(Cooldown);
        canInstantiate=true;
    }
}
