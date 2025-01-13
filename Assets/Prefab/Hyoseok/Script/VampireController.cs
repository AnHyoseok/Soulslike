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
        public float testAttackNumber;

        private int direction;  // ����

        public float time = 20f; // ���� ��� �ð�

        //������Ÿ��
        [SerializeField] float attack1;
        [SerializeField] float attack2;
        [SerializeField] float attack3;
        [SerializeField] float attack4;
        [Header("Attack1")]
        // ����1 
        // �� ���� ��ȯ���� ����� �÷��̾� �������� �̵��ؼ� Ÿ��
        public GameObject attackObjectPrefab; // ���� ���� ������Ʈ
        public GameObject impactEffectPrefab; // ����Ʈ ������ batEffect��smoke�� ���ݰ��� �־��
        [SerializeField] private float attackRange;      //���ݹ���
        [SerializeField] private float attackCount;     //����Ƚ��
        public GameObject attackRangePrefab; // ���ݹ��� ������
        public Transform[] attackObjects;
        public Transform hitPoint; // Ÿ�� ����
        [Header("Attack2")]
        //����2
        //������ �߾����� �̵��� ���ݸ�� ����Ʈ �ߵ��� �� �� �Ʒ� �� ������ �߻� 
        public Transform centerTeleport;    //���� �ڷ���Ʈ
        public GameObject attack2EffectPrefab; //���� ����Ʈ������ 
        public GameObject[] bloodBeams;   //�޿��Ʒ��� ����
        public GameObject[] attak3Ranges;
        [Header("Attack3")]
        //����3
        //������ �÷��̾ �ٶ󺸸� ��ä�� ������̺� ������
        public GameObject Attack3BatPrefab; //�΋H���� �÷��̾�� �������ִ� ����
        public float waveCount = 3f;     //���� ���̺� ī��Ʈ
        public GameObject attack3EffectPrefab;  //�΋H������ ��ƼŬ
        public Transform[] batTransforms;

        [Header("Attack4")]
        //����4
        //���� �ֺ��� �����(�����̼ǵ���) ���� 1~2 �������� �갳�� �÷��̾�������� ������ �߻�
        public GameObject[] summonObject;    //��ȯ��ġ
        public GameObject attak4Ranges;
        public float moveRadius = 2f; // �̵� �ݰ�
        public GameObject attack4EffectPrefab;  //������ ����Ʈ
        private Vector3[] originalPositions; // ������ġ
        [SerializeField] float attack4count = 3f;  //�ݺ�Ƚ��
        private int nextPattern = 0;
        #endregion
        void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            StartCoroutine(Attack1());



        }


        IEnumerator Attack1()
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

        IEnumerator Attack2()
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

            for (int i = 0; i < attak3Ranges.Length; i++)
            {
                //��ų ���� �׷��߉� ������ ������ ����
                attak3Ranges[i].SetActive(true);
                yield return new WaitForSeconds(1f);
                attak3Ranges[i].SetActive(false);
            }

            for (int i = 0; i < bloodBeams.Length; i++)
            {
                //���̺����ִ½ð�
                bloodBeams[i].SetActive(true);
                yield return new WaitForSeconds(1f);
                bloodBeams[i].SetActive(false);
            }

            NextPatternPlay();
            yield return null;

            yield return new WaitForSeconds(10f);
        }

        IEnumerator Attack3()
        {
            yield return new WaitForSeconds(7f);
            transform.LookAt(player.transform);
            //���̺�
            for (int i = 0; i < waveCount; i++)
            {
                foreach (Transform batTransform in batTransforms)
                {
                    GameObject bat = Instantiate(Attack3BatPrefab, batTransform.position, batTransform.rotation);
                    Rigidbody rb = bat.GetComponent<Rigidbody>();


                    Vector3 directionToPlayer = (player.transform.position - batTransform.position).normalized;
                    directionToPlayer.y = 0;
                    //directionToPlayer.z = 0;
                    Attack3Bat attack3Bat = bat.GetComponent<Attack3Bat>();
                    attack3Bat.Initialize(directionToPlayer, 20f);

                    Destroy(bat, 7f);

                    //�浹�� ����Ʈ 



                }
                yield return new WaitForSeconds(0.3f);
            }
            NextPatternPlay();
            yield return null;
        }

        IEnumerator Attack4()
        {
            yield return new WaitForSeconds(7f);

            for (int j = 0; j < attack4count; j++)
            {
                

                // ����4�� ��ġ ����
                originalPositions = new Vector3[summonObject.Length];
                for (int i = 0; i < summonObject.Length; i++)
                {
                    originalPositions[i] = summonObject[i].transform.position;
                }

                // ��ġ �̵�
                float elapsedTime = 0f;
                float moveDuration = 0.2f; // �̵��� �ɸ� �ð�

                Vector3[] targetPositions = new Vector3[summonObject.Length];
                for (int i = 0; i < summonObject.Length; i++)
                {
                    targetPositions[i] = originalPositions[i] + new Vector3(Random.Range(-moveRadius, moveRadius), 0, Random.Range(-moveRadius, moveRadius));
                }

                while (elapsedTime < moveDuration)
                {
                    for (int i = 0; i < summonObject.Length; i++)
                    {
                        if (summonObject[i] != null)
                        {
                            Vector3 newPosition = Vector3.Lerp(originalPositions[i], targetPositions[i], elapsedTime / moveDuration);
                            newPosition.y = originalPositions[i].y; // y�� ����
                            summonObject[i].transform.position = newPosition;

                            // �÷��̾ �ٶ󺸵��� ȸ���� ��, ������ ���� ���ϱ�
                            summonObject[i].transform.LookAt(player.transform);
                            Vector3 eulerAngles = summonObject[i].transform.rotation.eulerAngles;
                            eulerAngles.y += Random.Range(-10f, 10f);
                            //eulerAngles.z += Random.Range(-10f, 10f);
                            summonObject[i].transform.rotation = Quaternion.Euler(eulerAngles);
                        }
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }



                for (int i = 0; i < summonObject.Length; i++)
                {
                    if (summonObject[i] != null)
                    {
                        summonObject[i].transform.position = targetPositions[i];
                    }
                }

                //���ݹ���
                for( int i = 0; i< summonObject.Length; i++)
                {
                    GameObject attakRange = Instantiate(attak4Ranges, summonObject[i].transform.position, summonObject[i].transform.rotation);
                    attakRange.transform.parent = summonObject[i].transform;
                    Destroy(attakRange, 0.5f);
                }

                yield return new WaitForSeconds(0.5f);
                // ������ �߻�
                for (int i = 0; i < summonObject.Length; i++)
                {
                    GameObject attackEffect = Instantiate(attack4EffectPrefab, summonObject[i].transform.position, summonObject[i].transform.rotation);
                    attackEffect.transform.parent = summonObject[i].transform;
                    attackEffect.transform.rotation *= Quaternion.Euler(90f, 0f, 0f);
                    Destroy(attackEffect, 0.5f);
                }


                yield return new WaitForSeconds(1f);

                // �ٽ� ���� ��ġ��
                elapsedTime = 0f;
                while (elapsedTime < moveDuration)
                {
                    for (int i = 0; i < summonObject.Length; i++)
                    {
                        if (summonObject[i] != null)
                        {
                            summonObject[i].transform.position = Vector3.Lerp(targetPositions[i], originalPositions[i], elapsedTime / moveDuration);
                        }
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                for (int i = 0; i < summonObject.Length; i++)
                {
                    if (summonObject[i] != null)
                    {
                        summonObject[i].transform.position = originalPositions[i];
                    }
                }
            }
            yield return new WaitForSeconds(3f);
            NextPatternPlay();
            yield return null;
        }

        void StraightMove()
        {

        }


        void NextPatternPlay()
        {
            transform.LookAt(player.transform);
            nextPattern = (nextPattern % 4) + 1; // ������ ��ȯ
            switch (nextPattern)
            {
                case 1:
                    StartCoroutine(Attack1());
                    Debug.Log("1�����Ͻ���");
                    break;
                case 2:
                    StartCoroutine(Attack2());
                    Debug.Log("2�����Ͻ���");
                    break;
                case 3:
                    StartCoroutine(Attack3());
                    Debug.Log("3�����Ͻ���");
                    break;
                case 4:
                    StartCoroutine(Attack4());
                    Debug.Log("4�����Ͻ���");
                    break;
            }
        }
    }
}
