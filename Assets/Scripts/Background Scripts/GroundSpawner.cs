using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundPrefab; // Prefab do ch�o
    public Transform spawnPoint; // Ponto inicial para spawnar o ch�o
    public float groundWidth = 10f; // Largura do ch�o, deve ser igual � largura do prefab
    public float moveSpeed = 5f; // Velocidade de movimento do ch�o
    public int maxGroundCount = 3; // N�mero m�ximo de peda�os de ch�o na tela

    private List<GameObject> groundList = new List<GameObject>(); // Lista para armazenar os peda�os de ch�o

    // Start is called before the first frame update
    void Start()
    {
        // Spawn inicial de 3 peda�os de ch�o
        for (int i = 0; i < maxGroundCount; i++)
        {
            SpawnGround(i * groundWidth);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Verifica se o primeiro peda�o de ch�o saiu da tela para spawnar um novo
        if (groundList.Count > 0 && groundList[0].transform.position.x + groundWidth < spawnPoint.position.x )
        {
            // Remove e destr�i o peda�o de ch�o mais antigo
            Destroy(groundList[0]);
            groundList.RemoveAt(0);

            // Spawn um novo peda�o de ch�o
            SpawnGround(groundList[groundList.Count - 1].transform.position.x + groundWidth);
        }
    }
    private void SpawnGround(float xPosition)
    {
        // Instancia o novo peda�o de ch�o
        GameObject ground = Instantiate(groundPrefab, new Vector3(xPosition - 5f, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity);
        ground.GetComponent<GroundMover>().GndMoveSpeed = moveSpeed;

        // Adiciona o novo peda�o de ch�o � lista
        groundList.Add(ground);
    }
}
