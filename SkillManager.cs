using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour
{
    [Header(" Bullet References")]
    public GameObject skill1Bullet;
    public GameObject skill2Bullet;
    public GameObject skill3Missile;
    public GameObject skill4Turret;

    [Header("Skill Range References")]
    public GameObject[] skillRanges;
    public GameObject markPrefab;

    public Transform firePoint;
    public Transform player;
    public Rigidbody rb;

    [Header("Skill Stat")]
    public int skill2BulletCount = 10;
    public int skill3MissileCount = 2;

    private bool isUsingSkill = false;
    private float[] skillCooldowns = { 8f, 12f, 18f, 50f };  // ��Ÿ��
    private float[] skillTimers = { 0f, 0f, 0f, 0f };           // ��ų ��� �� �ð�
    private GameObject markInstance;

    private void Start()
    {
        for (int i = 0; i < skillRanges.Length; i++)
        {
            skillRanges[i].SetActive(false);
        }
    }

    void Update()
    {
        if(isUsingSkill) return;
        if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(RangeSkill1());
        if (Input.GetKeyUp(KeyCode.Alpha1)) StartCoroutine(UseSkill1());
        if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(RangeSkill2());
        if (Input.GetKeyUp(KeyCode.Alpha2)) StartCoroutine(UseSkill2());
        if (Input.GetKeyUp(KeyCode.Alpha3)) StartCoroutine(UseSkill3());
        if (Input.GetKeyDown(KeyCode.Alpha4)) StartCoroutine(RangeSkill4());
        if (Input.GetKeyUp(KeyCode.Alpha4)) StartCoroutine(UseSkill4());
    }

    IEnumerator RangeSkill1()
    {
        if (Time.time < skillTimers[0]) yield break;
        skillRanges[0].SetActive(true);
        while (Input.GetKey(KeyCode.Alpha1))
        {
            yield return null; // ���� �����ӱ��� ���
        }
        skillRanges[0].SetActive(false);
    }
    IEnumerator RangeSkill2()
    {
        if (Time.time < skillTimers[1]) yield break;
        skillRanges[1].SetActive(true);
        while (Input.GetKey(KeyCode.Alpha2))
        {
            yield return null; // ���� �����ӱ��� ���
        }
        skillRanges[1].SetActive(false);
    }
    IEnumerator RangeSkill4()
    {
        if (Time.time < skillTimers[3]) yield break;
        if (markInstance == null)
        {
            markInstance = Instantiate(markPrefab);
        }

        while (Input.GetKey(KeyCode.Alpha4))
        {
            // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Mark ��ġ�� ���콺�� ����Ű�� ������ ������Ʈ
                markInstance.transform.position = hit.point;
            }
            yield return null;
        }

        // 4�� Ű���� ���� ���� Mark ����
        if (markInstance != null)
        {
            Destroy(markInstance);
            markInstance = null;
        }
    }
    IEnumerator UseSkill1()
    {
        if (Time.time < skillTimers[0]) yield break;
        isUsingSkill = true;
        //yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Alpha1));
        Instantiate(skill1Bullet, firePoint.position, firePoint.rotation);
        rb.AddForce(-firePoint.forward * 50f, ForceMode.Impulse);
        skillTimers[0] = Time.time + skillCooldowns[0];
        isUsingSkill = false;
    }

    IEnumerator UseSkill2()
    {
        if(Time.time < skillTimers[1]) yield break;
        isUsingSkill = true;
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Alpha2));
        for(int i = 0; i < skill2BulletCount; i++)
        {
            float spreadAngle = Random.Range(-20f, 20f);
            Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0, spreadAngle, 0);
            Instantiate(skill2Bullet, firePoint.position, bulletRotation);
        }
        rb.AddForce(-firePoint.forward * 30f, ForceMode.Impulse);
        skillTimers[1] = Time.time + skillCooldowns[1];
        isUsingSkill = false;
    }

    IEnumerator UseSkill3()
    {
        if (Time.time < skillTimers[2]) yield break;
        isUsingSkill = true;
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Alpha3));
    
        for (int i = 0; i < skill3MissileCount; i++)
        {
            Vector3 missilePosition = player.position + new Vector3(0, 3f, 0);
            Instantiate(skill3Missile, missilePosition, Quaternion.identity);
            yield return new WaitForSeconds(0.5f); 
        }
        skillTimers[2] = Time.time + skillCooldowns[2];
        isUsingSkill = false;
    }

    IEnumerator UseSkill4()
    {
        if (Time.time < skillTimers[3]) yield break;
        isUsingSkill = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Instantiate(skill4Turret, hit.point, Quaternion.identity);
        }
        skillTimers[3] = Time.time + skillCooldowns[3];
        isUsingSkill = false;
    }
}
