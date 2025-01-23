using BS.Player;
using BS.PlayerInput;
using BS.UI;
using UnityEngine;
using UnityEngine.Audio;

public class DemonGameManager : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public GameObject player;
    private DungeonClearTime dungeEndGame;
    /*private void CheckGameEnd()
    {
        if (bossHealth.currentHealth <= 0)
        {
            PrepareClear();
        }
        else if (playerHealth.CurrentHealth <= 0)
        {
            PrepareDefeat();
        }
    }
    private void PrepareDefeat()
    {
        gameEnded = true;
        playerInputActions.enabled = false;
        audioSource.PlayOneShot(defeatSound);
        Invoke("Defeat", actorTime);
    }*/
}
