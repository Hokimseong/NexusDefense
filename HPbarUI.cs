using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HPbarUI : MonoBehaviour
{
    public Slider nexusHpbar;
    public Slider playerHpbar;
    public Slider enemyHpbar;

    public int nexusMaxHp = 500;
    public int playerMaxHp = 100;
    public int enemyMaxHp = 100;

    List<Transform> enemies = new List<Transform>();
    List<Slider> enemyHpbars = new List<Slider>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nexusHpbar.value = (float)GameManager.nexusHealth / nexusMaxHp;
        playerHpbar.value = (float)PlayerController.health / playerMaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHPbar();
        FindEnemyObject();
    }

    public void FindEnemyObject()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemy.Length > enemies.Count)
        {
            for (int i = 0; i < enemy.Length; i++)
            {
                if (!enemies.Contains(enemy[i].transform))
                {
                    enemies.Add(enemy[i].transform);
                    Slider eHpbar = Instantiate(enemyHpbar, transform);
                    enemyHpbars.Add(eHpbar);
                }
            }
        }
    }

    public void CheckHPbar()
    {
        nexusHpbar.value = (float)GameManager.nexusHealth / nexusMaxHp;
        playerHpbar.value = (float)PlayerController.health / playerMaxHp;
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null) // 적이 삭제되었으면
            {
                Destroy(enemyHpbars[i].gameObject); // 체력바 UI 삭제
                enemyHpbars.RemoveAt(i);
                enemies.RemoveAt(i);
            }
            else
            { 
                enemyHpbars[i].value = (float)enemies[i].GetComponent<EnemyAI>().health / enemyMaxHp;
                enemyHpbars[i].transform.position = Camera.main.WorldToScreenPoint(enemies[i].position + new Vector3(0, 2.0f, 0));
            }
        }
    }
}
