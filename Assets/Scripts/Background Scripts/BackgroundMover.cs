using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float BkMoveSpeed = 5f; // Velocidade de movimento do background

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move o background para a esquerda
        transform.Translate(Vector3.left * BkMoveSpeed * Time.deltaTime);
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += StopMovement;
    }
    private void StopMovement()
    {
        BkMoveSpeed = 0f;
    }
}
