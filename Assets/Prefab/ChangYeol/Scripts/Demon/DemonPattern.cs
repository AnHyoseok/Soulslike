using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BS.Demon
{
    public class DemonPattern : MonoBehaviour
    {
        #region Variables
        private DemonController controller;

        //패턴 1
        public BallRise ball;
        public Transform[] Points;
        public int minSpawnCount = 4; // 최소 생성 개수
        public int maxSpawnCount = 6; // 최대 생성 개수
        private HashSet<int> selectedIndices = new HashSet<int>(); //중복방지
        
        //패턴 2
        public Transform ballTranfrom;

        //공격범위
        [SerializeField] private GameObject attackRangePrefab;
        [SerializeField] private Vector3[] attackRangeScale = new Vector3[2];
        [SerializeField]private float[] rangeSize = new float[2];
        #endregion
        private void Start()
        {
            controller = GetComponent<DemonController>();
            //SpawnObjects();
        }
        //패턴 1
        public void SpawnObjects()
        {
            
            if (Points.Length == 0 || ball == null)
            {
                Debug.LogWarning("Spawn points or object to spawn not set!");
                return;
            }

            int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1); // 2 또는 3개 생성

            while (selectedIndices.Count < spawnCount)
            {
                int randomIndex = Random.Range(0, Points.Length);
                selectedIndices.Add(randomIndex);
            }

            foreach (int index in selectedIndices)
            {
                StartCoroutine(AttackRangeSpawn(index));
                GameObject game = Instantiate(ball.gameObject, Points[index].position, Quaternion.identity);
                game.GetComponent<BallRise>().StartRise();
                Debug.Log($"Spawned object at point {index + 1}");
                controller.lastAttackTime[0] = Time.time;
            }
        }
        IEnumerator AttackRangeSpawn(int index)
        {
            GameObject Range = Instantiate(attackRangePrefab, Points[index].position + new Vector3(0,0.2f,0), Quaternion.identity);
            Range.GetComponent<DemonAttackRange>().StartGrowing(attackRangeScale[0], rangeSize[0]);
            yield return new WaitForSeconds[3];
            Destroy(Range,2f);
        }
        //패턴 2
        public void AttackBall()
        {
            StartCoroutine(AttackRangeBall());
            GameObject effgo = Instantiate(controller.effect[0],ballTranfrom.position,Quaternion.identity);
            Destroy(effgo,2f);
            controller.lastAttackTime[1] = Time.time;
        }
        IEnumerator AttackRangeBall()
        {
            Vector3 ballRange = new Vector3(ballTranfrom.position.x, 0.2f, ballTranfrom.position.z);
            GameObject Range = Instantiate(attackRangePrefab, ballRange, Quaternion.identity);
            Range.GetComponent<DemonAttackRange>().StartGrowing(attackRangeScale[1], rangeSize[1]);
            yield return new WaitForSeconds[3];
            Destroy(Range, 2f);
        }
        //패턴 3
        public void ShootAttack()
        {
            controller.lastAttackTime[2] = Time.time;
        }
    }
}