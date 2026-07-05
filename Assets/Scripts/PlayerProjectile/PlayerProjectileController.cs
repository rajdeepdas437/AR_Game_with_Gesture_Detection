using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    [SerializeField] int damage;
    private ARPlayerHealthManager aRPlayerHealthManager;
    [SerializeField] int QiAmount;

    void Start()
    {
        aRPlayerHealthManager = FindAnyObjectByType<ARPlayerHealthManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            aRPlayerHealthManager.CollectQi(QiAmount);
            other.GetComponentInParent<EnemyController>().DamageEnemy(damage);
        }
        Destroy(transform.parent.gameObject);
    }
}
