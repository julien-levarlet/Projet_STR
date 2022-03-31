/*
------------------------------------------------------------------
  This file is part of UnitySharpNEAT 
  Copyright 2020, Florian Wolf
  https://github.com/flo-wolf/UnitySharpNEAT
------------------------------------------------------------------
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySharpNEAT
{
    public class NeatUI : MonoBehaviour
    {
        [SerializeField] 
        private NeatSupervisorCar neatSupervisorCar;

        /// <summary>
        /// Display simple Onscreen buttons for quickly accessing ceratain lifecycle funtions and to display generation info.
        /// </summary>
        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 110, 40), "Start EA"))
            {
                neatSupervisorCar.StartEvolution();
            }
            if (GUI.Button(new Rect(10, 60, 110, 40), "Stop + save EA"))
            {
                neatSupervisorCar.StopEvolution();
            }
            if (GUI.Button(new Rect(10, 110, 110, 40), "Run best"))
            {
                neatSupervisorCar.RunBest();
            }
            if (GUI.Button(new Rect(10, 160, 110, 40), "Delete Saves"))
            {
                ExperimentIO.DeleteAllSaveFiles(neatSupervisorCar.ExperimentCar);
            }

            GUI.Button(new Rect(10, Screen.height - 70, 110, 60), string.Format("Generation: {0}\nFitness: {1:0.00}", neatSupervisorCar.CurrentGeneration, neatSupervisorCar.CurrentBestFitness));
        }
    }
}
