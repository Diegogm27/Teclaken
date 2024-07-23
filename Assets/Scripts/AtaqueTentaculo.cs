using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AtaqueTentaculo : MonoBehaviour
{
    [SerializeField] private KeyCode keybind;
    [SerializeField] private GameObject tentaclePrefab;
    [SerializeField] private float cooldown = 5f;

    GameMaganer gameManager;

    public AudioClip sound;

    private List<GameObject> shipsInside = new List<GameObject>();
    private float timeToNextAttack = 0f;
    void Start()
    {
        Assert.AreNotEqual(keybind, KeyCode.None);
        gameManager = Object.FindFirstObjectByType<GameMaganer>();
    }

    void Update()
    {
        if (timeToNextAttack > Time.time) {
            var iLerp = Mathf.InverseLerp(0, cooldown, timeToNextAttack - Time.time);
            var lerp = Mathf.Lerp(0, .5f, iLerp);
            var sprite = GetComponent<SpriteRenderer>();
            var color = sprite.color;
            color.a = lerp;
            sprite.color = color;
        }

        else if (gameManager.CanKrakenAttack() &&  Input.GetKeyDown(keybind)) {

            GameObject tentacle = (GameObject)Instantiate(tentaclePrefab);
            tentacle.transform.position = transform.position;
            AudioSource.PlayClipAtPoint(sound, transform.position);
            Destroy(tentacle, tentacle.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length + 1f);

            for (var i = shipsInside.Count -1; i >= 0; i--)
            {
                if (shipsInside[i].GetComponent<Ship>().hasShield) {
                    shipsInside[i].GetComponent<Ship>().hasShield = false;
                }
                else {
                    gameManager.ReduceMorale(shipsInside[i].GetComponent<Ship>().faction);
                    Destroy(shipsInside[i]);
                }
                
            }

            timeToNextAttack = Time.time + cooldown;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (!collision.gameObject.GetComponent<Ship>()) return;
        if (shipsInside.Contains(collision.gameObject)) return;

        shipsInside.Add(collision.gameObject);

    }

    private void OnTriggerExit2D(Collider2D collision) {

        if (!collision.gameObject.GetComponent<Ship>()) return;
        if (!shipsInside.Contains(collision.gameObject)) return;

        shipsInside.Remove(collision.gameObject);
    }
}
