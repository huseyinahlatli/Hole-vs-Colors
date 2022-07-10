using UnityEngine;
using DG.Tweening;
public class UndergroundCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.isGameOver)
        {
            if (other.tag.Equals("Object"))
            {
                Level.Instance.objectsInScene--;
                UIManager.Instance.UpdateLevelProgress();
                Destroy(other.gameObject, 5);
                
                if (Level.Instance.objectsInScene == 0)
                {
                    UIManager.Instance.ShowLevelCompletedUI();
                    Level.Instance.PlayWinFx();
                    Invoke("NextLevel", 2.5f);
                }
            }
            
            if (other.tag.Equals("Obstacle"))
            {
                GameManager.isGameOver = true;
                Camera.main.transform
                    .DOShakePosition(1f, .2f, 20, 90f)
                    .OnComplete (() => {
                    Level.Instance.RestartLevel(); 
                });
            }
        }
    }

    void NextLevel()
    {
        Level.Instance.LoadNextLevel();
    }
}
