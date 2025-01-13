using BS.Player;
using UnityEngine;

namespace BS.Title
{
    public class StageTrigger : MonoBehaviour
    {
        #region Vaiables

        #endregion
        private void OnTriggerStay(Collider other)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if(Input.GetKeyDown(KeyCode.V))
                {
                    Debug.Log("OnTriggerStay");
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log("OnTriggerExit");
            }
        }
    }
}