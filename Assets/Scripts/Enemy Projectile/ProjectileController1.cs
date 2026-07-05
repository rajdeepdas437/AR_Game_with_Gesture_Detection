using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController1 : MonoBehaviour
{
    private Transform player;
    private Vector3 playerDirection;
    private Rigidbody rb;
    [SerializeField] float projectileSpeed=5f;
    [SerializeField] int damage=20;
    [SerializeField] GameObject projectilePrefab;

    void Start()
    {
        player=FindAnyObjectByType<ARPlayerHealthManager>().transform;
        playerDirection=player.position-transform.position;
        rb=GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.velocity = playerDirection*projectileSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<ARPlayerHealthManager>().TakeDamage(damage);
            Destroy(projectilePrefab);
        }
    }
}
