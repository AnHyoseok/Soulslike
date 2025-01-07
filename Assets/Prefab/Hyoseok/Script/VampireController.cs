using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BS.vampire
{
    /// <summary>
    /// ����,��,��2,��ȯ
    /// </summary>
    public class VampireController : MonoBehaviour
    {
        private int nextPattern = 0;

        //private static readonly int none = 0;
        //private static readonly int rush = 1;
        //private static readonly int shoot = 2;
        //private static readonly int shoot2 = 3;
        //private static readonly int summon = 4;

        IEnumerator Rush()
        {

            NextPatternPlay();
            yield return null;
        }

        IEnumerator Shoot()
        {

            NextPatternPlay();
            yield return null;
        }

        IEnumerator Shoot2()
        {

            NextPatternPlay();
            yield return null;
        }

        IEnumerator Summon()
        {

            NextPatternPlay();
            yield return null;
        }

        void Start()
        {
            StartCoroutine("rush");
        }

        void NextPatternPlay()
        {
            switch (nextPattern)
            {
                case 1:
                    StartCoroutine(Rush());
                    break;
                case 2:
                    StartCoroutine(Shoot());
                    break;
                case 3:
                    StartCoroutine(Shoot2());
                    break;
                case 4:
                    StartCoroutine(Summon());
                    break;
            }
        }
    }
}