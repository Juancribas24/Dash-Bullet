using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    public int number_of_columns;
    public float speed;
    public Sprite texture;
    public Color color;
    public float lifetime;
    public float firerate;
    public float size;
    private float angle;
    public Material material;
    public float spin_speed;
    private float time;

    public ParticleSystem system;
    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    private void Awake()
    {
        Summon();
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(0, 0, time * spin_speed);
    }

    void Summon()
    {
        angle = 360f / number_of_columns;
        for (int i = 0; i < number_of_columns; i++)
        {
            Material particleMaterial = material;

            // Create a green Particle System.
            var go = new GameObject("Particle System");
            go.transform.Rotate(angle * i, 90, 0); // Rotate so the system emits upwards.
            go.transform.parent = this.transform;
            go.transform.position = this.transform.position;
            system = go.AddComponent<ParticleSystem>();
            system.Stop();
            go.GetComponent<ParticleSystemRenderer>().material = particleMaterial;
            var mainModule = system.main;
            mainModule.startSpeed = speed;
            mainModule.maxParticles = 1500;
            mainModule.duration = 0f;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;

            var emission = system.emission;
            emission.enabled = false;

            var shapeModule = system.shape;
            shapeModule.enabled = true;
            shapeModule.shapeType = ParticleSystemShapeType.Sprite;
            shapeModule.sprite = null;

            var textureSheetAnimation = system.textureSheetAnimation;
            textureSheetAnimation.mode = ParticleSystemAnimationMode.Sprites;
            textureSheetAnimation.AddSprite(texture);

            var collision = system.collision;
            collision.enabled = true;
            collision.type = ParticleSystemCollisionType.World;
            collision.mode = ParticleSystemCollisionMode.Collision2D;
            collision.dampen = 0;
            collision.bounce = 0;
            collision.lifetimeLoss = 1f;
            collision.sendCollisionMessages = true;

            particleSystems.Add(system);
        }

        InvokeRepeating("DoEmit", 0f, firerate);
    }

    void DoEmit()
    {
        foreach (Transform child in transform)
        {
            system = child.GetComponent<ParticleSystem>();
            var emitParams = new ParticleSystem.EmitParams
            {
                startColor = color,
                startSize = size,
                startLifetime = lifetime
            };
            system.Emit(emitParams, 10);
        }
    }

    public void HandleParticleCollision(GameObject particle)
    {
        particle.SetActive(false);
        particle.SetActive(true);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var ps in particleSystems)
            {
                var particles = new ParticleSystem.Particle[ps.main.maxParticles];
                int particleCount = ps.GetParticles(particles);

                for (int i = 0; i < particleCount; i++)
                {
                    particles[i].remainingLifetime = 0; // Setea el tiempo restante a 0 para destruir la partícula
                }

                ps.SetParticles(particles, particleCount);
            }
            PlayerHealth.Instance.TakeDamage(1); // Resta 1 punto de vida al jugador
            Debug.Log("Particle hit player, reducing health.");
        }
    }
}
