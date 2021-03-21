using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class DeathInfo : PoolableObject
{
    [SerializeField] Image killImage;
    [SerializeField] TextMeshProUGUI killText;
    [SerializeField] TextMeshProUGUI deathText;

    public void SetupInfo(string killPlayer, string deathPlayer)
    {
        killText.text = killPlayer;
        deathText.text = deathPlayer;

        Invoke("Push", 3.0f);
    }

    public override void Push()
    {
        base.Push();
    }
}
