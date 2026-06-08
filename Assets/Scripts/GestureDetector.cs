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
    private bool canDoSpecialAttack=true;
    [SerializeField] float SpecialAttakCooldown=5f;
    private float SpecialAttackTimer;

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

        if(stateCounter>0 && canDoSpecialAttack)
        {
            switch(gestureSequence)
            {
                case GestureSequence.WaitingForOpen :

                    if(isPalmOpen)
                    {
                        startTimer=true;          //start timer for whole sequence
                        gestureSequence = GestureSequence.WaitingForClose;
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
                        SpecialGesture=true;     //instantiate sphere
                        startTimer=false;        //stop the timer
                        canDoSpecialAttack=false;         //start cooldown
                        gestureSequence = GestureSequence.WaitingForOpen;
                        Debug.Log("Gesture sequence complete");
                    }

                    break;    
            }
        }
        if(stateCounter<=0)
        {
            gestureSequence = GestureSequence.WaitingForOpen;
            Debug.Log("Time out!");
            startTimer = false;
        }
    }

    void Update()
    {
        //Manage sequence timer
        if(!startTimer)
        {
            stateCounter = stateTimer;
        }
        else stateCounter -= Time.deltaTime;

        //Manage cooldown timer
        if(!canDoSpecialAttack)
        {
            SpecialAttackTimer-=Time.deltaTime;
            if(SpecialAttackTimer<=0)
            {
                canDoSpecialAttack=true;
            }
        }
        else SpecialAttackTimer = SpecialAttakCooldown;
    }
}
