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
    private GameObject deathScreen;
    public Slider healthSlider;

    void Awake()
    {
        instance=this;
        deathScreen=transform.Find("DeathScreen").gameObject;
    }

    public void StartDeathScreen()
    {
        deathScreen.SetActive(true);
    }


}
