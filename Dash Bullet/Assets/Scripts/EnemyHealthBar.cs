using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthSlider; // Referencia al componente Slider de la barra de salud
    public EnemyHealth enemyHealth; // Referencia al script de salud del enemigo

    private void Start()
    {
        // Inicializar la barra de salud con la salud máxima del enemigo
        healthSlider.maxValue = enemyHealth.maxHealth;
        healthSlider.value = enemyHealth.GetCurrentHealth();

        // Suscribirse al evento de cambio de salud del enemigo
        enemyHealth.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento de cambio de salud del enemigo para evitar errores
        enemyHealth.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth)
    {
        // Actualizar el valor de la barra de salud
        healthSlider.value = currentHealth;
    }
}
