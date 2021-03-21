using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    #region 싱글톤
    public static CameraManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraManager>();
            }

            return _instance;
        }
    }
    private static CameraManager _instance; // 싱글톤이 할당될 변수

    #endregion

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    Transform followTarget;


    private void Awake()
    {
        if (virtualCamera == null)
        {
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        }
        else
        {
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }


    public void FollowTarget(Transform target)
    {
        followTarget = target;
        virtualCamera.Follow = followTarget;
        //virtualCamera.LookAt = followTarget;
    }

    public void ShakeCamera(float _time, float _ampltiude, float _frequency)
    {
        virtualCameraNoise.m_AmplitudeGain = _ampltiude;
        virtualCameraNoise.m_FrequencyGain = _frequency;
        Invoke("ShakeCameraOff", _time);
    }

    public void ShakeCameraOff()
    {
        virtualCameraNoise.m_AmplitudeGain = 0.0f;
        virtualCameraNoise.m_FrequencyGain = 0.0f;
    }

    public CinemachineVirtualCamera GetVirtualCamera() => virtualCamera;
}
