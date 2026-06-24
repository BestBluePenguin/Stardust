using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float rotationSpeed = 30f;

    private Vector2 direction;

    [SerializeField] private ComponentHurtBox hurtBox;
    [Header("Explosion")]
    [SerializeField] private GameObject explosionPrefab;


    private void Start()
    {
        direction = Random.insideUnitCircle.normalized;
    }

    private void Awake()
    {
        if (hurtBox != null)
            hurtBox.OnDestroyed += HandleDestroyed;
    }


    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
    private void HandleDamaged(ComponentHurtBox box, float damage)
    {
        Debug.Log($"💥 Asteroid [{name}] took {damage} damage | HP: {box.CurrentHP}/{box.MaxHP}");
    }

    private void HandleDestroyed(ComponentHurtBox box)
    {
        Debug.Log($"Asteroid [{name}] DESTROYED");
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (hurtBox != null)
            hurtBox.OnDestroyed -= HandleDestroyed;
    }
}