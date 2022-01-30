using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;

public class SlotDataProcessor {

    public static void DeserializeAndCreateEntities(SlotData slotData) {
        String[] entities = SerializationUtils.UnpackEntryData(slotData.entityData);

        foreach (String entity in entities) {
            String[] entityProperties = SerializationUtils.UnpackEntryData(entity);

            String typeStr = SerializationUtils.GetEntryValue(entityProperties, "type");
            String guidStr = SerializationUtils.GetEntryValue(entityProperties, "guid");
            String entitySpecificData = SerializationUtils.GetEntryValue(entityProperties, "data");

            EntityType type = Array.Find(AssetManager.Instance.entityTypes, type => type.name.Equals(typeStr));
            EntityManager.Instance.CreateEntity(type, Guid.Parse(guidStr), entitySpecificData);
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
        TileIndexMap tileIndexMap = new TileIndexMap(slotData.mapData);
        tileIndexMap.FillTilemapWithTiles();
        DecorationGenerator.GenerateDecorations(tileIndexMap);
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