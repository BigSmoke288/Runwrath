using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundPrefab; // Prefab do chão
    public Transform spawnPoint; // Ponto inicial para spawnar o chão
    public float groundWidth = 10f; // Largura do chão, deve ser igual à largura do prefab
    public float moveSpeed = 5f; // Velocidade de movimento do chão
    public int maxGroundCount = 3; // Número máximo de pedaços de chão na tela

    private List<GameObject> groundList = new List<GameObject>(); // Lista para armazenar os pedaços de chão

    // Start is called before the first frame update
    void Start()
    {
        // Spawn inicial de 3 pedaços de chão
        for (int i = 0; i < maxGroundCount; i++)
        {
            SpawnGround(i * groundWidth);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Verifica se o primeiro pedaço de chão saiu da tela para spawnar um novo
        if (groundList.Count > 0 && groundList[0].transform.position.x + groundWidth < spawnPoint.position.x )
        {
            // Remove e destrói o pedaço de chão mais antigo
            Destroy(groundList[0]);
            groundList.RemoveAt(0);

            // Spawn um novo pedaço de chão
            SpawnGround(groundList[groundList.Count - 1].transform.position.x + groundWidth);
        }
    }
    private void SpawnGround(float xPosition)
    {
        // Instancia o novo pedaço de chão
        GameObject ground = Instantiate(groundPrefab, new Vector3(xPosition - 5f, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity);
        ground.GetComponent<GroundMover>().GndMoveSpeed = moveSpeed;

        // Adiciona o novo pedaço de chão à lista
        groundList.Add(ground);
    }
}
