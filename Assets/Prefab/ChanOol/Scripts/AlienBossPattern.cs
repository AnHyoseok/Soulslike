using System.Collections;
using UnityEngine;

public class AlienBossPattern : MonoBehaviour
{
    public Animator animator;              // ���� �ִϸ�����

    public GameObject pt1Projectile;       // �߻�ü ������
    public Transform pt1SpawnPoint;        // �߻�ü ���� ��ġ
    public GameObject pt1AttackRange;      // ���ݹ��� ������

    public GameObject pt2Projectile;       // �߻�ü ������
    public Transform pt2SpawnPoint;        // �߻�ü ���� ��ġ
    public GameObject pt2AttackRange;      // ���ݹ��� ������

    public GameObject pt3Projectile;       // �߻�ü ������
    public Transform pt3SpawnPoint;        // �߻�ü ���� ��ġ
    public GameObject pt3AttackRange;      // ���ݹ��� ������

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
        if (Input.GetKeyDown(KeyCode.Q))        // ��������1
        {
            StartCoroutine(Pattern1());
        }
        else if (Input.GetKeyDown(KeyCode.W))   // ��������2
        {
            StartCoroutine(Pattern2());
        }
        else if (Input.GetKeyDown(KeyCode.E))   // ��������3
        {
            StartCoroutine(Pattern3());
        }
        /*else if (Input.GetKeyDown(KeyCode.R))
        {
            Pattern4();
        }*/
    }
    
    // ��������1 : ��ä�� ����
    public IEnumerator Pattern1()
    {
        // ���ݹ��� ����
        Instantiate(pt1AttackRange, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z + 5.0f), Quaternion.Euler(0f, 67.5f, 180f));

        // ������
        yield return new WaitForSeconds(1.0f);

        animator.SetInteger("Pattern", 1); // "Pattern" ���� 1�� ����

        StartCoroutine(pt1ProjectileTiming(0.4f));

        StartCoroutine(PatternReset(0.5f)); // ���� �ð� �� "Pattern" �� 0���� �ʱ�ȭ
    }


    // ��������2 : �� ��ü����(����)
    public IEnumerator Pattern2()
    {
        // ���ݹ��� ����
        Instantiate(pt2AttackRange, transform.position, Quaternion.Euler(0f, 0f, 0f));

        // ������
        yield return new WaitForSeconds(2.5f);

        animator.SetInteger("Pattern", 2); // "Pattern" ���� 2�� ����

        //yield return StartCoroutine(pt2ProjectileTiming(1.1f));
        yield return new WaitForSeconds(1.1f);

        animator.SetInteger("Pattern", 0); // "Pattern" �� 0���� �ʱ�ȭ

        Instantiate(pt2Projectile, pt2SpawnPoint.position, Quaternion.Euler(0f, 0f, 0f));

        //yield return StartCoroutine(PatternReset(0.5f)); // ���� �ð� �� "Pattern" �� 0���� �ʱ�ȭ
        
    }

    // ��������3 : �� ��ü����(�ĵ���)
    public IEnumerator Pattern3()
    {
        StartCoroutine(pt3AttackRanges());

        // ������
        yield return new WaitForSeconds(1.5f);

        animator.SetInteger("Pattern", 3); // "Pattern" ���� 3�� ����

        StartCoroutine(pt3Projectiles());

        StartCoroutine(PatternReset(0.5f)); // ���� �ð� �� "Pattern" �� 0���� �ʱ�ȭ
    }

    private IEnumerator pt3AttackRanges()
    {
        for (int i = 0; i < 5; i++)
        {
            // ���� ��ġ���� x������ -20 * i ��ŭ �̵�
            Vector3 spawnPosition = pt3SpawnPoint.position + new Vector3(20f * i, 0f, 0f);

            // pt3AttackRange ����
            Instantiate(pt3AttackRange, spawnPosition, Quaternion.Euler(0f, 0f, 0f));

            // ������
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator pt3Projectiles()
    {
        for (int i = 0; i < 5; i++)
        {
            // ���� ��ġ���� x������ -20 * i ��ŭ �̵�
            Vector3 spawnPosition = pt3SpawnPoint.position + new Vector3(20f * i, 0f, 0f);

            // pt3Projectile ����
            GameObject spawnedObject = Instantiate(pt3Projectile, spawnPosition, Quaternion.Euler(0f, 0f, 0f));

            // ������
            yield return new WaitForSeconds(0.3f);

            Destroy(spawnedObject, 1.0f);
        }
    }

    // ��������4 : Ŭ�� ��ȯ
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
