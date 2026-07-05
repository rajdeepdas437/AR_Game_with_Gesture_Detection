using System;
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
    private float healCooldown=1f;
    private float healDuration=2f;
    [SerializeField] ARPlayerHealthManager aRPlayerHealthManager;
    private bool canHeal=true;
    private float elapsedTime;
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
        if(gestureDetector.isHealSign && canHeal && elapsedTime < healDuration)
        {
            Debug.Log("Healing");
            elapsedTime += Time.deltaTime;
            float t = elapsedTime/healDuration;
            UIManager.instance.easeQiSlider.value = Mathf.Lerp(aRPlayerHealthManager.CurrentQi(), aRPlayerHealthManager.CurrentQi()-20, t);
            UIManager.instance.mainQiSlider.value = aRPlayerHealthManager.CurrentQi()-20;
            Debug.Log("easeQiSlider value = " + UIManager.instance.easeQiSlider.value);
            if(UIManager.instance.easeQiSlider.value == aRPlayerHealthManager.CurrentQi()-20)
            {
                Debug.Log("Healed");
                elapsedTime=0f;
                aRPlayerHealthManager.QiToHP();
                StartCoroutine(HealCooldown());
            }
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

    IEnumerator HealCooldown()
    {
        canHeal = false;
        yield return new WaitForSeconds(healCooldown);
        canHeal = true;
    }


}
