using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillTimer : MonoBehaviour
{
    [Header(" Skill References")]
    public GameObject[] hideSkill;
    public GameObject[] textPros; // 스킬 텍스트
    public TextMeshProUGUI[] hideSkillTimeTexts;
    public Image[] hideSkillImages;

    private bool[] isHideSKills = { false, false, false, false, false };
    private float[] skillCooldowns = { 8f, 12f, 18f, 50f, 3f };  // 쿨타임
    private float[] skillTimers = { 0f, 0f, 0f, 0f, 0f };           // 스킬 사용 후 시간
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<TextMeshProUGUI>();
            hideSkill[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HideSkillCheck();
        KeyCode[] keyCodes = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyUp(keyCodes[i]) && !isHideSKills[i])
            {
                HideSkillSetting(i);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isHideSKills[4]) HideSkillSetting(4);
    }
    public void HideSkillSetting(int skillNum)
    {
        hideSkill[skillNum].SetActive(true);
        skillTimers[skillNum] = skillCooldowns[skillNum];
        isHideSKills[skillNum] = true;
    }

    private void HideSkillCheck()
    {
        for (int i = 0; i < isHideSKills.Length; i++)
        {
            if (isHideSKills[i])
            {
                StartCoroutine(SkillTimeCheck(i));
            }
        }
    }

    IEnumerator SkillTimeCheck(int skillNum)
    {
        yield return null;
        if (skillTimers[skillNum] > 0)
        {
            skillTimers[skillNum] -= Time.deltaTime;
            if (skillTimers[skillNum] < 0)
            {
                skillTimers[skillNum] = 0;
                isHideSKills[skillNum] = false;
                hideSkill[skillNum].SetActive(false);
            }
            hideSkillTimeTexts[skillNum].text = skillTimers[skillNum].ToString("00");
            float time = skillTimers[skillNum] / skillCooldowns[skillNum];
            hideSkillImages[skillNum].fillAmount = time;
        }
    }
}
