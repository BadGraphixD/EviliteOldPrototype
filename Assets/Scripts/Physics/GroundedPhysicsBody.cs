using UnityEngine;

public class GroundedPhysicsBody : DynamicPhysicsBody {
    
    [SerializeField] protected BoxCollider2D groundTrigger;

    public bool Grounded {
        get; private set;
    }

    public Utils.Callback OnLanded;
    public Utils.Callback OnLeaveGround;
    
    new protected void FixedUpdate() {
        base.FixedUpdate();
        checkGrounded();
    }

    private void checkGrounded() {
        Bounds bounds = groundTrigger.bounds;
        Collider2D[] collisions = Physics2D.OverlapBoxAll(bounds.center, bounds.size / 2f, 0f, AssetManager.Instance.groundMask);

        bool groundedBefore = Grounded;
        Grounded = collisions.Length > 0;

        if (!groundedBefore && Grounded) OnLanded?.Invoke();
        if (groundedBefore && !Grounded) OnLeaveGround?.Invoke();
    }
}