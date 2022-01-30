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

    public Entity CreateEntity(EntityType type, Guid guid, String data) {
        Entity entity = Instantiate(type.prefab, Vector3.zero, Quaternion.identity, GameManager.Instance.entityParent).GetComponent<Entity>();

        if (entity == null) {
            return null;
        }

        entity.guid = guid;
        entity.type = type;
        entity.Deserialize(data);
        return entity;
    }
    
}