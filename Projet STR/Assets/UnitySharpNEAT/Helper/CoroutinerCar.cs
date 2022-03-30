/*
------------------------------------------------------------------
  This file is part of UnitySharpNEAT 
  Copyright 2020, Florian Wolf
  https://github.com/flo-wolf/UnitySharpNEAT
------------------------------------------------------------------
*/
using UnityEngine;
using System.Collections;

namespace UnitySharpNEAT
{
    /// <summary>
    /// Classes that do not inherit from MonoBehaviour, or static 
    /// functions within MonoBehaviours are inertly unable to 
    /// call StartCoroutine. This Class creates a MonoBehaviour proxy, 
    /// which is used by a static reference to start Coroutines from anywhere.
    /// </summary>
    public class CoroutinerCar
    {
        public const bool DONT_DESTROY_ON_LOAD = true;
        public static CoroutinerInstanceCar Instance;

        public static Coroutine StartCoroutine(IEnumerator iterationResult)
        {
            if (Instance == null)
            {
                // Create an instance, in case there is none to Start the Coroutine from
                GameObject routineHandlerGo = new GameObject("Coroutiner");
                Instance = routineHandlerGo.AddComponent(typeof(CoroutinerInstanceCar)) as CoroutinerInstanceCar;

                if (DONT_DESTROY_ON_LOAD)
                    GameObject.DontDestroyOnLoad(routineHandlerGo);
            }

            return Instance.ProcessWork(iterationResult);
        }
    }

    public class CoroutinerInstanceCar : MonoBehaviour
    {
        public Coroutine ProcessWork(IEnumerator iterationResult)
        {
            return StartCoroutine(iterationResult);
        }
    }
}

