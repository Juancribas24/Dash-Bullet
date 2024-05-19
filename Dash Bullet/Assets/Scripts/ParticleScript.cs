using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        PlayerHealth.Instance.TakeDamage(1); // Resta 1 punto de vida al jugador
        Debug.Log("Particle hit player, reducing health.");
    }
}
