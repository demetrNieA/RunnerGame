using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyroControlVariant : MonoBehaviour
{
    
    /// <summary>
    /// This replaces a piece of the MyroMovement functionality and is called every Update()
    /// </summary>
    /// <param name="moveScript"></param>
    public virtual void RunMovementUpdateVariant(MyroMovement moveScript)
    {

    }
    /// <summary>
    /// This replaces a piece of the MyroMovement functionality and is called every FixedUpdate()
    /// </summary>
    /// <param name="moveScript"></param>
    public virtual void RunMovementFixedUpdateVariant(MyroMovement moveScript)
    {

    }

    /// <summary>
    /// Intended to communicate with the various Myro Control scripts to start the functionality
    ///  - IE CameraControl, MyroController, MyroMovement
    ///  - Additionally, external ability-related calls may be made here
    /// </summary>
    /// <param name="mController"></param>
    public virtual void InitializeControlVariant(MyroController mController)
    {

    }
    /// <summary>
    /// Called by MyroController.EndControlVariant.  Be careful to not call MyroController.EndControlVariant
    /// within this function or you will end up with an infinite loop
    /// </summary>
    /// <param name="mController"></param>
    public virtual void EndControlVariant(MyroController mController)
    {

    }
}
