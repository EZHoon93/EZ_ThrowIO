

[System.Serializable]
public class UserHasSeverKey 
{

    public bool isSelect; //현재 사용여부
    public string severKey;
    public UserHasSeverKey(string _severKey, bool _isSelect)
    {
        isSelect = _isSelect;
        severKey = _severKey;
    }
}
