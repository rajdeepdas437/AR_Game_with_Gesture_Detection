using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using Mediapipe.Tasks.Vision.HandLandmarker;
using UnityEngine;
using Mediapipe;
using System.Numerics;
using Unity.VisualScripting;

public class GestureDetector : MonoBehaviour
{
    // [SerializeField] GameHandTracker handTracker;
    [SerializeField] ARPlayerController aRPlayerController;
    public bool isPinching;
    
    private bool isPalmOpen;
    private bool isPalmClosed;
    private float stateTimer=2f;
    private float stateCounter;
    private bool startTimer=false;
    public bool SpecialGesture;
    private bool canDoSpecialAttack=true;
    [SerializeField] float SpecialAttakCooldown=5f;
    private float SpecialAttackTimer;
    private bool doFadingAnim;

    public bool isPointing;
    public bool isRockSign;
    public bool isHealSign;

    void Start()
    {
        Debug.Log("[GESTURE] GestureDetector script is running");
        UIManager.instance.specialAttackSlider.maxValue=SpecialAttakCooldown;
        UIManager.instance.specialAttackSlider.value=SpecialAttackTimer;
    }

    // public void Pinching(HandLandmarkerResult result)
    // {
    //     if (result.handLandmarks != null && result.handLandmarks.Count > 0)
    //     {
    //         var hand = result.handWorldLandmarks[0];

    //         var thumbTip = hand.landmarks[4];
    //         var indexTip = hand.landmarks[8];

    //         Vector2 thumb = new Vector2(thumbTip.x, thumbTip.y);
    //         Vector2 index = new Vector2(indexTip.x, indexTip.y);

    //         if (Vector2.Distance(thumb, index) < 0.01f)
    //         {
    //             Debug.Log("[GESTURE] Pinching");
    //             isPinching = true;
    //         }
    //         else isPinching = false;
    //     }
    // }

    // public void Pointing(HandLandmarkerResult result)
    // {
    //     if(result.handLandmarks == null)
    //         return;
    
    //     var hand = result.handLandmarks[0];

    //     isPointing = hand.landmarks[8].y < hand.landmarks[6].y &&
    //     hand.landmarks[12].y > hand.landmarks[10].y &&
    //     hand.landmarks[16].y > hand.landmarks[14].y &&
    //     hand.landmarks[20].y > hand.landmarks[18].y &&
    //     hand.landmarks[4].x > hand.landmarks[2].x;
    // }

    public void RockSign(HandLandmarkerResult result)
    {
        if(result.handLandmarks == null)
            return;
        
        var hand = result.handLandmarks[0];

        isRockSign = hand.landmarks[6].y > hand.landmarks[8].y &&
        hand.landmarks[18].y > hand.landmarks[20].y &&
        hand.landmarks[12].y > hand.landmarks[10].y &&
        hand.landmarks[16].y > hand.landmarks[14].y;
    }

    public void HealSign(HandLandmarkerResult result)
    {
        if(result.handLandmarks == null)
            return;
        
        var hand = result.handLandmarks[0];

        var thumb = new UnityEngine.Vector2(hand.landmarks[4].x, hand.landmarks[4].y);
        var ring = new UnityEngine.Vector2(hand.landmarks[14].x, hand.landmarks[14].y);

        isHealSign = hand.landmarks[8].y < hand.landmarks[6].y &&
        hand.landmarks[12].y < hand.landmarks[10].y &&
        hand.landmarks[16].y > hand.landmarks[14].y &&
        hand.landmarks[20].y > hand.landmarks[18].y &&
        (UnityEngine.Vector2.Distance(thumb, ring)<0.05f);
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
        {
            return;
        }
        
        var hand = result.handLandmarks[0];

        isPalmOpen = hand.landmarks[8].y < hand.landmarks[6].y &&
        hand.landmarks[12].y < hand.landmarks[10].y &&
        hand.landmarks[16].y < hand.landmarks[14].y &&
        hand.landmarks[20].y < hand.landmarks[18].y &&
        hand.landmarks[4].x < hand.landmarks[2].x;;

        isPalmClosed = hand.landmarks[8].y > hand.landmarks[6].y &&
        hand.landmarks[12].y > hand.landmarks[10].y &&
        hand.landmarks[16].y > hand.landmarks[14].y &&
        hand.landmarks[20].y > hand.landmarks[18].y &&
        hand.landmarks[4].x > hand.landmarks[2].x;

        if(stateCounter>0 && canDoSpecialAttack)
        {
            switch(gestureSequence)
            {
                case GestureSequence.WaitingForOpen :

                    if(isPalmOpen)
                    {
                        startTimer=true;          //start timer for whole sequence
                        gestureSequence = GestureSequence.WaitingForClose;
                        // Debug.Log("Open Palm detected");
                    }

                    break;

                case GestureSequence.WaitingForClose :

                    if(isPalmClosed)
                    {
                        gestureSequence = GestureSequence.WaitingForOpenAgain;
                        // Debug.Log("Close Palm detected");
                    }

                    break;

                case GestureSequence.WaitingForOpenAgain :

                    if(isPalmOpen)
                    {
                        SpecialGesture=true;     //instantiate sphere
                        startTimer=false;        //stop the timer
                        canDoSpecialAttack=false;         //start cooldown
                        doFadingAnim=true;
                        gestureSequence = GestureSequence.WaitingForOpen;
                        // Debug.Log("Gesture sequence complete");
                    }

                    break;    
            }
        }
        if(stateCounter<=0)
        {
            gestureSequence = GestureSequence.WaitingForOpen;
            // Debug.Log("Time out!");
            startTimer = false;
        }
    }

    void Update()
    {
        if(Time.frameCount==500)
        {
            Debug.Log("[GESTURE] GestureDetector still running");
        }
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
            UIManager.instance.specialAttackSlider.value=SpecialAttakCooldown-SpecialAttackTimer;
            if(SpecialAttackTimer<=0)
            {
                canDoSpecialAttack=true;
            }
            UIManager.instance.specialAttackText.text="On Cooldown ...";
            
        }
        else
        {
            SpecialAttackTimer = SpecialAttakCooldown;
            UIManager.instance.specialAttackSlider.value=SpecialAttackTimer;
            UIManager.instance.specialAttackText.text="Attack Ready!";
        }

        if(doFadingAnim)
        {
            StartCoroutine(CooldownFading());
        } 
    }

    IEnumerator CooldownFading()
    {
        doFadingAnim=false;
        
        for(int i=0; i<5; i++)
        {
            UIManager.instance.specialAttackText.color = 
            new UnityEngine.Color(
                UIManager.instance.specialAttackText.color.r,
                UIManager.instance.specialAttackText.color.g,
                UIManager.instance.specialAttackText.color.b,
                0f
            );

            yield return new WaitForSeconds(0.5f);

            UIManager.instance.specialAttackText.color =
            new UnityEngine.Color(
                UIManager.instance.specialAttackText.color.r,
                UIManager.instance.specialAttackText.color.g,
                UIManager.instance.specialAttackText.color.b,
                255f
            );

            yield return new WaitForSeconds(0.5f);
        }
    }
}
