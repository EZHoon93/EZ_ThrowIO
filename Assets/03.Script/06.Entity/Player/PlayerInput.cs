using UnityEngine;
using Photon.Pun;

public abstract class PlayerInput : MonoBehaviourPun
{
    public Vector2 MoveVector { get; set; } //감지된 움직인 벡터 값
    public Vector2 AttackVector { get; set; } //어택 조이스틱

    public Vector2 SkillVector { get; set; } //어택 조이스틱

    public Vector2 LastAttackVector { get; set; }

    public Vector2 LastSkillVector { get; set; }

    public bool AttackAuto { get; set; }
    public bool SkillAuto { get; set; }

    public bool MyCharacter => photonView.IsMine && IsUser;

    public bool ActiveSkill1 { get; set; }
    public bool ActiveSkill2 { get; set; }


    public float attackMagntiude = 0.99f;    //줌/공격 경계선
    public bool IsUser;

    
    


    


    public abstract void AllInputToZero();

    public void ResetComponent()
    {
        this.enabled = true;
    }

}
