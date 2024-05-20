using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Health, Shield, DamageBoost }
    public PowerUpType powerUpType;
    public int points = 500;
    public float duration = 5f; // Duración del power-up (si aplica)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyPowerUp(other.gameObject);
            ScoreManager.Instance.AddPoints(points);
            Destroy(gameObject); // Destruir el power-up después de ser recogido
        }
    }

    void ApplyPowerUp(GameObject player)
    {
        switch (powerUpType)
        {
            case PowerUpType.Health:
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(3); // Incrementa la salud del jugador
                }
                break;
            case PowerUpType.Shield:
                PlayerShield playerShield = player.GetComponent<PlayerShield>();
                if (playerShield != null)
                {
                    playerShield.ActivateShield(duration); // Activa el escudo por la duración especificada
                }
                break;
            case PowerUpType.DamageBoost:
                PlayerShoot playerShoot = player.GetComponent<PlayerShoot>();
                if (playerShoot != null)
                {
                    playerShoot.ActivateDamageBoost(duration); // Aumenta el daño por la duración especificada
                }
                break;
        }
    }
}
