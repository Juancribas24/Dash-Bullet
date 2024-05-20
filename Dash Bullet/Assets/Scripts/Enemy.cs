using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public BulletPool bulletPool; // Referencia al pool de balas
    public float bulletSpeed = 3.0f; // Velocidad de la bala
    public Transform player;

    public ParticleSystem bulletParticleSystem;

    // Lista de patrones de ataque
    public List<IEnumerator> attackPatterns;
    private Coroutine currentPatternCoroutine;

    // Tiempo que cada patrón debe durar
    public float patternDuration = 10.0f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        attackPatterns = new List<IEnumerator>
        {
            SurroundPattern(),
            SpiralPattern(),
            FlowerPattern(),
            VariableAnglePattern(),
            WallBouncePattern(),
            StarPattern(),
            TargetedPattern(player),   
            WavePattern(),
            DoubleSpiralPattern(),
            SpinningLaserPattern()
        };

        // Empezar a alternar patrones de ataque
        StartCoroutine(SwitchPatterns());
    }

    public IEnumerator SwitchPatterns()
    {
        while (true)
        {
            // Detener el patrón actual si hay uno corriendo
            if (currentPatternCoroutine != null)
            {
                StopCoroutine(currentPatternCoroutine);
            }

            // Seleccionar un patrón de ataque aleatorio
            int patternIndex = UnityEngine.Random.Range(0, attackPatterns.Count);
            currentPatternCoroutine = StartCoroutine(attackPatterns[patternIndex]);

            // Esperar un tiempo antes de cambiar al siguiente patrón
            yield return new WaitForSeconds(patternDuration); // Usar el tiempo configurado para cada patrón
        }
    }

    public IEnumerator SurroundPattern() //Check
    {
        float radius = 1.0f;
        while (true)
        {
            for (int i = 0; i < 20; i++)
            {
                float angle = (360f / 10) * i;
                Vector2 direction = new Vector2(
                    radius * Mathf.Sin(angle * Mathf.Deg2Rad),
                    radius * Mathf.Cos(angle * Mathf.Deg2Rad)
                );

                GameObject bullet = bulletPool.GetBullet("BulletType1");
                if (bullet != null)
                {
                    bullet.transform.position = transform.position;
                    bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(audioSource.clip); // Reproducir el sonido de disparo
                    }
                }
            }
            yield return new WaitForSeconds(1.0f);
            
        }
    }


    public IEnumerator SpiralPattern()
    {
        float angle = 0f;
        float angleIncrement = 10f; // Ajusta el incremento del ángulo según sea necesario
        while (true)
        {
            for (int i = 0; i < 15; i++) // 36 balas para una espiral completa
            {
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                GameObject bullet = bulletPool.GetBullet("BulletType2");
                if (bullet != null)
                {
                    bullet.transform.position = transform.position;
                    bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                    if (audioSource != null)
                    {
                        audioSource.PlayOneShot(audioSource.clip); // Reproducir el sonido de disparo
                    }
                }
                angle += angleIncrement;
            }
            yield return new WaitForSeconds(0.5f); // Intervalo entre disparos
            

        }
    }

    public IEnumerator FlowerPattern()
    {
        int totalPetals = 6;
        int bulletsPerPetal = 12;
        float radius = 1.0f;
        while (true)
        {
            for (int p = 0; p < totalPetals; p++)
            {
                float petalAngle = (360f / totalPetals) * p;
                for (int i = 0; i < bulletsPerPetal; i++)
                {
                    float angle = petalAngle + (360f / bulletsPerPetal) * i;
                    Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    GameObject bullet = bulletPool.GetBullet("BulletType3");
                    if (bullet != null)
                    {
                        bullet.transform.position = transform.position + (Vector3)(direction * radius);
                        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                        if (audioSource != null)
                        {
                            audioSource.PlayOneShot(audioSource.clip); // Reproducir el sonido de disparo
                        }
                    }
                }
            }
            yield return new WaitForSeconds(1.0f); // Intervalo entre disparos
          
        }
    }
    public IEnumerator VariableAnglePattern()
    {
        float angle = 0f;
        float initialAngle = -180f;
        float maxAngle = 360f;
        bool moveChange = false;

        while (true)
        {
            float bulletAngle = initialAngle + angle;
            Vector2 direction = new Vector2(Mathf.Sin(bulletAngle * Mathf.Deg2Rad), Mathf.Cos(bulletAngle * Mathf.Deg2Rad));

            GameObject bullet = bulletPool.GetBullet("BulletType4");
            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                if (audioSource != null)
                {
                    audioSource.PlayOneShot(audioSource.clip); // Reproducir el sonido de disparo
                }
            }

            if (angle >= maxAngle || angle <= -maxAngle)
            {
                moveChange = !moveChange;
            }

            if (moveChange)
            {
                angle += 5f;
            }
            else
            {
                angle -= 5f;
            }

            yield return new WaitForSeconds(0.3f);
           
        }
    }
    public IEnumerator WallBouncePattern()
    {
        while (true)
        {
            GameObject bullet = bulletPool.GetBullet("BulletType5");
            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = -transform.up * bulletSpeed;

                if (audioSource != null)
                {
                    audioSource.PlayOneShot(audioSource.clip); // Reproducir el sonido de disparo
                }
            }
            yield return new WaitForSeconds(0.05f);
            
        }
    }

    public IEnumerator StarPattern()
    {
        int points = 10;
        int bulletsPerPoint = 2;
        float angleOffset = 45f;
        while (true)
        {
            for (int p = 0; p < points; p++)
            {
                float pointAngle = (360f / points) * p + angleOffset;
                for (int i = 0; i < bulletsPerPoint; i++)
                {
                    float angle = pointAngle + (360f / bulletsPerPoint) * i;
                    Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    GameObject bullet = bulletPool.GetBullet("BulletType6");
                    if (bullet != null)
                    {
                        bullet.transform.position = transform.position;
                        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                        if (audioSource != null)
                        {
                            audioSource.PlayOneShot(audioSource.clip); // Reproducir el sonido de disparo
                        }
                    }
                }
            }
            angleOffset += 5f; // Ajusta el ángulo de desplazamiento para cada nuevo disparo
            yield return new WaitForSeconds(1.0f); // Intervalo entre disparos
           
        }
    }

    public IEnumerator TargetedPattern(Transform player)
    {
        int totalBullets = 20;
        float initialSpeed = 2.0f;
        float acceleration = 1f; // Aceleración lineal
        float duration = 7.0f; // Duración máxima de las balas antes de regresar al pool

        while (true)
        {
            Vector2 targetPosition = player.position;

            for (int i = 0; i < totalBullets; i++)
            {
                Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
                GameObject bullet = bulletPool.GetBullet("BulletType7");
                if (bullet != null)
                {
                    bullet.transform.position = transform.position;
                    StartCoroutine(AccelerateBullet(bullet, direction, initialSpeed, acceleration, duration));

                }
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(audioSource.clip); // Reproducir el sonido de disparo
                }
                yield return new WaitForSeconds(0.1f); // Pequeño intervalo entre disparos
            }
            yield return new WaitForSeconds(0.5f); // Intervalo entre series de disparos
            
        }
    }

    private IEnumerator AccelerateBullet(GameObject bullet, Vector2 direction, float initialSpeed, float acceleration, float duration)
    {
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        float elapsedTime = 0f;
        float currentSpeed = initialSpeed;

        while (elapsedTime < duration)
        {
            currentSpeed += acceleration * Time.deltaTime;
            rb.velocity = direction * currentSpeed;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        bulletPool.ReturnBullet(bullet);
    }


    private async Task<Vector2[]> PrecalculateDirectionsAsync(float amplitude, float frequency)
    {
        return await Task.Run(() =>
        {
            Vector2[] result = new Vector2[41];
            for (int i = 0; i < 35; i++)
            {
                float x = -10.0f + 1f * i;
                float y = amplitude * Mathf.Sin(frequency * x);
                result[i] = new Vector2(x, y).normalized;
            }
            return result;
        });
    }

    public IEnumerator WavePattern()
    {
        float amplitude = 2.5f;
        float frequency = 4f;
        float duration = 8.0f; // Duración máxima de las balas antes de regresar al pool

        // Llamar al método asincrónico para precalcular direcciones
        var directionsTask = PrecalculateDirectionsAsync(amplitude, frequency);

        // Esperar a que se complete la tarea
        while (!directionsTask.IsCompleted)
        {
            yield return null;
        }

        Vector2[] directions = directionsTask.Result;

        while (true)
        {
            for (int i = 0; i < 30; i++)
            {
                GameObject bullet = bulletPool.GetBullet("BulletType8");
                if (bullet != null)
                {
                    bullet.transform.position = transform.position;
                    bullet.GetComponent<Rigidbody2D>().velocity = directions[i] * bulletSpeed;
                    StartCoroutine(RotateAndReturnBullet(bullet, duration));
                }
            }
            yield return new WaitForSeconds(0.15f); // Intervalo entre disparos
            
        }
    }


    private IEnumerator RotateAndReturnBullet(GameObject bullet, float duration)
    {
        float elapsedTime = 0f;
        float rotationSpeed = UnityEngine.Random.Range(-90f, 90f); // Velocidad de rotación aleatoria

        while (elapsedTime < duration)
        {
            bullet.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        bulletPool.ReturnBullet(bullet);
    }
    public IEnumerator DoubleSpiralPattern()
    {
        float angle1 = 0f;
        float angle2 = 180f;
        int totalBullets = 14; // Número total de balas en la espiral

        // Precalcular direcciones para una vuelta completa
        Vector2[] directions1 = new Vector2[totalBullets];
        Vector2[] directions2 = new Vector2[totalBullets];  
        for (int i = 0; i < totalBullets; i++)
        {
            float angleIncrement = 10f; // Ajusta el incremento del ángulo según sea necesario
            float angleRad1 = (angle1 + angleIncrement * i) * Mathf.Deg2Rad;
            float angleRad2 = (angle2 + angleIncrement * i) * Mathf.Deg2Rad;
            directions1[i] = new Vector2(Mathf.Cos(angleRad1), Mathf.Sin(angleRad1));
            directions2[i] = new Vector2(Mathf.Cos(angleRad2), Mathf.Sin(angleRad2));
        }

        while (true)
        {
            for (int i = 0; i < totalBullets; i++)
            {
                GameObject bullet1 = bulletPool.GetBullet("BulletType9");
                GameObject bullet2 = bulletPool.GetBullet("BulletType10");

                if (bullet1 != null)
                {
                    bullet1.transform.position = transform.position;
                    bullet1.GetComponent<Rigidbody2D>().velocity = directions1[i] * bulletSpeed;
                }

                if (bullet2 != null)
                {
                    bullet2.transform.position = transform.position;
                    bullet2.GetComponent<Rigidbody2D>().velocity = directions2[i] * bulletSpeed;
                }
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(audioSource.clip); // Reproducir el sonido de disparo
                }
            }

            yield return new WaitForSeconds(0.15f); // Intervalo entre disparos
            
        }
    }

    public IEnumerator SpinningLaserPattern()
    {
        int totalLasers = 8;
        float rotationSpeed = 360.0f / 3.0f; // 360 grados en 3 segundos
        float duration = 3.0f; // Duración de los láseres
        float waitTime = 0.1f; // Intervalo entre disparos

        while (true)
        {
            for (int i = 0; i < totalLasers; i++)
            {
                float angle = (360f / totalLasers) * i;
                StartCoroutine(FireLaser(angle, rotationSpeed, duration));
                yield return new WaitForSeconds(waitTime); // Pequeño intervalo entre disparos
            }
            yield return new WaitForSeconds(2.0f); // Intervalo entre series de disparos
            
        }
    }

    private IEnumerator FireLaser(float startAngle, float rotationSpeed, float duration)
    {
        float elapsedTime = 0f;
        GameObject laser = bulletPool.GetBullet("BulletType11");

        if (laser != null)
        {
            laser.transform.position = transform.position;
            laser.transform.rotation = Quaternion.Euler(0, 0, startAngle);

            while (elapsedTime < duration)
            {
                laser.transform.rotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            bulletPool.ReturnBullet(laser);
        }
    }

    public void IncreaseBulletSpeed()
    {
        bulletSpeed += 0.5f;
    }
}
