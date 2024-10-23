using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMover : MonoBehaviour
{
    public float GndMoveSpeed = 5f; // Velocidade de movimento do chão

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Movimenta o chão para a esquerda
        transform.Translate(Vector3.left * GndMoveSpeed * Time.deltaTime);
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += StopMovement;
    }
    private void StopMovement()
    {
        GndMoveSpeed = 0f;
    }
}
