using Unity.Cinemachine;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    public LineRenderer laserLine;
    public float laserLength = 100f;

    private float nextFireTime = 0f;
    private bool isUsingSkill = false;

    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        laserLine.positionCount = 2;
        laserLine.startWidth = 0.05f;
        laserLine.endWidth = 0.05f;
        laserLine.startColor = Color.red;
        laserLine.endColor = Color.red;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLaser();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && !isUsingSkill)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void UpdateLaser()
    {
        Vector3 laserEnd = firePoint.position + firePoint.forward * laserLength;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, laserLength))
        {
            laserEnd = hit.point;
        }
        laserLine.SetPosition(0, firePoint.position);
        laserLine.SetPosition(1, laserEnd);
    }
    void Shoot()
    {
        CameraManager.instance.CameraShake(impulseSource);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
