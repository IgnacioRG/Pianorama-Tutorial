using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using System.Linq;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

namespace JuegoPiano{


public class KeysController : MonoBehaviour
{
    public Button[] keys, primerOctavaKeys;
    [SerializeField] private Button _botonPausa, _reto, _borrar, _listo, _secuencia, _continuar, _salir;
    public AudioClip[] Notes;

    [SerializeField] private Text[] _textoDeOrden, _primerOctavaOrden;
    public AudioSource pianoAudioSource;
    public Text contadorAciertos_text, contadorErrores, warning, levelNo, ListaDeNotas, primerOctavaNotas, contadorIntentos;

    private int _errores = 0;
    private int _aciertos = 0, _nivel = 1;
    //private bool activateLvl2 = false, activatelvl3 = false, activatelvl4 = false, activatelvl5 = false;// activatelvl6 = false;

    private int _intentos;

    List<string> _genNotes;
    int _nextKeyIndex = 0, _transitionNextKeyIndex = 0, _indice, _indiceObtenido;

    //Pila de notas ingresadas por usuario
    private List<string> _notasIngresadas = new List<string>();

    private bool _iguales;

    private Color _colorAnterior;

    public Dictionary<string, Button> pianoKeys = new Dictionary<string, Button>();
    
    public Dictionary<string, Button> primerOctavaPiano = new Dictionary<string, Button>();
    public Dictionary<string, AudioClip> keysAndSounds = new Dictionary<string, AudioClip>();

    public NotesPresenter genNotesP;

        public AudioClip victoria_audioClip;
        public AudioClip derrota_audioClip;
        public AudioClip subeNivel_audioClip;
        public AudioClip bajanivel_audioClip;

    private string[] _listaNotas, _listaNotasCompleta, _listaPrimerOctava;

    private string _numeroDeTeclaTemporal;

    //Texto a mostrar cuando la secencia sea correcta, incorrecta,
    //se retroceda un nivel o se avance un nivel
    [SerializeField] private GameObject _panelAcierto, _panelError, _panelRegresaNivel, _panelAvanzaNivel, _menuPausa;

    //Paneles de niveles 1 a 5 y 6 a 10 respectivamente
    [SerializeField] private GameObject _panelPrimerOctava, _panelPianoCompleto;

    //Notas para niveles predefinidos del nivel 6 al 10
    private string[,] _NivelesPredefinidosSeis = new string[10, 4]{
        {"MI 2", "FA 2", "SOL 2", "DO 2"},
        {"RE 2", "FA 2", "SOL 2", "MI 2"},
        {"DO 2", "SI 1", "SOL 1", "DO 2"},
        {"DO 2", "RE 2", "SOL 2", "MI 2"},
        {"SOL 1", "LA 1", "MI 2", "DO 2"},
        {"MI 2", "LA 2", "SOL 2", "MI 2"},
        {"FA#2", "SOL 2", "LA 2", "RE 2"},
        {"MI 1", "SOL#1", "SI 1", "LA 1"},
        {"MI 1", "SOL 1", "SOL 1", "DO 1"},
        {"MI 2", "DO#2", "LA 2", "RE 2"}
     };

     private string[,] _NivelesPredefinidosSiete = new string[10, 5]{
        {"DO 2", "DO 2","RE 2", "MI 2", "DO 2"},
        {"DO 1", "RE 1", "MI 1", "SOL 1", "DO 1"},
        {"RE 2", "MI 2", "FA#2", "SOL 2", "FA#2"},
        {"MI 2", "SOL 2", "SOL 2", "LA 2", "SI 2"},
        {"SOL 1", "LA 1", "MI 2", "RE 2", "DO 2"},
        {"SOL 2", "MI 2", "FA 2", "RE 2", "MI 2"},
        {"SOL 1", "FA#1", "MI 1", "RE 1", "SOL 1"},
        {"SI 1", "RE 2", "SOL 2", "LA 2", "SI 2"},
        {"DO 1", "SOL 1", "MI 1", "SI 1", "DO 2"},
        {"SOL 1", "MI 1", "RE 2", "SI 1", "DO 2"}
     };
     
     private string[,] _NivelesPredefinidosOcho = new string[10, 6]{
        {"SOL 2", "FA 2", "DO 2", "RE 2", "MI 2", "DO 2"},
        {"MI 2", "MI 2", "DO 2", "DO 2", "RE 2", "DO 2"},
        {"MI 2", "RE 2", "DO#2", "SI 1", "DO#2", "LA 1"},
        {"RE 1", "FA#1", "FA#1", "SOL 1", "LA 1", "FA#1"},
        {"SOL 1", "LA 1", "SI 1", "DO 2", "RE 2", "SI 1"},
        {"SOL 1", "DO 2", "MI 2", "RE 2", "SI 1", "DO 2"},
        {"DO 2", "MI 2", "RE 2", "DO 2", "FA 2", "SOL 2"},
        {"LA 1", "DO#2", "SI 1", "DO#2", "RE 2", "MI 2"},
        {"MI 2", "MI 2", "SOL 2", "SOL 2", "SI 2", "LA 2"},
        {"DO 2", "MI 2", "FA 2", "SOL 2", "LA 2", "SOL 2"}
     };

