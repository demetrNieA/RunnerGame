using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyroController : MonoBehaviour
{
    [SerializeField] MyroControlVariant chargeVariant;
    [SerializeField] FLookControlVariant fLookControlVariant;
    [SerializeField] MyroControlVariant fireBreathControlVariant;
    MyroControlVariant breathAlternateControlVariant;
    MyroControlVariant chargeAlternateControlVariant;
    MyroControlVariant storedControlVariant = null;
    bool variantFlipped = false;

    MyroAnimator myroAnimator;
    MyroMovement myroMovement;

    public static MyroController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        // Link this player up with the main camera
        if (Camera.main.GetComponent<CameraControl>() == null)
            Camera.main.gameObject.AddComponent<CameraControl>();
        Camera.main.GetComponent<CameraControl>().SetFollowTarget(gameObject);

        myroAnimator = GetComponent<MyroAnimator>();
        myroMovement = GetComponent<MyroMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        RunCharge();

        RunFLook();

        RunBreath();

        variantFlipped = false;
    }

    #region Button Controls
    private void RunFLook()
    {
        if (IsFLookKeyPressed() && fLookControlVariant != null && storedControlVariant == null)
            AssignControlVariant(fLookControlVariant);
    }
    void RunCharge()
    {
        if (ChargeKeyPressed() && storedControlVariant == null
            && (chargeVariant != null) || chargeAlternateControlVariant != null)
        {
            if(chargeAlternateControlVariant != null) AssignControlVariant(chargeAlternateControlVariant);
            else AssignControlVariant(chargeVariant);
        }
    }

    void RunBreath()
    {
        if (IsBreathAttackKeyPressed() && storedControlVariant == null
            && (fireBreathControlVariant != null || breathAlternateControlVariant != null))
        {
            if(breathAlternateControlVariant != null) AssignControlVariant(breathAlternateControlVariant);
            else AssignControlVariant(fireBreathControlVariant);
        }
    }
    #endregion
    #region Alt Control Set functions
    // This allows external control variants to assign themselves to the MyroController components
    public void AssignControlVariant(MyroControlVariant variant)
    {
        if (variantFlipped) return;
        EndControlVariant();
        variant.InitializeControlVariant(this);
        storedControlVariant = variant;
    }

    // This allows different control variants to chain into one another
    public void SwapControlVariant(MyroControlVariant newControlVariant)
    {
        if (storedControlVariant != null) storedControlVariant.EndControlVariant(this);
        EndControlVariant();
        newControlVariant.InitializeControlVariant(this);
        storedControlVariant = newControlVariant;
    }
    // This ends the control variant's influence over the Myro Controller components
    public void EndControlVariant()
    {
        if (storedControlVariant != null) storedControlVariant.EndControlVariant(this);
        myroMovement.EndControlVariant();
        breathAlternateControlVariant = null;
        storedControlVariant = null;
        variantFlipped = true;
    }

    public void SetBreathAlternateControlVariant(MyroControlVariant newBreath)
    {
        breathAlternateControlVariant = newBreath;
    }
    public void SetChargeAlternateControlVariant(MyroControlVariant newCharge)
    {
        chargeAlternateControlVariant = newCharge;
    }
    #endregion

    #region SetControls
    public void SetToNormalControls()
    {
        Camera.main.GetComponent<CameraControl>().SetStandardSettings();
    }
    
    
    #endregion

    #region Key Presses
    public bool JumpKeyIsPressedDown()
    {
        bool jumpPressedDown = false;

        if (Input.GetKeyDown(KeyCode.Space)) jumpPressedDown = true;

        return jumpPressedDown;
    }

    public bool ForwardMovementKeyPressed()
    {
        bool forwardMovementKeyPressed = false;

        if (Input.GetKey(KeyCode.W)) forwardMovementKeyPressed = true;

        return forwardMovementKeyPressed;
    }

    public bool BackwardMovementKeyPressed()
    {
        bool backwardMovementKeyPressed = false;

        if (Input.GetKey(KeyCode.S)) backwardMovementKeyPressed = true;

        return backwardMovementKeyPressed;
    }

    public bool TurnRightMovementKeyPressed()
    {
        bool turnRightMovementKeyPressed = false;

        if (Input.GetKey(KeyCode.D)) turnRightMovementKeyPressed = true;

        return turnRightMovementKeyPressed;
    }

    public bool TurnLeftMovementKeyPressed()
    {
        bool turnLeftMovementKeyPressed = false;

        if (Input.GetKey(KeyCode.A)) turnLeftMovementKeyPressed = true;

        return turnLeftMovementKeyPressed;
    }

    public bool StrafeRightMovementKeyPressed()
    {
        bool strafeRightMovementKeyPressed = false;

        if (Input.GetKey(KeyCode.E)) strafeRightMovementKeyPressed = true;

        return strafeRightMovementKeyPressed;
    }

    public bool StrafeLeftMovementKeyPressed()
    {
        bool strafeLeftMovementKeyPressed = false;

        if (Input.GetKey(KeyCode.Q)) strafeLeftMovementKeyPressed = true;

        return strafeLeftMovementKeyPressed;
    }

    public bool ChargeKeyPressed()
    {
        bool chargePressed = false;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1)) chargePressed = true;

        return chargePressed;
    }

    public bool IsFLookKeyPressed()
    {
        bool fLookKeyPressed = false;

        if (Input.GetKeyDown(KeyCode.F)) fLookKeyPressed = true;

        return fLookKeyPressed;
    }

    public bool IsBreathAttackKeyPressed()
    {
        bool breathAttackKeyPressed = false;

        if (Input.GetMouseButtonDown(0)) breathAttackKeyPressed = true;

        return breathAttackKeyPressed;
    }
    #endregion

    #region Utility
    public MyroAnimator GetMyroAnimator()
    {
        return myroAnimator;
    }
    public MyroMovement GetMyroMovement()
    {
        return myroMovement;
    }

    #endregion

    #region Death and Revive functions
    public void TriggerDeathSequence()
    {
        myroAnimator.TriggerDeathAnimation();
        myroMovement.SetDead();
    }
    public void TriggerRevive()
    {
        myroAnimator.ReviveAnimation();
        myroMovement.Revive();
    }
    #endregion
}
