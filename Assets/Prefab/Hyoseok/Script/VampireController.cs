using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using BS.Player;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using BS.Particle;

namespace BS.vampire
{
    /// <summary>
    /// ���ݻ��¸ӽ�
    /// </summary>
    public class VampireController : MonoBehaviour
    {
        #region Variables
        public Animator animator;
        public GameObject player;
        private int direction;  // ����

        public float time = 20f; // ���� ��� �ð�

        //������Ÿ��
        [SerializeField] float attack1;
        [SerializeField] float attack2;
        [SerializeField] float attack3;
        [SerializeField] float attack4;
        // ����1 
        // �� ���� ��ȯ���� ����� �÷��̾� �������� �̵��ؼ� Ÿ��
        public GameObject attackObjectPrefab; // ���� ���� ������Ʈ
        public GameObject impactEffectPrefab; // ����Ʈ ������ batEffect��smoke�� ���ݰ��� �־��
        [SerializeField] private float attackRange;      //���ݹ���
        [SerializeField] private float attackCount;     //����Ƚ��
        public GameObject attackRangePrefab; // ���ݹ��� ������
        public Transform[] attackObjects;
        public Transform hitPoint; // Ÿ�� ����

        //����2
        //������ �߾����� �̵��� ���ݸ�� ����Ʈ �ߵ��� �� �� �Ʒ� �� ������ �߻� 
        public Transform centerTeleport;    //���� �ڷ���Ʈ
        public GameObject attack2EffectPrefab; //���� ����Ʈ������ 
        public GameObject[] bloodBeams;   //�޿��Ʒ��� ����
      

        private int nextPattern = 0;
        #endregion
        void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            StartCoroutine(Attak1());
        }


        IEnumerator Attak1()
        {
            transform.LookAt(player.transform);
            yield return new WaitForSeconds(5f);
            for (int i = 0; i < attackCount; i++)
            {
                // ���� ��� �ð�
                GameObject attackObject1 = Instantiate(attackObjectPrefab, attackObjects[0].position, Quaternion.identity);
                GameObject attackObject2 = Instantiate(attackObjectPrefab, attackObjects[1].position, Quaternion.identity);
                animator.SetTrigger("Attack1");

                hitPoint = player.transform;


                yield return new WaitForSeconds(0.5f);

                //���� ���� ǥ��
                //GameObject attackRangeIndicator = Instantiate(attackRangePrefab, hitPoint.position, Quaternion.identity);
                //VampireAttackRange rangeComponent = attackRangeIndicator.GetComponent<VampireAttackRange>();
                //rangeComponent.StartGrowing(new Vector3(1, 0.1f, 1), attackRange);
                //Destroy(attackRangeIndicator, 1f);


                Debug.Log("Attak1 start");



                // �̵� ��� (y �� ����, x �� z �� ����)
                Vector3[] path1 = {
                attackObject1.transform.position,
                new Vector3((attackObject1.transform.position.x + hitPoint.position.x) / 2, attackObject1.transform.position.y, hitPoint.position.z + 2),
                hitPoint.position
            };

                Vector3[] path2 = {
                attackObject2.transform.position,
                new Vector3((attackObject2.transform.position.x + hitPoint.position.x) / 2, attackObject2.transform.position.y, hitPoint.position.z - 2),
                hitPoint.position
            };

                // � �̵� �� �÷��̾� �ٶ󺸰� ����
                attackObject1.transform.DOPath(path1, 0.5f, PathType.CatmullRom).SetOptions(false, AxisConstraint.Y).SetEase(Ease.InOutSine).OnUpdate(() =>
                {
                    attackObject1.transform.LookAt(player.transform);
                }).OnComplete(() =>
                {
                    // Ÿ�� ������ ���� �� ��Ʈ���� ����
                    GameObject impactEffect = Instantiate(impactEffectPrefab, attackObject1.transform.position, Quaternion.identity);

                    Destroy(attackObject1, 1f);
                    Destroy(impactEffect, 1.5f);
                    //Debug.Log("ù ��° ������Ʈ�� Ÿ�� �������� ���ŵǾ����ϴ�.");
                });

                attackObject2.transform.DOPath(path2, 0.5f, PathType.CatmullRom).SetOptions(false, AxisConstraint.Y).SetEase(Ease.InOutSine).OnUpdate(() =>
                {
                    attackObject2.transform.LookAt(player.transform);
                }).OnComplete(() =>
                {
                    // Ÿ�� ������ ���� �� ��Ʈ���� ����
                    GameObject impactEffect = Instantiate(impactEffectPrefab, attackObject2.transform.position, Quaternion.identity);
                    Destroy(attackObject2, 1f);
                    Destroy(impactEffect, 1.5f);
                    //Debug.Log("�� ��° ������Ʈ�� Ÿ�� �������� ���ŵǾ����ϴ�.");
                });
                yield return new WaitForSeconds(0.1f);
            }
            NextPatternPlay();
            yield return null;
        }

        IEnumerator Attak2()
        {
            //���� �߾��̵�
            transform.position = centerTeleport.position;
            //���� ��ų ����
            GameObject skillEffectGo = Instantiate(attack2EffectPrefab, transform.position, Quaternion.identity);
            Destroy(skillEffectGo, 2f);
            //�ִϸ��̼�
            animator.SetTrigger("Attack2");
            yield return new WaitForSeconds(1f);    //���۴��ð�
            //����Ʈ

            for (int i = 0; i < bloodBeams.Length; i++)
            {
                //��ų ���� �׷��߉� ������ ������ ����
     ;
                bloodBeams[i].SetActive(true);
                yield return new WaitForSeconds(2f); //���̺����ִ½ð�
                bloodBeams[i].SetActive(false);
            }

            NextPatternPlay();
            yield return null;

            yield return new WaitForSeconds(20f);
        }

        IEnumerator Attak3()
        {
            transform.LookAt(player.transform);
            NextPatternPlay();
            yield return null;
        }

        IEnumerator Attak4()
        {
            transform.LookAt(player.transform);
            NextPatternPlay();
            yield return null;
        }



        void NextPatternPlay()
        {
            transform.LookAt(player.transform);
            nextPattern = (nextPattern % 4) + 1; // ������ ��ȯ
            switch (nextPattern)
            {
                case 1:
                    StartCoroutine(Attak1());
                    Debug.Log("1�����Ͻ���");
                    break;
                case 2:
                    StartCoroutine(Attak2());
                    Debug.Log("2�����Ͻ���");
                    break;
                case 3:
                    StartCoroutine(Attak3());
                    Debug.Log("3�����Ͻ���");
                    break;
                case 4:
                    StartCoroutine(Attak4());
                    Debug.Log("4�����Ͻ���");
                    break;
            }
        }
    }
}
