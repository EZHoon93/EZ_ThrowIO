using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : HealthUI
{
    [Header("Player UI")]
    [SerializeField] Transform infoUI; //이름,체력,기력 UI
    [SerializeField] Transform moveDirectionUI;
    [SerializeField] Transform damageUI;
    [SerializeField] Transform skillUI;
    [SerializeField] PlayerEnergyUI playerEnergyUI;

    [SerializeField] Image redImage;
    [SerializeField] Image yellowImage;
    [SerializeField] Image image_energyWaring;

    IEnumerator waringEnumator;
    IEnumerator energyShakeEnumerator;


    public override void SetUpHealthUI(float newValue, bool isMyCharacter)
    {
        base.SetUpHealthUI(newValue, isMyCharacter);
        //로컬
        if (isMyCharacter)
        {
            playerEnergyUI.gameObject.SetActive(true);
            moveDirectionUI.gameObject.SetActive(true);  
            
        }
        //
        else
        {
            playerEnergyUI.gameObject.SetActive(false);
            moveDirectionUI.gameObject.SetActive(false);
        }
        skillUI.gameObject.SetActive(false);
        damageUI.gameObject.SetActive(false);
    }
    public override void ResetComponent()
    {
        base.ResetComponent();
        if (redImage) redImage.enabled = false;
        if (yellowImage) yellowImage.enabled = false;
        image_energyWaring.gameObject.SetActive(false);

    }



    public override void OnDamage(int damage, int currentHealth)
    {
        base.OnDamage(damage, currentHealth);
        if (waringEnumator != null)
        {
            StopCoroutine(waringEnumator);
        }
        waringEnumator = WaringHP();
        StartCoroutine(waringEnumator);
    }

    #region Shooter UI
    public void UpdateDamageUI(Vector3 targetPoint, float damageRange)
    {
        var pos = targetPoint;
        pos.y = 0.1f;
        damageUI.position = pos;
        damageUI.localScale = new Vector3(damageRange, damageRange, damageRange);
        damageUI.gameObject.SetActive(true);
    }
    public void UpdateSkillDamageUI(Vector3 targetPoint, float damageRange)
    {
        var pos = targetPoint;
        pos.y = 0.1f;
        skillUI.position = pos;

        skillUI.localScale = new Vector3(damageRange * 2, damageRange * 2, damageRange * 2);
        skillUI.gameObject.SetActive(true);
    }

    public void ShakeEnergy(float shakeTime)
    {
        if (energyShakeEnumerator != null) StopCoroutine(energyShakeEnumerator);
        energyShakeEnumerator = Process_ShakeEnergyUI(shakeTime);
        StartCoroutine(energyShakeEnumerator);
    }

    IEnumerator Process_ShakeEnergyUI(float shakeTime )
    {
        var startTime = Time.time;
        var origionPosion = playerEnergyUI.transform.localPosition;
        while (Time.time < startTime + shakeTime)
        {
            image_energyWaring.gameObject.SetActive(true);

            float range = Random.Range(0, 0.1f);
            yield return null;
            playerEnergyUI.transform.localPosition = origionPosion + new Vector3(range, 0, 0);
            yield return null;
            playerEnergyUI.transform.localPosition = origionPosion + new Vector3(-range, 0, 0);
            image_energyWaring.gameObject.SetActive(false);
            yield return null;
        }
        image_energyWaring.gameObject.SetActive(false);
        playerEnergyUI.transform.localPosition = new Vector3(0,-0.13f,0);

    }

    #endregion

    IEnumerator WaringHP()
    {
        float ratio = healthSlider.value / healthSlider.maxValue;
        while (ratio < 0.6f)
        {
            ratio = healthSlider.value / healthSlider.maxValue;
            if (ratio < 0.4f)
            {
                redImage.enabled = true;
                yellowImage.enabled = false;

                yield return new WaitForSeconds(0.6f);
                redImage.enabled = false;
                yield return new WaitForSeconds(0.6f);

                print("깜박이는중");

            }
            else if (ratio >= 0.4f && ratio < 0.7f)
            {
                yellowImage.enabled = true;
                redImage.enabled = false;
                yield return new WaitForSeconds(0.6f);
                yellowImage.enabled = false;
                yield return new WaitForSeconds(0.6f);
            }



        }
        yellowImage.enabled = false;
        redImage.enabled = false;
    }



    public Transform GetDamageUI() => damageUI;
    public Transform GetSkillDamageUI() => skillUI;
    //public Slider GetEnergySlider() => energySlider;
    public Slider GetHealthSlider() => healthSlider;

    public void UpdateDirectionUI(Vector2 moveInput) => moveDirectionUI.transform.localPosition = moveInput * 0.7f;
    public void UpdateEnergySlider(float newValue) => playerEnergyUI.UpdateEnergy(newValue);

    public void SetUpMaxEnergyhSlider(float newValue) => playerEnergyUI.SetupMaxCount((int)newValue);
    

    
    
}
