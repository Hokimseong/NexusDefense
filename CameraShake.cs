using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    public Transform player;
    private Vector3 originalPos; 
    private float shakeDuration;
    private float shakeMagnitude;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        originalPos = player.transform.position;
    }
    public void Shake(float duration, float magnitude, float frequency = 1f, float distanceFactor = 1f)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude * distanceFactor, frequency));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude, float frequency)
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            transform.position = originalPos + Random.insideUnitSphere * shakeMagnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }
}
