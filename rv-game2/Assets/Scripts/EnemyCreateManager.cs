using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    public void SpawnEnemy()
    {
        // Tenta achar uma posição navegável aleatória
        Vector3 randomPosition = GetRandomNavMeshPosition();
        if (randomPosition != Vector3.zero)
        {
            // Instancia o inimigo na posição encontrada
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No valid NavMesh position found!");
        }
    }

    Vector3 GetRandomNavMeshPosition()
    {
        // Define um raio maior para a amostragem do NavMesh, como 20f
        Vector3 randomDirection = Random.insideUnitSphere * 20f;
        randomDirection += transform.position;  // Baseia-se na posição do gerenciador
        NavMeshHit hit;

        // Ajuste: aumenta a distância de busca para um raio maior
        if (NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas))  // Ajuste o raio (10f) para maior área de busca
        {
            return hit.position;  // Retorna a posição encontrada no NavMesh
        }

        // Se não encontrar, retorna um vetor nulo
        return Vector3.zero;
    }
}
