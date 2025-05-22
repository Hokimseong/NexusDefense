using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.Cinemachine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform target;
    public Rigidbody rb;
    public GameObject explosionEffect;

    [Header("Attributes")]
    public int health = 100;
    public bool isDead = false;
    //private Animator animator;
    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent.SetDestination(target.position);
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void TakeDamage(int damage, float force, Vector3 forceDirection)
    {
        health -= damage;
        if (health <= 0)
        {            agent.enabled = false;
            rb.isKinematic = false;
            rb.AddForce(forceDirection.normalized * force, ForceMode.Impulse);
            StartCoroutine(DelayedExplosionEffect());
        }
    }
    IEnumerator DelayedExplosionEffect()
    {
        if (!isDead)
        {
            isDead = true;
            yield return new WaitForSeconds(1f);
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            CameraManager.instance.CameraShake(impulseSource);
            Destroy(gameObject);
            GameManager.instance.EnemyDefeated();
        }
       
    }
}
