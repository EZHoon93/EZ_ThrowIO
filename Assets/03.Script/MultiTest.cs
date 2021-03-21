using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiTest : MonoBehaviourPun ,  IPunObservable
{
    PlayerMovement playerMovement;
    Vector3 lastPos = Vector3.zero;
    Vector3 direciton = Vector3.zero;
    float gap;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.transform.position);
        }

        else
        {
            var pos = (Vector3)stream.ReceiveNext();
            if(lastPos == Vector3.zero)
            {
                lastPos = pos;
                this.transform.position = lastPos;
            }
            else
            {
                lastPos = pos;
                direciton = lastPos - this.transform.position;
            }

        }
    }



    private void FixedUpdate()
    {
        if (photonView.IsMine) return;
    
    }

    private void Update()
    {
        if (photonView.IsMine) return;

        var result = direciton * playerMovement.MoveSpeed * (Time.deltaTime + gap);
        this.transform.Translate(direciton, Space.World);
    }
}
