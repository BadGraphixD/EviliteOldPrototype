using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

public class SlotDataProcessor {

    public static void DeserializeAndCreateEntities(SlotData slotData) {
        String[] entities = SerializationUtils.UnpackEntryData(slotData.entityData);

        if (entities == null) {
            return;
        }

        foreach (EntityType entityType in AssetManager.Instance.entityTypes) {
            String[] entityInstances = SerializationUtils.UnpackEntryData(SerializationUtils.GetEntryValue(entities, entityType.name));
            
            if (entityInstances == null) {
                continue;
            }

            foreach (String entityInstanceData in entityInstances) {
                EntityManager.Instance.CreateEntity(entityType, entityInstanceData);
            }
        }
    }

    public static String SerializeEntities() {

        DataEntryList entitySaveData = new DataEntryList();

        Dictionary<EntityType, DataEntryList> entityDataPerType = new Dictionary<EntityType, DataEntryList>();
        foreach (EntityType entityType in AssetManager.Instance.entityTypes) {
            entityDataPerType.Add(entityType, new DataEntryList());
        }
        
        Entity[] entities = GameObject.FindObjectsOfType<Entity>();
        foreach (Entity entity in entities) {
            entityDataPerType[entity.type].AddEntry(entity.Serialize());
        }

        foreach (EntityType entityType in AssetManager.Instance.entityTypes) {
            entitySaveData.AddEntry(entityType.name + "=" + entityDataPerType[entityType].GetString());
        }

        return entitySaveData.GetString();
    }

    public static void DeserializeAndCreateMap(SlotData slotData) {
        String[] mapData = SerializationUtils.UnpackEntryData(slotData.mapData);
        
        if (mapData == null) {
#if UNITY_EDITOR
            Debug.Log($"Error: Invalid map data: \"{slotData.mapData}\"");
#endif
            return;
        }

        int minx = int.Parse(SerializationUtils.GetEntryValue(mapData, "minx"));
        int miny = int.Parse(SerializationUtils.GetEntryValue(mapData, "miny"));
        int maxx = int.Parse(SerializationUtils.GetEntryValue(mapData, "maxx"));
        int maxy = int.Parse(SerializationUtils.GetEntryValue(mapData, "maxy"));

        int width  = (maxx - minx) + 1;
        int height = (maxy - miny) + 1;

        String tileData = SerializationUtils.GetEntryValue(mapData, "tiles");
        String[] tileIndices = SerializationUtils.UnpackEntryData(tileData);

        if (GameManager.Instance.tilemap != null) {
            GameManager.Instance.tilemap.ClearAllTiles();
        }

        if (width <= 0 || height <= 0) {
            return;
        }

        int tilePos = 0;
        foreach (String tileIndex in tileIndices) {

            int tilePosY = tilePos % height;
            int tilePosX = tilePos / height;
            tilePos++;

            int tileIdx = int.Parse(tileIndex);

            if (tileIdx < 0 || tileIdx >= AssetManager.Instance.tileTypes.Length) {
                continue;
            }

            TileBase tileBase = AssetManager.Instance.tileTypes[tileIdx].tileBase;
            GameManager.Instance.tilemap.SetTile(new Vector3Int(minx + tilePosX, miny + tilePosY, 0), tileBase);
        }
    }

    public static String SerializeMap() {

        DataEntryList mapData = new DataEntryList();

        GameManager.Instance.tilemap.CompressBounds();

        int minx = GameManager.Instance.tilemap.cellBounds.min.x;
        int miny = GameManager.Instance.tilemap.cellBounds.min.y;
        int maxx = GameManager.Instance.tilemap.cellBounds.max.x - 1;
        int maxy = GameManager.Instance.tilemap.cellBounds.max.y - 1;

        mapData.AddEntry("minx=" + minx);
        mapData.AddEntry("miny=" + miny);
        mapData.AddEntry("maxx=" + maxx);
        mapData.AddEntry("maxy=" + maxy);

        DataEntryList tileIndices = new DataEntryList();

        for(int x = minx; x <= maxx; x++){
            for(int y = miny; y <= maxy; y++){
                TileBase tileBase = GameManager.Instance.tilemap.GetTile(new Vector3Int(x, y, 0));

                int tileIdx = -1;

                if (tileBase != null) {
                    for (int i = 0; i < AssetManager.Instance.tileTypes.Length; i++) {
                        if (AssetManager.Instance.tileTypes[i].tileBase == tileBase) {
                            tileIdx = i;
                            break;
                        }
                    }
                }

                tileIndices.AddEntry(tileIdx.ToString());
            }
        }

        mapData.AddEntry("tiles=" + tileIndices.GetString());
        return mapData.GetString();
    }

}