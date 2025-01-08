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
        public int minSpawnCount = 4; // �ּ� ���� ����
        public int maxSpawnCount = 6; // �ִ� ���� ����
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

            int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1); // 2 �Ǵ� 3�� ����
            HashSet<int> selectedIndices = new HashSet<int>(); // �ߺ� ����

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
        /*public Transform target;    // ��ä�ÿ� ���ԵǴ��� �Ǻ��� Ÿ��
        public float angleRange = 30f;
        public float radius = 3f;

        Color _blue = new Color(0f, 0f, 1f, 0.2f);
        Color _red = new Color(1f, 0f, 0f, 0.2f);

        bool isCollision = false;

        
        void Update()
        {
            Vector3 interV = target.position - transform.position;

            // target�� �� ������ �Ÿ��� radius ���� �۴ٸ�
            if (interV.magnitude <= radius)
            {
                // 'Ÿ��-�� ����'�� '�� ���� ����'�� ����
                float dot = Vector3.Dot(interV.normalized, transform.forward);
                // �� ���� ��� ���� �����̹Ƿ� ���� ����� cos�� ���� ���ؼ� theta�� ����
                float theta = Mathf.Acos(dot);
                // angleRange�� ���ϱ� ���� degree�� ��ȯ
                float degree = Mathf.Rad2Deg * theta;

                // �þ߰� �Ǻ�
                if (degree <= angleRange / 2f)
                    isCollision = true;
                else
                    isCollision = false;

            }
            else
                isCollision = false;
        }

        // ����Ƽ �����Ϳ� ��ä���� �׷��� �޼ҵ�
        private void OnDrawGizmos()
        {
            Handles.color = isCollision ? _red : _blue;
            // DrawSolidArc(������, ��ֺ���(��������), �׷��� ���� ����, ����, ������)
            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, radius);
            Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, radius);
        }*/
    }
}