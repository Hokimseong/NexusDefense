using UnityEngine;

public class LvUpButton : MonoBehaviour
{
    public GameObject player;
    public GameObject turret;

    public void OnClickS2LvButton()
    {
        player.GetComponentInChildren<SkillManager>().skill2BulletCount += 10;
        GameManager.instance.S2Level++;
        GameManager.instance.ResumeGame();
    }
    public void OnClickS3LvButton()
    {
        player.GetComponentInChildren<SkillManager>().skill3MissileCount += 1;
        GameManager.instance.S3Level++;
        GameManager.instance.ResumeGame();
    }
    public void OnClickS4LvButton()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");

        if (!turret.GetComponentInChildren<Turret>().unlockMissile)
        {
            turret.GetComponentInChildren<Turret>().unlockMissile = true;
        }
        else
        {
            turret.GetComponentInChildren<Turret>().MissileCount ++;
        }

        foreach (GameObject Turret in turrets)
        {
            Turret spawnedTurret = Turret.GetComponentInChildren<Turret>();
            if(!spawnedTurret.unlockMissile)
            {
                spawnedTurret.unlockMissile = true;
            }
            else
            {
                spawnedTurret.MissileCount = turret.GetComponentInChildren<Turret>().MissileCount;
            }
        }
        GameManager.instance.S4Level++;
        GameManager.instance.ResumeGame();
    }
}
