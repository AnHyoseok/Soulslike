using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;  // OnDrawGizmos

namespace BS.Demon
{
    public class DemonPattern : MonoBehaviour
    {
        #region Variables
        public BallRise ball;
        public Transform[] Points;
        public int minSpawnCount = 4; // 최소 생성 개수
        public int maxSpawnCount = 6; // 최대 생성 개수
        #endregion
        private void Start()
        {
            //SpawnObjects();
        }
        public void SpawnObjects()
        {
            if (Points.Length == 0 || ball == null)
            {
                Debug.LogWarning("Spawn points or object to spawn not set!");
                return;
            }

            int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1); // 2 또는 3개 생성
            HashSet<int> selectedIndices = new HashSet<int>(); // 중복 방지

            while (selectedIndices.Count < spawnCount)
            {
                int randomIndex = Random.Range(0, Points.Length);
                selectedIndices.Add(randomIndex);
            }

            foreach (int index in selectedIndices)
            {
                GameObject game = Instantiate(ball.gameObject, Points[index].position, Quaternion.identity);
                game.GetComponent<BallRise>().StartRise();
                Debug.Log($"Spawned object at point {index + 1}");
            }
        }
        /*public Transform target;    // 부채꼴에 포함되는지 판별할 타겟
        public float angleRange = 30f;
        public float radius = 3f;

        Color _blue = new Color(0f, 0f, 1f, 0.2f);
        Color _red = new Color(1f, 0f, 0f, 0.2f);

        bool isCollision = false;

        
        void Update()
        {
            Vector3 interV = target.position - transform.position;

            // target과 나 사이의 거리가 radius 보다 작다면
            if (interV.magnitude <= radius)
            {
                // '타겟-나 벡터'와 '내 정면 벡터'를 내적
                float dot = Vector3.Dot(interV.normalized, transform.forward);
                // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
                float theta = Mathf.Acos(dot);
                // angleRange와 비교하기 위해 degree로 변환
                float degree = Mathf.Rad2Deg * theta;

                // 시야각 판별
                if (degree <= angleRange / 2f)
                    isCollision = true;
                else
                    isCollision = false;

            }
            else
                isCollision = false;
        }

        // 유니티 에디터에 부채꼴을 그려줄 메소드
        private void OnDrawGizmos()
        {
            Handles.color = isCollision ? _red : _blue;
            // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, radius);
            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, radius);
        }*/
    }
}