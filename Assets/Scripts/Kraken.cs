using UnityEngine;
using static Ship;

public class Kraken : MonoBehaviour
{

    [SerializeField] public float health = 1000;
    [SerializeField] GameObject explosion;
    public AudioClip explosionSound;

    GameMaganer gameManager;
    Animator animator;
    void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameMaganer>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        
    }

    void Damage(float value) {

        health -= value;
        animator.SetFloat("Health", health);
        if (health <= 0) {
            gameManager.Defeat();

        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        var ship = collision.gameObject.GetComponent<Ship>();
        var bullet = collision.gameObject.GetComponent<Bullet>();
        if (ship && ship.meleeDamage > 0) {
            Damage(ship.meleeDamage);
            GameObject spawnedExplosion = (GameObject)Instantiate(explosion, collision.transform.position, Quaternion.identity);
            spawnedExplosion.transform.localScale *= 2;        
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, .7f);
            Destroy(spawnedExplosion, spawnedExplosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
        else if (bullet) {
            Damage(bullet.damage);
            GameObject spawnedExplosion = (GameObject)Instantiate(explosion, collision.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, .5f);
            Destroy(spawnedExplosion, spawnedExplosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            Destroy(collision.gameObject);
        }

    }
}