    private string[,] _NivelesPredefinidosNueve = new string[10, 8]{
        {"RE 1", "LA 1", "SOL 1", "FA 1", "MI 1", "FA 1", "MI 1", "RE 1"},
        {"LA 1", "LA 1", "SI 1", "SI 1", "DO 1", "DO 1", "RE 1", "LA 1"},
        {"DO 2", "RE 2", "MI 2", "FA 2", "MI 2", "RE 2", "DO 2", "DO 2"},
        {"SOL 1", "SOL 1", "LA 1", "SI 1", "DO 2", "SI 1", "LA 1", "SOL 1"},
        {"FA 1", "FA 1", "DO 2", "DO 2", "LA#1", "LA#1", "SOL 1", "FA 1"},
        {"LA 1", "SI 1", "DO 2", "RE 2", "MI 2", "RE 2", "DO 2", "SI 1"},
        {"DO 2", "MI 2", "RE 2", "FA 2", "MI 2", "SOL 2", "RE 2", "DO 2"},
        {"LA 1", "LA 1", "MI 2", "RE 2", "DO 2", "SI 1", "DO 2", "LA 1"},
        {"DO 2", "DO 2", "SOL 1", "FA 2", "SOL 2", "LA 2", "SOL 2", "DO 2"},
        {"DO 2", "MI 2", "RE 2", "DO 2", "MI 2", "SOL 2", "FA 2", "MI 2"}
     };

    private string[,] _NivelesPredefinidosDiez = new string[10, 10]{
        {"RE 2", "MI 2", "FA 2", "SOL 2", "LA 2", "LA 2", "SOL 2", "FA 2","MI 2", "RE 2"},
        {"DO 2", "RE 2", "MI 2", "FA 2", "MI 2", "RE 2", "DO 2", "SOL 1", "SOL 1", "DO 2"},
        {"DO 2", "DO 2", "RE 2", "RE 2", "MI 2","MI 2", "FA 2", "FA 2", "SOL 2", "DO 2"},
        {"LA 1", "SI 1", "DO#2", "RE 2", "MI 2", "MI 2", "RE 2", "DO#2", "SI 1", "LA 1"},
        {"SOL 1", "SOL 1", "RE 2", "RE 2", "LA 1", "LA 1", "DO 2", "DO 2", "SI 1", "SOL 1"},
        {"MI 2", "DO 2", "RE 2", "MI 2", "FA 2", "SOL 2", "LA 2", "SI 2", "SOL 2", "MI 2"},
        {"LA 2", "SOL 2", "FA 2", "MI 2", "RE 2", "SOL 2", "FA 2", "MI 2", "RE 2", "DO 2"},
        {"DO 2", "SI 1", "LA 1", "SOL 2", "LA 1", "SOL 1", "FA 1", "MI 1", "RE 1", "DO 1"},
        {"MI 2", "MI 2", "FA 2", "SOL 2", "SOL 2", "FA 2", "MI 2", "RE 2", "DO 2", "DO 2"},
        {"RE 2", "MI 2", "FA 2", "SOL 2", "FA 2", "MI 2", "RE 2", "DO 2", "SI 2", "DO 2"}
     };


    //Notas para melodias de Martinillo, Canon, Para Elisa y La Mariposa

    private string[,] _NivelesPredefinidosOnce = new string[4, 14]{
        {"SOL 1", "LA 1", "SI 1", "SOL 1", "SOL 1", "LA 1", "SI 1", "SI 1", "DO 2", "RE 2", "SI 1", "DO 2", "RE 2", "RE 2"},
        {"RE 2", "SOL 1", "SI 1", "FA#1", "SOL 1", "RE 1", "SOL 1", "LA 1", "RE 2", "LA 1", "SI 1", "FA#1", "SOL 1", "RE 1"},
        {"MI 2", "RE#2", "MI 2", "RE#2", "MI 2", "SI 1", "RE 2", "DO 2", "LA 1", "DO 1", "MI 1", "LA 1", "SI 1", "MI 1"},
        {"RE 2", "MI 2", "FA#2", "SOL 2", "FA#2", "MI 2", "RE 2", "SI 1", "RE 2", "LA 1", "RE 2", "SI 1", "RE 2", "MI 2"}
     };

    private List<int> _listaDeSeleccionNiveles = new List<int>();
    
    private int _secuenciaAnterior = -1;

    System.Random _selectorNiveles = new System.Random();

    public int elegirNivel;
    
    [SerializeField]
    int _NeurocoinsRecibidasAlGanar = 5;
    [SerializeField]
    int _NeurocoinsRecibidasAlSubirDeNivel = 20;

    public GameObject mascota;
    public GameObject coins_panel;
    public Text coins_text;

