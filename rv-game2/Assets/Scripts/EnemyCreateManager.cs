using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public float spawnRadius = 10f; 
    public int maxAttempts = 10; 

    public void SpawnEnemy()
    {
        if (FindRandomPoint(transform.position, spawnRadius, out Vector3 spawnPoint))
        {
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Não foi possível encontrar um local válido para spawn.");
        }
    }

    bool FindRandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * range; // Gera ponto no círculo
            Vector3 randomPoint = new Vector3(center.x + randomCircle.x, center.y, center.z + randomCircle.y);

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }
}
