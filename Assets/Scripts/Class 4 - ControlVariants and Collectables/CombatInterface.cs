using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatAttackType { FireBreath, FireBall, Charge, EnemyAttack }
[RequireComponent(typeof(Rigidbody))]
public class CombatInterface : MonoBehaviour
{
    [SerializeField] CombatAttackType attackType = CombatAttackType.FireBreath;

    public virtual void HitCombatInteractionEntity(CombatInteractionEntity entityHit)
    {
        entityHit.HitByAttack(attackType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CombatInteractionEntity>())
            HitCombatInteractionEntity(other.GetComponent<CombatInteractionEntity>());
    }
}
