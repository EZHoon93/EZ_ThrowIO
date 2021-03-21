
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HealthUI : MonoBehaviour
{
    [SerializeField] PoolableContainer UI_DamageText;
    [SerializeField] Transform FixedUI;

    [SerializeField] Image hpFillImage;
    [SerializeField] Image backHpFillImage;
    [Header("Slider")]
    [SerializeField] protected Slider healthSlider;
    [SerializeField] protected Slider backHpSliderUI;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI hpText;

    [Header("Varilable")]
    [SerializeField] bool isBackHpHit;
    IEnumerator sclaeEnumartor;



    public virtual void SetUpHealthUI(float newValue, bool isMyCharacter)
    {
        healthSlider.maxValue = newValue;
        backHpSliderUI.maxValue = newValue;
        backHpSliderUI.value = newValue;

        if (isMyCharacter)
        {
            hpFillImage.sprite = UISetting.Instance.bar_green;
            backHpFillImage.sprite = UISetting.Instance.bar_red;
        }
        else
        {
            hpFillImage.sprite = UISetting.Instance.bar_red;
            backHpFillImage.sprite = UISetting.Instance.bar_yellow;
        }
    }

    public virtual void ResetComponent()
    {
        isBackHpHit = false;
        
    }


    private void Update()
    {
        RotateLocalUI();
        UpdateBackHealthSlider();
        UpdateSclaer();
    }



 


    public virtual void OnDamage(int damage, int currentHealth)
    {
        Invoke("BackHpFun", 1);
        var dagmeText = ObjectPoolManger.Instance.PopPoolableObject(UI_DamageText.sId) as DamageText;
        dagmeText.Play(damage.ToString(), this.transform.position);
        //if (sclaeEnumartor != null)
        //{
        //    StopCoroutine(sclaeEnumartor);
        //}
        //sclaeEnumartor = UpdateSclaer();

        //var text = ObjectPoolManger.Instance.PopPoolableObject(PoolableKeyType.) as DamageText;
        //text.Setup("damage", healthSlider.transform);
    }

   
    IEnumerator UpdateSclaer()
    {
        var size = 1.2f;
        healthSlider.transform.localScale = new Vector3(size, size, 1);
        while(size > 1.0f)
        {
            size -=  Time.deltaTime * 0.6f;
            healthSlider.transform.localScale = new Vector3(size,size, 1);
            yield return null;
        }

    }

    #region BackHealth
    void BackHpFun()
    {
        isBackHpHit = true;
    }
    public virtual void UpdateBackHealthSlider()
    {
        
        if (!isBackHpHit) return;
        backHpSliderUI.value = Mathf.Lerp(backHpSliderUI.value, healthSlider.value, Time.deltaTime * 5);

        
        if (healthSlider.value >= backHpSliderUI.value - 2f)
        {
            isBackHpHit = false;
            backHpSliderUI.value = healthSlider.value;
        }

    }
    #endregion

    private void RotateLocalUI() => this.FixedUI.rotation = Quaternion.Euler(90, 0, 0);


    public virtual void UpdateHealthSlider(int newValue)
    {
        
        hpText.text = newValue.ToString();
        healthSlider.value = newValue;

    }
}
