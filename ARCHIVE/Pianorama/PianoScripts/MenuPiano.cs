using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuPiano : MonoBehaviour
{
    public void Jugar() {
        SceneManager.LoadScene("Piano");        
    }

    public void Salir() {
        SceneManager.LoadScene("Menu Juegos");
    }
}
