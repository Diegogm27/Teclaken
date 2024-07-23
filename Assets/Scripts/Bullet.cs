using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20;
    public float damage = 2f;
    public Transform target;
    private Vector2 dir;
    void Start()
    {
        dir = target.position - transform.position;
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
