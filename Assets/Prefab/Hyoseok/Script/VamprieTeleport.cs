using System.Collections;
using UnityEngine;

namespace BS.vampire
{
    /// <summary>
    /// 랜덤 이동 시스템 
    /// 중앙이동시 CircleShoot발사
    /// 외곽이동시 PingPongShot발사
    /// </summary>
    public class VamprieTeleport : MonoBehaviour
    {
        #region Variables
        public GameObject pingpongShot;
        //public GameObject CircleShot;
        public GameObject teleportEffect;
        public Transform[] teleports; //순간이동 위치 0~3 랜덤 4는 중앙
        public float time = 20; //순간이동 쿨타임
        public Animator animator;
        private int previousIndex = -1; //이전 위치값
        #endregion

        private void Start()
        {

            //pingpongShot = pingpongShot.GetComponent<ParticleSystem>();
            //var emission = pingpongShot.emission;
            //emission.rateOverTime = 0f;
            //CircleShot = CircleShot.GetComponent<ParticleSystem>();
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            StartCoroutine(RandomTeleport());
        }

        IEnumerator RandomTeleport()
        {
            while (true)
            {
                yield return new WaitForSeconds(time);
                //애니메이션 연출 3초후에 이동
                animator.SetTrigger("Teleport");
                GameObject potalEffect = Instantiate(teleportEffect, transform.position, Quaternion.identity);
                potalEffect.transform.parent = transform;
                Destroy(potalEffect, 3.3f);
                yield return new WaitForSeconds(2.5f);
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, 3);
                }
                while (randomIndex == previousIndex); //같은값 연속방지
                //보스 위치를 랜덤이동
                transform.position = teleports[randomIndex].position;
                previousIndex = randomIndex;



                pingpongShot.SetActive(true);

                yield return new WaitForSeconds(5f);
                pingpongShot.SetActive(false);


            }
        }
    }
}