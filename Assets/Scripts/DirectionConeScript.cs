using UnityEngine;
using UnityEngine.Animations.Rigging;

public class DirectionConeScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D shipRb; // Reference to the Player's Rigidbody2D
    private SpriteRenderer coneRenderer;

    private float maxOffset = 10.0f;
    private float fadeSpeed = 10.0f;

    void Update()
    {
        if (shipRb == null || coneRenderer == null)
            return;

        if (shipRb.linearVelocity.magnitude > 0.01f)
        {
            float speed = shipRb.linearVelocity.magnitude;
            Vector2 dir = shipRb.linearVelocity.normalized;

            // Rotate cone
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Position cone ahead of vessel
            float offset = Mathf.Lerp(0f, maxOffset, speed / fadeSpeed);
            transform.position = shipRb.position + dir * offset;
        }

        float alpha = Mathf.InverseLerp(10f, 20f, shipRb.linearVelocity.magnitude);

        Color c = coneRenderer.color;
        c.a = alpha;
        coneRenderer.color = c;


    }
}
