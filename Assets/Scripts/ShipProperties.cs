using UnityEngine;

/// <summary>
/// List of ship classes. Used for sorting and categorizing ships in the game. Can be expanded with more classes as needed.
/// </summary>
public enum ShipClass
{
    DESTROYER,
    FRIGATE,
    CRUISER
}

/// <summary>
/// Property each ship and method of rotation control
/// </summary>
[CreateAssetMenu(fileName = "New Ship Properties", menuName = "Ships/Ship Properties")]
public class ShipProperties : ScriptableObject
{
    [Header("Identity")]
    public string shipName;
    [TextArea(3, 10)]
    public string description;
    public ShipClass shipClass;

    [Header("Controls")]
    public bool trackMouse;

    [Header("Physical properties")]
    [Tooltip("in metric tons")]
    public float shipMass;

    [Header("Movement")]
    [Tooltip("in m/s")]
    public float maxLinearSpeed; //Maximum linear velocity the ship can reach
    [Tooltip("in deg/s")]
    public float maxAngularSpeed; //Maximum rate a ship can rotate at
    [Tooltip("in kN")]
    public float thrustPower;
    [Tooltip("in kN")]
    public float retroThrustPower;
    [Tooltip("in kN")]
    public float rcsPower;
    [Tooltip("in m, distance of rcs power applied from the COG")]
    public float leverArm;

}
