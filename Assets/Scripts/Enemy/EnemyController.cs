using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform player;
    private Vector3 moveDirection;
    private Rigidbody rb;
    [SerializeField] float enemySpeed=3f;
    [SerializeField] float attackRange=6f;
    [SerializeField] float attackCooldown=3f;
    [SerializeField] GameObject projectile;
    private bool canThrow;
    [SerializeField] int enemyMaxHealth = 100;
    [SerializeField] int enemyCurrentHealth;

    [SerializeField] Transform Armature;
    private float YRotAngle, z, x;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject enemyPrefab;

    void Start()
    {
        player = FindAnyObjectByType<ARPlayerHealthManager>().transform;
        canThrow=true;
        rb = GetComponent<Rigidbody>();
        enemyCurrentHealth=enemyMaxHealth;    
    }

    
    void Update()
    {
        if(Vector3.Distance(transform.position, player.position)<=attackRange)
        {
            moveDirection=Vector3.zero;
            if(canThrow && player.gameObject.activeInHierarchy)
                StartCoroutine(ThrowProjectile());
        }
        else
        {
            moveDirection = player.position - transform.position;
        }
        moveDirection.Normalize();
        rb.velocity = moveDirection*enemySpeed;

        z = player.position.z - transform.position.z;
        x = player.position.x - transform.position.x;
        YRotAngle = Mathf.Atan2(z, x)*Mathf.Rad2Deg;

        Armature.transform.rotation = Quaternion.Euler(-90f, Armature.transform.position.y - YRotAngle, 90f);
 
        
    }

    IEnumerator ThrowProjectile()
    {
        canThrow=false;
        Instantiate(projectile, firePoint);
        yield return new WaitForSeconds(attackCooldown);
        canThrow=true;
    }

    public void DamageEnemy(int damage)
    {
        enemyCurrentHealth -= damage;
        if(enemyCurrentHealth<=0)
        {
            Destroy(enemyPrefab);
        }
    }
}
