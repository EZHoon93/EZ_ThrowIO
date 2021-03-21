
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class CreaterScriptable : MonoBehaviour
{

    private const string SettingFileDirectory = "Assets/Resources";
    private const string SettingFilePath = "Assets/Resources/DataSetting.asset";//정확한 파일 위치,만약파
    public GameObject[] characterprefabObject;
    public string folderName;
    public string fileName;

    List<string> tempList = new List<string>();

    // Start is called before the first frame update
    public void Create()
    {
#if UNITY_EDITOR

        //만약 없으면 파일경로에 해당 파일이 있는지.
        if (!AssetDatabase.IsValidFolder(path: SettingFileDirectory + "/" + folderName))
        {
            //없다면 폴더 생성
            AssetDatabase.CreateFolder(parentFolder: "Assets/Resources", newFolderName: folderName);   //파일IO.도되지만 유니티 에디터에 즉각생성안되므로

        }
        int i = 0;
        foreach (var t in characterprefabObject)
        {
            CharacterContainer data = ScriptableObject.CreateInstance<CharacterContainer>();
            //고유 번호가 중복된지 확인/
            string temp = null;
            do
            {
                temp = Utility.RandomPID.GetRandomPassword(6);

            } while (Check(temp));
            //중복이 없으면 아래 시작 및 추가
            tempList.Add(temp);
            var pID = temp;
            //data.Setup(pID,  t.GetComponent<CharacterObject>() );
            string dataName = fileName + i;
            AssetDatabase.CreateAsset(data, "Assets/Resources/" + folderName + "/" + dataName + ".asset");
            AssetDatabase.SaveAssets();
            Selection.activeObject = data;
            i++;
        }
#endif

          

        }

    bool Check(string _pID)
    {
        //같은값이 있으면 true
        return tempList.Any(s => s == _pID);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
