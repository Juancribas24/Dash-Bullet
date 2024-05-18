using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public BulletPool bulletPool;
    public float fireRate = 1.0f;
    public float bulletSpeed = 5.0f;
    private float nextFireTime = 0.0f;

    // Ejemplo de diferentes patrones de ataque
    public string[] attackPatterns;
    private int currentPatternIndex = 0;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1.0f / fireRate;
        }
    }

    void Fire()
    {
        string bulletType = attackPatterns[currentPatternIndex];
        GameObject bullet = bulletPool.GetBullet(bulletType);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Bullet>().Shoot(Vector2.down, bulletSpeed);

        // Cambiar al siguiente patrón de ataque (puede ser más complejo según tu lógica)
        currentPatternIndex = (currentPatternIndex + 1) % attackPatterns.Length;
    }
}
