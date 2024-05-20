using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public GameObject shieldVisual; // Referencia al objeto visual del escudo
    private bool isShieldActive = false;
    private float shieldEndTime;
    public BulletPool bulletPool; // Referencia al BulletPool

    void Update()
    {
        if (isShieldActive && Time.time > shieldEndTime)
        {
            DeactivateShield();
        }
    }

    public void ActivateShield(float duration)
    {
        isShieldActive = true;
        shieldEndTime = Time.time + duration;
        shieldVisual.SetActive(true); // Activa el objeto visual del escudo
    }

    void DeactivateShield()
    {
        isShieldActive = false;
        shieldVisual.SetActive(false); // Desactiva el objeto visual del escudo
    }

    public bool IsShieldActive()
    {
        return isShieldActive;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isShieldActive && other.CompareTag("Bullet"))
        {
            bulletPool.ReturnBullet(other.gameObject); // Devuelve la bala al pool
           
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (isShieldActive)
        {
            ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
                int numParticlesAlive = particleSystem.GetParticles(particles);

                for (int i = 0; i < numParticlesAlive; i++)
                {
                    // Desactivar las partículas que colisionan con el escudo
                    particles[i].remainingLifetime = 0;
                }

                // Actualizar las partículas en el sistema de partículas
                particleSystem.SetParticles(particles, numParticlesAlive);
                
            }
        }
    }
}
