using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxSpawner : MonoBehaviour
{
    public GameObject[] backPrefabs; // Array de prefabs do fundo
    public Transform spawnPoint; // Ponto inicial para spawnar o fundo
    public float[] backWidths; // Largura dos fundos, deve corresponder ao n�mero de prefabs
    public float[] moveSpeeds; // Velocidades de movimento dos fundos, deve corresponder ao n�mero de prefabs
    public int maxBackCount = 3; // N�mero m�ximo de peda�os de fundo na tela

    private List<GameObject>[] backLists; // Listas para armazenar os peda�os de fundo de cada prefab

    // Start is called before the first frame update
    void Start()
    {
        // Inicializa as listas de fundo
        backLists = new List<GameObject>[backPrefabs.Length];
        for (int i = 0; i < backPrefabs.Length; i++)
        {
            backLists[i] = new List<GameObject>();
        }

        // Spawn inicial de peda�os de fundo para cada camada
        for (int i = 0; i < backPrefabs.Length; i++)
        {
            for (int j = 0; j < maxBackCount; j++)
            {
                SpawnBack(i, j * backWidths[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move e verifica a posi��o de cada fundo
        for (int i = 0; i < backPrefabs.Length; i++)
        {
            // Move todos os fundos para a esquerda
            foreach (var back in backLists[i])
            {
                back.transform.Translate(Vector3.left * moveSpeeds[i] * Time.deltaTime);
            }

            // Verifica se o primeiro peda�o de fundo saiu da tela para spawnar um novo
            if (backLists[i].Count > 0 && backLists[i][0].transform.position.x + backWidths[i] < spawnPoint.position.x - 10)
            {
                // Remove e destr�i o peda�o de fundo mais antigo
                Destroy(backLists[i][0]);
                backLists[i].RemoveAt(0);

                // Spawn um novo peda�o de fundo
                SpawnBack(i, backLists[i][backLists[i].Count - 1].transform.position.x + backWidths[i]);
            }
        }
    }

    private void SpawnBack(int index, float xPosition)
    {
        // Instancia o novo peda�o de fundo
        GameObject back = Instantiate(backPrefabs[index], new Vector3(xPosition, spawnPoint.position.y, backPrefabs[index].transform.position.z), Quaternion.identity);

        // Adiciona o novo peda�o de fundo � lista correspondente
        backLists[index].Add(back);
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += StopMovement;
    }
    private void StopMovement()
    {
        for (int i = 0; i < moveSpeeds.Length; i++)
        {
            moveSpeeds[i] = 0f;
        }
    }
}