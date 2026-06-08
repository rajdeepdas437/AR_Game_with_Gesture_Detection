using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using UnityEngine;

public class ARPlayerController : MonoBehaviour
{
    [SerializeField] GameObject Cube;
    [SerializeField] GameObject Sphere;
    [SerializeField] Camera arCamera;
    private GestureDetector gestureDetector;
    private bool canInstantiateCube;
    [SerializeField] float Cooldown = 2f;
    void Start()
    {
        canInstantiateCube = true;
        gestureDetector = GetComponent<GestureDetector>();
    }

    void Update()
    {
        if(gestureDetector.isPinching && canInstantiateCube)
        {
           StartCoroutine(CreateCube()); 
        }
        if(gestureDetector.SpecialGesture)
        {
            StartCoroutine(CreateSphere());
        }
        
    }


    IEnumerator CreateCube()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
        canInstantiateCube=false;
        GameObject spawnedCube = Instantiate(Cube, ray.origin, arCamera.transform.rotation);
        spawnedCube.GetComponentInChildren<Rigidbody>().velocity = ray.direction*10f;
        yield return new WaitForSeconds(Cooldown);
        canInstantiateCube=true;
    }

    IEnumerator CreateSphere()
    {
        gestureDetector.SpecialGesture=false;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
        GameObject spawnedSphere = Instantiate(Sphere, ray.origin, arCamera.transform.rotation);
        spawnedSphere.GetComponent<Rigidbody>().velocity=ray.direction*10f;
        yield return new WaitForSeconds(Cooldown);
    }


}
