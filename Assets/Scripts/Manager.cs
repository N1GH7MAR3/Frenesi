using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    private Kawsarin kawsarin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kawsarin = FindFirstObjectByType<Kawsarin>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IniciarJuego() { 
        Time.timeScale = 1f;
        SceneManager.LoadScene("Nivel1");
        kawsarin.ResetearPersonaje();
    }

    public void SalirDelJuego () { 
        Application.Quit();
    }


}
