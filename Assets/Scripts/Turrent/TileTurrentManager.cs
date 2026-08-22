using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TileTurretManager : MonoBehaviour {
    [Header("Grid & Prefab Settings")]
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private Tilemap roomTilemap; // Assign your Room Tilemap (Floor/Room layer)

    [Header("Cost & Managers")]
    [SerializeField] private int turretCost = 8;
    [SerializeField] private CoinManager coinManager;

    [Header("Fixed UI Settings")]
    [SerializeField] private GameObject turretUIButton;

    private Vector3Int selectedCellPos;
    private Vector3 selectedWorldPos;
    private bool hasTileSelected = false;
    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();

    private void Start() {
        if (turretUIButton != null)
            turretUIButton.SetActive(false);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
            SelectTileUnderMouse();
        }
    }

    private void SelectTileUnderMouse() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int cellPos = grid.WorldToCell(mouseWorldPos);

        // 1. Only allow selecting tiles inside the Room Tilemap
        if (roomTilemap != null && !roomTilemap.HasTile(cellPos)) {
            Debug.Log("Cannot place turret outside room!");
            HideUI();
            return;
        }

        // 2. Check if tile is already occupied
        if (occupiedTiles.Contains(cellPos)) {
            Debug.Log("Tile already occupied!");
            HideUI();
            return;
        }

        // 3. Check if player has enough coins (8 coins)
        if (coinManager == null || coinManager.CurrentCoinCount < turretCost) {
            Debug.Log($"Not enough coins! Need {turretCost} coins.");
            HideUI();
            return;
        }

        selectedCellPos = cellPos;
        selectedWorldPos = grid.GetCellCenterWorld(cellPos);
        selectedWorldPos.z = 0;
        hasTileSelected = true;

        if (turretUIButton != null) turretUIButton.SetActive(true);
    }

    private void HideUI() {
        hasTileSelected = false;
        if (turretUIButton != null) turretUIButton.SetActive(false);
    }

    public void BuildTurretOnSelectedTile() {
        if (!hasTileSelected) return;

        // Deduct 8 coins and build turret
        if (coinManager != null && coinManager.SpendCoins(turretCost)) {
            Instantiate(turretPrefab, selectedWorldPos, Quaternion.identity);
            occupiedTiles.Add(selectedCellPos);
            HideUI();
            Debug.Log($"Turret built at cell position: {selectedCellPos}");
        }
    }
}