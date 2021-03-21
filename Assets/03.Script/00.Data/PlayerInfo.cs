using System.Collections.Generic;
using UnityEngine;

public static class PlayerInfo
{
    public static string nickName;
    public static string usingCharacterId;
    public static string usingProjectileId;
    
    private static readonly string jsonDataName = "userData.json";
    private static readonly string optionDataName = "optionData.json";



    public static UserData userData;
    public static OptionData optionData;
    

    public static void Iniatilze()
    {

    }

    public static bool Login()
    {
        if (UserDataSystem.DoseSaveGameExist(jsonDataName))
        {
            userData = UserDataSystem.LoadData<UserData>(jsonDataName);
            optionData = UserDataSystem.LoadData<OptionData> (optionDataName  );

            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool Save()
    {
        try
        {
            UserDataSystem.SaveData(userData, jsonDataName);
            UserDataSystem.SaveData(optionData, optionDataName);

            return true;
        }
        catch (System.Exception)
        {

            return false;
        }
    }


    ////현재 사용중인 캐릭터 리턴
    //public static UserHasCharacterKey GetUsingHumanAvaterData()
    //{
    //    return userData.characterKeys.Find(s => s.isSelect == true);
    //}

    public static CharacterContainer GetUsingCharacterContainer()
    {
        var userHasCharacterKey = userData.characterKeys.Find(s => s.isSelect == true);

        var resultContainer = DataContainer.Instance.GetCharacterContainerBySeverKey(userHasCharacterKey.severKey);

        return resultContainer;
    }


    public static ProjectileContainer GetUsingProjectileContainer()
    {
        var userHasProjectilerKey = userData.projectilerKeys.Find(s => s.isSelect == true);

        var resultContainer = DataContainer.Instance.GetProjectileContainerByServerKey(userHasProjectilerKey.severKey);

        return resultContainer;
    }

  





}
