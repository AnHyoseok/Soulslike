using System.Collections;
using UnityEngine;

public class AlienBossPattern : MonoBehaviour
{
    public Animator animator;              // 보스 애니메이터

    public GameObject pt1Projectile;       // 발사체 프리팹
    public Transform pt1SpawnPoint;        // 발사체 생성 위치
    public GameObject pt1AttackRange;      // 공격범위 프리팹

    public GameObject pt2Projectile;       // 발사체 프리팹
    public Transform pt2SpawnPoint;        // 발사체 생성 위치
    public GameObject pt2AttackRange;      // 공격범위 프리팹

    public GameObject pt3Projectile;       // 발사체 프리팹
    public Transform pt3SpawnPoint;        // 발사체 생성 위치
    public GameObject pt3AttackRange;      // 공격범위 프리팹

    /*public GameObject cloneAlien;
    public Transform pt4SpawnPoint1;
    public Transform pt4SpawnPoint2;*/


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))        // 공격패턴1
        {
            StartCoroutine(Pattern1());
        }
        else if (Input.GetKeyDown(KeyCode.W))   // 공격패턴2
        {
            StartCoroutine(Pattern2());
        }
        else if (Input.GetKeyDown(KeyCode.E))   // 공격패턴3
        {
            StartCoroutine(Pattern3());
        }
        /*else if (Input.GetKeyDown(KeyCode.R))
        {
            Pattern4();
        }*/
    }
    
    // 공격패턴1 : 부채꼴 공격
    public IEnumerator Pattern1()
    {
        // 공격범위 생성
        Instantiate(pt1AttackRange, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z + 5.0f), Quaternion.Euler(0f, 67.5f, 180f));

        // 딜레이
        yield return new WaitForSeconds(1.0f);

        animator.SetInteger("Pattern", 1); // "Pattern" 값을 1로 설정

        StartCoroutine(pt1ProjectileTiming(0.4f));

        StartCoroutine(PatternReset(0.5f)); // 일정 시간 후 "Pattern" 값 0으로 초기화
    }


    // 공격패턴2 : 맵 전체공격(원형)
    public IEnumerator Pattern2()
    {
        // 공격범위 생성
        Instantiate(pt2AttackRange, transform.position, Quaternion.Euler(0f, 0f, 0f));

        // 딜레이
        yield return new WaitForSeconds(2.5f);

        animator.SetInteger("Pattern", 2); // "Pattern" 값을 2로 설정

        //yield return StartCoroutine(pt2ProjectileTiming(1.1f));
        yield return new WaitForSeconds(1.1f);

        animator.SetInteger("Pattern", 0); // "Pattern" 값 0으로 초기화

        Instantiate(pt2Projectile, pt2SpawnPoint.position, Quaternion.Euler(0f, 0f, 0f));

        //yield return StartCoroutine(PatternReset(0.5f)); // 일정 시간 후 "Pattern" 값 0으로 초기화
        
    }

    // 공격패턴3 : 맵 전체공격(파도형)
    public IEnumerator Pattern3()
    {
        StartCoroutine(pt3AttackRanges());

        // 딜레이
        yield return new WaitForSeconds(1.5f);

        animator.SetInteger("Pattern", 3); // "Pattern" 값을 3로 설정

        StartCoroutine(pt3Projectiles());

        StartCoroutine(PatternReset(0.5f)); // 일정 시간 후 "Pattern" 값 0으로 초기화
    }

    private IEnumerator pt3AttackRanges()
    {
        for (int i = 0; i < 5; i++)
        {
            // 현재 위치에서 x축으로 -20 * i 만큼 이동
            Vector3 spawnPosition = pt3SpawnPoint.position + new Vector3(20f * i, 0f, 0f);

            // pt3AttackRange 생성
            Instantiate(pt3AttackRange, spawnPosition, Quaternion.Euler(0f, 0f, 0f));

            // 딜레이
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator pt3Projectiles()
    {
        for (int i = 0; i < 5; i++)
        {
            // 현재 위치에서 x축으로 -20 * i 만큼 이동
            Vector3 spawnPosition = pt3SpawnPoint.position + new Vector3(20f * i, 0f, 0f);

            // pt3Projectile 생성
            GameObject spawnedObject = Instantiate(pt3Projectile, spawnPosition, Quaternion.Euler(0f, 0f, 0f));

            // 딜레이
            yield return new WaitForSeconds(0.3f);

            Destroy(spawnedObject, 1.0f);
        }
    }

    // 공격패턴4 : 클론 소환
    /*public void Pattern4()
    {
        Instantiate(cloneAlien, pt4SpawnPoint1.position, Quaternion.Euler(0f, 0f, 0f));
        Instantiate(cloneAlien, pt4SpawnPoint2.position, Quaternion.Euler(0f, 90f, 0f));
    }*/

    private IEnumerator PatternReset(float delay)
    { 
        yield return new WaitForSeconds(delay);
        animator.SetInteger("Pattern", 0);
    }

    private IEnumerator pt1ProjectileTiming(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(pt1Projectile, pt1SpawnPoint.position, Quaternion.Euler(0f, 0f, 0f));
    }

    private IEnumerator pt2ProjectileTiming(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(pt2Projectile, pt2SpawnPoint.position, Quaternion.Euler(0f, 0f, 0f));
    }
}
