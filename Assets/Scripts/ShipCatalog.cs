using UnityEngine;

[CreateAssetMenu(fileName = "ShipCatalog", menuName = "Scriptable Objects/ShipCatalog")]
public class ShipCatalog : ScriptableObject
{
    [SerializeField] public GameObject[] playerPrefabCatalog;
}
