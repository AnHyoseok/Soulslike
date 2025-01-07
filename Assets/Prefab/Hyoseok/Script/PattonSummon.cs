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
        public Transform[] summonLocations; // 소환 위치
        public Transform effectLocation; // 소환이펙트 위치
        public Transform shildLocation; // 쉴드  위치
        public float summonInterval = 5f; // 소환 간격 

        private GameObject currentShild; //현재 쉴드 상태
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
                // 현재 위치에 Bat 오브젝트가 없는지 확인
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

        //소환 몹 존재시 쉴드 생성 없을시 쉴드 파괴
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
