using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{

    [SerializeField] private HardPoint[] hardpoints;
    private Controls controls;
    private InputAction fire;
    private bool isPlayerControlled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        controls = new Controls();
        fire = controls.Player.Primary;
        hardpoints = GetComponentsInChildren<HardPoint>();

    }
    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Start()
    {
        Debug.Log($"Weapons assigned: {hardpoints.Length}");
    }

    void Update()
    {
        if (!isPlayerControlled)
            return;

        if (fire.WasPressedThisFrame())
        {
            fireWeapon();
        }
    }

    void fireWeapon()
    {
        Vector2 direction = transform.up;
        Debug.Log($"Weapon fired: {name}");
        foreach (var hp in hardpoints)
        {
            if (hp.HasWeapon)
            {
                hp.Weapon.Fire(direction);
            }
        }
    }

    public void SetPlayerControlled(bool value)
    {
        isPlayerControlled = value;
    }
}
