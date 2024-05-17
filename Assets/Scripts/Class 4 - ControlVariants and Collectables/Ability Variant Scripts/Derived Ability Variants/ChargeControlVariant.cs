using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeControlVariant : MyroControlVariant
{
    [SerializeField] GameObject chargeCombatInterface;
    GameObject savedChargeObject;
    Vector3 oldPosition = Vector3.zero;

    public override void RunMovementUpdateVariant(MyroMovement moveScript)
    {
        moveScript.RunNormalMovementUpdate();
        if (!moveScript.GetMyroController().ChargeKeyPressed() || oldPosition == transform.position)
            moveScript.GetMyroController().EndControlVariant();
        oldPosition = transform.position;
    }

    public override void RunMovementFixedUpdateVariant(MyroMovement moveScript)
    {
        moveScript.RunNormalMovementFixedUpdate(2);
        moveScript.GetMyroAnimator().SetCharging(true);
	}

    public override void InitializeControlVariant(MyroController mController)
    {
        
        oldPosition = transform.position + Vector3.one;
        mController.GetMyroMovement().SetControlVariant(this);
        if(chargeCombatInterface != null)
        {
            savedChargeObject = Instantiate(chargeCombatInterface, mController.gameObject.transform);
            savedChargeObject.transform.position = mController.transform.position + mController.transform.forward;
        }
    }

    public override void EndControlVariant(MyroController mController)
    {
        mController.GetComponent<MyroAnimator>().SetCharging(false);
        if (savedChargeObject != null) Destroy(savedChargeObject);
    }
}
