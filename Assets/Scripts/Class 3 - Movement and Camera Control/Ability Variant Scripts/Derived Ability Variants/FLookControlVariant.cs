using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLookControlVariant : MyroControlVariant
{
    public override void RunMovementUpdateVariant(MyroMovement moveScript)
    {
        
        if (moveScript.GetMyroController().IsFLookKeyPressed()) moveScript.GetMyroController().EndControlVariant();

    }

    public override void RunMovementFixedUpdateVariant(MyroMovement moveScript)
    {
        if (!moveScript.GetCharacterController().isGrounded) moveScript.ApplyGravity();
    }

    public override void InitializeControlVariant(MyroController mController)
    {
        Camera.main.GetComponent<CameraControl>().SetFLookSettings();
        mController.GetMyroMovement().SetControlVariant(this);
    }
    public override void EndControlVariant(MyroController mController)
    {
        mController.GetComponent<MyroAnimator>().SetCharging(false);
        Camera.main.GetComponent<CameraControl>().SetStandardSettings();
    }
}
