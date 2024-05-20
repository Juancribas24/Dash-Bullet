using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider healthSlider; // Referencia al componente Slider de la barra de salud

    private void Start()
    {
        // Inicializar la barra de salud con la salud máxima del jugador
        healthSlider.maxValue = PlayerHealth.Instance.maxHealth;
        healthSlider.value = PlayerHealth.Instance.GetCurrentHealth();

        // Suscribirse al evento de cambio de salud del jugador
        PlayerHealth.Instance.OnHealthChanged += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento de cambio de salud del jugador para evitar errores
        PlayerHealth.Instance.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int currentHealth)
    {
        // Actualizar el valor de la barra de salud
        healthSlider.value = currentHealth;
    }
}
