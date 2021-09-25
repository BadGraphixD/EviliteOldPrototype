using UnityEngine;
using System;
using System.Collections.Generic;

public class DecorationGenerator {

    private class ExistingObject {
        public Vector2 pos;
        public float radius;

        public ExistingObject(Vector2 pos, float radius) {
            this.pos = pos;
            this.radius = radius;
        }
    }

    public static void GenerateDecorations(int minx, int maxx, int miny, int maxy, int[] tileIndices) {

        foreach (DecorationObject decorationObject in AssetManager.Instance.decorationObjects) {
            List<ExistingObject> existingObjects = new List<ExistingObject>();

            for (int x = 0; x <= maxx - minx; x++) {
                for (int y = 0; y <= maxy - miny; y++) {

                    Vector2Int generatingIntervalls = new Vector2Int(1, 1);

                    switch (decorationObject.placementRules.direction) {
                        case DecorationObject.PlacementRules.Direction.TOP:
                        case DecorationObject.PlacementRules.Direction.BOTTOM:
                            generatingIntervalls.x = 4;
                            break;
                        case DecorationObject.PlacementRules.Direction.SIDE:
                            generatingIntervalls.y = 4;
                            break;
                    }

                    for (int i = 0; i < generatingIntervalls.x; i++) {
                        for (int j = 0; j < generatingIntervalls.y; j++) {

                            Vector2 mapSpacePos = new Vector2(
                                x + i / (float)generatingIntervalls.x,
                                y + j / (float)generatingIntervalls.y
                            );
                            Vector2 worldSpacePos = mapSpacePos + new Vector2(minx, miny);
                            
                            if (canGenerate(decorationObject, existingObjects, mapSpacePos, tileIndices, (maxx - minx) + 1, (maxy - miny) + 1)) {

                                UnityEngine.Object.Instantiate(Utils.selectRandom(decorationObject.prefabs), worldSpacePos, Quaternion.identity);
                                existingObjects.Add(new ExistingObject(mapSpacePos, decorationObject.placementRules.neighbourRadius));
                            }
                        }
                    }
                }
            }
        }
    }

    private static bool canGenerate(DecorationObject decorationObject, List<ExistingObject> existingObjects, Vector2 pos, int[] tileIndices, int mapWidth, int mapHeight) {

        if (UnityEngine.Random.value > decorationObject.placementRules.frequency) {
            return false;
        }
        
        float foundationMinPos = pos.x - decorationObject.placementRules.foundationRadius;
        float foundationMaxPos = pos.x + decorationObject.placementRules.foundationRadius;

        float airMinPos = pos.y;
        float airMaxPos = pos.y + decorationObject.placementRules.height;

        for (int f = Utils.floor(foundationMinPos - 0.125f); f <= Utils.floor(foundationMaxPos); f++) {
            if (!compareTileMaterial(getTileIdx(new Vector2(f, pos.y - 1f), tileIndices, mapWidth, mapHeight), decorationObject.material)) {
                return false;
            }
        }
        
        for (int f = Utils.floor(foundationMinPos); f <= Utils.floor(foundationMaxPos - 0.125f); f++) {
            for (int a = Utils.floor(airMinPos); a <= Utils.floor(airMaxPos); a++) {
                if (!tileIsEmpty(getTileIdx(new Vector2(f, a), tileIndices, mapWidth, mapHeight))) {
                    return false;
                }
            }
        }

        foreach (ExistingObject existingObject in existingObjects) {
            float minDist = existingObject.radius + decorationObject.placementRules.neighbourRadius;

            if ((pos - existingObject.pos).magnitude < minDist) {
                return false;
            }
        }

        return true;
    }

    private static int getTileIdx(Vector2 posf, int[] tileIndices, int mapWidth, int mapHeight) {
        Vector2Int pos = new Vector2Int(Utils.floor(posf.x), Utils.floor(posf.y));

        if (pos.x < 0 || pos.x >= mapWidth || pos.y < 0 || pos.y >= mapHeight) {
            return -1;
        }

        return tileIndices[pos.x * mapHeight + pos.y];
    }

    private static bool tileIsEmpty(int tileIdx) {
        return tileIdx == -1;
    }

    private static bool compareTileMaterial(int tileIdx, TileMaterial material) {
        if (tileIdx == -1 || tileIdx >= AssetManager.Instance.tileTypes.Length) {
            return false;
        }
        
        if (AssetManager.Instance.tileTypes[tileIdx].material != material) {
            return false;
        }

        return true;
    }

}