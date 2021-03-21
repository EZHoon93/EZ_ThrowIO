using System.Collections.Generic;

[System.Serializable]
public class UserData : EzData
{
    public string nickName;  //플레이어 닉네임
    public string pId;  //플레이어 고유번호
    public int level; //레벨
    public int coin; //돈
    public int gem;
    public int exp; //현재 경험치
    public int maxExp; //목표경험치
    public List<UserHasSeverKey> characterKeys = new List<UserHasSeverKey>();  //보유 아바타
    public List<UserHasSeverKey> projectilerKeys = new List<UserHasSeverKey>();

    //최초 생성 값
    public UserData(string _name, string _pID)
    {
        this.nickName = _name;


        this.pId = _pID;
        level = 11;
        coin = 11110;
        gem = 0;
        exp = 0;
        maxExp = Utility.GetMaxExp(level + 1);

        characterKeys.Clear();
        characterKeys.Add(new UserHasSeverKey(DataContainer.Instance.sCharacterContainers[0].sCharacterStatsData.sServerKey, true));
        projectilerKeys.Clear();
        projectilerKeys.Add(new UserHasSeverKey(DataContainer.Instance.sProjectileContainers[0].sProjectileData.sServerKey, true ));
        
        
    }

    public UserData(string _name, string _pId, int _level, int _coin,
                    int _exp, List<UserHasSeverKey> _characterKeys, List<UserHasSeverKey> _projectilerKeys)
    {
        this.nickName = _name;
        this.pId = _pId;
        this.level = _level;
        this.coin = _coin;
        this.exp = _exp;

        characterKeys = _characterKeys;
        projectilerKeys = _projectilerKeys;

    }

    public void AddExp(int addAmount)
    {
        exp += addAmount;
        if(exp >= maxExp)
        {
            level++;
            exp = exp - maxExp;
            maxExp = Utility.GetMaxExp(level);
        }
    }

}