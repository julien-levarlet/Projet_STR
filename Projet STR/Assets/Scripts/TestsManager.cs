using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
	
	private const int NbValuesStored = 1000;
	private const float Epsilon = 0.01f;

	// Vérifiaction de la contraite de nombre d'images par seconde
	private int _fpsIndex;
	private float[] _fpsValues; // tableaux des valeurs de temps entre deux images mesurés (parcours de façon cyclique)
	
	// Vérification de la contrainte de temps de réaction
	private int _reactionIndex;
	private float _beginReationTime=0f;
	private float[] _reactionValues;
	
	// Vérification de la contrainte de temps de détection
	private int _detectionIndex;
	private float _beginDetectionTime=0f;
	private float[] _detectionValues;

	[SerializeField] private const string LogFile = "log.csv";
	
	// instance du singleton
	private static TestsManager _instance;

	public static TestsManager GetInstance()
	{
		return _instance;
	}

	private void Start()
	{
		_fpsValues = new float[NbValuesStored];
		_reactionValues = new float[NbValuesStored];
		_detectionValues = new float[NbValuesStored];
		_fpsIndex = -1;
		Application.targetFrameRate = 60;
	}

	private void Awake()  // à l'instanciation 
	{
		if (_instance == null) // si on est la première instance on l'affecte à _instance
		{
			_instance = this;
			return;
		}
		// si une instance est déjà existante, cet objet n'est pas valide, il faut le détruire
		Debug.LogError("Une instance de TestManager est déjà existante");
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
	public void BeginDetection()
	{
		if (!IsZero(_beginDetectionTime))
			return;
		//Debug.Log("begindetection");
		_beginDetectionTime = Time.time;
	}

	/// <summary>
	/// A appeler quand l'ennemi à détecté le joueur
	/// </summary>
	public void EndDetection()
	{
		if (IsZero(_beginDetectionTime))
			return;
		//Debug.Log("enddetection");
		var detectionTime = Time.time - _beginDetectionTime;
		_detectionIndex = (_detectionIndex + 1) % NbValuesStored;
		_detectionValues[_detectionIndex] = detectionTime;
		_beginDetectionTime = 0;
	}
	
	/// <summary>
	/// A appeler quand le joueur fait une action
	/// </summary>
	public void BeginReaction()
	{
		//Debug.Log("Beginreaction");
		_beginReationTime = Time.time;
	}

	/// <summary>
	/// A appeler quand l'ennemi réagit à l'action du joueur
	/// </summary>
	public void EndReaction()
	{
		if (IsZero(_beginReationTime))
			return;
		//Debug.Log("endreaction");
		var reactionTime = Time.time - _beginReationTime;
		_reactionIndex = (_reactionIndex + 1) % NbValuesStored;
		_reactionValues[_reactionIndex] = reactionTime;
	}

	/// <summary>
	/// Enregistrement des mesures faites dans le fichier indiqué dans LogFile.
	/// Remarque : appeler cette fonction risque d'affecter les performances (écriture en mémoire)
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
		Debug.Log("Valeurs enregistrées");
	}

	public static bool IsZero(float value)
	{
		if (value < Epsilon && value > -Epsilon)
			return true;
		return false;
	}
}