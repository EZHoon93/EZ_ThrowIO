using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : PoolableObject
{
    [SerializeField] UI_AbilityPanel selectAbilityPanel;
    [SerializeField] AbilityType abilityType;
    [SerializeField] Image abilityImage;
    [SerializeField] Image outlineImage;
    [SerializeField] TextMeshProUGUI infoText;
    AbilityContainer abilityContainer;


    private void OnEnable()
    {
        RestComponent();
    }
    public void RestComponent()
    {

    }

    public void Setup(UI_AbilityPanel _selectAbilityPanel , AbilityContainer _abilityContainer)
    {
        selectAbilityPanel = _selectAbilityPanel;
        abilityContainer = _abilityContainer;

        abilityImage.sprite = abilityContainer.sAbilityImage;
        outlineImage.sprite = abilityContainer.sOutLineImage;
        abilityType = abilityContainer.sAbilityType;
        infoText.text = _abilityContainer.sInfoContent;
    }

    public void ClickButton()
    {
        selectAbilityPanel.ClickAblityButton(this, abilityContainer);
        print("ClickButton");
    }

    
}
