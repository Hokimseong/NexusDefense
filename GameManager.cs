using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Intro,
    Playing,
    StageClear,
    GameOver
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state;

    [Header("Reference")]
    public GameObject introUI;
    public GameObject gameOverUI;
    public GameObject SkillLvUI;
    public GameObject player;
    public GameObject turret;
    public GameObject[] EnemySpawner;
    public TextMeshProUGUI stageUI;
    public TextMeshProUGUI enemyUI;
    public Button S4Btton;
    static public int nexusHealth = 500; // 넥서스 체력
    public int CurrentStage = 1;
    public TextMeshProUGUI S2LevelText;
    public TextMeshProUGUI S3LevelText;
    public TextMeshProUGUI S4LevelText;
    public int S2Level;
    public int S3Level;
    public int S4Level;

    [Header("Gameplay")]
    private float stageTimer = 0f;
    private float spawnInterval = 5f;
    private float maxSpawnDuration;
    private int enemiesRemaining = 0;
    private int spawningEnemies = 0;
    public int SpawnCount = 2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    void Start()
    {
        Intro();
        SkillLvUI.SetActive(false);
        turret.GetComponentInChildren<Turret>().unlockMissile = false;
        turret.GetComponentInChildren<Turret>().MissileCount = 1;
        S2Level = S3Level = S4Level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        stageUI.text = CurrentStage.ToString();
        enemyUI.text = enemiesRemaining.ToString();
        S2LevelText.text = S2Level.ToString();
        S3LevelText.text = S3Level.ToString();
        S4LevelText.text = S4Level.ToString();
        if (state == GameState.Intro && Input.GetKeyDown(KeyCode.Return))
        {
            StartStage();
        }

        if (state == GameState.Playing)
        {
            stageTimer += Time.deltaTime;
            if (enemiesRemaining <= 0)
            {
                StageClear();
            }
        }

        if(state == GameState.StageClear)
        {
            stageTimer += Time.deltaTime;
            if (stageTimer >= 5f)
            {
                StartStage();
            }
        }
    }
    void StartStage()
    {
        state = GameState.Playing;
        introUI.SetActive(false);
        stageTimer = 0f;
        spawningEnemies = CurrentStage * 5;
        enemiesRemaining = spawningEnemies * 2;
        //maxSpawnDuration = CurrentStage * 25;
        player.SetActive(true);
        StartCoroutine(SpawnEnemies());
    }

    void Intro()
    {
        state = GameState.Intro;
        introUI.SetActive(true);
        player.SetActive(false);
        gameOverUI.SetActive(false);
    }

    IEnumerator SpawnEnemies()
    {
        //float elapsed = 0f;
        int spawnIndex = 0;
        while (spawnIndex < spawningEnemies)
        {
            ActivateRandomSpawner();
            yield return new WaitForSeconds(spawnInterval);
            //elapsed += spawnInterval;
            spawnIndex++;
        }
    }

    void ActivateRandomSpawner()
    {
        int region = Random.Range(0, 4); 
        int startIdx = region * 4;
        List<int> selectedSpawners = new List<int>();

        while (selectedSpawners.Count < SpawnCount)
        {
            int idx = Random.Range(startIdx, startIdx + 4);
            if (!selectedSpawners.Contains(idx))
            {
                selectedSpawners.Add(idx);
                EnemySpawner[idx].SetActive(true);
                Debug.Log("Enemy spawned!");

            }
        }
    }
    public void EnemySpawned()
    {
        enemiesRemaining++;
    }

    public void EnemyDefeated()
    {
        enemiesRemaining--;
        Debug.Log("Enemy destroyed!");
    }

    public void DamageNexus(int damage)
    {
        nexusHealth -= damage;
        if (nexusHealth <= 0)
        {
            GameOver();
        }
        Debug.Log("Nexus Health: " + nexusHealth);
    }
    void StageClear()
    {
        state = GameState.StageClear;
        stageTimer = 0f;
        if(nexusHealth < 300)
        {
            nexusHealth += 200;
        }
        else if(300 < nexusHealth && nexusHealth < 500 )
        {
            nexusHealth = 500;
        }
            player.GetComponent<PlayerController>().HealHP();
        StartCoroutine(LevelUpUI());
        Debug.Log("Stage Clear! Next Stage: " + CurrentStage);
        CurrentStage++;
    }
    public void GameOver()
    {
        state = GameState.GameOver;
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Game Over!");
    }

    IEnumerator LevelUpUI()
    {
        SkillLvUI.SetActive(true);
        if(CurrentStage%2 == 0)
        {
            S4Btton.GetComponent<Button>().interactable = true;
        }
        else
        {
            S4Btton.GetComponent<Button>().interactable = false;
        }
            Time.timeScale = 0f;
        yield return null;
    }
    public void ResumeGame()
    {
        SkillLvUI.SetActive(false); // UI 비활성화
        Time.timeScale = 1f; // 시간 흐름 재개
    }
}
