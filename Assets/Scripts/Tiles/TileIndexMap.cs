using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TileIndexMap {

    private int minx, maxx, miny, maxy;
    private int[] tileIndices;

    public int Width { get { return (maxx - minx) + 1; } }
    public int Height { get { return (maxy - miny) + 1; } }

    public int TileAmount {
        get { return tileIndices.Length; }
    }
    
    public Vector2 Position {
        get { return new Vector2(minx, miny); }
    }

    public TileIndexMap(String mapData) {
        String[] unpackedMapData = SerializationUtils.UnpackEntryData(mapData);

        minx = int.Parse(SerializationUtils.GetEntryValue(unpackedMapData, "minx"));
        miny = int.Parse(SerializationUtils.GetEntryValue(unpackedMapData, "miny"));
        maxx = int.Parse(SerializationUtils.GetEntryValue(unpackedMapData, "maxx"));
        maxy = int.Parse(SerializationUtils.GetEntryValue(unpackedMapData, "maxy"));

        String[] tileIndicesAsStr = SerializationUtils.UnpackEntryData(SerializationUtils.GetEntryValue(unpackedMapData, "tiles"));
        tileIndices = new int[tileIndicesAsStr.Length];
        for (int i = 0; i < tileIndicesAsStr.Length; i++) {
            tileIndices[i] = int.Parse(tileIndicesAsStr[i]);
        }
    }

    public void FillTilemapWithTiles() {

        if (GameManager.Instance.tilemap != null) {
            GameManager.Instance.tilemap.ClearAllTiles();
        }

        if (Width <= 0 || Height <= 0) {
            return;
        }

        int tilePos = 0;
        foreach (int tileIdx in tileIndices) {

            int tilePosY = tilePos % Height;
            int tilePosX = tilePos / Height;
            tilePos++;

            if (tileIdx < 0 || tileIdx >= AssetManager.Instance.tileTypes.Length) {
                continue;
            }

            TileBase tileBase = AssetManager.Instance.tileTypes[tileIdx].tileBase;
            GameManager.Instance.tilemap.SetTile(new Vector3Int(minx + tilePosX, miny + tilePosY, 0), tileBase);
        }
    }

    public bool TileIsEmptyOrDestructible(Vector2 pos) {
        int tileIdx = getTileIdx(pos);
        return tileIdx == -1 || AssetManager.Instance.tileTypes[tileIdx].material.isDestructible;
    }

    public bool CompareTileMaterial(Vector2 pos, TileMaterial material) {
        int tileIdx = getTileIdx(pos);
        return tileIdx != -1 && AssetManager.Instance.tileTypes[tileIdx].material == material;
    }

    private int getTileIdx(Vector2 posf) {
        Vector2Int pos = new Vector2Int(Utils.FloorToInt(posf.x), Utils.FloorToInt(posf.y));

        if (pos.x < 0 || pos.x >= Width || pos.y < 0 || pos.y >= Height) {
            return -1;
        }

        return tileIndices[pos.x * Height + pos.y];
    }

}