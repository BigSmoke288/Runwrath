using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100; // Sa�de m�xima do objeto
    private int currentHealth;
    private enum State { Run, Death, idle };
    private State state = State.Run;
    private bool isDead;
    private bool gameOver;

    public Animator animator; // Refer�ncia para o Animator
    private GameManager GameManager; // Refer�ncia para o Script GameManager
    public SpriteRenderer spriteRenderer; // Refer�ncia para o SpriteRenderer
    public Collider2D collider2DEnemy; // Refer�ncia para o Collider2D

    void Start()
    {
        isDead = false;
        gameOver = false;
        animator = GetComponent<Animator>();
        GameManager = FindObjectOfType<GameManager>();
        currentHealth = maxHealth; // Inicializa a sa�de atual
    }
    private void Update()
    {
        StateChange();
    }
    private void StateChange()
    {
        if (isDead == true)
        {
            state = State.Death;
        }
        if (gameOver == true)
        {
            state = State.idle;
        }

        animator.SetInteger("state", (int)state);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameOver = true;
        }
    }

    private void Die()
    {
        if (GameManager != null)
        {
            GameManager.score += 2;
        }

        DisableCollision();
        StartCoroutine(WaitForAnimationAndDisable());
    }

    private void DisableCollision()
    {
        // Desativa apenas o SpriteRenderer e o Collider2D
        if (collider2DEnemy != null)
        {
            collider2DEnemy.enabled = false;
        }
        StateChange();
    }
    private void DisableVisual()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
    private IEnumerator WaitForAnimationAndDisable()
    {
        // Espera at� que a anima��o de morte termine
        yield return new WaitForSeconds(10);

        DisableVisual();
    }
}
