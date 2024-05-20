using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; // Array de prefabs de power-ups
    public Transform[] spawnPoints; // Array de puntos de aparición
    public float spawnInterval = 3f; // Intervalo de tiempo entre apariciones de power-ups

    private List<Transform> availableSpawnPoints;

    void Start()
    {
        availableSpawnPoints = new List<Transform>(spawnPoints);
        StartCoroutine(SpawnPowerUps());
    }

    IEnumerator SpawnPowerUps()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnPowerUp();
        }
    }

    void SpawnPowerUp()
    {
        if (availableSpawnPoints.Count == 0)
        {
            Debug.Log("No available spawn points for power-ups.");
            return;
        }

        // Seleccionar un power-up y un punto de aparición aleatorios
        int powerUpIndex = Random.Range(0, powerUpPrefabs.Length);
        int spawnPointIndex = Random.Range(0, availableSpawnPoints.Count);

        // Guardar la referencia del punto de aparición antes de eliminarlo de la lista
        Transform selectedSpawnPoint = availableSpawnPoints[spawnPointIndex];

        // Instanciar el power-up en el punto de aparición seleccionado
        GameObject powerUp = Instantiate(powerUpPrefabs[powerUpIndex], selectedSpawnPoint.position, Quaternion.identity);

        // Eliminar el punto de aparición de la lista de disponibles
        availableSpawnPoints.RemoveAt(spawnPointIndex);

        // Destruir el power-up después de 5 segundos si no es recogido
        Destroy(powerUp, 5f);

        // Reañadir el punto de aparición a la lista después de que el power-up sea destruido
        StartCoroutine(ReAddSpawnPointAfterDelay(selectedSpawnPoint, 5f));
    }

    IEnumerator ReAddSpawnPointAfterDelay(Transform spawnPoint, float delay)
    {
        yield return new WaitForSeconds(delay);
        availableSpawnPoints.Add(spawnPoint);
    }
}
