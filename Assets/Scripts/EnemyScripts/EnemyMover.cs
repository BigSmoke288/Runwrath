using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    private GameManager gameManager;
    public float EnemySpeed = 5f; // Velocidade do movimento dos obst�culos

    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        // Move o obst�culo para a esquerda
        transform.Translate(Vector2.left * EnemySpeed * Time.deltaTime);
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += StopMovement;
    }
    private void StopMovement()
    {
        EnemySpeed = 0f;
    }

}
