using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Ship;

public class GameMaganer : MonoBehaviour
{

    public Dictionary<Ship.Faction, float> morale;
    private int maxTentacles = 8;

    public AudioClip BGMLoop;
    public AudioClip victoryJingle;
    public AudioClip defeatJingle;
    private AudioSource audioSource;

    public GameObject victoryUI;
    public GameObject defeatUI;
    public TMP_Text defeatText;

    private bool partidaTerminada = false;

    void Start() {

        partidaTerminada = false;
        Time.timeScale = 1;
        morale = new()
        {
            { Faction.Black,   50 },
            { Faction.Red,     50 },
            { Faction.Green,   50 },
            { Faction.Blue,    50 },
            { Faction.Yellow,  50 }
        };

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BGMLoop;
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        if (!partidaTerminada && CheckVictory()) Victory();
    }

    public void ReduceMorale(Faction faction) {

        morale[faction]-=10;
        foreach (var key in morale.Keys.ToList())
        {
            if(key != faction && morale[key] > 0) {
                morale[key] += 2;
            }
        }
    }

    public float GetKrakenHealth() {
        return GameObject.FindFirstObjectByType<Kraken>().health;
    }

    public bool CanKrakenAttack() {
        return GameObject.FindGameObjectsWithTag("Tentacle").Length < maxTentacles;
    }
    public bool CheckVictory() {

        foreach (var faction in morale.Values) {
            if (faction > 0) return false;
        }
        partidaTerminada = true;
        return true;
    }

    private void Victory() {
        Time.timeScale = 0;
        victoryUI.SetActive(true);
        print("VICTORY");

        audioSource.Pause();
        audioSource.clip = victoryJingle;
        audioSource.loop = false;
        audioSource.volume = 0.15f;
        audioSource.Play();
    }

    public void Defeat() {
        Time.timeScale = 0;
        string factionHighest = "";
        switch (morale.Aggregate((x, y) => x.Value > y.Value ? x : y).Key) {
            case Faction.Black:
                factionHighest = "Piratas Negros";
                    break;
            case Faction.Red:
                factionHighest = "Templarios Rojos";
                    break;
            case Faction.Green:
                factionHighest = "Acolitos Verdes";
                    break;
            case Faction.Blue:
                factionHighest = "Rompeolas Azules";
                    break;
            case Faction.Yellow:
                factionHighest = "Guerrilleros Amarillos";
                    break;
        }
        defeatText.text = defeatText.text.Replace("{faction}", factionHighest);
        defeatUI.SetActive(true);
        print("GAME OVER");
        partidaTerminada = true;


        audioSource.Pause();
        audioSource.clip = defeatJingle;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayAgain() {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void MainMenu() {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
}
