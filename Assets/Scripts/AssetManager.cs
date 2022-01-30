using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

public class AssetManager : MonoBehaviour {

    private static AssetManager instance;
    public static AssetManager Instance {
        get {
            if (instance == null) {
                instance = (Instantiate(Resources.Load("AssetManager")) as GameObject).GetComponent<AssetManager>();
            }
            return instance;
        }
    }

    public TileBase[] tileBasesToAdd;
    public TileMaterial tileMaterialToAdd;

    public EntityType[] entityTypes;
    public TileType[] tileTypes;
    public DecorationObject[] decorationObjects;
    public GameObject gameSlotUIPrefab;
    public LayerMask groundMask;

    public float globalGravity;

    // TODO: remove temporary method (or migrate to editor script)
    public void AddTileTypes() {
        Debug.Log("Adding Types!");

        List<TileType> typeList = tileTypes.ToList();

        foreach (TileBase tileBase in tileBasesToAdd) {
            TileType type = new TileType();
            type.tileBase = tileBase;
            type.material = tileMaterialToAdd;

            typeList.Add(type);
        }

        tileTypes = typeList.ToArray();
    }

}