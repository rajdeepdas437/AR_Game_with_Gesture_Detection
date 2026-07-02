using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    [SerializeField] int damage;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().DamageEnemy(damage);
        }
        Destroy(gameObject);
    }
}
