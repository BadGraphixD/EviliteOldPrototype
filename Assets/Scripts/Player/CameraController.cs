using UnityEngine;

public class CameraController : MonoBehaviour {
    
    private static CameraController instance;
    public static CameraController Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<CameraController>();
            }
            return instance;
        }
    }

    private Transform target;
    [SerializeField] private Vector2 maxDistFromTarget;
    [SerializeField] private Vector2 positionLerpSpeed;

    private void Update() {
        if (target != null) {
            followTarget();
        }
    }

    private void followTarget() {
        Vector2 vectorToTarget = new Vector2(
            target.position.x - transform.position.x,
            target.position.y - transform.position.y
        );

        Vector3 position = transform.position;

        if (Mathf.Abs(vectorToTarget.x) > maxDistFromTarget.x) {
            position.x = Mathf.Lerp(transform.position.x,
                target.position.x - maxDistFromTarget.x * Mathf.Sign(vectorToTarget.x),
                positionLerpSpeed.x);
        }
        if (Mathf.Abs(vectorToTarget.y) > maxDistFromTarget.y) {
            position.y = Mathf.Lerp(transform.position.y,
                target.position.y - maxDistFromTarget.y * Mathf.Sign(vectorToTarget.y),
                positionLerpSpeed.y);
        }

        transform.position = position;
    }

    public void SetTarget(Transform newTarget) {
        target = newTarget;
    }

}