using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleArrowIndicator : MonoBehaviour
{
    public GameObject arrowPrefab; // ȭ��ǥ ������ (Canvas �ȿ� �־�� ��)
    public Transform player; // �÷��̾� ��ġ
    public Camera mainCamera;
    private List<Transform> enemies = new List<Transform>(); // ���� �����ϴ� ����
    private Dictionary<Transform, RectTransform> enemyArrows = new Dictionary<Transform, RectTransform>(); // ���� ȭ��ǥ ����

    void Start()
    {
        InvokeRepeating(nameof(UpdateEnemyList), 0f, 1f); // 1�ʸ��� �� ��� ������Ʈ
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
                //����ġ ���� �� ���
                RectTransform newArrow = Instantiate(arrowPrefab, transform).GetComponent<RectTransform>();
                enemyArrows[enemy.transform] = newArrow;
            }
            enemies.Add(enemy.transform);
        }

        //ǥ�ñ� ����
        List<Transform> removedEnemies = new List<Transform>();

        foreach (Transform enemy in enemyArrows.Keys)
        {
            if (!enemies.Contains(enemy))
            {
                Destroy(enemyArrows[enemy].gameObject); // ǥ�� ����
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
        float offset = 50f; // ȭ�� �����ڸ����� �ణ �������� ��ġ

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

                //��ũ�� ��輱���� ��ġ ����
                if (screenPos.x < 0) arrowScreenPos.x = offset; // ���� ���
                if (screenPos.x > screenWidth) arrowScreenPos.x = screenWidth - offset; // ������ ���
                if (screenPos.y < 0) arrowScreenPos.y = offset; // �Ʒ��� ���
                if (screenPos.y > screenHeight) arrowScreenPos.y = screenHeight - offset; // ���� ���

                arrow.position = arrowScreenPos;

                //ȭ��ǥ ȸ�� (�� ������ ����Ű����)
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
