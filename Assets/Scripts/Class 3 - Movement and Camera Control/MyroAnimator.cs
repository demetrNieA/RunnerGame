using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyroAnimator : BasicAnimator
{

    public void SetWalking(bool val)
    {
        thisAnimator.SetBool("Walking", val);
    }
    public void SetGrounded(bool val)
    {
        thisAnimator.SetBool("Grounded", val);
    }
    public void SetCharging(bool val)
    {
        thisAnimator.SetBool("Charging", val);
    }
    public void JumpTrigger()
    {
        thisAnimator.SetTrigger("Jump");
    }
    public void SetGliding(bool val)
    {
        thisAnimator.SetBool("Gliding", val);
    }
}
