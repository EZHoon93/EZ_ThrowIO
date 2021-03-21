using System.Collections;

using UnityEngine;
using Photon.Pun;
public class BuffState_Posion : BuffState
{
    int lastEnemyViewID;

    public void SetupEnemyViewID(int enemyViewID)
    {
        lastEnemyViewID = enemyViewID;
    }

    IEnumerator Process_Posion(PlayerStats playerStats)
    {
        //5초 1초마다 딜
        float tickTimeInterval = DataEtc.Instance.S_PosionTickTime;
        int dmage = DataEtc.Instance.S_PosionTickDamage;
        while (this.gameObject.activeSelf)
        {
            if (playerStats)
            {
                playerStats.Local_ApplyDamage(1001, dmage, Vector3.zero);        //대미지
                yield return new WaitForSeconds(tickTimeInterval);
            }
        }
    }

   

}
