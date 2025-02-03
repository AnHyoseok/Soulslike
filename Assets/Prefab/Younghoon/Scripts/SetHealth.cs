using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

// Renderer와 머티리얼 인덱스를 관리하기 위한 구조체
[System.Serializable]
public struct RendererIndexData
{
    public Renderer renderer;
    public int metarialIndx;

    public RendererIndexData(Renderer _renderer, int index)
    {
        renderer = _renderer;
        metarialIndx = index;
    }
}

public class SetHealth : MonoBehaviour, IDamageable
{
    #region Variables

    [SerializeField] private float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
        private set { maxHealth = value; }
    }

    private float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        private set
        {
            //죽음 체크
            if (isDeath)
                return;

            currentHealth = value;
            bosshealthBarImage.fillAmount = GetRatio();
            bossHealthText.text = $"{GetRatio() * 100:F0}%";

            if (currentHealth <= 0f)
            {
                isDeath = true;
                //TODO :: 죽음 구현
            }
        }
    }

    private bool isDeath = false;           //죽음 체크

    public float GetRatio() => CurrentHealth / MaxHealth;

    private Image bosshealthBarImage;           // 보스 체력바
    private TextMeshProUGUI bossHealthText;     // 보스 체력 텍스트

    #region 피격 효과
    public Material bodyMaterial; // 데미지를 줄 머티리얼
    [GradientUsage(true)] public Gradient hitEffectGradient; // 데미지 컬러 그라디언트 효과
    private List<RendererIndexData> bodyRenderers = new List<RendererIndexData>();
    private MaterialPropertyBlock bodyFlashMaterialPropertyBlock;
    private float lastTimeEffect; // 마지막으로 효과가 발생한 시간
    [SerializeField] private float flashDuration = 0.5f;
    private bool isFlashing; // 반짝거림 상태
    private Gradient currentEffectGradient; // 현재 적용 중인 그라디언트
    #endregion

    #endregion

    private void Start()
    {
        currentHealth = MaxHealth;

        FindAndResetComponents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            TakeDamage(10);
        }

        // 데미지 효과 업데이트
        if (isFlashing)
        {
            UpdateEffect();
        }
    }

    private void TriggerEffect()
    {
        lastTimeEffect = Time.time; // 효과 시작 시간 기록
        currentEffectGradient = hitEffectGradient; // 현재 적용 중인 그라디언트 설정
        isFlashing = true; // 반짝거림 시작
    }

    private void UpdateEffect()
    {
        // 반짝거림 지속 시간 계산
        float elapsed = Time.time - lastTimeEffect;

        if (elapsed > flashDuration)
        {
            // 반짝거림 효과 종료
            ResetMaterialProperties();
            return;
        }

        // 현재 시간에 따른 색상 계산
        Color currentColor = currentEffectGradient.Evaluate(elapsed / flashDuration);
        bodyFlashMaterialPropertyBlock.SetColor("_EmissionColor", currentColor);

        // 각 렌더러에 효과 적용
        foreach (var data in bodyRenderers)
        {
            data.renderer.SetPropertyBlock(bodyFlashMaterialPropertyBlock, data.metarialIndx);
        }
    }

    private void ResetMaterialProperties()
    {
        // 반짝거림 효과를 초기화하여 원래 상태로 되돌림
        bodyFlashMaterialPropertyBlock.SetColor("_EmissionColor", Color.black);
        foreach (var data in bodyRenderers)
        {
            data.renderer.SetPropertyBlock(bodyFlashMaterialPropertyBlock, data.metarialIndx);
        }
        isFlashing = false;
    }


    public void TakeDamage(float damage)
    {
        //죽었으면 실행하지 않음
        if (isDeath) return;

        //데미지 계산
        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);

        TriggerEffect();
    }

    private void FindAndResetComponents()
    {
        GameObject bossHealthBar = GameObject.Find("BossHealthBar");

        if (bossHealthBar != null)
        {
            // bossHealthBar의 모든 하위 오브젝트에서 컴포넌트를 가진 오브젝트들을 찾음
            // FirstOrDefault()는 첫 번째로 조건을 만족하는 요소를 반환하고, 없으면 null을 반환
            bosshealthBarImage = bossHealthBar.GetComponentsInChildren<Image>()
                .FirstOrDefault(img => img.gameObject.name == "FillAmount");

            bossHealthText = bossHealthBar.GetComponentsInChildren<TextMeshProUGUI>()
                .FirstOrDefault(txt => txt.gameObject.name == "HealthText");

            if (bosshealthBarImage == null)
            {
                Debug.LogError("FillAmount 에러");
            }
            if (bossHealthText == null)
            {
                Debug.LogError("HealthText 에러");
            }
        }
        else
        {
            Debug.LogWarning("BossHealthBar 오브젝트를 찾을 수 없습니다.");
        }

        // 반짝임 효과 초기화
        bodyFlashMaterialPropertyBlock = new MaterialPropertyBlock();
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);
        foreach (var renderer in renderers)
        {
            for (int i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                if (renderer.sharedMaterials[i] == bodyMaterial)
                {
                    bodyRenderers.Add(new RendererIndexData(renderer, i));
                }
            }
        }

    }
}
