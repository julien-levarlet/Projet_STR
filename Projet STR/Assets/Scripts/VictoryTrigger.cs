using System;
using UnityEngine;

public class VictoryTrigger :MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
            gameManager.PlayerWon();
    }
}