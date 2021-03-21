using System.Collections;

using UnityEngine;

/// <summary>
/// 팝업에서 의 버튼 이벤트 목록
/// </summary>
public class UI_Pops : MonoBehaviour
{
    

    public void Click_GameExitConfirm()  => UIManager.instance.uI_CommonScene.Click_ExitConfirm();  


    public void Click_GameFindConfirm() => UIManager.instance.uI_CommonScene.Click_GameFindConfirm();

    public void Click_BuyConfirm() => UIManager.instance.uI_ProductManager.Click_BuyConfirm();

    public void Click_GameResultConfirm() => UIManager.instance.SetActive(UIState.Wait);
}
