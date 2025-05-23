using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleArrowIndicator : MonoBehaviour
{
    public GameObject arrowPrefab; // 화살표
    public Transform player; 
    public Camera mainCamera;
    private List<Transform> enemies = new List<Transform>(); // 현재 존재하는 적들
    private Dictionary<Transform, RectTransform> enemyArrows = new Dictionary<Transform, RectTransform>(); // 화살표 매핑

    void Start()
    {
        InvokeRepeating(nameof(UpdateEnemyList), 0f, 1f); //적 목록 업데이트
    }

    void UpdateEnemyList()
    {
        enemies.Clear();
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach (GameObject enemy in enemyObjects)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (!enemyArrows.ContainsKey(enemy.transform) && !enemyAI.isDead)
            {
                //적위치 생성 및 등록
                RectTransform newArrow = Instantiate(arrowPrefab, transform).GetComponent<RectTransform>();
                enemyArrows[enemy.transform] = newArrow;
            }
            enemies.Add(enemy.transform);
        }

        //표시기 정리
        List<Transform> removedEnemies = new List<Transform>();

        foreach (Transform enemy in enemyArrows.Keys)
        {
            if (!enemies.Contains(enemy))
            {
                Destroy(enemyArrows[enemy].gameObject); // 표시 제거
                removedEnemies.Add(enemy);
            }
        }

        foreach (Transform removed in removedEnemies)
        {
            enemyArrows.Remove(removed);
        }
    }

    void Update()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float offset = 50f; // 화면 약간 안쪽으로 배치

        foreach (Transform enemy in enemies)
        {
            if (enemy == null) continue;

            RectTransform arrow = enemyArrows[enemy];
            Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.position);
            bool isOffScreen = screenPos.x < 0 || screenPos.x > screenWidth || screenPos.y < 0 || screenPos.y > screenHeight;

            if (isOffScreen)
            {
                arrow.gameObject.SetActive(true);

                Vector3 enemyDir = (screenPos - new Vector3(screenWidth / 2, screenHeight / 2)).normalized;
                Vector3 arrowScreenPos = screenPos;

                //스크린 경계선으로 위치 조정
                if (screenPos.x < 0) arrowScreenPos.x = offset; // 왼쪽 경계
                if (screenPos.x > screenWidth) arrowScreenPos.x = screenWidth - offset; // 오른쪽 경계
                if (screenPos.y < 0) arrowScreenPos.y = offset; // 아래쪽 경계
                if (screenPos.y > screenHeight) arrowScreenPos.y = screenHeight - offset; // 위쪽 경계

                arrow.position = arrowScreenPos;

                float angle = Mathf.Atan2(enemyDir.y, enemyDir.x) * Mathf.Rad2Deg;
                arrow.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                arrow.gameObject.SetActive(false);
            }
        }
    }
 }
