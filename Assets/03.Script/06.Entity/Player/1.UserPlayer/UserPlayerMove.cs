using System.Collections;
using UnityEngine;
using Photon.Pun;

public class UserPlayerMove : PlayerMovement
{



    //protected override void Awake()
    //{
    //    base.Awake();
    //}

    //protected override void OnEnable()
    //{
    //    base.OnEnable();
    //}

  

    //protected override void PlayerMove(Vector2 _moveInput, bool run)
    //{
    //    base.PlayerMove(_moveInput, run);
    //    if (_moveInput.sqrMagnitude == 0)
    //    {
    //        playerRigidbody.velocity = Vector3.zero;
    //        var newEnergy = currentEnergy + Time.deltaTime;
    //        currentEnergy = Mathf.Clamp(newEnergy, 0, resultMaxEnergy);


    //        return;
    //    }
    //    //뛰기
    //    else if (run)
    //    {
    //        var quaternion = Quaternion.Euler(0, CameraManager.instance.MainCamera.transform.eulerAngles.y, 0);
    //        var temp = new Vector3(_moveInput.x, 0, _moveInput.y);
    //        var newDirection = quaternion * temp;
    //        Vector3 moveDistance =
    //                    newDirection.normalized * resultRunSpeed * Time.deltaTime;
    //        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);

    //        var newEnergy = currentEnergy - Time.deltaTime;
    //        currentEnergy = Mathf.Clamp(newEnergy, 0, resultMaxEnergy);
    //        if (currentEnergy <= 0)
    //        {
    //            RecoveryEnergy();
    //        }


    //    }

    //    else
    //    {
    //        var quaternion = Quaternion.Euler(0, CameraManager.instance.MainCamera.transform.eulerAngles.y, 0);
    //        var temp = new Vector3(_moveInput.x, 0, _moveInput.y);
    //        var newDirection = quaternion * temp;
    //        Vector3 moveDistance =
    //                    newDirection.normalized * resultWalkSpeed * Time.deltaTime;
    //        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);

    //        var newEnergy = currentEnergy + Time.deltaTime;
    //        currentEnergy = Mathf.Clamp(newEnergy, 0, resultMaxEnergy);

    //    }

    //}

    //protected override void PlayerRotate(Vector2 moveInput, Vector2 attackInput, bool run)
    //{
    //    base.PlayerRotate(moveInput, attackInput, run);
    //    //if (playerShooter.state != PlayerShooter.State.GunMove || playerShooter.state != PlayerShooter.State.GunZoom  ) return;
    //    if (run)
    //    {
    //        if (moveInput.sqrMagnitude == 0) return;
    //        var quaternion = Quaternion.Euler(0, CameraManager.instance.MainCamera.transform.eulerAngles.y, 0);

    //        var temp = new Vector3(moveInput.x, 0, moveInput.y);
    //        var newDirection = quaternion * temp;

    //        Quaternion newRotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0, newDirection.z));

    //        playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation,
    //            newRotation, rotationSpeed * Time.deltaTime);
    //    }

    //    else
    //    {
    //        if (attackInput.sqrMagnitude == 0)
    //        {
    //            if (moveInput.sqrMagnitude == 0) return;
    //            var quaternion = Quaternion.Euler(0, CameraManager.instance.MainCamera.transform.eulerAngles.y, 0);

    //            var temp = new Vector3(moveInput.x, 0, moveInput.y);
    //            var newDirection = quaternion * temp;

    //            Quaternion newRotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0, newDirection.z));

    //            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation,
    //                newRotation, rotationSpeed * Time.deltaTime);
    //        }
    //        else
    //        {
    //            var quaternion = Quaternion.Euler(0, CameraManager.instance.MainCamera.transform.eulerAngles.y, 0);

    //            var temp = new Vector3(attackInput.x, 0, attackInput.y);
    //            var newDirection = quaternion * temp;

    //            Quaternion newRotation = Quaternion.LookRotation(new Vector3(newDirection.x, 0, newDirection.z));

    //            playerRigidbody.rotation = Quaternion.Slerp(playerRigidbody.rotation,
    //                newRotation, rotationSpeed * Time.deltaTime);
    //        }
    //    }

    //}


    //protected override void PlayerAnimation(Vector2 moveInput, Vector2 attackVector, bool run)
    //{
    //    if (run)
    //    {
    //        playerAnimator.SetBool("Run", true);
    //        playerAnimator.SetFloat("X", moveInput.x);
    //        playerAnimator.SetFloat("Y", moveInput.y);
    //    }
    //    else
    //    {
    //        if (attackVector.sqrMagnitude == 0)
    //        {
    //            var quaternion = Quaternion.Euler(0, -this.transform.eulerAngles.y, 0);
    //            var temp = new Vector3(moveInput.x, 0, moveInput.y);
    //            var newDirection = quaternion * temp;
    //            playerAnimator.SetBool("Run", false);
    //            playerAnimator.SetFloat("X", newDirection.x);
    //            playerAnimator.SetFloat("Y", newDirection.z);
    //        }
    //        else
    //        {
    //            var quaternion = Quaternion.Euler(0, -this.transform.eulerAngles.y, 0);
    //            var temp = new Vector3(moveInput.x, 0, moveInput.y);
    //            var newDirection = quaternion * temp;
    //            playerAnimator.SetBool("Run", false);
    //            playerAnimator.SetFloat("X", newDirection.x);
    //            playerAnimator.SetFloat("Y", newDirection.z);
    //        }

    //    }
    //}
}