        private void Awake()
    {
        primerOctavaPiano.Add("DO 1", primerOctavaKeys[0]);
        primerOctavaPiano.Add("DO#1", primerOctavaKeys[1]);
        primerOctavaPiano.Add("RE 1", primerOctavaKeys[2]);
        primerOctavaPiano.Add("RE#1", primerOctavaKeys[3]);
        primerOctavaPiano.Add("MI 1", primerOctavaKeys[4]);
        primerOctavaPiano.Add("FA 1", primerOctavaKeys[5]);
        primerOctavaPiano.Add("FA#1", primerOctavaKeys[6]);
        primerOctavaPiano.Add("SOL 1", primerOctavaKeys[7]);
        primerOctavaPiano.Add("SOL#1", primerOctavaKeys[8]);
        primerOctavaPiano.Add("LA 1", primerOctavaKeys[9]);
        primerOctavaPiano.Add("LA#1", primerOctavaKeys[10]);
        primerOctavaPiano.Add("SI 1", primerOctavaKeys[11]);
        
        pianoKeys.Add("DO 1", keys[0]);
        pianoKeys.Add("DO#1", keys[1]);
        pianoKeys.Add("RE 1", keys[2]);
        pianoKeys.Add("RE#1", keys[3]);
        pianoKeys.Add("MI 1", keys[4]);
        pianoKeys.Add("FA 1", keys[5]);
        pianoKeys.Add("FA#1", keys[6]);
        pianoKeys.Add("SOL 1", keys[7]);
        pianoKeys.Add("SOL#1", keys[8]);
        pianoKeys.Add("LA 1", keys[9]);
        pianoKeys.Add("LA#1", keys[10]);
        pianoKeys.Add("SI 1", keys[11]);
        pianoKeys.Add("DO 2", keys[12]);
        pianoKeys.Add("DO#2", keys[13]);
        pianoKeys.Add("RE 2", keys[14]);
        pianoKeys.Add("RE#2", keys[15]);
        pianoKeys.Add("MI 2", keys[16]);
        pianoKeys.Add("FA 2", keys[17]);
        pianoKeys.Add("FA#2", keys[18]);
        pianoKeys.Add("SOL 2", keys[19]);
        pianoKeys.Add("SOL#2", keys[20]);
        pianoKeys.Add("LA 2", keys[21]);
        pianoKeys.Add("LA#2", keys[22]);
        pianoKeys.Add("SI 2", keys[23]);

        keysAndSounds.Add("DO 1", Notes[0]);
        keysAndSounds.Add("DO#1", Notes[1]);
        keysAndSounds.Add("RE 1", Notes[2]);
        keysAndSounds.Add("RE#1", Notes[3]);
        keysAndSounds.Add("MI 1", Notes[4]);
        keysAndSounds.Add("FA 1", Notes[5]);
        keysAndSounds.Add("FA#1", Notes[6]);
        keysAndSounds.Add("SOL 1", Notes[7]);
        keysAndSounds.Add("SOL#1", Notes[8]);
        keysAndSounds.Add("LA 1", Notes[9]);
        keysAndSounds.Add("LA#1", Notes[10]);
        keysAndSounds.Add("SI 1", Notes[11]);
        keysAndSounds.Add("DO 2", Notes[12]);
        keysAndSounds.Add("DO#2", Notes[13]);
        keysAndSounds.Add("RE 2", Notes[14]);
        keysAndSounds.Add("RE#2", Notes[15]);
        keysAndSounds.Add("MI 2", Notes[16]);
        keysAndSounds.Add("FA 2", Notes[17]);
        keysAndSounds.Add("FA#2", Notes[18]);
        keysAndSounds.Add("SOL 2", Notes[19]);
        keysAndSounds.Add("SOL#2", Notes[20]);
        keysAndSounds.Add("LA 2", Notes[21]);
        keysAndSounds.Add("LA#2", Notes[22]);
        keysAndSounds.Add("SI 2", Notes[23]);
       
        foreach (var key in keys)
        {
            key.enabled = false;
        }
        foreach (var key in primerOctavaKeys)
        {
            key.enabled = false;
        }
        _borrar.enabled = false;
        _listo.enabled = false;
        _secuencia.enabled = false;
        _reto.enabled = false;

        if (elegirNivel > 5){
                
                _panelPrimerOctava.SetActive(false);
                _panelPianoCompleto.SetActive(true);
        }

        else{
                
                _panelPrimerOctava.SetActive(true);
                _panelPianoCompleto.SetActive(false);
        }

        _nivel = elegirNivel;
        if (_nivel <=0 || _nivel > 11){
            _nivel = 1;
        }

        _genNotes = genNotesP.generatedNotes;

    }

    void Start()
    {
        levelNo.text = _nivel.ToString();
        ListaDeNotas.text = "";
        warning.text = "";
        keySounds();
        ActivadorDeBotones();
        //keysManager(generatedNotes);
        _listaNotas = genNotesP.getNotesArray;
        _listaNotasCompleta = genNotesP.getNotesandBemolArray;
        _listaPrimerOctava = genNotesP.getPrimerOctava;
        CambiarValoresPorNivel();
        
        //StartCoroutine(ShowNextsequence());
    }


