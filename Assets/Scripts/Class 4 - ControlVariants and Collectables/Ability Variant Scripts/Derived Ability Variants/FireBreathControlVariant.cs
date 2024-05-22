using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathControlVariant : MyroControlVariant
{
    [SerializeField] GameObject fireBreathCombatInterfacePrefab;
    float cooldownTimer = 1.5f;
    [SerializeField] float cooldownTime = 1.5f;
    bool coolingDown = true;

    private void Update()
    {
        if (coolingDown)
        {
            cooldownTimer -= Time.deltaTime;
            if(cooldownTimer <= 0)
            {
                MyroController.instance.EndControlVariant();
                coolingDown = false;
            }
        }
    }

    public override void InitializeControlVariant(MyroController mController)
    {
        if (coolingDown) return;

        if(fireBreathCombatInterfacePrefab != null)
        {
            GameObject newBreath = Instantiate(fireBreathCombatInterfacePrefab, mController.transform);
            newBreath.transform.position = mController.transform.position + (mController.transform.forward * 2);
            cooldownTimer = cooldownTime;
            coolingDown = true;
            mController.GetMyroAnimator().BreathTrigger();
        }
    }

}
