using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new Tile Material Collection", menuName = "Evolid/Tile Material Collection", order = 0)]
public class TileMaterialCollection : ScriptableObject {
    
    public enum TileMaterial {
        Air,
        Grass,
        Dirt,
        Stone
    }
    
    public TileBase[] tiles;
    public TileMaterial material;

}