    //Metodo para anotar orden en que se presionan teclas
    void MuestraNumeroDeTeclas(){

        foreach (Text _text in _textoDeOrden)
        {

            _text.text = " ";
            _text.gameObject.SetActive(false);

        }

        foreach (Text _text in _primerOctavaOrden)
        {

            _text.text = " ";
            _text.gameObject.SetActive(false);

        }

        if (_notasIngresadas.Count > 0)

            for (int indiceListaRespuestas = 0; indiceListaRespuestas < _notasIngresadas.Count; indiceListaRespuestas++)
            {
                if (_nivel > 5){

                    _indiceObtenido = ObtenerIndiceDeArray(_notasIngresadas[indiceListaRespuestas]);
                    _textoDeOrden[_indiceObtenido].gameObject.SetActive(true);
                    _numeroDeTeclaTemporal = _textoDeOrden[_indiceObtenido].text;
                    indiceListaRespuestas++;
                    _textoDeOrden[_indiceObtenido].text = _numeroDeTeclaTemporal + " " + indiceListaRespuestas.ToString();
                    
                }
                else{

                    _indiceObtenido = ObtenerIndiceDeArrayPrimerOctava(_notasIngresadas[indiceListaRespuestas]);
                    _primerOctavaOrden[_indiceObtenido].gameObject.SetActive(true);
                    _numeroDeTeclaTemporal = _primerOctavaOrden[_indiceObtenido].text;
                    indiceListaRespuestas++;
                    _primerOctavaOrden[_indiceObtenido].text = _numeroDeTeclaTemporal + " " + indiceListaRespuestas.ToString();
                    
                }

                indiceListaRespuestas--;
                
            }
    }

    //Metodo para encontrar tecla presionada
    int ObtenerIndiceDeArray(string notaIngresada)
    {
        _indice = 0;
        
        while (_listaNotasCompleta[_indice] != notaIngresada)
        
            _indice++;
        
        return _indice;

    }

    //Metodo para encontrar tecla presionada en primer octava
    int ObtenerIndiceDeArrayPrimerOctava(string notaIngresada)
    {
        _indice = 0;
        
        while (_listaPrimerOctava[_indice] != notaIngresada)
        
            _indice++;
        
        return _indice;

    }

    //Metodo para asignar acciones a botones del menu de pausa y 
    //Reproducir reto, secuencia, borrar y listo
    void ActivadorDeBotones(){

        _reto.onClick.AddListener(() => ReproducirReto());
        _borrar.onClick.AddListener(() => Borrar());
        _secuencia.onClick.AddListener(() => ReproducirSecuencia());
        _listo.onClick.AddListener(() => RevisarRespuesta());
        _botonPausa.onClick.AddListener(() => Pausar());
        _continuar.onClick.AddListener(() => ContinuarJuego());
        _salir.onClick.AddListener(() => SalirDeJuego());

    }

    //Metodo para comparar contenido de listas
    bool CompararListas(List<string> lista1, List<string> lista2){
        
        _iguales = true;
        _indice = 0;

        if (lista1.Count != lista2.Count)

            _iguales = false;

        else
        {

            do
            {
                if (lista1[_indice] != lista2[_indice])
                
                    _iguales = false;

                _indice++;

            } while (_indice < lista1.Count && _iguales);

        }

        return _iguales;
    }

    //Metodo para revisar respuesta con boton de listo
    void RevisarRespuesta()
    {
        _reto.enabled = false;
        _secuencia.enabled = false;
        _borrar.enabled = false;
        _listo.enabled = false;

        foreach (var key in keys)
        {
            key.enabled = false;
        }
        foreach (var key in primerOctavaKeys)
        {
            key.enabled = false;
        }

        if (CompararListas(_genNotes, _notasIngresadas))
        {
            GuardaPartida(true,false);
            _aciertos++;
            _errores = 0;
            StartCoroutine(MostrarCorrecto());

        }
        else
        {
            GuardaPartida(false,false);
            _errores++;
            _aciertos = 0;
            StartCoroutine(MostrarIncorrecto());
        }
        
        _notasIngresadas.Clear();
        resetColors();
        ListaDeNotas.gameObject.SetActive(false);
        primerOctavaNotas.gameObject.SetActive(false);

    }


    //Metodo para seleccionar niveles predefinidos del 6 al 11
    private void EscojerNivelPredefinido(string[,]  _MemoriaNiveles){

        if (_listaDeSeleccionNiveles.Count == 0){
                for (int _i = 0;  _i < _MemoriaNiveles.GetLength(0); _i++)

                    _listaDeSeleccionNiveles.Add(_i);

        }

        _genNotes.Clear();
        int _auxiliarSelector = _selectorNiveles.Next(0, _listaDeSeleccionNiveles.Count);

        if (_listaDeSeleccionNiveles[_auxiliarSelector] == _secuenciaAnterior && _secuenciaAnterior < _MemoriaNiveles.GetLength(1))

            _auxiliarSelector++;

        else if(_listaDeSeleccionNiveles[_auxiliarSelector] == _secuenciaAnterior)

            _auxiliarSelector = 0;

        _secuenciaAnterior = _listaDeSeleccionNiveles[_auxiliarSelector];
        
        for(int _lector = 0; _lector < _MemoriaNiveles.GetLength(1); _lector ++)

            _genNotes.Add(_MemoriaNiveles[_secuenciaAnterior,_lector]);

            
        _listaDeSeleccionNiveles.RemoveAt(_auxiliarSelector);


    }

