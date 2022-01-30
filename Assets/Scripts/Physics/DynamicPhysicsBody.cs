using UnityEngine;

public class DynamicPhysicsBody : PhysicsBody {

    new protected void Awake() {
        base.Awake();
        rb.gravityScale = AssetManager.Instance.globalGravity;
    }
    protected void FixedUpdate() {
        rb.simulated = !PauseManager.Instance.Paused;
    }

}