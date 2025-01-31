using BS.Utility;
using UnityEngine;

namespace BS.Player
{
    public class UsePotion : MonoBehaviour
    {
        private PlayerHealth playerHealth;
        public AudioClip potionSound;
        private void Awake()
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        private void Update()
        {
         
            if (Input.GetKeyDown(KeyCode.T))
            {
                playerHealth.UsePotion();
                AudioUtility.CreateSFX(potionSound, transform.position, AudioUtility.AudioGroups.Sound);
            }
        }
    }
}