    //Se obtienen nuevas secuencias y
    //se ajustan los numeros de notas por secuencia y reproducciones
    //permitidas por reto dependiendo del nivel
    void CambiarValoresPorNivel()
    {

        if (_nivel>=1 && _nivel <= 5) 
            
            _intentos = 1;

        else

            _intentos = 2;

        if (_nivel == 1)

            genNotesP.NotesPerLevelOneToFive(2);

        else if (_nivel == 2)

            genNotesP.NotesPerLevelOneToFive(3);

        else if (_nivel == 3)

            genNotesP.NotesPerLevelOneToFive(3);

        else if (_nivel == 4 || _nivel == 5)

            genNotesP.NotesPerLevelOneToFive(4);

        else  if (_nivel == 6)

            EscojerNivelPredefinido(_NivelesPredefinidosSeis);

        else if (_nivel == 7)

            EscojerNivelPredefinido(_NivelesPredefinidosSiete);

        else if (_nivel == 8)

            EscojerNivelPredefinido(_NivelesPredefinidosOcho);

        else if (_nivel == 9)

            EscojerNivelPredefinido(_NivelesPredefinidosNueve);

        else if (_nivel == 10)

            EscojerNivelPredefinido(_NivelesPredefinidosDiez);

        else if (_nivel == 11)
            
            EscojerNivelPredefinido(_NivelesPredefinidosOnce);


        if (_nivel > 5)

            _genNotes = genNotesP.generatedNotes;

        //Se actualiza contador de aciertos, errores e intentos
        contadorAciertos_text.text = _aciertos.ToString()+"/3";
        contadorErrores.text = _errores.ToString()+"/3";
        contadorIntentos.text = _intentos.ToString();
        StartCoroutine(ShowNextsequence());

    }

    //Metodo en caso de que se introduzca respuesta correcta
    IEnumerator MostrarCorrecto() 
    {

        if (_aciertos < 3)
        {
            _panelAcierto.gameObject.SetActive(true);
            pianoAudioSource.clip = victoria_audioClip;
            pianoAudioSource.Play();
            mascota.GetComponent<Animator>().Rebind();
            mascota.GetComponent<Animator>().SetInteger("Estado", 1); //aplauso
            coins_panel.SetActive(true);
            coins_text.text = "¡+" + _NeurocoinsRecibidasAlGanar + " Neurocoins!";
            ActualizaNeurocoins(_NeurocoinsRecibidasAlGanar);
            yield return new WaitForSeconds(3f);
            coins_panel.SetActive(false);
            _panelAcierto.gameObject.SetActive(false);
        }
        else
        {
            if (_nivel != 11)
                _nivel++;
            else
                _nivel = 11;
            _panelAvanzaNivel.gameObject.SetActive(true);
            pianoAudioSource.clip = subeNivel_audioClip;
            pianoAudioSource.Play();
            mascota.GetComponent<Animator>().Rebind();
            mascota.GetComponent<Animator>().SetInteger("Estado", 0); //celebrando
            coins_panel.SetActive(true);
            coins_text.text = "¡+" + _NeurocoinsRecibidasAlSubirDeNivel + " Neurocoins!";
            ActualizaNeurocoins(_NeurocoinsRecibidasAlSubirDeNivel);
            yield return new WaitForSeconds(4f);
            _panelAvanzaNivel.gameObject.SetActive(false);
            coins_panel.SetActive(false);
            levelNo.text = _nivel.ToString();
            if (_nivel > 5){
                _panelPrimerOctava.SetActive(false);
                _panelPianoCompleto.SetActive(true);
            }
            else{                
                _panelPrimerOctava.SetActive(true);
                _panelPianoCompleto.SetActive(false);
            }
            _aciertos = 0;
            _secuenciaAnterior = -1;
            if (_listaDeSeleccionNiveles.Count > 0)
                _listaDeSeleccionNiveles.Clear();
        }
        CambiarValoresPorNivel();
    }

    //Metodo en caso de que se introduzca respuesta incorrecta
    IEnumerator MostrarIncorrecto() 
    {
        if (_errores < 3)
        {
           pianoAudioSource.clip = derrota_audioClip;
           pianoAudioSource.Play();
           _panelError.gameObject.SetActive(true);
           yield return new WaitForSeconds(3f);
           _panelError.gameObject.SetActive(false);
        }
        else
        {
            if (_nivel != 1)
                _nivel--;
            else
                _nivel = 1;                
            _panelRegresaNivel.gameObject.SetActive(true);
            pianoAudioSource.clip = bajanivel_audioClip;
            pianoAudioSource.Play();
            coins_panel.SetActive(true);
            coins_text.text = "¡No te rindas!";
            mascota.GetComponent<Animator>().Rebind();
            mascota.GetComponent<Animator>().SetInteger("Estado", 2); //triste
            yield return new WaitForSeconds(4f);
            _panelRegresaNivel.gameObject.SetActive(false);
            coins_panel.SetActive(false);
            levelNo.text = _nivel.ToString();
            
            if (_nivel > 5){

                _panelPrimerOctava.SetActive(false);
                _panelPianoCompleto.SetActive(true);
            }

            else{
                
                _panelPrimerOctava.SetActive(true);
                _panelPianoCompleto.SetActive(false);
            }
            
            _errores = 0;
            _secuenciaAnterior = -1;
            if (_listaDeSeleccionNiveles.Count > 0)
                _listaDeSeleccionNiveles.Clear();

        }

        CambiarValoresPorNivel();

    }

