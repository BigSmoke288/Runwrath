using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage = 100; // Dano padrão do projétil
    public float AttackDuration = 0.75f;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o projétil colidiu com um inimigo ou objeto que pode receber dano
        if (collision.CompareTag("Enemy"))
        {
            // Tenta obter o componente Health do alvo
            Health enemyHealth = collision.GetComponent<Health>();

            if (enemyHealth != null)
            {
                // Aplica dano ao inimigo
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}