using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletPool bulletPool;
    public GameObject owner;
    public int damage = 1;
    public int pointsPerShot = 50;

    void Awake()
    {
        bulletPool = FindObjectOfType<BulletPool>();
    }

    public void Shoot(Vector2 direction, float speed, GameObject shooter)
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed;
        owner = shooter;
    }

    void OnBecameInvisible()
    {
        bulletPool.ReturnBullet(gameObject); // Regresar la bala al pool cuando salga de la pantalla
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth.Instance.TakeDamage(1); // Resta 1 punto de vida al jugador
            bulletPool.ReturnBullet(gameObject); // Devuelve la bala al pool
        }
        if (collision.gameObject.CompareTag("Boundary"))
        {
            bulletPool.ReturnBullet(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("PlayerBullet") && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            bulletPool.ReturnBullet(gameObject); // Devuelve la bala al pool
            ScoreManager.Instance.AddPoints(pointsPerShot);
        }
    }
}
