using System.Collections.Generic;
using UnityEngine;

public class TurretPlacementManager : MonoBehaviour {
    [Header("Grid References")]
    [SerializeField] private Grid grid; // Assign your Scene Grid object
    [SerializeField] private GameObject turretPrefab; // Turret Prefab to instantiate

    private bool isPlacementModeActive = false;
    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();

    // Call this function when clicking the UI Turret Button
    public void EnableTurretPlacementMode() {
        isPlacementModeActive = true;
        Debug.Log("Turret Placement Mode Enabled. Click on a tile to build!");
    }

    private void Update() {
        if (!isPlacementModeActive) return;

        // Check if mouse clicked and not clicking over UI elements
        if (Input.GetMouseButtonDown(0)) {
            TryPlaceTurret();
        }
    }

    private void TryPlaceTurret() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // Convert world position to isometric cell coordinate
        Vector3Int cellPos = grid.WorldToCell(mouseWorldPos);

        // Prevent placing multiple turrets on the same tile
        if (occupiedTiles.Contains(cellPos)) {
            Debug.LogWarning("Tile already occupied!");
            return;
        }

        // Get cell center world position for placement
        Vector3 spawnWorldPos = grid.GetCellCenterWorld(cellPos);
        spawnWorldPos.z = 0; // Adjust Z offset depending on your sorting setup

        // Instantiate Turret
        Instantiate(turretPrefab, spawnWorldPos, Quaternion.identity);
        occupiedTiles.Add(cellPos);

        // Exit placement mode after building (optional)
        isPlacementModeActive = false;
        Debug.Log($"Turret placed at cell {cellPos}!");
    }
}