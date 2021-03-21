using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataEtc : ScriptableObject
{
    private const string SettingFileDirectory = "Assets/Resources";
    private const string SettingFilePath = "Assets/Resources/DataEtc.asset";//정확한 파일 위치,만약파

    private static DataEtc _instacne;

    public static DataEtc Instance
    {
        get
        {

            if (_instacne != null)
            {
                return _instacne;
            }

            //만약 없으면 파일경로에 해당 파일이 있는지.
            _instacne = Resources.Load<DataEtc>(path: "DataEtc");
            //에디터에서만 작용하므로 전처리기.
            //런타임이 아니라 에디터에서 사용 => usingUenityEditor
            //해당파일도 없으면
            if (_instacne == null)
            {
#if UNITY_EDITOR
                //존재하지않으면 폴더만듬
                if (!AssetDatabase.IsValidFolder(path: SettingFileDirectory))
                {
                    //폴더 만듬
                    AssetDatabase.CreateFolder(parentFolder: "Assets", newFolderName: "Resources");   //파일IO.도되지만 유니티 에디터에 즉각생성안되므로
                }

                //어떠한 이유로 실패해서 안가져올수도있으므로 하드하게 강제적으로 직접 설정.
                _instacne = AssetDatabase.LoadAssetAtPath<DataEtc>(SettingFilePath);

                //그래도 null이라면 만든다.
                if (_instacne == null)
                {
                    _instacne = CreateInstance<DataEtc>();   //아직 메모리에만 존재
                    AssetDatabase.CreateAsset(_instacne, SettingFilePath); //생성 및 저장파일 경로
                }
#endif


            }
            return _instacne;
        }

    }


    [SerializeField] float addMaxHealthRatio;
    [SerializeField] float addMaxEnergyRatio;
    [SerializeField] float addMoveSpeedRatio;

    [SerializeField] float addDamageRatio;
    [SerializeField] float addRangeRatio;
    [SerializeField] float addProjectileSpeedRatio;

    [SerializeField] int abilityMaxPoint;



    [SerializeField] float lightingStuneTime;
    [SerializeField] float posionTime;
    [SerializeField] float slowTime;
    [SerializeField] float slowRatio;

    [SerializeField] float posionTickTime;
    [SerializeField] float fireTickTime;

    [SerializeField] int posionTickDamage;
    [SerializeField] int fireTickDamage;
    [SerializeField] float probabilityStun; //스턴확률


    public float S_AddMaxHealthRatio => addMaxHealthRatio;
    public float S_AddMaxEnergyRatio => addMaxEnergyRatio;
    public float S_AddMoveSpeedRatio => addMoveSpeedRatio;

    public float S_AddDamageRatio => addDamageRatio;
    public float S_AddRangeRatio => addRangeRatio;
    public float S_AddProjectileSpeedRatio => addProjectileSpeedRatio;

    public int S_AbilityMaxPoint => abilityMaxPoint;

    public float S_LightingStunTime => lightingStuneTime;
    public float S_PosionTime => posionTime;
    public float S_IceSlowTime => slowTime;
    public float S_IceSlowRatio => slowRatio;

    public float S_PosionTickTime => posionTickTime;
    public float S_FireTickTime => fireTickTime;

    public int S_PosionTickDamage => posionTickDamage;

    public int S_FireDamage => fireTickDamage;

    public float S_ProbiblityStun => probabilityStun;// 레벨당 스턴 확률

}