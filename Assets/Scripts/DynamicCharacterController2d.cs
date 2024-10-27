using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class DynamicCharacterController2d: MonoBehaviour {
    [SerializeField] private float speed = 10;
    [SerializeField] private float speedJump = 10;

    [Header("Gravity Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CircleCollider2D groundCollider;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float maxSlopeAngle = 60f;

    private Rigidbody2D body;
    private ContactFilter2D groundFilter;
    private Collider2D[] collides = new Collider2D[16];
    private RaycastHit2D[] raycasts = new RaycastHit2D[16];
    private Vector2? slopeNormal;

    private bool isJumpInput;
    private Vector2 movementInput;

    private void Awake () {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start () {
        groundFilter.SetLayerMask(groundLayer);
        groundFilter.useLayerMask = true;
        groundFilter.useTriggers = false;
    }

    public void OnJump () {
        isJumpInput = true;
    }

    public void OnMove (InputValue input) {
        movementInput = input.Get<Vector2>();
    }

    private void FixedUpdate () {
        GroundCheck();
        SlopeCheck();

        if (isJumpInput && isGrounded)
        {
            body.AddForce(speedJump * Vector2.up, ForceMode2D.Impulse);
        }

        isJumpInput = false;

        var movementDirection = movementInput;
        if (Mathf.Abs(movementInput.x) > 0.1f)
        {
            if (slopeNormal.HasValue)
            {
                movementDirection = -Mathf.Sign(movementInput.x) * Vector2.Perpendicular(slopeNormal.Value).normalized;
            }
        }

        var additionalForce = movementDirection * speed;
        body.AddForce(additionalForce, ForceMode2D.Force);
    }

    private void GroundCheck () {
        var count = groundCollider.OverlapCollider(groundFilter, collides);
        isGrounded = count > 0;
    }

    private void SlopeCheck () {
        var maxAngle = 0f;
        Vector2? normal = null;
        Vector2[] directions = { Vector2.down, Vector2.right, Vector2.left };
        foreach (var direction in directions)
        {
            var count = groundCollider.Cast(direction, groundFilter, raycasts, 0.1f);
            for (int index = 0; index < count; index++)
            {
                var hit = raycasts[index];
                if (hit.collider != null)
                {
                    float angle = Vector2.Angle(hit.normal, Vector2.up);
                    if (angle <= maxSlopeAngle && angle > maxAngle)
                    {
                        maxAngle = angle;
                        normal = hit.normal;
                    }
                }
            }
        }

        slopeNormal = normal;
    }
}
