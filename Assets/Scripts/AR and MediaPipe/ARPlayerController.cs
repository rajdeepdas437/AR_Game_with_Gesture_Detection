using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class ARPlayerController : MonoBehaviour
{
    [SerializeField] GameObject Cube;
    [SerializeField] GameObject Sphere;
    [SerializeField] Camera arCamera;
    private GestureDetector gestureDetector;
    private bool canInstantiateCube;
    [SerializeField] float Cooldown = 2f;
    private float healTimer=2f;
    private float healCounter;
    [SerializeField] ARPlayerHealthManager aRPlayerHealthManager;
    void Start()
    {
        canInstantiateCube = true;
        gestureDetector = GetComponent<GestureDetector>();
    }

    void Update()
    {
        if(gestureDetector.isRockSign && canInstantiateCube)
        {
           StartCoroutine(CreateCube()); 
        }
        if(gestureDetector.SpecialGesture)
        {
            StartCoroutine(CreateSphere());
        }


        // heal mechanic
        if(gestureDetector.isHealSign)
        {
            healCounter += Time.deltaTime;
            if(healCounter>=healTimer)
            {
                healCounter = 0f;
                aRPlayerHealthManager.QiToHP();
            }
        }
        else
        {
            healCounter=0f;
        }
        
    }


    IEnumerator CreateCube()
    {
        Ray ray = arCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
        canInstantiateCube=false;
        GameObject spawnedCube = Instantiate(Cube, ray.origin, arCamera.transform.rotation);
        spawnedCube.GetComponentInChildren<Rigidbody>().velocity = ray.direction*10f;
        yield return new WaitForSeconds(Cooldown);
        canInstantiateCube=true;
    }

    IEnumerator CreateSphere()
    {
        gestureDetector.SpecialGesture=false;
        Ray ray = arCamera.ViewportPointToRay(new Vector3(0.5f,0.5f,0f));
        GameObject spawnedSphere = Instantiate(Sphere, ray.origin, arCamera.transform.rotation);
        spawnedSphere.GetComponentInChildren<Rigidbody>().velocity=ray.direction*10f;
        yield return new WaitForSeconds(Cooldown);
    }


}
