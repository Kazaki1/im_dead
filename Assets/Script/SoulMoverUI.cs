using UnityEngine;
using UnityEngine.UI;

public class SoulMoverUI : MonoBehaviour
{
    public RectTransform soul;          // Soul object (UI)
    public RectTransform battleBoxUI;   // Battle box image (UI)
    public float moveSpeed = 200f;      // Pixels per second

    private Vector2 minBounds;
    private Vector2 maxBounds;

    void Start()
    {
        // Lấy biên giới hạn trong khung
        Vector3[] corners = new Vector3[4];
        battleBoxUI.GetWorldCorners(corners);
        minBounds = corners[0]; // bottom-left
        maxBounds = corners[2]; // top-right
    }

    void Update()
    {
        Vector2 move = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ) * moveSpeed * Time.deltaTime;

        Vector3 newPos = soul.position + (Vector3)move;

        // Clamp giới hạn trong battleBox
        newPos.x = Mathf.Clamp(newPos.x, minBounds.x, maxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y, maxBounds.y);

        soul.position = newPos;
    }
}