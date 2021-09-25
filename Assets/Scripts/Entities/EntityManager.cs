using UnityEngine;
using System;

public class EntityManager : MonoBehaviour {
    
    private static EntityManager instance;
    public static EntityManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<EntityManager>();
            }
            return instance;
        }
    }

    public Entity CreateEntity(EntityType type, String data) {
        Entity entity = Instantiate(type.prefab).GetComponent<Entity>();

        if (entity == null) {
            return null;
        }

        entity.guid = Guid.NewGuid();
        entity.type = type;
        entity.Deserialize(data);
        return entity;
    }
    
}