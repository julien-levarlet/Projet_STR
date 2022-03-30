using System;
using UnityEngine;

public class VictoryTrigger :MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
            GameManager.GetInstance().PlayerWon();
    }
}