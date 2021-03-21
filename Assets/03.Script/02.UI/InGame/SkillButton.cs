using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] Image image_outLine;
    [SerializeField] UltimateJoystick ultimateJoystick;
    [SerializeField] Image image_activeSkill;
    private void Awake()
    {
        ultimateJoystick = GetComponent<UltimateJoystick>();
    }

    private void OnEnable()
    {
        image_outLine.fillAmount = 1;
        ultimateJoystick.enabled = true;
        if (image_activeSkill)
        image_activeSkill.enabled = false;
    }
    public void SetupAcitveSkill(Sprite sprite)
    {
        image_activeSkill.sprite = sprite;
        image_activeSkill.enabled = true;
    }
    public void SetupFillAmount(float value)
    {
        print("게이지 " + value);
        image_outLine.fillAmount = value;
    }

    public void PlayCoolTime(float coolTime)
    {
        StartCoroutine(ProcessCoolTime(coolTime));
    }

    IEnumerator ProcessCoolTime(float coolTime)
    {
        ultimateJoystick.isSkill = true;
        var waitTime = Time.time + coolTime;
        print("프로세스중" + waitTime );
        while (Time.time < waitTime)
        {
            print(( waitTime- Time.time) / coolTime);
            image_outLine.fillAmount = 1 -  (waitTime -Time.time ) / coolTime;
            yield return null;
        }
        image_outLine.fillAmount = 1;

        ultimateJoystick.isSkill = false;

    }
}
