using UnityEngine;

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

    public EntityType[] entityTypes;
    public TileType[] tileTypes;
    public GameObject gameSlotUIPrefab;

    public GameObject overworldPrefab;
    public GameObject catacombsPrefab;
    public GameObject templePrefab;

}