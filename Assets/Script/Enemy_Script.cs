using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    [SerializeField]
    float attackRange;

    [SerializeField]
    private int attackDamage = 10;

    private float canAttack;

    [SerializeField]
    Transform player;
    
    [SerializeField]
    float agroRange;
    
    [SerializeField]
    float moveSpeed;

    Rigidbody2D rb2d;

    public Animator animator;

    public int maxHealth = 100;
    int currentHealth;

    public PlayerHealth playerHealth;

    public GameObject slimeToDestroy;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        //For the detection and agro
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if(playerHealth.currentHealth <= 0)
        {
            StopChasingPlayer();
        }
        else
        {
            if (distToPlayer < agroRange)
            {
                //Agro the enemy
                ChasePlayer();
            }
            else
            {
                //Stop chasing player
                StopChasingPlayer();
            }
        }
    }

    void ChasePlayer()
    {
        if(transform.position.x < player.position.x)
        {
            //enemy is to the left side of the player, so move right
            rb2d.velocity = new Vector2(moveSpeed, 0);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            //enemy is to the right side of the player, so move left
            rb2d.velocity = new Vector2(-moveSpeed, 0);
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void StopChasingPlayer()
    {
        rb2d.velocity = new Vector2(0, 0);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            animator.SetTrigger("Attack");
            playerHealth.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            StopChasingPlayer();
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        Debug.Log("Enemy died");

        animator.SetTrigger("IsDead");
        yield return new WaitForSeconds(1f);

        Destroy(slimeToDestroy);
    }
}
