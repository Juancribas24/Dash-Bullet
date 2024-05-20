using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public BulletPool bulletPool; // Referencia al pool de balas
    public float bulletSpeed = 5.0f; // Velocidad a la que se dispararán las balas
    public float fireRate = 0.5f; // Tiempo entre disparos
    private float nextFireTime = 0.0f; // Tiempo para el próximo disparo

    private bool isDamageBoosted = false;
    private float damageBoostEndTime;
    private AudioSource audioSource;

    

    void Update()
    {
        if (Time.time > nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate;
        }

        if (isDamageBoosted && Time.time > damageBoostEndTime)
        {
            DeactivateDamageBoost();
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtener el componente AudioSource
    }

    void FireBullet()
    {
        GameObject bullet = bulletPool.GetBullet("PlayerBullet"); // Asume que tienes un tipo de bala definido para el jugador
        if (bullet != null)
        {
            bullet.transform.position = transform.position; // Establece la posición de la bala en la del jugador
            bullet.transform.rotation = Quaternion.identity; // Restablece la rotación para asegurar una dirección constante    
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float currentBulletSpeed = isDamageBoosted ? bulletSpeed * 3f : bulletSpeed; // Incrementa la velocidad de la bala si el daño está potenciado
                rb.velocity = Vector2.up * bulletSpeed; // Impulsa la bala hacia adelante
            }
            if (audioSource != null)
            {
                audioSource.Play(); // Reproducir el sonido de disparo
            }
            
        }
    }
    public void ActivateDamageBoost(float duration)
    {
        isDamageBoosted = true;
        damageBoostEndTime = Time.time + duration;
        
    }

    void DeactivateDamageBoost()
    {
        isDamageBoosted = false;
        
    }
}
