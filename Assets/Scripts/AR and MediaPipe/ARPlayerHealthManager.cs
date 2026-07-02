using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPlayerHealthManager : MonoBehaviour
{
    [SerializeField] int maxHealth=100;
    [SerializeField] int currentHealth;
    void Start()
    {
        currentHealth=maxHealth;
        UIManager.instance.healthSlider.maxValue = maxHealth;
        UIManager.instance.healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UIManager.instance.healthSlider.value = currentHealth;
        if(currentHealth<=0)
        {
            UIManager.instance.StartDeathScreen();
            gameObject.SetActive(false);
        }
    }
}
