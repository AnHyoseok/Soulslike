using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using BS.Player;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

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

        // ����1
        // �� ���� ��ȯ���� ����� �÷��̾� �������� �̵��ؼ� Ÿ��
        public GameObject attackObjectPrefab; // ���� ���� ������Ʈ
        public GameObject impactEffectPrefab; // ����Ʈ ������
        public GameObject attackRangePrefab; // ���ݹ��� ������
        public Transform[] attackObjects;
        public Transform hitPoint; // Ÿ�� ����

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
            // ���� ��� �ð�
            GameObject attackObject1 = Instantiate(attackObjectPrefab, attackObjects[0].position, Quaternion.identity);
            GameObject attackObject2 = Instantiate(attackObjectPrefab, attackObjects[1].position, Quaternion.identity);
            animator.SetTrigger("Attack1");
            yield return new WaitForSeconds(1f);
            //���� ���� ǥ��
            hitPoint = player.transform;
            GameObject attackRangeIndicator = Instantiate(attackRangePrefab, hitPoint.position, Quaternion.identity);
            VampireAttackRange rangeComponent = attackRangeIndicator.GetComponent<VampireAttackRange>();
            rangeComponent.StartGrowing(new Vector3(2, 0.1f, 2), 3f);
            Destroy(attackRangeIndicator, 1f);
 

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
            attackObject1.transform.DOPath(path1, 1f, PathType.CatmullRom).SetOptions(false, AxisConstraint.Y).SetEase(Ease.InOutSine).OnUpdate(() =>
            {
                attackObject1.transform.LookAt(player.transform); 
            }).OnComplete(() => {
                // Ÿ�� ������ ���� �� ��Ʈ���� ����
                GameObject impactEffect = Instantiate(impactEffectPrefab, attackObject1.transform.position, Quaternion.identity);
       
                Destroy(attackObject1);
                Destroy(impactEffect, 3f);
                //Debug.Log("ù ��° ������Ʈ�� Ÿ�� �������� ���ŵǾ����ϴ�.");
            });

            attackObject2.transform.DOPath(path2, 1f, PathType.CatmullRom).SetOptions(false, AxisConstraint.Y).SetEase(Ease.InOutSine).OnUpdate(() =>
            {
                attackObject2.transform.LookAt(player.transform); 
            }).OnComplete(() => {
                // Ÿ�� ������ ���� �� ��Ʈ���� ����
                GameObject impactEffect = Instantiate(impactEffectPrefab, attackObject2.transform.position, Quaternion.identity);
                Destroy(attackObject2);
                Destroy(impactEffect, 3f);
                //Debug.Log("�� ��° ������Ʈ�� Ÿ�� �������� ���ŵǾ����ϴ�.");
            });

            NextPatternPlay();
            yield return null;
        }

        IEnumerator Attak2()
        {
            transform.LookAt(player.transform);
            NextPatternPlay();
            yield return null;
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
                    break;
                case 2:
                    StartCoroutine(Attak2());
                    break;
                case 3:
                    StartCoroutine(Attak3());
                    break;
                case 4:
                    StartCoroutine(Attak4());
                    break;
            }
        }
    }
}
