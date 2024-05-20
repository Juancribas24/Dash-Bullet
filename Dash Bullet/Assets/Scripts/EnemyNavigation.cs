using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNavigation : MonoBehaviour
{
    public List<Transform> waypoints; // Lista de waypoints
    public float speed = 2.0f; // Velocidad de movimiento del enemigo
    private Transform currentTarget; // Waypoint actual hacia el que se mueve el enemigo
    private int lastIndex = -1; // �ndice del �ltimo waypoint seleccionado
    public float minWaitTime = 1.0f; // Tiempo m�nimo de espera en segundos
    public float maxWaitTime = 3.0f; // Tiempo m�ximo de espera en segundos

    void Start()
    {
        StartCoroutine(MoveToNextWaypoint()); // Empieza movi�ndote hacia el primer waypoint con espera
    }

    void Update()
    {
        if (currentTarget != null)
        {
            MoveTowardsWaypoint(); // Mueve el enemigo hacia el waypoint actual
        }
    }

    void MoveTowardsWaypoint()
    {
        // Mover al enemigo hacia el waypoint actual
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);

        // Comprobar si el enemigo ha llegado al waypoint
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            StartCoroutine(MoveToNextWaypoint()); // Seleccionar un nuevo waypoint con espera una vez alcanzado el actual
        }
    }

    IEnumerator MoveToNextWaypoint()
    {
        if (waypoints.Count == 0) yield break;

        int newIndex = Random.Range(0, waypoints.Count);

        // Aseg�rate de no seleccionar el mismo waypoint si hay m�s de uno disponible
        if (waypoints.Count > 1)
        {
            while (newIndex == lastIndex)
            {
                newIndex = Random.Range(0, waypoints.Count);
            }
        }

        lastIndex = newIndex; // Guardar el �ndice del nuevo waypoint seleccionado
        currentTarget = waypoints[newIndex]; // Actualizar el waypoint objetivo

        // Esperar un tiempo aleatorio antes de moverse hacia el nuevo waypoint
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);
    }
}
