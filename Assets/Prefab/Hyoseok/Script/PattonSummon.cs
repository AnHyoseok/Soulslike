using BS.Utility;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace BS.vampire
{
    public class PattonSummon : MonoBehaviour
    {

        public AudioClip shildSummonSound;
        public AudioClip shildBreakSound;

        public GameObject batPrefab;
        public GameObject summonEffect;
        public GameObject shildEffect;
        public Transform[] summonLocations; // 소환 위치
        public Transform effectLocation; // 소환이펙트 위치
        public Transform shildLocation; // 쉴드  위치
        public float summonInterval = 5f; // 소환 간격 

        private GameObject currentShild; //현재 쉴드 상태
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
                // 현재 위치에 Bat 확인
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

        //소환 몹 존재시 쉴드 생성 없을시 쉴드 파괴
        void shild(bool batExist)
        {
            if (batExist)
            {
                if (currentShild == null)
                {
                    AudioUtility.CreateSFX(shildSummonSound, transform.position, AudioUtility.AudioGroups.Sound);
                    currentShild = Instantiate(shildEffect, shildLocation.position, shildLocation.rotation);
                    currentShild.transform.parent = shildLocation.transform;
                    //보스 무적 온
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
                    AudioUtility.CreateSFX(shildBreakSound, transform.position, AudioUtility.AudioGroups.Sound);
                    Destroy(currentShild);
                    currentShild = null;
                    //보스 무적 해제
                    vamprieState.SetInvincible(false);

                }
            }

        }


    }


}
