using UnityEngine;
using UnityEngine.UI;

public class UpdateSliders : MonoBehaviour
{
    [SerializeField] Ship.Faction faction;

    GameMaganer gameManager;

    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameMaganer>();
    }


    void Update()
    {
        if(faction == Ship.Faction.Lenght)
            GetComponent<Slider>().value = gameManager.GetKrakenHealth();
        else
            GetComponent<Slider>().value = gameManager.morale[faction];
    }
}
