using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum InputState
{
    Idle,
    Attack,
    AutoAttack,
    Skill,
    AutoSkill
}

public class UserPlayerInput : PlayerInput
{
    bool isAuto_Attack;
    bool isAttack;
    bool isSkill;
    bool isAuto_Skill;
    public InputState inputState;
    private void Awake()
    {
        IsUser = true;
        isAuto_Attack = false;
        isAuto_Skill = false;
    }
    void FixedUpdate()
    {
        //// 로컬 플레이어가 아닌 경우 입력을 받지 않음
        if (!photonView.IsMine)
        {
            return;
        }


#if UNITY_ANDROID
        MoveVector = new Vector2(UltimateJoystick.GetHorizontalAxis("Move"), UltimateJoystick.GetVerticalAxis("Move"));
#endif

#if UNITY_EDITOR

        MoveVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        //MoveVector = new Vector2(UltimateJoystick.GetHorizontalAxis("Move"), UltimateJoystick.GetVerticalAxis("Move"));
#endif


        CheckAttack();
        CheckSkill();
        ActiveSkill1 = UltimateJoystick.GetJoystickState("Skill1");
        ActiveSkill2 = UltimateJoystick.GetJoystickState("Skill2");


    }

    void CheckAttack()
    {
        if (UltimateJoystick.GetJoystickState("Attack"))
        {
            AttackVector = new Vector2(UltimateJoystick.GetHorizontalAxis("Attack"), UltimateJoystick.GetVerticalAxis("Attack"));
            isAttack = true;
            if (AttackVector.sqrMagnitude > 0)
            {
                isAuto_Attack = false;
            }
        }
        else
        {
            if (isAttack == false)
            {
                AttackAuto = false;
                isAuto_Attack = true;
                return;
            }

            if (AttackVector.sqrMagnitude != 0)
            {
                LastAttackVector = AttackVector;
            }
            else
            {
                AttackAuto = isAuto_Attack;
            }

            AttackVector = Vector2.zero;
            isAttack = false;
        }
    }

    void CheckSkill()
    {

        if (UltimateJoystick.GetJoystickState("Skill"))
        {
            SkillVector = new Vector2(UltimateJoystick.GetHorizontalAxis("Skill"), UltimateJoystick.GetVerticalAxis("Skill"));
            isSkill = true;
            if (SkillVector.sqrMagnitude > 0) isAuto_Skill = true;
        }
        else
        {
            if (isSkill == false)
            {
                isAuto_Skill = false;
                SkillAuto = false;
                LastSkillVector = Vector2.zero;
            }

            if (SkillVector.sqrMagnitude != 0)
            {
                LastSkillVector = SkillVector;
            }
            else
            {
                isSkill = isAuto_Skill;
            }

            SkillVector = Vector2.zero;
            isSkill = false;
        }
    }

    private void LateUpdate()
    {
        if (!photonView.IsMine) return;

        if (LastAttackVector != Vector2.zero)
        {
            LastAttackVector = Vector2.zero;
        }
    }

    public override void AllInputToZero()
    {
        MoveVector = Vector2.zero;
        AttackVector = Vector2.zero;
        SkillVector = Vector2.zero;
    }
}