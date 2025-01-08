using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace BS.vampire
{
    public class PattonSummon : MonoBehaviour
    {

        public GameObject batPrefab;
        public GameObject summonEffect;
        public GameObject shildEffect;
        public Transform[] summonLocations; // ��ȯ ��ġ
        public Transform effectLocation; // ��ȯ����Ʈ ��ġ
        public Transform shildLocation; // ����  ��ġ
        public float summonInterval = 5f; // ��ȯ ���� 

        private GameObject currentShild; //���� ���� ����
       public VamprieState vamprieState;
        void Start()
        {
            if (vamprieState == null)
            {
                vamprieState = FindAnyObjectByType<VamprieState>();
            }
            StartCoroutine(SummonBat());
        }

        IEnumerator SummonBat()
        {
            while (true)
            {
                yield return new WaitForSeconds(summonInterval);
                Summon();
            }
        }

        void Summon()
        {
            bool Summoned = false;
            foreach (Transform location in summonLocations)
            {
                // ���� ��ġ�� Bat Ȯ��
                if (location.childCount == 0)
                {
                    GameObject bat = Instantiate(batPrefab, location.position, location.rotation, location);
                    Summoned = true;

                }
            }
            if (Summoned)
            {
                GameObject effectGo = Instantiate(summonEffect, effectLocation.position, effectLocation.rotation);
                effectGo.transform.parent = effectLocation.transform;
                Destroy(effectGo, 3f);
            }
            shild(Summoned);

        }

        //��ȯ �� ����� ���� ���� ������ ���� �ı�
        void shild(bool batExist)
        {
            if (batExist)
            {
                if (currentShild == null)
                {
                    currentShild = Instantiate(shildEffect, shildLocation.position, shildLocation.rotation);
                    currentShild.transform.parent = shildLocation.transform;
                    //���� ���� ��
                    vamprieState.SetInvincible(true);   
                }
            }
            else
            {
                bool allEmpty = true;
                foreach (Transform location in summonLocations)
                {
                    if (location.childCount > 0)
                    {
                        allEmpty = false; break;
                    }
                }
                if (allEmpty && currentShild != null)
                {
                    Destroy(currentShild);
                    currentShild = null;
                    //���� ���� ����
                    vamprieState.SetInvincible(false);

                }
            }

        }


    }


}
