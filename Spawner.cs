using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Reference")]
    public GameObject Enemy;
    public GameObject MySelf;
    public Transform nexusTarget;
    public Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        Spawn();
    }

    void Spawn()
    {
        GameObject enemyObj = Instantiate(Enemy, transform.position, transform.rotation);

        EnemyAI enemyAI = enemyObj.GetComponent<EnemyAI>();
        EnemyTank enemyTank = enemyObj.GetComponentInChildren<EnemyTank>();

        if (enemyAI != null && enemyTank != null)
        {
            enemyAI.target = nexusTarget;
            enemyTank.nexusTarget = nexusTarget;
            enemyTank.player = player;
        }
        Invoke("OnDisable", 2f);
    }

    void OnDisable()
    {
        MySelf.SetActive(false);
    }

}
