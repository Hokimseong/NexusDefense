using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject turretMissile;
    public Transform firePoint;
    public float fireRate = 3f;
    public float missileRate = 8f;
    public float detectionRange = 50f;
    public float bulletRange = 50f;
    public float missileRange = 50f;
    public bool unlockMissile;
    public int MissileCount;

    private Transform target;
    private float nextFireTime;
    private float nextMissileTime;

    void Update()
    {
        FindClosestEnemy();

        if (target)
        {
            RotateTowardsTarget();

            if (Vector3.Distance(transform.position, target.position) <= bulletRange && Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
            if (Vector3.Distance(transform.position, target.position) <= missileRange && Time.time >= nextMissileTime)
            {
                if (unlockMissile)
                {
                    FireMissile();
                    nextMissileTime = Time.time + missileRate;
                }
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
                if (distance < shortestDistance && distance <= detectionRange)
                {
                    shortestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }
        }
        target = closestEnemy;
    }

    void RotateTowardsTarget()
    {
        if (!target) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Fire()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    void FireMissile()
    {
        StartCoroutine(FireMissileCoroutine());
    }

    IEnumerator FireMissileCoroutine()
    {
        for (int i = 0; i < MissileCount; i++)
        {
            Vector3 missilePosition = transform.position + new Vector3(0, 4f, 0);
            Instantiate(turretMissile, missilePosition, firePoint.rotation);
            yield return new WaitForSeconds(0.8f); 
        }
    }

}
