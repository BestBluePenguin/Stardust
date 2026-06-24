using System;
using UnityEngine;

public enum ComponentType
{
    hull,
    engine,
    thruster,
    fuel,
    reactor,
    weapon,
    crew,
    astroid,
    other
}

public class ComponentHurtBox : MonoBehaviour
{
    [SerializeField] private ComponentType componentType;
    [SerializeField] private string componentID;

    [SerializeField] private float damageMultiplier = 1f;
    [SerializeField] private float maxHP = 100f;


    private float currentHP;
    private bool destroyed;

    public ComponentType ComponentType => componentType;
    public string ComponentID => string.IsNullOrEmpty(componentID) ? gameObject.name : componentID;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    public bool IsDestroyed => destroyed;
    public float HealthNormalized => maxHP <= 0f ? 0f : currentHP / maxHP;


    public event Action<ComponentHurtBox> OnDestroyed;
    public event Action<ComponentHurtBox, float> OnDamaged;
        
    private void Awake()
    {
        currentHP = maxHP;
    }

    /// <summary>
    /// Damages the component
    /// </summary>
    /// <param name="damage"></param> Damage ammount
    /// <param name="hitPoint"></param> Point of impact
    public void TakeDamage(float damage, Vector3 hitPoint)
    {
        if (destroyed)
            return;

        float effectiveDamage = damage * damageMultiplier;
        Debug.DrawLine(hitPoint, hitPoint + Vector3.up * 0.2f, Color.red, 1f);

        currentHP -= effectiveDamage;
        OnDamaged?.Invoke(this, effectiveDamage);

        if (currentHP <= 0f)
        {
            currentHP = 0f;
            destroyed = true;
            DestroyComponent();
        }
    }

    private void DestroyComponent()
    {

        destroyed = true;

        OnDestroyed?.Invoke(this);
        Debug.Log($"{componentType} [{componentID}] destroyed");
    }

}
