using System;
using UnityEngine;
using UnityEngine.AI;

public class Ship : MonoBehaviour
{
    public enum Faction {
        Black,
        Red,
        Green,
        Blue,
        Yellow,

        Lenght
    }
    public Faction faction;
    GameMaganer gameManager;

    [SerializeField] private GameObject bulletPrefab;
    public bool canAttack = false;
    public float cooldown = 2f;
    public float bulletSpeed = 20f;
    public float bulletDamage = 2f;
    public float meleeDamage = 10f;

    public bool hasShield = false; 
    public bool hadShield = false; 
    

    private float timeToNextAttack = 0f;

    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameMaganer>();
    }


    void Update()
    {
        UpdateStats();

        if (canAttack && bulletDamage > 0 && timeToNextAttack <= Time.time) {

            GameObject bullet = (GameObject)Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().speed = bulletSpeed;
            bullet.GetComponent<Bullet>().damage = bulletDamage;
            bullet.GetComponent<Bullet>().target = GameObject.FindFirstObjectByType<Kraken>().gameObject.transform;
            timeToNextAttack = Time.time + cooldown;
        }

        //GetComponent<SpriteRenderer>().color = hasShield ? Color.cyan : Color.white;
        GetComponentsInChildren<SpriteRenderer>()[1].enabled = hasShield;
    }

    private void UpdateStats() {
        switch (faction) {
            case Faction.Black: 
                UpdateStatsBlack();
                break;
            case Faction.Red: 
                UpdateStatsRed();
                break;
            case Faction.Green: 
                UpdateStatsGreen();
                break;
            case Faction.Blue: 
                UpdateStatsBlue();
                break;
            case Faction.Yellow: 
                UpdateStatsYellow();
                break;

        }
    }

    private void UpdateStatsBlack() {
        var morale = Mathf.InverseLerp(0, 100, gameManager.morale[Faction.Black]);
        cooldown = Mathf.Lerp(0.5f, 3.5f, morale);
    }

    private void UpdateStatsRed() {
        var morale = Mathf.InverseLerp(0, 100, gameManager.morale[Faction.Red]);
        bulletDamage = Mathf.Lerp(1.5f, 10f, morale);
    }

    private void UpdateStatsGreen() {
        if (hadShield) return;
        if (gameManager.morale[Faction.Green] < 50) {
            hasShield = false;
        }
        else {
            hadShield = true;
            hasShield = true;
        }

    }

    private void UpdateStatsBlue() {
        var morale = Mathf.InverseLerp(0, 100, gameManager.morale[Faction.Blue]);
        gameObject.GetComponent<NavMeshAgent>().speed = Mathf.Lerp(30, 150, morale);
        gameObject.GetComponent<NavMeshAgent>().acceleration = Mathf.Lerp(5, 15, morale);

    }

    private void UpdateStatsYellow() {
        if (gameManager.morale[Faction.Yellow] >= 50) canAttack = true;
    }
}
