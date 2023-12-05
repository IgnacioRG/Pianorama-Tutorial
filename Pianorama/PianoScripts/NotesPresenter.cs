using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

namespace JuegoPiano{
public class NotesPresenter : MonoBehaviour
{
    public List<string> randomGeneratedNotes;

    public KeysController kc; 

    System.Random _numeroAleatorio = new System.Random();

    int _indiceDeSeleccion;

    string[] _notesArray1 = new string[] { "DO 1", "RE 1", "MI 1", "FA 1", "SOL 1", "LA 1", "SI 1" };
    string[] _bemolArray1 = new string[] { "DO#1", "RE#1", "FA#1", "SOL#1", "LA#1" };
    string[] _notesArray2 = new string[] { "DO 2", "RE 2", "MI 2", "FA 2", "SOL 2", "LA 2", "SI 2" };
    string[] _bemolArray2 = new string[] { "DO#2", "RE#2", "FA#2", "SOL#2", "LA#2" };

    string[] _completeNotesArray, _bemolArray, _notesAndBemolArray, _primerOctava;


    public string[] getNotesArray
    {
        get{ return _completeNotesArray; }
    }

    
    public string[] getNotesandBemolArray
    {
        get{ return _notesAndBemolArray; }
    }

    public string[] getPrimerOctava
    {
        get{ return _primerOctava; }
    }

    public List<string> generatedNotes = new List<string>();

    private List<string> _listaDeSeleccion = new List<string>();

    void Awake()
    {
        _primerOctava = new string[_notesArray1.Length + _bemolArray1.Length];
        _notesArray1.CopyTo(_primerOctava, 0);
        _bemolArray1.CopyTo(_primerOctava, _notesArray1.Length);
        
        _completeNotesArray = new string[_notesArray1.Length + _notesArray2.Length];
        _notesArray1.CopyTo(_completeNotesArray, 0);
        _notesArray2.CopyTo(_completeNotesArray, _notesArray1.Length);

        _bemolArray = new string[_bemolArray1.Length + _bemolArray2.Length];
        _bemolArray1.CopyTo(_bemolArray, 0);
        _bemolArray2.CopyTo(_bemolArray, _bemolArray1.Length);

        _notesAndBemolArray = new string[_completeNotesArray.Length + _bemolArray.Length];
        _completeNotesArray.CopyTo(_notesAndBemolArray, 0);
        _bemolArray.CopyTo(_notesAndBemolArray, _completeNotesArray.Length);
        
        NotesPerLevelOneToFive(2);


    }

    // Start is called before the first frame update
    void Start()
    {
        kc.keysManager(generatedNotes);
    }

    public void NotesPerLevelOneToFive(int _numeroDeNotas)
    {
        
        _listaDeSeleccion = new List<string>(_primerOctava);
        generatedNotes.Clear();

        for (int _repeticiones = 0; _repeticiones < _numeroDeNotas; _repeticiones++){

            
            int _indiceDeSeleccion = _numeroAleatorio.Next(0, _listaDeSeleccion.Count);
            generatedNotes.Add(_listaDeSeleccion[_indiceDeSeleccion]);
            _listaDeSeleccion.RemoveAt(_indiceDeSeleccion);
            
        }

    }
    
    public void NotesPerLevelSixToTen(int _numeroDeNotas)
    {
       
        _listaDeSeleccion = new List<string>(_notesAndBemolArray);
        generatedNotes.Clear();

        for (int _repeticiones = 0; _repeticiones < _numeroDeNotas; _repeticiones++){

            
            int _indiceDeSeleccion = _numeroAleatorio.Next(0, _listaDeSeleccion.Count);
            if (generatedNotes.Contains(_listaDeSeleccion[_indiceDeSeleccion]))
            {
                
                generatedNotes.Add(_listaDeSeleccion[_indiceDeSeleccion]);
                _listaDeSeleccion.RemoveAt(_indiceDeSeleccion);

            }
            else

                generatedNotes.Add(_listaDeSeleccion[_indiceDeSeleccion]);
            
        }
    }


}
}


