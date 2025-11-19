using UnityEngine;

public class BarrierBoss : MonoBehaviour
{
    public int nScene;

    private void Start()
    {
        if (GameManager.instance.BossDestroyed[nScene] && !GameManager.instance.puzzleResolved[nScene])
        {
            gameObject.SetActive(true);
        }
        else { 
            gameObject.SetActive(false);
        }
    }

    public void OnBossDefeated()
    {
        gameObject.SetActive(false);
    }
}


