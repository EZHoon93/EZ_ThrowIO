using System.Collections;

using Photon.Pun;

using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject ItemSmallExpPrefab;
    public GameObject ItemBigExpPrefab;

    public Transform[] small_spawnPoints;
    public Transform[] big_spawnPoints;



    private void OnEnable()
    {
        GameStart();
    }

    public void GameStart()
    {
        for(int i =0; i < small_spawnPoints.Length; i++)
        {
            PhotonNetwork.InstantiateRoomObject(ItemSmallExpPrefab.name, small_spawnPoints[i].position, small_spawnPoints[i].rotation);
        }

    }
}
