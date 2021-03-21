using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class EzData
{

}

public static class UserDataSystem
{

    //public UserData userData = new UserData();

    public static bool SaveData(object userData, string name)
    {
        try
        {
            Debug.Log(userData);

            string jsonData = JsonUtility.ToJson(userData, true);
            #region 암호화
            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
            //string code = System.Convert.ToBase64String(bytes);
            //File.WriteAllText(path, code);
            #endregion
            string path = Path.Combine(Application.persistentDataPath, name);

            //filePath = Application.persistentDataPath + "/MyItemText.txt";
            File.WriteAllText(path, jsonData);
            Debug.Log(GetSavePath(name));
            Debug.Log("저장완료" + jsonData);

        }
        catch (Exception)
        {
            Debug.Log("저장실패");

            return false;
        }
        //string path = Path.Combine(Application.dataPath, name);
        return true;

    }

    public static T LoadData <T> (string name ) where T : EzData
    {
        if (!DoseSaveGameExist(name))
        {
            Debug.Log("lod x");
            return default;
        }
        try
        {

            string path = Path.Combine(Application.persistentDataPath, name);
            string code = File.ReadAllText(path);
            #region 암호화
            //byte[] bytes = System.Convert.FromBase64String(code);
            //string jsonData = System.Text.Encoding.UTF8.GetString(bytes);
            #endregion

            string jsonData = File.ReadAllText(path);
            var userData = JsonUtility.FromJson<T>(jsonData);


            

         



            return userData;
        }
        catch (Exception)
        {
            return default;
        }


    }

    public static bool DeleteSaveGame(string name)
    {
        try
        {
            Debug.Log("삭제중");
            string path = Path.Combine(Application.persistentDataPath, name);
            File.Delete(path);
        }
        catch (Exception)
        {
            Debug.Log("삭제실패");

            return false;
        }
        return true;
    }
    public static bool DoseSaveGameExist(string name)
    {
        return File.Exists(GetSavePath(name));
    }


    private static string GetSavePath(string name)
    {
        return Path.Combine(Application.persistentDataPath, name);
        
    }
}

[Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;

    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() { return target; }

    public Serialization(Dictionary<TKey, TValue> target)
    {
        this.target = target;
    }

    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Math.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i)
        {
            target.Add(keys[i], values[i]);
        }
    }
}








