using System.Collections;
using UnityEngine;

public class AlienBossPattern : MonoBehaviour
{
    public Animator animator;              // 보스 애니메이터

    public GameObject pt1Particle;         // 발사체 프리팹
    public Transform pt1SpawnPoint;        // 발사체 생성 위치
    public GameObject pt1AttackRange;      // 공격범위 프리팹

    public GameObject pt2Particle;         // 발사체 프리팹
    public Transform pt2SpawnPoint;        // 발사체 생성 위치
    public GameObject pt2AttackRange;      // 공격범위 프리팹

    public GameObject pt3Particle;         // 발사체 프리팹
    public Transform pt3SpawnPoint;        // 발사체 생성 위치
    public GameObject pt3AttackRange;      // 공격범위 프리팹

    public GameObject pt4Particle;         // 발사체 프리팹
    public Transform pt4SpawnPoint;        // 발사체 생성 위치
    public GameObject pt4AttackRange;      // 공격범위 프리팹

    public GameObject pt5Particle;         // 이펙트 프리팹
    public Transform pt5SpawnPoint;        // 발사체 생성 위치
    public GameObject pt5AttackRange;      // 공격범위 프리팹
    public float pt5radius = 5f;           // 공격범위 반지름
    public int pt5spawnCount = 5;          // 이펙트 갯수

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
        else if (Input.GetKeyDown(KeyCode.R))   // 공격패턴4
        {
            StartCoroutine(Pattern4());
        }
        else if (Input.GetKeyDown(KeyCode.T))   // 공격패턴5
        {
            StartCoroutine(Pattern5());
        }
    }
    
    // 공격패턴1 : 부채꼴 공격
    public IEnumerator Pattern1()
    {
        // 공격범위 표시
        Instantiate(pt1AttackRange, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z + 5.0f), Quaternion.Euler(0f, 67.5f, 180f));

        yield return new WaitForSeconds(1.0f);

        animator.SetInteger("Pattern", 1); // 애니메이터 "Pattern" 값을 1로 설정

        StartCoroutine(pt1ProjectileTiming(0.4f));

        StartCoroutine(PatternReset(0.5f)); // 일정시간 후 "Pattern" 값을 0으로 초기화
    }

    // 공격패턴2 : 맵 전체공격(원형)
    public IEnumerator Pattern2()
    {
        // 공격범위 표시
        Instantiate(pt2AttackRange, transform.position, Quaternion.Euler(0f, 0f, 0f));

        yield return new WaitForSeconds(2.0f);

        animator.SetInteger("Pattern", 2); // 애니메이터 "Pattern" 값을 2로 설정

        yield return new WaitForSeconds(1.1f);

        animator.SetInteger("Pattern", 0); // 애니메이터 "Pattern" 값을 0으로 초기화

        Instantiate(pt2Particle, pt2SpawnPoint.position, Quaternion.Euler(0f, 0f, 0f));
    }

    // 공격패턴3 : 맵 전체공격(파도형)
    public IEnumerator Pattern3()
    {
        // 공격범위 표시
        StartCoroutine(pt3AttackRanges());

        yield return new WaitForSeconds(1.5f);

        animator.SetInteger("Pattern", 3); // "Pattern" 값을 3로 설정

        StartCoroutine(pt3Projectiles());

        StartCoroutine(PatternReset(0.5f)); // 일정 시간 후 "Pattern" 값 0으로 초기화
    }

    // 공격패턴3 공격범위 표시
    private IEnumerator pt3AttackRanges()
    {
        for (int i = 0; i < 5; i++)
        {
            // 현재 위치에서 x축으로 -20 * i 만큼 이동
            Vector3 spawnPosition = pt3SpawnPoint.position + new Vector3(10f * i, 0f, 0f);

            // pt3AttackRange 생성
            Instantiate(pt3AttackRange, spawnPosition, Quaternion.Euler(0f, 0f, 0f));

            // 딜레이
            yield return new WaitForSeconds(0.3f);
        }
    }

    // 공격패턴3 공격 이펙트
    private IEnumerator pt3Projectiles()
    {
        for (int i = 0; i < 5; i++)
        {
            // 현재 위치에서 x축으로 -20 * i 만큼 이동
            Vector3 spawnPosition = pt3SpawnPoint.position + new Vector3(10f * i, 0f, 0f);

            // pt3Projectile 생성
            GameObject spawnedObject = Instantiate(pt3Particle, spawnPosition, Quaternion.Euler(0f, 0f, 0f));

            // 딜레이
            yield return new WaitForSeconds(0.3f);

            Destroy(spawnedObject, 1.0f);
        }
    }

    // 공격패턴4 : 근접 공격
    public IEnumerator Pattern4()
    {
        // 공격범위 표시
        Instantiate(pt4AttackRange, new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z + 2.0f), Quaternion.Euler(0f, 0f, 0f));

        yield return new WaitForSeconds(1.5f);

        animator.SetInteger("Pattern", 4); // "Pattern" 값을 4로 설정

        yield return new WaitForSeconds(0.2f);

        Instantiate(pt4Particle, pt4SpawnPoint.position, Quaternion.Euler(0f, 0f, 0f));

        yield return new WaitForSeconds(0.3f);

        animator.SetInteger("Pattern", 0); // "Pattern" 값 0으로 초기화

        
    }

    // 공격패턴5 : 낙석 공격
    public IEnumerator Pattern5()
    {
        animator.SetInteger("Pattern", 5); // "Pattern" 값을 5로 설정

        yield return new WaitForSeconds(1.0f);

        animator.SetInteger("Pattern", 0); // "Pattern" 값 0으로 초기화

        // 공격범위 표시
        StartCoroutine(InstantiatePt5AttackRange());

        //Instantiate(pt4Particle, pt4SpawnPoint.position, Quaternion.Euler(0f, 0f, 0f));
    }

    private IEnumerator InstantiatePt5AttackRange()
    {
        for (int i = 0; i < pt5spawnCount; i++)
        {
            // 원형 범위 내 랜덤한 위치 계산
            Vector2 randomPos = Random.insideUnitCircle * pt5radius;

            // 프리팹 생성 (2D 위치를 3D로 변환)
            Instantiate(pt5AttackRange, new Vector3(randomPos.x, 0, randomPos.y), Quaternion.identity);

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator PatternReset(float delay)
    { 
        yield return new WaitForSeconds(delay);
        animator.SetInteger("Pattern", 0);
    }

    private IEnumerator pt1ProjectileTiming(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(pt1Particle, pt1SpawnPoint.position, Quaternion.Euler(0f, 0f, 0f));
    }

    private IEnumerator pt2ProjectileTiming(float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(pt2Particle, pt2SpawnPoint.position, Quaternion.Euler(0f, 0f, 0f));
    }
}
