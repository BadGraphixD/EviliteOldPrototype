using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PhysicsBody : MonoBehaviour {

    public Rigidbody2D rb {
        get; private set;
    }

    protected void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
}