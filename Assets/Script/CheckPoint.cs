using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
            {
                player.SetCheckpoint(transform.position);
                activated = true;

                // ✅ Tuỳ chọn: thay đổi màu hoặc hiệu ứng
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.color = Color.green;

                Debug.Log("Checkpoint triggered and saved!");
            }
        }
    }
}
