using UnityEngine;
using System.Collections;

public class EnemyTank : MonoBehaviour
{
    public Transform nexusTarget;
    public Transform currentTarget;
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;
    //public GameObject muzzleFlashPrefab;

    public float nexusRange = 40f;
    public float detectionRange = 80f;
    public float attackRange = 80f;
    public float fireRate = 3f;
    private Transform turret;
    private float nextFireTime = 0f;
    private EnemyAI enemyAI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTarget = nexusTarget;
        // StartCoroutine(FindTurret());
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectTargets();

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
        if (detectionRange > distanceToTarget)
        {
            RotateTowardsTarget();
        }

        if (attackRange > distanceToTarget)
        {
            if (Time.time >= nextFireTime)
            {
                StartCoroutine(FireBullet());
                nextFireTime = Time.time + fireRate;
            }
        }
    }
    void DetectTargets()
    {
        GameObject turretObj = GameObject.FindWithTag("Turret");
        if (turretObj) turret = turretObj.transform;

        bool nexusDetected = nexusTarget && Vector3.Distance(transform.position, nexusTarget.position) <= nexusRange;
        bool playerDetected = player && Vector3.Distance(transform.position, player.position) <= detectionRange;
        bool turretDetected = turret && Vector3.Distance(transform.position, turret.position) <= detectionRange;

        if (nexusDetected) currentTarget = nexusTarget; 
        else if (playerDetected) currentTarget = player; 
        else if (turretDetected) currentTarget = turret;
        else currentTarget = nexusTarget;
    }
    void RotateTowardsTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            direction.y = 0; 
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 50f);
        }
    }

    IEnumerator FireBullet()
    {
        yield return new WaitForSeconds(1f); 

        if(enemyAI.isDead) yield break; 

        if (currentTarget != null)
        {
            Vector3 direction = (currentTarget.position - firePoint.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 2, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);

            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

    }
}
