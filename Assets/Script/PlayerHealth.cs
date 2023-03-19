using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;

    [SerializeField] private float invicibilityFlashDelay = 0.2f;
    [SerializeField] private float invicibilityTimeAfterHit = 3f;

    [SerializeField] private SpriteRenderer graphics;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Animator animator;
    [SerializeField] private Enemy_Script Enemy;
    [SerializeField] private GameOverManager gameOverManager;
    [SerializeField] private CameraFollow cameraFollow;

    public bool IsInvicible = false;


    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (!IsInvicible)
        {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                Die();
                return;
            }

            IsInvicible = true;
            StartCoroutine(InvincibilityFlash());
            StartCoroutine(HandleInvicibilityDelay());
        }

    }

    public IEnumerator InvincibilityFlash()
    {
        animator.SetTrigger("Damaged");
        yield return new WaitForSeconds(0.7f);
        while (IsInvicible)
        {
            graphics.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invicibilityFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invicibilityFlashDelay);
        }
    }

    public IEnumerator HandleInvicibilityDelay()
    {
        yield return new WaitForSeconds(invicibilityTimeAfterHit);
        IsInvicible = false;
    }

    public void Die()
    {
        PlayerMovement.Instance.enabled = false;
        cameraFollow.enabled = false;
        Debug.Log("Player Dead");
        animator.SetTrigger("Dead");
        gameOverManager.OnPlayerDeath();
    }
}

