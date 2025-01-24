using BS.Demon;
using BS.Player;
using BS.PlayerInput;
using BS.UI;
using UnityEngine;
using UnityEngine.Audio;

public class DemonGameManager : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public DemonPattern bossHealth;
    public GameObject player;
    private DungeonClearTime dungeEndGame;
    private float actorTime = 5f;
    private PlayerInputActions playerInputActions;

    [HideInInspector]public bool gameEnded = false;
    private void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        dungeEndGame = FindFirstObjectByType<DungeonClearTime>();
        playerInputActions = player.GetComponent<PlayerInputActions>();
    }
    private void Update()
    {
        if (!gameEnded)
        {
            if (playerHealth.CurrentHealth <= 0)
            {
                PrepareDefeat();
            }
        }
    }
    private void PrepareDefeat()
    {
        gameEnded = true;
        bossHealth.demon.sceneManager.drectingCamera.SetActive(true);
        bossHealth.demon.animator.SetBool("IsDefeat", true);
        playerInputActions.UnInputActions();
        bossHealth.demon.source.PlayOneShot(bossHealth.audioManager.sounds[8].audioClip);
        Invoke("Defeat", actorTime);
    }
    private void Defeat()
    {
        bossHealth.demon.source.clip = bossHealth.audioManager.sounds[8].audioClip;
        bossHealth.demon.source.Play();
        dungeEndGame.DefeatDungeon();
        Destroy(bossHealth.gameObject);
    }
}
