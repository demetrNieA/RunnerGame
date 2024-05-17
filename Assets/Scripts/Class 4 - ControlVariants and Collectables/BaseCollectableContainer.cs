using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollectableContainer : CombatInteractionEntity
{
    [SerializeField] GameObject collectableToDrop;

    public override void HitByCharge()
    {
        base.HitByCharge();
        TakeDamage(1);
    }
    public override void HitByFireBall()
    {
        base.HitByFireBall();
        TakeDamage(1);
    }
    public override void HitByFireBreath()
    {
        base.HitByFireBreath();
        TakeDamage(1);
    }
    public override void Die()
    {
        base.Die();
        if (collectableToDrop != null) Instantiate(collectableToDrop, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
