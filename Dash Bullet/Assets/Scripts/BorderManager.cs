using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BorderManager : MonoBehaviour
{
    public RectTransform leftBorder;
    public RectTransform rightBorder;
    public float borderWidth = 50f; // Ancho del borde en píxeles

    void Start()
    {
        AdjustBorders();
    }

    void AdjustBorders()
    {
        // Ajustar el borde izquierdo
        leftBorder.sizeDelta = new Vector2(borderWidth, Screen.height);
        leftBorder.anchoredPosition = new Vector2(-borderWidth / 2, 0);

        // Ajustar el borde derecho
        rightBorder.sizeDelta = new Vector2(borderWidth, Screen.height);
        rightBorder.anchoredPosition = new Vector2(borderWidth / 2, 0);
    }
}
