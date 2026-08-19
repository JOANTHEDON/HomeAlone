using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]private TextAsset _levelFile;
    [SerializeField]private Tile _groundTile;
    [SerializeField]private Tile _wallTile;
    [SerializeField]private Tile _leftCornerWallTile;
    [SerializeField]private Tile _rightCornerWallTile;
    [SerializeField]private Tile _topCornerWallTile;
    [SerializeField]private Tile _bottomCornerWallTile;
    [SerializeField]private Tile _leftWallSide;
    [SerializeField]private Tile _rightWallSide;
    [SerializeField]private Tile _topWallSide;
    [SerializeField]private Tile _bottomWallSide;
    [SerializeField]private Tilemap _groundTileMap;
    [SerializeField]private Tilemap _wallTileMap;

    public void Awake()
    {
        ReadCSV();

    }

    private void ReadCSV()
    {
        string[] rows = _levelFile.text.Split('\n');
        for(int y =0; y< rows.Length; y++)
        {
            string row = rows[y].Trim();

            if(string.IsNullOrEmpty(row))
                continue;

            string[] cells = row.Split(',');
            for(int x =0; x< cells.Length; x++)
            {
                int value = int.Parse(cells[x].Trim());
                Debug.Log($"x: {x}, y:{y}, value: {value}");
                SpawnGroundTile(value, x, y);
            }    
        }
    }

    private void SpawnGroundTile(int value, int x, int y)
    {
        Vector3Int position = new Vector3Int(x, -y,0);
        _groundTileMap.SetTile(position, _groundTile);

        switch (value)
        {
            case 1:
                _wallTileMap.SetTile(position, _leftCornerWallTile);
                break;
            
            case 2:
                _wallTileMap.SetTile(position, _rightCornerWallTile);
                break;
            
            case 3:
                _wallTileMap.SetTile(position, _topCornerWallTile);
                break;

            case 4: 
                _wallTileMap.SetTile(position, _bottomCornerWallTile);
                break;

            case 5: 
                _wallTileMap.SetTile(position, _leftWallSide);
                break;

            case 6: 
                _wallTileMap.SetTile(position, _rightWallSide);
                break;

            case 7: 
                _wallTileMap.SetTile(position, _topWallSide);
                break;

            case 8: 
                _wallTileMap.SetTile(position, _bottomWallSide);
                break;

        }
    }
}
