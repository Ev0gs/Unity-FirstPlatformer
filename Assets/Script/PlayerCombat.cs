using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;

    public Transform leftAttackPoint;
    public Transform rightAttackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public int attackDamage = 50;

    private bool Right;

    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        //For the facing of the charater and the side of its attack point
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            Right = true;
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            Right = false;
        }
    }

    void Attack()
    {
        //Play an atttack animation
        animator.SetTrigger("Attack");
        if (Right)
        {
            //Detect ennemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(rightAttackPoint.position, attackRange, enemyLayers);
            //Damage them
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy_Script>().TakeDamage(attackDamage);
            }
        }
        else if (!Right)
        {
            //Detect ennemies in range of attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(leftAttackPoint.position, attackRange, enemyLayers);
            //Damage them
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<Enemy_Script>().TakeDamage(attackDamage);
            }
        }

        //Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(rightAttackPoint.position, attackRange, enemyLayers);

    }

    void OnDrawGizmosSelected()
    {
        if(leftAttackPoint == null && rightAttackPoint)
        {
            return;
        }
        Gizmos.DrawWireSphere(rightAttackPoint.position, attackRange);
        Gizmos.DrawWireSphere(leftAttackPoint.position, attackRange);

    }
}
