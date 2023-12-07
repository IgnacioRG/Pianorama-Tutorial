using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Script para ejecutar el tutorial de Pianorama.
public class TutorialPiano : MonoBehaviour
{
    //Gameobject de tipo ui.
    public GameObject paso_ui;
    public GameObject exp_ui;

    //Gameobject de tipo boton.
    public GameObject sig_boton;

    //Assets de explicacion.
    public List<Sprite> mecanica1_sp;
    public List<Sprite> mecanica2_sp;
    public Sprite ayuda1_sp;
    public List<Sprite> ayuda2_sp;
    public List<Sprite> victoria_sp;
    public List<Sprite> derrota_sp;

    private bool _sig = false;

    //Inicializacion de parametros y gameobjects
    void Start()
    {
        _sig = false;
        sig_boton.transform.GetChild(0).GetComponent<Text>().text = "Siguiente";
        sig_boton.GetComponent<Button>().onClick.RemoveAllListeners();
        sig_boton.GetComponent <Button>().onClick.AddListener(SiguientePaso);

        StartCoroutine(TutorialFlujo());
    }

    /**
     * Metodo para volver a la escena principal tras completar el tutorial.
     */
    public void VolverAlPrincipal()
    {
        SceneManager.LoadScene("MenuPiano");
    }

    /**
     * SiguientePaso se llama cada vez que el jugador termina de leer la instruccion
     * actual (boton siguiente).
     */
    public void SiguientePaso()
    {
        _sig = true;
    }

    /**
     * Flujo normal del tutorial en el que se explica en 4 pasos las mecanicas del juego.
     * 1. Mecanica principal de memorizacion.
     * 2. Mecanica interactiva para replicar la melodia.
     * 3. Ayuda de reproduccion de melodia.
     * 4. Ayuda de borrador de secuencia.
     * 5. Condiciones de victoria.
     * 6. Condiciones de derrota.
     */
    IEnumerator TutorialFlujo()
    {
        int indx = 0;
        exp_ui.GetComponent<Text>().text = "Recuerda el orden en el que la melodía se toca en el piano.";
        while (!_sig)
        {
            foreach (Sprite sp in mecanica1_sp)
            {
                paso_ui.GetComponent<Image>().sprite = sp;
                if (_sig)
                {
                    break;
                }
                yield return new WaitForSeconds(1);
            }
        }

        _sig = false;
        indx = 0;
        exp_ui.GetComponent<Text>().text = "Vuelve a tocar cada nota en el orden correcto.";
        while (!_sig)
        {
            foreach (Sprite sp in mecanica2_sp)
            {
                paso_ui.GetComponent<Image>().sprite = sp;
                if (_sig)
                {
                    break;
                }
                yield return new WaitForSeconds(1);
            }
        }

        _sig = false;
        exp_ui.GetComponent<Text>().text = "Si no la recuerdas, puedes volver a reproducirla.";
        while (!_sig)
        {
            paso_ui.GetComponent <Image>().sprite = ayuda1_sp;
            yield return null;
        }

        _sig = false;
        indx = 0;
        exp_ui.GetComponent<Text>().text = "Si te equivocas, puedes borrar la melodía que tocaste.";
        while (!_sig)
        {
            foreach (Sprite sp in ayuda2_sp)
            {
                paso_ui.GetComponent<Image>().sprite = sp;
                if (_sig)
                {
                    break;
                }
                yield return new WaitForSeconds(1);
            }
        }

        _sig = false;
        indx = 0;
        exp_ui.GetComponent<Text>().text = "¡Sube de nivel al tocar melodías correctamente y desbloquea más retos!";
        while (!_sig)
        {
            foreach (Sprite sp in victoria_sp)
            {
                paso_ui.GetComponent<Image>().sprite = sp;
                if (_sig)
                {
                    break;
                }
                yield return new WaitForSeconds(1);
            }
        }

        _sig = false;
        indx = 0;
        exp_ui.GetComponent<Text>().text = "Si te equivocas muchas veces seguidas, bajarás de nivel.";
        while (!_sig)
        {
            sig_boton.transform.GetChild(0).GetComponent<Text>().text = "Comenzar";
            sig_boton.GetComponent<Button>().onClick.RemoveAllListeners();
            sig_boton.GetComponent<Button>().onClick.AddListener(VolverAlPrincipal);
            foreach (Sprite sp in derrota_sp)
            {
                paso_ui.GetComponent<Image>().sprite = sp;
                if(_sig)
                {
                    break;
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}