    //Metodo para salir de juego en pausa
    public void SalirDeJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu Juegos");

    }

    //Metodo para abrir menu de pausa
    public void Pausar()
    {

        Time.timeScale = 0f;
        _menuPausa.SetActive(true);
        _botonPausa.enabled = false;

    }

    //Metodo para cerrar menu de pausa
    public void ContinuarJuego()
    {

        Time.timeScale = 1.0f;
        _menuPausa.SetActive(false);
        _botonPausa.enabled = true;

    }

    //Metodo para reproducir reto con boton
    void ReproducirReto(){

        _intentos--;
        contadorIntentos.text = _intentos.ToString();
        resetColors();
        StartCoroutine(ShowNextsequence());
            
    }

    //Metodo para borrar de lista de respuesta de usuario
    void Borrar(){

        _notasIngresadas.RemoveAt(_notasIngresadas.Count - 1);
        if (_nivel > 5)
            ListaDeNotas.text = ListToText(_notasIngresadas);

        else
            primerOctavaNotas.text = ListToText(_notasIngresadas);
        
        foreach (var key in keys)
        {
            key.enabled = true;
        }
        
        foreach (var key in primerOctavaKeys)
        {
            key.enabled = true;
        }

        resetColors();

        if (_nivel == 1 || _nivel == 2 || _nivel == 4)      
            
            foreach(string _Tecla in _genNotes)

                primerOctavaPiano[_Tecla].image.color = Color.yellow;

        
        if (_notasIngresadas.Count <= 0){

            _borrar.enabled = false;
            _secuencia.enabled = false;
            _listo.enabled = false;
            ListaDeNotas.gameObject.SetActive(false);
            primerOctavaNotas.gameObject.SetActive(false);

        }
        else 

            EfectoAzul();

        
    }

    //Metodo para reproducir secuencia ingresada por usuario
    //con boton
    void ReproducirSecuencia(){

        foreach (var key in keys)
        {
            key.enabled = false;
        }
        
        foreach (var key in primerOctavaKeys)
        {
            key.enabled = false;
        }

        StartCoroutine(MostrarRespuestaDeJugador());
            
    }

    //Metodo para mostrar la respuesta registrada del usuario
    IEnumerator MostrarRespuestaDeJugador() //List<string> generatedNotes
    {

        yield return new WaitForSeconds(0.10f);
        _reto.enabled = false;
        _borrar.enabled = false;
        _listo.enabled = false;
        _secuencia.enabled = false;
        
        if (_nivel > 5){
            
            _colorAnterior = pianoKeys[_notasIngresadas[_nextKeyIndex]].image.color;
            pianoKeys[_notasIngresadas[_nextKeyIndex]].image.color = Color.green;

            pianoAudioSource.PlayOneShot(keysAndSounds[_notasIngresadas[_nextKeyIndex]]);
            _transitionNextKeyIndex = _nextKeyIndex;
            yield return new WaitForSeconds(1f);
            pianoKeys[_notasIngresadas[_nextKeyIndex]].image.color = _colorAnterior;
        
        }
        else{
             
             _colorAnterior = primerOctavaPiano[_notasIngresadas[_nextKeyIndex]].image.color;
            primerOctavaPiano[_notasIngresadas[_nextKeyIndex]].image.color = Color.green;

            pianoAudioSource.PlayOneShot(keysAndSounds[_notasIngresadas[_nextKeyIndex]]);
            _transitionNextKeyIndex = _nextKeyIndex;
            yield return new WaitForSeconds(1f);
            primerOctavaPiano[_notasIngresadas[_nextKeyIndex]].image.color = _colorAnterior;
        
        }

        _nextKeyIndex++;

        
        if (_nextKeyIndex < _notasIngresadas.Count)
        {
            StartCoroutine(MostrarRespuestaDeJugador());
        } else
        {
            _transitionNextKeyIndex = 0;
            _nextKeyIndex = 0;

            
            if (_notasIngresadas.Count < _genNotes.Count)
            {
                foreach (var key in keys)
                {
                    key.enabled = true;
                }
                
                foreach (var key in primerOctavaKeys)
                {
                    key.enabled = true;
                }
            
            }
            if (_intentos > 0)

                _reto.enabled = true;


            if (_notasIngresadas.Count > 0){

                _borrar.enabled = true;
                _listo.enabled = true;
                _secuencia.enabled = true;

            }
        }
    }


