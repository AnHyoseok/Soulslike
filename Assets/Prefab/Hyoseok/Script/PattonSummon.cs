using System.Collections;
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
        void Start()
        {       
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
                // ���� ��ġ�� Bat ������Ʈ�� ������ Ȯ��
                if (location.childCount == 0)
                {
                   GameObject bat = Instantiate(batPrefab, location.position, location.rotation, location);
                    Summoned = true;
           
                }
            }
            if (Summoned)
            {
                GameObject effectGo = Instantiate(summonEffect, effectLocation.position, effectLocation.rotation); 
                Destroy(effectGo, 3f);
            }
            shild(Summoned);    
        }

        //��ȯ �� ����� ���� ���� ������ ���� �ı�
        void shild(bool batExist)
        {
            if (batExist)
            {
                if(currentShild == null)

                {
                  
                    currentShild = Instantiate(shildEffect, shildLocation.position, shildLocation.rotation);
                }
            }
            else
            {
                if (currentShild != null)
                {
                    Destroy(currentShild);
                    currentShild = null;
                }
            }

        }

       
    }


}
