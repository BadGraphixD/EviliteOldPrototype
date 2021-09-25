using UnityEngine;
using System;

[Serializable]
public class DecorationObject {
    
    [Serializable]
    public struct PlacementRules {

        public enum Direction {
            TOP,
            BOTTOM,
            SIDE
        }

        public Direction direction;

        public float foundationRadius;
        public float height;
        public float neighbourRadius;
        public float frequency;
    }

    public GameObject[] prefabs;

    public TileMaterial material;
    public PlacementRules placementRules;

}