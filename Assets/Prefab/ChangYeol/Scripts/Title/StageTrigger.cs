using BS.Player;
using TMPro;
using UnityEngine;

namespace BS.Title
{
    public abstract class StageTrigger : MonoBehaviour
    {
        protected abstract void TriggerKeyDown();
        #region Vaiables
        public TextMeshProUGUI keyText;
        public TextMeshProUGUI stageText;
        public GameObject canvas;
        //생성되는 Enemy 변수
        public GameObject Enemy;
        //생성되는 Enemy
        [SerializeField]protected GameObject InstEnemy;
        [HideInInspector]public bool isEnemy = false;
        public string stageName;
        public KeyCode keyCode = KeyCode.V;
        #endregion
        private void Start()
        {
            keyText.text = "";
            stageText.text = "";
        }
        private void OnTriggerStay(Collider other)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                keyText.text = "[ " + keyCode.ToString() +" ]";
                TriggerKeyDown();
                canvas.SetActive(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                stageText.text = "";
                keyText.text = "";
                Debug.Log("OnTriggerExit");

                canvas.SetActive(false);

            }
        }
    }
}