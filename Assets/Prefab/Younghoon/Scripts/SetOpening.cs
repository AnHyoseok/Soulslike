using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;

namespace BS.Enemy.Set
{
    public class SetOpening : MonoBehaviour
    {
        #region Variables
        [SerializeField] GameObject player;
        [SerializeField] GameObject gameHUD;
        [SerializeField] GameObject boss;
        [SerializeField] ParticleSystem chargingParticle;
        [SerializeField] ParticleSystem explosionParticle;
        [SerializeField] CinemachineSequencerCamera cutSceneCamera;
        Animator animator;

        #endregion

        private void Awake()
        {
            player.SetActive(false);
            gameHUD.gameObject.SetActive(false);
            boss.SetActive(false);
            chargingParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            explosionParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        void Start()
        {
            animator = GetComponent<Animator>();
            StartCoroutine(Opening());
        }


        IEnumerator Opening()
        {
            yield return new WaitForSeconds(1f);
            animator.SetTrigger(SetProperty.SET_ANIM_TRIGGER_ROAR);
            yield return new WaitForSeconds(1f);
            chargingParticle.Play();
            yield return new WaitForSeconds(2.2f);
            chargingParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            explosionParticle.Play();
            yield return new WaitForSeconds(4.5f);

            player.SetActive(true);
            gameHUD.gameObject.SetActive(true);
            boss.SetActive(true);
            cutSceneCamera.gameObject.SetActive(false);

            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            StopCoroutine(Opening());
        }
    }
}