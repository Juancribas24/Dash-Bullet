using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    public int maxHealth = 10;
    private int currentHealth;

    public event Action<int> OnHealthChanged; // Evento para notificar el cambio de salud

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth); // Notificar el cambio de salud
        

        if (currentHealth <= 0)
        {
            // Manejar la muerte del jugador aquí
            StartCoroutine(GameOver());
            
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnHealthChanged?.Invoke(currentHealth); // Notificar el cambio de salud
        
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private IEnumerator GameOver()
    {
        Time.timeScale = 0f; // Detener el tiempo
        yield return new WaitForSecondsRealtime(2f); // Esperar 2 segundos en tiempo real
        Time.timeScale = 1f; // Restablecer el tiempo
        SceneManager.LoadScene("Menu"); // Cambiar a la escena del menú
    }
}
