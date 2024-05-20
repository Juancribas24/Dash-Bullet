using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; // Salud m�xima del enemigo
    private int currentHealth; // Salud actual del enemigo


    public event Action<int> OnHealthChanged; // Evento para notificar el cambio de salud
    public GameObject victoryText;

    void Start()
    {
        currentHealth = maxHealth; // Inicializar la salud al m�ximo al comenzar
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reducir la salud en funci�n del da�o recibido
        OnHealthChanged?.Invoke(currentHealth); // Notificar el cambio de salud

        if (currentHealth <= 0)
        {
            Die(); // Llamar a la funci�n Die si la salud es 0 o menos
        }
    }

    void Die()
    {
        // Aqu� puedes a�adir l�gica adicional, como animaciones de muerte o efectos de sonido
        Destroy(gameObject); // Destruir el objeto enemigo
        StartCoroutine(Win());
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private IEnumerator Win()
    {
        victoryText.SetActive(true);
        Time.timeScale = 0f; // Detener el tiempo
        yield return new WaitForSecondsRealtime(2f); // Esperar 2 segundos en tiempo real
        Time.timeScale = 1f; // Restablecer el tiempo
        SceneManager.LoadScene("Menu"); // Cambiar a la escena del men�
    }
}
