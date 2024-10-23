using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] EnemyPrefabs; // Array de prefabs dos obst�culos
    public Transform spawnPoint; // Ponto inicial para spawnar os obst�culos
    public float moveSpeed = 5f; // Velocidade de movimento dos obst�culos
    public float minDistance = 5f; // Dist�ncia m�nima entre os obst�culos
    public float maxDistance = 10f; // Dist�ncia m�xima entre os obst�culos
    public int maxEnemyCount = 3; // N�mero m�ximo de obst�culos na tela

    private List<GameObject> EnemyList = new List<GameObject>(); // Lista para armazenar os obst�culos
    private float leftBoundary; // Limite esquerdo para destruir obst�culos

    void Start()
    {
        // Espera 1 segundo antes de come�ar a spawnar os obst�culos
        Invoke("StartSpawnEnemy", 1f);

        // Calcula o limite esquerdo da tela
        Camera cam = Camera.main;
        float screenHalfWidth = cam.orthographicSize * cam.aspect;
        leftBoundary = cam.transform.position.x - screenHalfWidth - 5f; // Adiciona uma margem
    }

    void StartSpawnEnemy()
    {
        float lastXPosition = spawnPoint.position.x;

        // Spawn inicial de obst�culos
        for (int i = 0; i < maxEnemyCount; i++)
        {
            float randomDistance = Random.Range(minDistance, maxDistance);
            lastXPosition += randomDistance;
            SpawnEnemy(lastXPosition);
        }
    }

    void Update()
    {
        // Verifica se o primeiro obst�culo saiu da tela para spawnar um novo
        if (EnemyList.Count > 0)
        {
            GameObject firstEnemy = EnemyList[0];
            if (firstEnemy.transform.position.x + GetEnemyWidth(firstEnemy) < leftBoundary)
            {
                // Remove e destr�i o obst�culo mais antigo
                Destroy(firstEnemy);
                EnemyList.RemoveAt(0);

                // Spawn um novo obst�culo
                float lastXPosition = EnemyList.Count > 0 ? EnemyList[EnemyList.Count - 1].transform.position.x : spawnPoint.position.x;
                float randomDistance = Random.Range(minDistance, maxDistance);
                SpawnEnemy(lastXPosition + randomDistance);
            }
        }
    }

    private void SpawnEnemy(float xPosition)
    {
        // Escolhe um prefab aleatoriamente
        int randomIndex = Random.Range(0, EnemyPrefabs.Length);
        GameObject EnemyPrefab = EnemyPrefabs[randomIndex];

        // Instancia o novo obst�culo na posi��o de spawn
        Vector3 spawnPosition = new Vector3(xPosition, spawnPoint.position.y - 1.87f, spawnPoint.position.z-5f);
        GameObject Enemy = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);

        // Configura a velocidade de movimento
        EnemyMover mover = Enemy.GetComponent<EnemyMover>();
        if (mover != null)
        {
            mover.EnemySpeed = moveSpeed;
        }

        // Adiciona o novo obst�culo � lista
        EnemyList.Add(Enemy);
    }

    private float GetEnemyWidth(GameObject obstacle)
    {
        SpriteRenderer sr = obstacle.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            return sr.bounds.size.x;
        }
        else
        {
            Debug.LogWarning("O obst�culo n�o possui um SpriteRenderer. Defina um valor padr�o para a largura.");
            return 1f; // Valor padr�o
        }
    }
}
