using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIState currentState;
    [Header("Controller")]
    public UI_LobbyScene uI_LobbyScene;
    public UI_CommonScene uI_CommonScene;
    public UI_ProductManager uI_ProductManager;
    public UI_GameScene  uI_GameScene;
    [Header("GameObject")]
    public GameObject panel_joysticks;
    public GameObject panel_popups;
    public GameObject panel_loading;

    #region 싱글톤
    // 외부에서 싱글톤 오브젝트를 가져올때 사용할 프로퍼티
    public static UIManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<UIManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static UIManager m_instance; // 싱글톤이 할당될 static 변수

    #endregion
    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        

    }
    /// <summary>
    /// 플레이어의 정보로 조이스틱 셋업 => 로그인 부분에서 구현
    /// </summary>

    public void SetActive(UIState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case UIState.Login:

                break;
            case UIState.Lobby:
                SetActiveJoysticks(false);
                break;
            case UIState.Wait:
                SetActiveJoysticks(false);
                break;
            case UIState.Game:
                break;
        }
        uI_CommonScene.SetActive(currentState);
        uI_GameScene.SetActive(currentState);
        uI_LobbyScene.SetActive(currentState);
        uI_ProductManager.SetActive(currentState);
        SetActiveLoading(false);
    }
  
    public void SetActiveJoysticks(bool active)
    {
        panel_joysticks.SetActive(active);
    }

    public void SetActiveLoading(bool active) => panel_loading.SetActive(active);

    public void FindGameRoom(string roomName, bool isSceret, UnityEngine.UI.Button gameJoinButton)
    {
        

    }
    private void Update()
    {
    }



}