    void keySounds() {

        keys[0].onClick.AddListener(() => playKey(0));
        keys[1].onClick.AddListener(() => playKey(1));
        keys[2].onClick.AddListener(() => playKey(2));
        keys[3].onClick.AddListener(() => playKey(3));
        keys[4].onClick.AddListener(() => playKey(4));
        keys[5].onClick.AddListener(() => playKey(5));
        keys[6].onClick.AddListener(() => playKey(6));
        keys[7].onClick.AddListener(() => playKey(7));
        keys[8].onClick.AddListener(() => playKey(8));
        keys[9].onClick.AddListener(() => playKey(9));
        keys[10].onClick.AddListener(() => playKey(10));
        keys[11].onClick.AddListener(() => playKey(11));
        keys[12].onClick.AddListener(() => playKey(12));
        keys[13].onClick.AddListener(() => playKey(13));
        keys[14].onClick.AddListener(() => playKey(14));
        keys[15].onClick.AddListener(() => playKey(15));
        keys[16].onClick.AddListener(() => playKey(16));
        keys[17].onClick.AddListener(() => playKey(17));
        keys[18].onClick.AddListener(() => playKey(18));
        keys[19].onClick.AddListener(() => playKey(19));
        keys[20].onClick.AddListener(() => playKey(20));
        keys[21].onClick.AddListener(() => playKey(21));
        keys[22].onClick.AddListener(() => playKey(22));
        keys[23].onClick.AddListener(() => playKey(23));

        primerOctavaKeys[0].onClick.AddListener(() => playKey(0));
        primerOctavaKeys[1].onClick.AddListener(() => playKey(1));
        primerOctavaKeys[2].onClick.AddListener(() => playKey(2));
        primerOctavaKeys[3].onClick.AddListener(() => playKey(3));
        primerOctavaKeys[4].onClick.AddListener(() => playKey(4));
        primerOctavaKeys[5].onClick.AddListener(() => playKey(5));
        primerOctavaKeys[6].onClick.AddListener(() => playKey(6));
        primerOctavaKeys[7].onClick.AddListener(() => playKey(7));
        primerOctavaKeys[8].onClick.AddListener(() => playKey(8));
        primerOctavaKeys[9].onClick.AddListener(() => playKey(9));
        primerOctavaKeys[10].onClick.AddListener(() => playKey(10));
        primerOctavaKeys[11].onClick.AddListener(() => playKey(11));

    }

    void playKey(int numberOfKey)
    {
        pianoAudioSource.PlayOneShot(Notes[numberOfKey]);
    }


    public void keysManager(List<string> generatedNotes) {

        foreach (KeyValuePair<string, Button> kvp_ in pianoKeys)
        {
            pianoKeys[kvp_.Key].onClick.AddListener(() => keyPressed(pianoKeys[kvp_.Key]));
        }
        foreach (KeyValuePair<string, Button> kvp_ in primerOctavaPiano)
        {
            primerOctavaPiano[kvp_.Key].onClick.AddListener(() => keyPressed(pianoKeys[kvp_.Key]));
        }

    }

    void resetColors() {
        foreach (KeyValuePair<string, Button> kvp_ in pianoKeys)
        {
            pianoKeys[kvp_.Key].image.color = Color.white;
        }
        foreach (KeyValuePair<string, Button> kvp_ in primerOctavaPiano)
        {
            primerOctavaPiano[kvp_.Key].image.color = Color.white;
        }
        
        primerOctavaPiano["DO#1"].image.color = Color.black;
        primerOctavaPiano["RE#1"].image.color = Color.black;
        primerOctavaPiano["FA#1"].image.color = Color.black;
        primerOctavaPiano["SOL#1"].image.color = Color.black;
        primerOctavaPiano["LA#1"].image.color = Color.black;

        pianoKeys["DO#1"].image.color = Color.black;
        pianoKeys["RE#1"].image.color = Color.black;
        pianoKeys["FA#1"].image.color = Color.black;
        pianoKeys["SOL#1"].image.color = Color.black;
        pianoKeys["LA#1"].image.color = Color.black;
        pianoKeys["DO#2"].image.color = Color.black;
        pianoKeys["RE#2"].image.color = Color.black;
        pianoKeys["FA#2"].image.color = Color.black;
        pianoKeys["SOL#2"].image.color = Color.black;
        pianoKeys["LA#2"].image.color = Color.black;

        EfectoAzul();

    }

    //Se muestra la tecla que se presiona de color verde por un momento
    IEnumerator EfectoVerde(Button button)
    {

        foreach (var key in keys)

            key.enabled = false;

            
        foreach (var key in primerOctavaKeys)
        {
            key.enabled = false;
        }

        button.image.color = Color.green;
        yield return new WaitForSeconds(0.10f);

        if (_notasIngresadas.Count > 0)

            EfectoAzul();

        yield return new WaitForSeconds(0.10f);

        if (_notasIngresadas.Count < _genNotes.Count){
            
            foreach (var key in keys)

                key.enabled = true;
           
            foreach (var key in primerOctavaKeys)
            {
                key.enabled = true;
            }

        }
          
    }

    //Efecto para teclas presionadas
    void EfectoAzul ()
    {
        if (_nivel > 5)

            foreach ( string _nota in _notasIngresadas)

                if (_listaNotas.Contains(_nota))
                
                    pianoKeys[_nota].image.color = Color.cyan;
                
                else
                
                    pianoKeys[_nota].image.color = Color.blue;
        else
            foreach ( string _nota in _notasIngresadas)

                if (_listaNotas.Contains(_nota))
                
                    primerOctavaPiano[_nota].image.color = Color.cyan;
                
                else
                
                    primerOctavaPiano[_nota].image.color = Color.blue;
    

        MuestraNumeroDeTeclas();
    }

