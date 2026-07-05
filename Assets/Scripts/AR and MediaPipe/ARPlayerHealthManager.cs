using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPlayerHealthManager : MonoBehaviour
{
    [SerializeField] int maxHealth=100;
    [SerializeField] int currentHealth;
    [SerializeField] int qi;
    private int maxQi = 100;
    void Start()
    {
        currentHealth=maxHealth;
        UIManager.instance.healthSlider.maxValue = maxHealth;
        UIManager.instance.healthSlider.value = currentHealth;


        UIManager.instance.mainQiSlider.maxValue = 100;
        UIManager.instance.easeQiSlider.maxValue = 100;
        UIManager.instance.mainQiSlider.value = qi;
        UIManager.instance.easeQiSlider.value = qi;
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

    public void Heal(int healthAmount)
    {
        currentHealth += healthAmount;
        if(currentHealth>=maxHealth)
        {
            currentHealth=maxHealth;
        }
        UIManager.instance.healthSlider.value = currentHealth;
    }

    public void CollectQi(int new_qi)
    {
        qi += new_qi;
        if(qi>=100)
        {
            qi=100;
        }
        UIManager.instance.mainQiSlider.value = qi;
        UIManager.instance.easeQiSlider.value = qi;
    }

    public void QiToHP()
    {
        if(qi>=20)
        {
            qi -= 20;
            Heal(20);
        }
        else return;
    }

    public int CurrentQi()
    {
        return qi;
    }
}
