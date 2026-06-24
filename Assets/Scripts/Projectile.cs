using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileDef data;
    private Vector2 direction;
    private float life;

    private TrailRenderer trail;

    public void Init(ProjectileDef def, Vector2 dir)
    {
        data = def;
        direction = dir.normalized;
        life = def.lifeTime;

        trail = GetComponent<TrailRenderer>();
        Debug.Log("Projectile spawned");

        if (trail != null)
        {
            trail.startColor = data.tracerColor;
            trail.endColor = new Color(
                data.tracerColor.r,
                data.tracerColor.g,
                data.tracerColor.b,
                0f);
        }
    }

    void Update()
    {
        transform.position += (Vector3)(direction * data.velocity * Time.deltaTime);

        life -= Time.deltaTime;
        if (life <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Hit: {other.name}");

        if (other.TryGetComponent(out ComponentHurtBox hurtBox))
        {
            hurtBox.TakeDamage(data.damage, transform.position);
            Destroy(gameObject);
        }
    }
}