    //Metodo para convertir lista de notas ingresadas por usuario
    //a texto
    private string ListToText(List<string> list)
    {
        string resultado = "";

        foreach (string miembro in list){

            char[] _arregloCadena = miembro.ToCharArray();
            _arregloCadena[1] = char.ToLower(_arregloCadena[1]);
            _arregloCadena[2] = char.ToLower(_arregloCadena[2]);
            string _cadena = new string(_arregloCadena);
            _cadena = _cadena.Replace('1', ' ');
            _cadena = _cadena.Replace( " ", "");
            resultado += _cadena + " ";


        }
        return resultado;
    }

    void keyPressed(Button button) {

        if (_notasIngresadas.Count < _genNotes.Count){
    
            StartCoroutine(EfectoVerde(button));
            _notasIngresadas.Add(button.tag);
            if (_notasIngresadas.Count >= _genNotes.Count)
            {
                
                foreach (var key in keys)
                {
                    key.enabled = false;
                }
                foreach (var key in primerOctavaKeys)
                {
                    key.enabled = false;
                }

            }

        }
        
        if (_nivel > 5){
                
            ListaDeNotas.text = ListToText(_notasIngresadas);
            ListaDeNotas.gameObject.SetActive(true);

        }
        else {

            primerOctavaNotas.text = ListToText(_notasIngresadas);
            primerOctavaNotas.gameObject.SetActive(true);
        }

        if (_notasIngresadas.Count > 0){

            _borrar.enabled = true;
            _listo.enabled = true;
            _secuencia.enabled = true;

        }

    }

    //Metodo para mostrar la secuencia al jugador
    IEnumerator ShowNextsequence() //List<string> generatedNotes
    {
        _reto.enabled = false;
        _borrar.enabled = false;
        _listo.enabled = false;
        _secuencia.enabled = false;
        
        if (_nextKeyIndex == 0) {
                
            if (_nivel > 5)         
                ListaDeNotas.gameObject.SetActive(false);

            else
                primerOctavaNotas.gameObject.SetActive(false);

            foreach (var key in keys)
            {
                key.enabled = false;
            }
            foreach (var key in primerOctavaKeys)
            {
                key.enabled = false;
            }

            warning.text = "¡ATENCION!";
            warning.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            warning.gameObject.SetActive(false);

            if (_nivel > 5){
                
                ListaDeNotas.text = ListToText(_genNotes);
                ListaDeNotas.gameObject.SetActive(true);

            }
            else{

                primerOctavaNotas.text = ListToText(_genNotes);
                primerOctavaNotas.gameObject.SetActive(true);

            }
        }
        
        //pianoKeys[genNotes[transitionNextKeyIndex]].image.color = Color.green;
        if (_nivel > 5)
            pianoKeys[_genNotes[_nextKeyIndex]].image.color = Color.yellow;
        
        else
            primerOctavaPiano[_genNotes[_nextKeyIndex]].image.color = Color.yellow;

        pianoAudioSource.PlayOneShot(keysAndSounds[_genNotes[_nextKeyIndex]]);
        _transitionNextKeyIndex = _nextKeyIndex;
        yield return new WaitForSeconds(1f);
        resetColors();
        _nextKeyIndex++;

        
        if (_nextKeyIndex < _genNotes.Count)
        {
            StartCoroutine(ShowNextsequence());
        } 
        
        else
        {
            _transitionNextKeyIndex = 0;
            _nextKeyIndex = 0;
            if (_nivel > 5)
                foreach(string _Tecla in _genNotes)
                {
                    pianoKeys[_Tecla].image.color = Color.yellow;
                } 
            else 
                foreach(string _Tecla in _genNotes)
                {
                    primerOctavaPiano[_Tecla].image.color = Color.yellow;
                }  
            if (_nivel != 1 && _nivel != 2 && _nivel != 4)      
            {
                
                yield return new WaitForSeconds(1f);
                resetColors();

            }    
            
            else 
                
                EfectoAzul();
            
            warning.text = "¡COMIENZA!";
            warning.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            warning.gameObject.SetActive(false);
    
            
            foreach (var key in keys)
            {
                key.enabled = true;
            }
            
            foreach (var key in primerOctavaKeys)
            {
                key.enabled = true;
            }
            
            if (_intentos > 0)

                _reto.enabled = true;


            if (_notasIngresadas.Count > 0){

                _borrar.enabled = true;
                _listo.enabled = true;
                _secuencia.enabled = true; 

                if (_nivel > 5){
                    
                    ListaDeNotas.text = ListToText(_notasIngresadas);
                    ListaDeNotas.gameObject.SetActive(true);

                }
                else{

                    primerOctavaNotas.text = ListToText(_notasIngresadas);
                    primerOctavaNotas.gameObject.SetActive(true);

                }

            }
            else{

                ListaDeNotas.gameObject.SetActive(false);
                primerOctavaNotas.gameObject.SetActive(false);

            }

        }
    }
        public void GuardaPartida(bool victoria, bool agoto_tiempo)
        {
            Partida p = new Partida();
            p.nivel = _nivel;
            p.juego = "Pianorama";
            p.fecha = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            p.victoria = victoria;
            p.agoto_tiempo = agoto_tiempo;
            StartCoroutine(AduanaCITAN.SubePartidaA_CITAN(p));
        }

        public void ActualizaNeurocoins(int coins)
        {
            StartCoroutine(AduanaCITAN.ActualizaNeurocoins_CITAN(coins));
        }
    }
}