using System.Collections;
using UnityEngine;
using Photon.Pun;
public class PlayerMovement : MoveEntity
{

    PlayerInput playerInput;
    PlayerStats playerStats;
    PlayerUI playerUI;

    public float MoveSpeed
    {
        get => moveSpeed * (1 -slowRatio +  playerStats.playerAbilityStats.AbilityStatsDic["SC_e"] * DataEtc.Instance.S_AddMoveSpeedRatio);
    }

    public  void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerUI = GetComponent<PlayerUI>();
        playerStats = GetComponent<PlayerStats>();
    }
    
    public void ResetComponent()
    {
        this.enabled = true;
    }
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (playerStats.noInputStats.Count > 0) return;

        PlayerMove(playerInput.MoveVector);
        PlayerRotate(playerInput.MoveVector);
        PlayerAnimation(playerInput.MoveVector);
        playerUI.UpdateDirectionUI(playerInput.MoveVector);
    }



    protected virtual void PlayerMove(Vector2 moveInput)
    {
        if (playerStats.State == PlayerState.Run)
        {
            Vector3 _moveDistance = this.transform.forward * playerStats.ResultSpeed * 8 * Time.deltaTime; //정면으로 이속의 3배
            this.transform.Translate(_moveDistance, Space.World);
        }
        else
        {
            if (moveInput.sqrMagnitude == 0) return;
            var temp = new Vector3(moveInput.x, 0, moveInput.y);
            Vector3 _moveDistance =temp * playerStats.ResultSpeed * Time.deltaTime;
            this.transform.Translate(_moveDistance, Space.World);
        }


    }
    protected virtual void PlayerRotate(Vector2 moveInput)
    {
        if (playerStats.State == PlayerState.Run)
        {
        }
        else
        {
            if (moveInput.sqrMagnitude == 0) return;
            var temp = new Vector3(moveInput.x, 0, moveInput.y);
            Quaternion newRotation = Quaternion.LookRotation(temp);
            this.transform.rotation = Quaternion.Slerp(this.transform.localRotation, newRotation, rotationSpeed * Time.deltaTime);    //즉시변환
        }
    }

    public void PlayerRotateImmeditaly()
    {
        var temp = new Vector3(playerInput.MoveVector.x, 0, playerInput.MoveVector.y);
        Quaternion newRotation = Quaternion.LookRotation(temp);
        this.transform.rotation = Quaternion.Slerp(this.transform.localRotation, newRotation, 360 * Time.deltaTime);    //즉시변환
    }

    protected virtual void PlayerAnimation(Vector2 moveInput)
    {
        switch (playerStats.State)
        {
            case PlayerState.Throwing:
                return;
            case PlayerState.Run:
                return;
            default:
                if(moveInput.sqrMagnitude == 0)
                {
                    playerStats.State = PlayerState.Idle;
                }
                else
                {
                    playerStats.State = PlayerState.Move;
                }
                return;
        }

    }



    public override void Stop(float time)
    {
     
    }


    

}