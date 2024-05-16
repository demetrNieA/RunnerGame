using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimator : MonoBehaviour
{
    [SerializeField] public Animator thisAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TriggerDeathAnimation()
    {
        thisAnimator.SetTrigger("Die");
    }
    public virtual void ReviveAnimation()
    {
        thisAnimator.SetTrigger("Revive");
    }
}
