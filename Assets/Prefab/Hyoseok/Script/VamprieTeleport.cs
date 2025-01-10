using System.Collections;
using UnityEngine;

namespace BS.vampire
{
    /// <summary>
    /// ���� �̵� �ý��� 
    /// �߾��̵��� CircleShoot�߻�
    /// �ܰ��̵��� PingPongShot�߻�
    /// </summary>
    public class VamprieTeleport : MonoBehaviour
    {
        #region Variables
        public GameObject pingpongShot;
        //public GameObject CircleShot;
        public GameObject teleportEffect;
        public Transform[] teleports; //�����̵� ��ġ 0~3 ���� 4�� �߾�
        public float time = 20; //�����̵� ��Ÿ��
        public Animator animator;
        private int previousIndex = -1; //���� ��ġ��
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
                //�ִϸ��̼� ���� 3���Ŀ� �̵�
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
                while (randomIndex == previousIndex); //������ ���ӹ���
                //���� ��ġ�� �����̵�
                transform.position = teleports[randomIndex].position;
                previousIndex = randomIndex;



                pingpongShot.SetActive(true);

                yield return new WaitForSeconds(5f);
                pingpongShot.SetActive(false);


            }
        }
    }
}