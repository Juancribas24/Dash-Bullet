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
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth.Instance.TakeDamage(1); // Resta 1 punto de vida al jugador
            bulletPool.ReturnBullet(gameObject);
        }
        else if (collision.gameObject.CompareTag("Boundary"))
        {
            bulletPool.ReturnBullet(gameObject);
        }
    }
}
