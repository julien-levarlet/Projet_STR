using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

/// <summary>
/// Classe chargée de vérifier que les différentes contraites temps réel sont bien respectées
/// Cette est un singleton, il ne peut exister qu'une seule fois dans la scene
/// Pour enregistrer les valeurs sur un fichier il faut appuyer sur S
/// </summary>
public class TestsManager : MonoBehaviour
{
	// affichage des fps
	public TextMeshProUGUI fpsText;

	// stockage des valeurs des contraintes 
	private const int NbValuesStored = 1000;
	private int _fpsIndex;
	private float[] _fpsValues; // tableaux des valeurs de temps entre deux images mesurés (parcours de façon cyclique)
	
	// instance du singleton
	private static TestsManager _instance;

	private void Start()
	{
		_fpsValues = new float[1000];
		_fpsIndex = -1;
		Application.targetFrameRate = 61;
	}

	void Awake()  // à l'instanciation 
	{
		if (_instance == null) // si on est la première instance on l'affecte à _instance
		{
			_instance = this;
			return;
		}
		// si une instance est déjà existante, cet objet n'est pas valide, il faut le détruire
		Destroy(gameObject);
	}
	
	void Update()
	{
		_fpsIndex = (_fpsIndex + 1) % NbValuesStored;
		_fpsValues[_fpsIndex] = Time.deltaTime;
		if (Input.GetKey(KeyCode.S)) // appuie sur S pour enregistrer les constantes
		{
			SaveValues();
		}
	}

	private void OnGUI()
	{
		fpsText.text = Mathf.RoundToInt(1/_fpsValues[_fpsIndex]) + "FPS"; // affichage du nb de fps
	}

	private static void SaveValues()
	{
		
	}
}