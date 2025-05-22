using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [Header("Reference")]
    public Transform target;
    public Transform nexusTarget;
    public GameManager gameManager;

    [Header("Camera Setting")]
    public float height = 25f; // ī�޶� ����
    public float distance = 20f; // �÷��̾���� �Ÿ�
    public float followSpeed = 3f; // ���󰡴� �ӵ�
    public float fixedAngle = 60f; // ������ ī�޶� ����

    private void LateUpdate()
    {
        gameManager = GameManager.instance;
        if(gameManager.state == GameState.Intro)
        {
            Vector3 targetPosition = nexusTarget.position + new Vector3(0, height, -distance);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Euler(fixedAngle, 0, 0);
        }
        else if(gameManager.state == GameState.Playing)
        {
            if (target == null) return;
            Vector3 targetPosition = target.position + new Vector3(0, height, -distance);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Euler(fixedAngle, 0, 0);
        }
    }
}
