using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction;

    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > 20 || Mathf.Abs(transform.position.y) > 20)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player trúng đạn!");
            Destroy(gameObject);
        }
    }   
    void OnBecameInvisible()
    {
        Destroy(gameObject); // tự hủy khi ra khỏi màn hình
    }
}
