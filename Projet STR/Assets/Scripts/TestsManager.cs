using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Classe charg�e de v�rifier que les diff�rentes contraites temps r�el sont bien respect�es
/// </summary>
public class TestsManager : MonoBehaviour
{
	public TextMeshProUGUI FpsText;

	private float pollingTime = 1f;
	private float temps;
	private int nbImages;

	void Update()
	{
		// affichage du taux d'images par seconde
		temps += Time.deltaTime;
		nbImages++;

		if (temps >= pollingTime)
        {
			int frameRate = Mathf.RoundToInt(nbImages / temps);
			FpsText.text = frameRate.ToString() + " SPF";
			temps -= pollingTime;
			nbImages = 0;
        }
	}

}