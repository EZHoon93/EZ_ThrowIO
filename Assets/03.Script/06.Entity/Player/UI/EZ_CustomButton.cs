using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class EZ_CustomButton : MonoBehaviour , IPointerDownHandler, IDragHandler, ISettingUI
{

    public enum ScalingAxis { Width, Height }
    public ScalingAxis scalingAxis = ScalingAxis.Height;
    public enum Anchor { Left, Right };
    public Anchor anchor;
    bool isSetting;


    RectTransform baseTrans;
    [SerializeField] protected Button button;
    public string buttonName;
    [Range(1, 4)]
    public float buttonSize;
    [Range(0, 50)]
    public float customSpacing_X;
    [Range(0, 100)]
    public float customSpacing_Y;



    private void Awake()
    {
        isSetting = false;
    }
#if UNITY_EDITOR
    void Update()
    {
        // Keep the joystick updated while the game is not being played.
        if (Application.isPlaying == false)
            UpdateSizeAndPlacement();
    }
# endif

    public void OnPointerDown(PointerEventData touchInfo)
    {
        if (isSetting)
        {
            //FindObjectOfType<UI_JoystickSetting>().Setup(this);
        }
    }
    public void OnDrag(PointerEventData touchInfo)
    {
        if (isSetting)
        {
            float referenceSize = scalingAxis == ScalingAxis.Height ? baseTrans.sizeDelta.y : baseTrans.sizeDelta.x;
            float textureSize = referenceSize * (buttonSize / 10);
            var x = ((100 * touchInfo.position.x) - (50 * textureSize)) / (baseTrans.sizeDelta.x - textureSize);
            var y = ((100 * touchInfo.position.y) - (50 * textureSize)) / (baseTrans.sizeDelta.y - textureSize);
            if (anchor == Anchor.Right)
            {
                x = 100 - x;
            }
            if (x > 50) return;
            customSpacing_X = x;
            customSpacing_Y = y;
            UpdatePositioning();
        }
    }


    public void UpdatePositioning()
    {
        UpdateSizeAndPlacement();
    }

    void UpdateSizeAndPlacement()
    {
        


        float referenceSize = scalingAxis == ScalingAxis.Height ? Screen.height : Screen.width;
        float textureSize = referenceSize * (buttonSize / 10);

        if (baseTrans == null)
            baseTrans = GetComponent<RectTransform>();


        Vector2 imagePosition = ConfigureImagePosition(new Vector2(textureSize, textureSize), new Vector2(customSpacing_X, customSpacing_Y));


        // Temporary float to store a modifier for the touch area size.
        float fixedTouchSize = 1.01f;

        // Temporary Vector2 to store the default size of the joystick.
        Vector2 tempVector = new Vector2(textureSize, textureSize);

        // Apply the joystick size multiplied by the fixedTouchSize.
        baseTrans.sizeDelta = tempVector * fixedTouchSize;

        // Apply the imagePosition modified with the difference of the sizeDelta divided by 2, multiplied by the scale of the parent canvas.
        baseTrans.position = imagePosition - ((baseTrans.sizeDelta - tempVector) / 2);


        if(button == null)
        {
            button = GetComponent<Button>();
        }
        button.transform.position = imagePosition;

    }
    Vector2 ConfigureImagePosition(Vector2 textureSize, Vector2 customSpacing)
    {
        // First, fix the customSpacing to be a value between 0.0f and 1.0f.
        Vector2 fixedCustomSpacing = customSpacing / 100;

        // Then configure position spacers according to the screen's dimensions, the fixed spacing and texture size.
        float positionSpacerX = Screen.width * fixedCustomSpacing.x - (textureSize.x * fixedCustomSpacing.x);
        float positionSpacerY = Screen.height * fixedCustomSpacing.y - (textureSize.y * fixedCustomSpacing.y);

        // Create a temporary Vector2 to modify and return.
        Vector2 tempVector;

        // If it's left, simply apply the positionxSpacerX, else calculate out from the right side and apply the positionSpaceX.
        //tempVector.x = Achor == achor.Left ? positionSpacerX  : (Screen.width ) - positionSpacerX;
        tempVector.x = anchor == Anchor.Left ? positionSpacerX : (Screen.width - textureSize.x) - positionSpacerX;
        // Apply the positionSpacerY variable.
        tempVector.y = positionSpacerY;

        // Return the updated temporary Vector.
        return tempVector;
    }


    public virtual void ClickButton()
    {

    }
    public void ChangeAnchor()
    {
        if (anchor == Anchor.Left)
        {
            anchor = Anchor.Right;
        }
        else
        {
            anchor = Anchor.Left;
        }
    }

    public void ChangeSettingMode(bool _isSetting)
    {
        isSetting = _isSetting;
        gameObject.SetActive(true);
    }

    public void SetupJoystick(JoystickSetting joystickSetting)
    {
        customSpacing_X = joystickSetting.vector2.x;
        customSpacing_Y = joystickSetting.vector2.y;
        buttonSize = joystickSetting.size;
        UpdatePositioning();
    }

    public void ResetJoystickBySettingValue()
    {
        foreach (var u in UISetting.Instance.joystickSettings)
        {
            if (string.Compare(u.joystickName, buttonName) == 0)
            {
                customSpacing_X = u.vector2.x;
                customSpacing_Y = u.vector2.y;
                buttonSize= u.size;
                return;
            }
        }
    }

    public string GetName() => buttonName;

    public void ChangeSize(float sizeValue)
    {
        buttonSize = sizeValue;
        UpdatePositioning();

    }
    public float GetSizeValue()
    {
        return buttonSize;
    }
    public Vector2 GetPosion()
    {
        return new Vector2(customSpacing_X, customSpacing_Y);
    }
}
