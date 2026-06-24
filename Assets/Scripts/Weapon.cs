using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private ProjectileDef projectile;
    [SerializeField] private Transform emitter;
    [SerializeField] private float fireRate = 0.5f;

    private float nextFireTime;

    private void Awake()
    {
        if (projectile == null)
            Debug.LogError($"{name}: ProjectileDef missing");

        if (emitter == null)
            Debug.LogError($"{name}: Emitter missing");

        if (projectile.prefab == null)
            Debug.LogError($"{name}: Projectile prefab missing in ProjectileDef");
    }
    public void Fire(Vector2 direction)
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireRate;

        if (projectile == null || projectile.prefab == null || emitter == null)
            return;

        Projectile proj = Instantiate(projectile.prefab, emitter.position, emitter.rotation);
        proj.Init(projectile, direction);
    }
}
