using UnityEngine;

public class GravityPull : MonoBehaviour
{
    public float pullForce = 10f; // 끌어당기는 힘의 크기
    public float maxDistance = 5f; // 최대 끌어당김 거리

    private void FixedUpdate()
    {
        // Physics.OverlapSphere를 사용해 maxDistance 내의 모든 콜라이더 검색
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance);

        #region 플레이어 레이어 추가시 코드 교체
        // Physics.OverlapSphere를 사용해 maxDistance 내의 "Player" 레이어만 검색
        //Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance, playerLayerMask);

        //foreach (Collider collider in colliders)
        //{
        //    Transform target = collider.transform;

        //    if (target != null)
        //    {
        //        // 방향 벡터 계산 (중심점 - 오브젝트 위치)
        //        Vector3 direction = transform.position - target.position;

        //        // y 값을 제외하고 x와 z 값만 사용
        //        direction.y = 0;

        //        // 거리 계산 (수평 거리만)
        //        float distance = direction.magnitude;

        //        // 거리에 따라 힘 감소 (선형 감쇠)
        //        float speed = Mathf.Lerp(pullForce, 0, distance / maxDistance);

        //        // 힘 적용 (x, z 축만)
        //        target.position += direction.normalized * speed * Time.deltaTime;
        //    }
        //}
        #endregion

        #region 레이어 추가전 디버그용 GravityPull 효과 (추후 삭제 필요)
        foreach (Collider collider in colliders)
        {
            Debug.Log("1");
            if (collider.gameObject.tag != "Player")
            {
                Debug.Log("2");
                continue;
            }
            Debug.Log("3");
            Transform target = collider.transform;
            if (target != null)
            {
                // 방향 벡터 계산 (중심점 - 오브젝트 위치)
                Vector3 direction = transform.position - target.position;

                // y 값을 제외하고 x와 z 값만 사용
                direction.y = 0;

                // 거리 계산
                float distance = direction.magnitude;
                Debug.Log($"거리: {distance}, 대상: {collider.gameObject.name}");
                // 거리에 따라 힘 감소 (선형 감쇠)
                float speed = Mathf.Lerp(pullForce, 0, distance / maxDistance);

                // 힘 적용
                target.position += direction.normalized * speed * Time.deltaTime;
                Debug.Log($"속도: {speed}, 방향: {direction.normalized}");
            }
        }
        #endregion
    }

    #region 테스트 범위확인용 기즈모
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
    #endregion
}
