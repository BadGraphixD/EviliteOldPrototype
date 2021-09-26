using UnityEngine;
using System;
using System.Collections.Generic;

public class DecorationGenerator {

    private class ExistingObject {
        public Vector2 pos;

        public ExistingObject(Vector2 pos) {
            this.pos = pos;
        }
    }

    public static void GenerateDecorations(TileIndexMap tileIndexMap) {

        foreach (DecorationObject decorationObject in AssetManager.Instance.decorationObjects) {
            List<ExistingObject> existingObjects = new List<ExistingObject>();

            for (int x = 0; x < tileIndexMap.Width; x++) {
                for (int y = 0; y < tileIndexMap.Height; y++) {

                    Vector2Int generatingIntervalls = calcGenerationIntervall(decorationObject.placementRules.direction);

                    for (int i = 0; i < generatingIntervalls.x; i++) {
                        for (int j = 0; j < generatingIntervalls.y; j++) {

                            Vector2 mapSpacePos = calcMapSpacePos(x, y, i, j, generatingIntervalls);
                            Vector2 worldSpacePos = tileIndexMap.Position + mapSpacePos;
                            
                            if (canGenerate(decorationObject, existingObjects, mapSpacePos, tileIndexMap)) {

                                UnityEngine.Object.Instantiate(Utils.selectRandom(decorationObject.prefabs), worldSpacePos, Quaternion.identity);
                                existingObjects.Add(new ExistingObject(mapSpacePos));
                            }
                        }
                    }
                }
            }
        }
    }

    private static bool canGenerate(DecorationObject decorationObject, List<ExistingObject> existingObjects, Vector2 pos, TileIndexMap tileIndexMap) {

        if (UnityEngine.Random.value > decorationObject.placementRules.frequency) {
            return false;
        }

        Range foundationRange = new Range(
            pos.x - decorationObject.placementRules.foundationRadius,
            pos.x + decorationObject.placementRules.foundationRadius
        );
        Range airRange = new Range(
            pos.y,
            pos.y + decorationObject.placementRules.height
        );

        if (!hasFoundation(foundationRange, pos, tileIndexMap, decorationObject) ||
            !hasAirSpace(foundationRange, airRange, tileIndexMap) ||
            !hasNighbourSpace(existingObjects, decorationObject, pos)) {
            return false;
        }


        return true;
    }

    private static bool hasFoundation(Range foundationRange, Vector2 pos, TileIndexMap tileIndexMap, DecorationObject decorationObject) {
        for (int f = Utils.floor(foundationRange.min - 0.125f); f <= Utils.floor(foundationRange.max); f++) {
            if (!tileIndexMap.CompareTileMaterial(new Vector2(f, pos.y - 1f), decorationObject.material)) {
                return false;
            }
        }
        return true;
    }

    private static bool hasAirSpace(Range foundationRange, Range airRange, TileIndexMap tileIndexMap) {
        for (int f = Utils.floor(foundationRange.min); f <= Utils.floor(foundationRange.max - 0.125f); f++) {
            for (int a = Utils.floor(airRange.min); a <= Utils.floor(airRange.max); a++) {
                if (!tileIndexMap.TileIsEmptyOrDestructible(new Vector2(f, a))) {
                    return false;
                }
            }
        }
        return true;
    }

    private static bool hasNighbourSpace(List<ExistingObject> existingObjects, DecorationObject decorationObject, Vector2 pos) {
        foreach (ExistingObject existingObject in existingObjects) {
            if ((pos - existingObject.pos).magnitude < decorationObject.placementRules.neighbourRadius * 2) {
                return false;
            }
        }
        return true;
    }

    private static Vector2Int calcGenerationIntervall(DecorationObject.PlacementRules.Direction direction) {
        Vector2Int intervalls = new Vector2Int(1, 1);

        switch (direction) {
            case DecorationObject.PlacementRules.Direction.TOP:
            case DecorationObject.PlacementRules.Direction.BOTTOM:
                intervalls.x = 4;
                break;
            case DecorationObject.PlacementRules.Direction.SIDE:
                intervalls.y = 4;
                break;
        }

        return intervalls;
    }

    private static Vector2 calcMapSpacePos(int x, int y, int i, int j, Vector2Int intervalls) {
        return new Vector2(
            x + i / (float)intervalls.x,
            y + j / (float)intervalls.y
        );
    }

}