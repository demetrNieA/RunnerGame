using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInteractionEntity : MonoBehaviour
{
    [SerializeField] int hitsToDie = 1;
    bool alive = true;

    public virtual void HitByAttack(CombatAttackType attackType)
    {
        switch (attackType)
        {
            case CombatAttackType.Charge:
                HitByCharge();
                break;
            case CombatAttackType.FireBall:
                HitByFireBall();
                break;
            case CombatAttackType.FireBreath:
                HitByFireBreath();
                break;
            case CombatAttackType.EnemyAttack:
                HitByEnemyAttack();
                break;
        }
    }

    public virtual void HitByCharge()
    {
        
    }
    public virtual void HitByFireBall()
    {

    }
    public virtual void HitByFireBreath()
    {

    }
    public virtual void HitByEnemyAttack()
    {

    }

    public virtual void TakeDamage(int damage)
    {
        if (!alive) return;
        hitsToDie -= damage;
        if (hitsToDie <= 0) Die();
    }

    public virtual void Die()
    {
        alive = false;
    }
}
