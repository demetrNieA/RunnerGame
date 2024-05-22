using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogModeControlVariant : MyroControlVariant
{
    [SerializeField] GameObject cameraPosition;
    [SerializeField] GameObject cameraLookPosition;
    public override void RunMovementUpdateVariant(MyroMovement moveScript)
    {
        // Disable movement controls during this CV
    }
    public override void RunMovementFixedUpdateVariant(MyroMovement moveScript)
    {
        // Don't move during this CV
    }
    public override void InitializeControlVariant(MyroController mController)
    {
        // Set movement functions to do nothing
        mController.GetMyroMovement().SetControlVariant(this);
        // Set the Camera to focus on the designated look position, from the designated camera position
        CameraControl.instance.SetDialogLookSettings(cameraPosition.transform.position, cameraLookPosition.transform.position);

        mController.GetMyroAnimator().SetWalking(false);
    }
    public override void EndControlVariant(MyroController mController)
    {
        // Return controls to normal
        mController.GetMyroMovement().EndControlVariant();
        CameraControl.instance.SetStandardSettings();
    }
}
