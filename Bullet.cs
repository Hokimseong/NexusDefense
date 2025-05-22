using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Attributes")]
    public float speed = 50f;
    public int damage = 20;
    public float lifeTime = 2f;
    public float bulletForce = 5;

    [Header("Effects")]
    public GameObject hitEffectPrefab;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        string otherTag = other.gameObject.tag;

        if (otherTag == "Enemy")
        {
            if(gameObject.tag != "eBullet")
            {
                Vector3 forceDirection = other.transform.position - transform.position;
                other.GetComponent<EnemyAI>().TakeDamage(damage, bulletForce, forceDirection);
                SpawnHitEffect();
                if (gameObject.tag != "skillBullet") Destroy(gameObject);
            }
            
        }
        else if(otherTag == "Player")
        {
            if(gameObject.tag == "eBullet")
            {
                other.GetComponent<PlayerController>().TakeDamage(damage);
                SpawnHitEffect();
                Destroy(gameObject);
            }
        }
        else if(otherTag == "Bullet")
        {

        }
        else if(otherTag == "Turret")
        {
            if(gameObject.tag == "eBullet")
            {
                other.GetComponent<TurretAI>().TakeDamage(damage);
                SpawnHitEffect();
                Destroy(gameObject);
            }
        }
        else if (otherTag == "eBullet")
        {
            if (gameObject.tag == "skillBullet") Destroy(other.gameObject);
            else
            {
                SpawnHitEffect();
                Destroy(gameObject);
                Destroy(other.gameObject);
            }
        }
        else if (otherTag == "Turret")
        {
            if (gameObject.tag != "Bullet")
            {
                other.GetComponent<TurretAI>().TakeDamage(damage);
                SpawnHitEffect();
                Destroy(gameObject);
            }
        }
        else if (otherTag == "Nexus")
        {
            if (gameObject.tag == "eBullet")
            {
                GameManager.instance.DamageNexus(damage);
            }
            SpawnHitEffect();
            Destroy(gameObject);
        }
        else
        {
            SpawnHitEffect();
            Destroy(gameObject);
        }
        
    }
    void SpawnHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, transform.rotation * Quaternion.Euler(0, 180, 0));
            Destroy(effect, 1f); // 1.5초 후 삭제
        }
    }

}

