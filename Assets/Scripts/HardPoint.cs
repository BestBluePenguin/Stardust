using UnityEngine;

public class HardPoint : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    public bool HasWeapon => weapon != null;
    public Weapon Weapon => weapon;

    public Transform FirePoint => transform;
    public Vector2 Forward => transform.up;

    public void AttachWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;

        if (weapon != null)
        {
            weapon.transform.SetParent(transform);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }
    }

    public void DetachWeapon()
    {
        if (weapon == null) return;

        weapon.transform.SetParent(null);
        weapon = null;
    }
}