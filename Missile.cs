using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    [Header("References")]
    public GameObject explosionEffect;

    [Header("Attributes")]
    public float speed = 10f;
    public float turnSpeed = 5f;
    public float explosionRadius = 20f;
    public int damage = 50;
    public float lifetime = 5f;
    public float explosionForce = 20f;

    private Transform target;
    private bool isTracking = false;
   
    void Start()
    {
        StartCoroutine(LaunchSequence());
        StartCoroutine(ExplodeAfterLifetime());
        //Destroy(gameObject, lifetime);
    }
    IEnumerator LaunchSequence()
    {
        float startTime = Time.time;
        transform.rotation = Quaternion.LookRotation(Vector3.up);

        while (Time.time - startTime < 0.3f)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
            yield return null;
        }

        isTracking = true;
    }

    IEnumerator ExplodeAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Explode();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTracking)
        {
            FindClosestEnemy();
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
    }
    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null && !enemyAI.isDead)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }
        }
        target = closestEnemy;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !other.CompareTag("Turret"))
        {
            Explode();
        }
    }
    void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearby in colliders)
        {
            Vector3 forceDirection = nearby.transform.position - transform.position;
            nearby.GetComponent<EnemyAI>()?.TakeDamage(damage, explosionForce, forceDirection);
        }
        Destroy(gameObject);
    }
}
