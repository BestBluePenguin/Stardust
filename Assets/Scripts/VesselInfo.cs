using UnityEngine;

public class VesselInfo : MonoBehaviour
{
    [SerializeField] private ShipProperties properties;

    public ShipProperties Properties => properties;

    [System.Serializable]
    public struct HardpointLoadout
    {
        public string hardpointName;   // "HP_Front"
        public Weapon weaponPrefab;
    }   

    public HardpointLoadout[] loadout;
}