using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using Unity.VisualScripting;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    public GameHandTracker handTracker;
    [SerializeField] GestureDetector gestureDetector;
    void Start()
    {
        
    }

    void Update()
    {
        if(gestureDetector.isPinching)
        {
            transform.position += Vector3.up*Time.deltaTime;
        }
    }
}
