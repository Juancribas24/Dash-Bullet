using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour
{
    public void LoadLoadingScene()
    {
        SceneManager.LoadScene("PantallaCarga");
    }
}
