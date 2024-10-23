using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 15f; // For�a do pulo
    public float attackDuration = 0.75f; // Dura��o da hitbox ativa
    public GameObject attackHitboxPrefab; // Prefab da hitbox de ataque
    public GameObject gameManager;
    public Transform attackPoint; // Ponto de origem da hitbox
    public int MaxAttackHitBox = 1;

    public delegate void PlayerDeath();
    public static event PlayerDeath OnPlayerDeath;
    private enum State { Run, Jump, Attack, Death };
    private State state = State.Run;

    private Rigidbody2D rb;
    private Animator animator;
    public bool isAttacking = false;
    public bool isGrounded;
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Obt�m a refer�ncia do Animator
    }

    // Update is called once por frame
    void Update()
    {
        StateChange();
        // Verifica se o jogador pressiona a barra de espa�o e se o personagem est� no ch�o
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        // Verifica se o jogador pressiona o bot�o esquerdo do mouse para atacar
        if (MaxAttackHitBox == 1 && Input.GetMouseButtonDown(0))
        {
            Attack();
            MaxAttackHitBox--;
        }
    }

    private void StateChange()
    {
        if (isGrounded)
        {
            state = State.Run;
        }
        if (isGrounded == false)
        {
            state = State.Jump;
        }
        if (isAttacking == true)
        {
            state = State.Attack;
        }
        if (isDead == true)
        {
            state = State.Death;
        }

        animator.SetInteger("state", (int)state);
    }

    public void Attack()
    {
        if (MaxAttackHitBox > 0)
        {
            // Ativa a anima��o de ataque
            isAttacking = true;
            StateChange();

            // Cria a hitbox de ataque
            if (isAttacking && MaxAttackHitBox >= 1)
            {
                GameObject hitbox = Instantiate(attackHitboxPrefab, attackPoint.position, Quaternion.identity);
                hitbox.transform.parent = transform; // Para manter a hierarquia limpa

                // Destr�i a hitbox ap�s um determinado tempo
                Destroy(hitbox, attackDuration);
                StartCoroutine(SpawnHitboxCoroutine());
            }
        }
        isAttacking = false;
    }

    public void AttackButton()
    {
        if (isAttacking)
        {
            Attack();
            MaxAttackHitBox--;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false; // O personagem n�o est� mais no ch�o
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o personagem colidiu com o ch�o
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // O personagem est� no ch�o
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isDead = true;
            OnPlayerDeath?.Invoke(); // Chama o evento de morte do jogador
        }
    }

    private IEnumerator SpawnHitboxCoroutine()
    {
        GameObject hitbox = Instantiate(attackHitboxPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(attackDuration);
        Destroy(hitbox); // Destr�i a hitbox ap�s o tempo especificado
        MaxAttackHitBox++;
    }
}
