using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Slider specialAttackSlider;
    public TextMeshProUGUI specialAttackText;

    void Start()
    {
        instance=this;
    }


}
