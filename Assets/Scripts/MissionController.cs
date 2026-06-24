using Unity.Cinemachine;
using UnityEngine;

public class MissionController : MonoBehaviour
{

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private ShipCatalog catalog;
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private Weapon weaponPrefab;

    public GameObject activeVessel { get; private set; }
    public PlayerMovement ActiveMovement { get; private set; }

    private void Start()
    {
        spawnSelectedVessel();
    }

    private void spawnSelectedVessel()
    {   
        if (!canSpawn()) return;

        int index = Mathf.Clamp(SelectedVesselData.index, 0, catalog.playerPrefabCatalog.Length - 1);

        GameObject prefab = catalog.playerPrefabCatalog[index];
        if (prefab == null)
        {
            Debug.LogError($"No vessel prefab assigned at index {index}");
            return;
        }

        activeVessel = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        // --Movement--
        PlayerMovement movement = activeVessel.GetComponent<PlayerMovement>();
        if (movement == null)
        {
            Debug.LogError($"{activeVessel.name} is missing PlayerMovement");
            return;
        }
        movement.enableControl();

        // --Weapons--
        HardPoint[] hardpoints = activeVessel.GetComponentsInChildren<HardPoint>();
        
        foreach (var hp in hardpoints)
        {
            Weapon w = Instantiate(weaponPrefab, hp.transform);
            hp.AttachWeapon(w);
        }

        var controller = activeVessel.GetComponent<WeaponController>();
        if (controller != null)
        {
            controller.SetPlayerControlled(true);
        }

        // --Camera--
        assignCameraTarget(activeVessel.transform);
    }

    private bool canSpawn()
    {
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point not assigned!");
            return false;
        }
        if (catalog == null)
        {
            Debug.LogError("ShipCatalog not assigned!");
            return false;
        }
        if (catalog.playerPrefabCatalog == null || catalog.playerPrefabCatalog.Length == 0)
        {
            Debug.LogError("Player prefab catalog is empty!");
            return false;
        }
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not assigned!");
            return false;
        }

        return true;
    }

    private void assignCameraTarget(Transform target)
    {
        mainCamera.Follow = target;
        mainCamera.LookAt = target;
    }

}

public static class SelectedVesselData
{
    public static int index = 0;
}
