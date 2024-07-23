using System.Collections.Generic;
using UnityEngine;
using static Ship;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] shipPrefabs;
    [SerializeField] Transform kraken;

    private Vector2[] spawnPoints;
    [SerializeField] private float maxShips = 5;

    GameMaganer gameManager;
    void Start() {

        gameManager = Object.FindFirstObjectByType<GameMaganer>();
        spawnPoints = GetComponent<EdgeCollider2D>().points;
    }


    void Update()
    {
        if (!gameManager.CheckVictory() && Object.FindObjectsByType<Ship>(FindObjectsSortMode.None).Length < maxShips) CreateShip();
    }

    private void CreateShip() {

        var rng = Random.Range(0, spawnPoints.Length);
        Vector2 randomPoint = spawnPoints[rng];

        Faction faction;
        do {
            faction = (Faction)Random.Range(0, (int)Faction.Lenght);
        } while (gameManager.morale[faction] <= 0);



        GameObject spawnedShip = (GameObject)Instantiate(shipPrefabs[(int)faction], randomPoint, Quaternion.identity);
        spawnedShip.GetComponent<AgentMovement>().kraken = kraken;
    }
}
