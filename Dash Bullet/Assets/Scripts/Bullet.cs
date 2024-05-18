using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletPool bulletPool;

    void Awake()
    {
        bulletPool = FindObjectOfType<BulletPool>();
    }

    public void Shoot(Vector2 direction, float speed)
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    void OnBecameInvisible()
    {
        bulletPool.ReturnBullet(gameObject); // Regresar la bala al pool cuando salga de la pantalla
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bulletPool.ReturnBullet(gameObject); // Regresar la bala al pool en caso de colisión
    }
}
