using UnityEngine;
using System;

public class Entity : MonoBehaviour {

    public Guid guid {
        get; set;
    }

    public EntityType type {
        get; set;
    }

    public virtual String Serialize() { return null; }
    public virtual void Deserialize(String data) { }

}