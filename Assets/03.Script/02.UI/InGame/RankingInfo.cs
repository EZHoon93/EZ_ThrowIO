using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingInfo : MonoBehaviour
{
    [SerializeField] Transform panel;
    public List<PlayerScore> rankingUIs = new List<PlayerScore>();

    public void Sort()
    {
        rankingUIs.Sort(delegate (PlayerScore A, PlayerScore B)
        {
            if (A.Score < B.Score)
            {
                var bIndex = B.transform.childCount;
                B.transform.SetSiblingIndex(A.transform.childCount);
                A.transform.SetSiblingIndex(bIndex);
                return 1;
            }
            else if (A.Score > B.Score) return -1;
            return 0;
        });

        //rankingUIs.Sort((x1,x2) => x2.score.CompareTo(x)  )
        
    }

    public void SetupRakingUI(PlayerScore playerScore)
    {
        rankingUIs.Add(playerScore);
        playerScore.transform.SetParent(panel);
    }
}
