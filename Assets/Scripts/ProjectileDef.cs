using UnityEngine;

public enum DamageType
{
    Inert,    
    Explosive
}

[CreateAssetMenu(fileName = "ProjectileDef", menuName = "Scriptable Objects/ProjectileDef")]
public class ProjectileDef : ScriptableObject
{
    public string displayName;
    public DamageType damageType;
    public float velocity = 150f;
    public float damage = 25f;
    public float armorPenetration;
    public Color tracerColor;
    public float lifeTime = 10.0f;
    [Header("Prefab")]
    public Projectile prefab;
}
