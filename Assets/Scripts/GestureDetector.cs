using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using Mediapipe.Tasks.Vision.HandLandmarker;
using UnityEngine;
using Mediapipe;

public class GestureDetector : MonoBehaviour
{
    // [SerializeField] GameHandTracker handTracker;
    [SerializeField] ARPlayerController aRPlayerController;
    public bool isPinching;
    
    public bool isPalmOpen;
    public bool isPalmClosed;
    private float stateTimer=2f;
    private float stateCounter;
    private bool startTimer=false;
    public bool SpecialGesture;

    public void Pinching(HandLandmarkerResult result)
    {
        if (result.handLandmarks != null && result.handLandmarks.Count > 0)
        {
            var hand = result.handWorldLandmarks[0];

            var thumbTip = hand.landmarks[4];
            var indexTip = hand.landmarks[8];

            Vector2 thumb = new Vector2(thumbTip.x, thumbTip.y);
            Vector2 index = new Vector2(indexTip.x, indexTip.y);

            if (Vector2.Distance(thumb, index) < 0.01f)
            {
                Debug.Log("Pinching");
                isPinching = true;
            }
            else isPinching = false;
        }
    }

    private enum GestureSequence
    {
        WaitingForOpen,
        WaitingForClose,
        WaitingForOpenAgain
    }

    GestureSequence gestureSequence = GestureSequence.WaitingForOpen;

    public void GestureSequenceDetector(HandLandmarkerResult result)
    {
        if(result.handLandmarks == null || result.handLandmarks.Count==0)
            return;
        
        var hand = result.handLandmarks[0];

        isPalmOpen = hand.landmarks[8].y < hand.landmarks[6].y &&
        hand.landmarks[12].y < hand.landmarks[10].y &&
        hand.landmarks[16].y < hand.landmarks[14].y &&
        hand.landmarks[20].y < hand.landmarks[18].y;

        isPalmClosed = hand.landmarks[8].y > hand.landmarks[6].y &&
        hand.landmarks[12].y > hand.landmarks[10].y &&
        hand.landmarks[16].y > hand.landmarks[14].y &&
        hand.landmarks[20].y > hand.landmarks[18].y;

        if(stateCounter>0)
        {
            switch(gestureSequence)
            {
                case GestureSequence.WaitingForOpen :

                    if(isPalmOpen)
                    {
                        SpecialGesture=false;
                        gestureSequence = GestureSequence.WaitingForClose;
                        startTimer=true;
                        Debug.Log("Open Palm detected");
                    }

                    break;

                case GestureSequence.WaitingForClose :

                    if(isPalmClosed)
                    {
                        gestureSequence = GestureSequence.WaitingForOpenAgain;
                        Debug.Log("Close Palm detected");
                    }

                    break;

                case GestureSequence.WaitingForOpenAgain :

                    if(isPalmOpen)
                    {
                        gestureSequence = GestureSequence.WaitingForOpen;
                        SpecialGesture=true;
                        startTimer=false;
                        Debug.Log("Gesture sequence complete");
                    }

                    break;    
            }
        }
        else
        {
            gestureSequence = GestureSequence.WaitingForOpen;
            Debug.Log("Time out!");
            startTimer = false;
        }
    }

    void Update()
    {
        if(!startTimer)
        {
            stateCounter = stateTimer;
        }
        else stateCounter -= Time.deltaTime;
    }

    IEnumerator SpecialGestureSequence()
    {
        Debug.Log("Coroutine started!");
        
        yield return new WaitForSeconds(2f);
        SpecialGesture=false;
    }
    
}
