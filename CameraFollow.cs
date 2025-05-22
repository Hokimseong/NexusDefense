using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [Header("Reference")]
    public Transform target;
    public Transform nexusTarget;
    public GameManager gameManager;

    [Header("Camera Setting")]
    public float height = 25f; // 카메라 높이
    public float distance = 20f; // 플레이어와의 거리
    public float followSpeed = 3f; // 따라가는 속도
    public float fixedAngle = 60f; // 고정할 카메라 각도

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
