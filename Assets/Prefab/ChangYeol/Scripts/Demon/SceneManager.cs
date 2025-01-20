using BS.Player;
using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace BS.Demon
{
    public class SceneManager : MonoBehaviour
    {
        #region Variables
        public DemonController controller;
        public GameObject WarningCanvas;
        public Camera main;
        public GameObject drectingCamera;
        public GameObject bossCanvas;
        public Animator animator;
        public GameObject presentEffect;
        public GameObject player;
        #endregion

        private void Start()
        {
            StartCoroutine(OpeningDemon());
        }
        void Update()
        {
            WarningCanvas.transform.LookAt(WarningCanvas.transform.position + main.transform.rotation * Vector3.forward, main.transform.rotation * Vector3.up);
            if (Input.GetKeyDown(KeyCode.V))
            {
                controller.TakeDamage(5);
            }
        }
        IEnumerator OpeningDemon()
        {
            
            PlayerController playerController = player.GetComponent<PlayerController>();
            controller.enabled = false;
            playerController.enabled = false;
            controller.gameObject.SetActive(false);
            GameObject eff = Instantiate(presentEffect, controller.gameObject.transform.position, presentEffect.transform.rotation);
            yield return new WaitForSeconds(0.2f);
            controller.gameObject.SetActive(true);
            float descentDuration = 7f; // 내려오는 데 걸리는 시간
            float elapsedTime = 0f;
            
            yield return new WaitForSeconds(1.2f);
            while (elapsedTime < descentDuration)
            {
                eff.transform.localScale = Vector3.Lerp(eff.transform.localScale, new Vector3(0.1f,0.1f,0.1f)*-0.5f, elapsedTime / descentDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if(eff.transform.localScale == Vector3.zero)
            {
                Destroy(eff,1);
            }

            //TODO : 카메라 흔들면서 연출 효과 나오고 이름 나오고 시작
            yield return null;
        }
    }
}