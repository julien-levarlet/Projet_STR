using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

/// <summary>
/// Classe charg�e de v�rifier que les diff�rentes contraites temps r�el sont bien respect�es
/// Cette est un singleton, il ne peut exister qu'une seule fois dans la scene
/// Pour enregistrer les valeurs sur un fichier il faut appuyer sur S
/// </summary>
public class TestsManager : MonoBehaviour
{
	// affichage des fps
	public TextMeshProUGUI fpsText;
	
	private const int NbValuesStored = 1000;

	// V�rifiaction de la contraite de nombre d'images par seconde
	private int _fpsIndex;
	private float[] _fpsValues; // tableaux des valeurs de temps entre deux images mesur�s (parcours de fa�on cyclique)
	
	// V�rification de la contrainte de temps de r�action
	private int _reactionIndex;
	private float _beginReationTime=0f;
	private float[] _reactionValues;
	
	// V�rification de la contrainte de temps de d�tection
	private int _detectionIndex;
	private float _beginDetectionTime=0f;
	private float[] _detectionValues;

	[SerializeField] private const string LogFile = "log.csv";
	
	// instance du singleton
	private static TestsManager _instance;

	private void Start()
	{
		_fpsValues = new float[NbValuesStored];
		_reactionValues = new float[NbValuesStored];
		_detectionValues = new float[NbValuesStored];
		_fpsIndex = -1;
		Application.targetFrameRate = 61;
	}

	private void Awake()  // � l'instanciation 
	{
		if (_instance == null) // si on est la premi�re instance on l'affecte � _instance
		{
			_instance = this;
			return;
		}
		// si une instance est d�j� existante, cet objet n'est pas valide, il faut le d�truire
		Debug.LogError("Une instance de TestManager est d�j� existante");
		Destroy(gameObject);
	}
	
	private void Update()
	{
		_fpsIndex = (_fpsIndex + 1) % NbValuesStored;
		_fpsValues[_fpsIndex] = Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.M)) // appuie sur S pour enregistrer les mesures faites
		{
			SaveValues();
		}
	}

	private void OnGUI()
	{
		fpsText.text = Mathf.RoundToInt(1/_fpsValues[_fpsIndex]) + "FPS"; // affichage du nb de fps
	}

	/// <summary>
	/// A appeler quand le joueur entre dans le rayon d'action de l'ennemi
	/// </summary>
	private void BeginDetection()
	{
		_beginDetectionTime = Time.time;
	}

	/// <summary>
	/// A appeler quand l'ennemi � d�tect� le joueur
	/// </summary>
	private void EndDetection()
	{
		var detectionTime = Time.time - _beginDetectionTime;
		_detectionIndex = (_detectionIndex + 1) % NbValuesStored;
		_detectionValues[_detectionIndex] = detectionTime;
	}
	
	/// <summary>
	/// A appeler quand le joueur fait une action
	/// </summary>
	private void BeginReaction()
	{
		_beginReationTime = Time.time;
	}

	/// <summary>
	/// A appeler quand l'ennemi r�agit � l'action du joueur
	/// </summary>
	private void EndReaction()
	{
		var reactionTime = Time.time - _beginReationTime;
		_reactionIndex = (_reactionIndex + 1) % NbValuesStored;
		_reactionValues[_reactionIndex] = reactionTime;
	}

	/// <summary>
	/// Enregistrement des mesures faites dans le fichier indiqu� dans LogFile.
	/// Remarque : appeler cette fonction risque d'affecter les performances (�criture en m�moire)
	/// </summary>
	private void SaveValues()
	{
		using var sw = new StreamWriter(LogFile, false);
		for (int i = 0; i < NbValuesStored; ++i)
		{
			sw.WriteLine(
				_fpsValues[i].ToString(CultureInfo.InvariantCulture) + ", " +
				_reactionValues[i].ToString(CultureInfo.InvariantCulture) + ", " + 
				_detectionValues[i].ToString(CultureInfo.InvariantCulture));
		}
		Debug.Log("Valeurs enregistr�es");
	}
}