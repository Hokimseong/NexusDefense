using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public int health = 100;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}
