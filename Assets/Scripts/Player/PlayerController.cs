using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    private static PlayerController instance;
    public static PlayerController Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }

    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerJumpForce;

    [SerializeField] private Timer jumpPressedRememberTimer;
    [SerializeField] private Timer groundedRememberTimer;
    private Player player;
    private GroundedPhysicsBody playerGpb;

    private void Awake() {
        jumpPressedRememberTimer.cooldown = -1f;
        groundedRememberTimer.cooldown = -1f;
    }

    private void Start() {
        InputManager.Instance.SubscribeKeyDownCallback(InputManager.InputAction.JUMP, JumpInvoked);
        InputManager.Instance.SubscribeKeyDownCallback(InputManager.InputAction.DASH, DashInvoked);
        InputManager.Instance.SubscribeKeyDownCallback(InputManager.InputAction.ATTACK, AttackInvoked);
        InputManager.Instance.SubscribeKeyDownCallback(InputManager.InputAction.RECALL, RecallInvoked);
    }

    private void OnDestroy() {
        InputManager.Instance?.UnsubscribeKeyDownCallback(InputManager.InputAction.JUMP, JumpInvoked);
        InputManager.Instance?.UnsubscribeKeyDownCallback(InputManager.InputAction.DASH, DashInvoked);
        InputManager.Instance?.UnsubscribeKeyDownCallback(InputManager.InputAction.ATTACK, AttackInvoked);
        InputManager.Instance?.UnsubscribeKeyDownCallback(InputManager.InputAction.RECALL, RecallInvoked);
    }

    private void Update() {
        if (player != null && !PauseManager.Instance.Paused) {
            if (playerGpb.Grounded) {
                groundedRememberTimer.Reset();
            }

            if (!jumpPressedRememberTimer.Finished && !groundedRememberTimer.Finished) {
                jumpPressedRememberTimer.cooldown = -1f;
                groundedRememberTimer.cooldown = -1f;

                executeJump();
            }

            jumpPressedRememberTimer.Update();
            groundedRememberTimer.Update();
        }
    }

    private void FixedUpdate() {
        if (player != null && !PauseManager.Instance.Paused) {
            Vector2 velocity = playerGpb.rb.velocity;

            velocity.x = 0f;
            velocity.x += InputManager.Instance.GetKey(InputManager.InputAction.LEFT) ? -playerSpeed : 0f;
            velocity.x += InputManager.Instance.GetKey(InputManager.InputAction.RIGHT) ? playerSpeed : 0f;

            playerGpb.rb.velocity = velocity;
        }
    }

    public void SetPlayer(Player playerToControl) {
        player = playerToControl;
        playerGpb = player.GetComponent<GroundedPhysicsBody>();
    }

    public void JumpInvoked() {
        if (player != null && !PauseManager.Instance.Paused) {
            Debug.Log("Jump");
            jumpPressedRememberTimer.Reset();
        }
    }

    public void DashInvoked() {
        if (player != null && !PauseManager.Instance.Paused) {
            Debug.Log("Dash");
        }
    }

    public void AttackInvoked() {
        if (player != null && !PauseManager.Instance.Paused) {
            Debug.Log("Attack");
        }
    }

    public void RecallInvoked() {
        if (player != null && !PauseManager.Instance.Paused) {
            Debug.Log("Recall");
        }
    }

    private void executeJump() {
        Vector2 velocity = playerGpb.rb.velocity;
        velocity.y = playerJumpForce;
        playerGpb.rb.velocity = velocity;
    }

}