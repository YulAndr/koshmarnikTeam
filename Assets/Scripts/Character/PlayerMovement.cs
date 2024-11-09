using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {
    [Header("Speed Settings")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float speedJump = 10;

    [Header("Gravity Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private float gravity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float maxSlopeAngle = 60f;

    public Animator animator;

    private Rigidbody2D body;
    private ContactFilter2D groundFilter;
    private Collider2D[] collidesBuffer = new Collider2D[16];
    private RaycastHit2D[] raycastsBuffer = new RaycastHit2D[16];
    private Vector2? slopeNormal;
    // Добавим поле для сохранения текущего обьекта под ногами персонажа
    private Rigidbody2D parent;

    private bool jumpInput;
    private int extraJump;
    private int extraJumpValue = 1;

    private Vector2 movementInput;
    private Vector2 gravityVelocity;
    bool isFacingRight = true;

    public bool IsGrounded => isGrounded;

    public bool interactionInput;
    //[SerializeField] private InputActionReference interaction;
    //public bool interactionPressed = false;

    //private void OnEnable () {
    //    interaction.action.performed += PerformInteraction;
    //}

    //private void OnDisable () {
    //    interaction.action.performed -= PerformInteraction;
    //}

    //private void PerformInteraction (InputAction.CallbackContext obj) {
    //    interactionPressed = true;
    //}

    private void Awake () {
        body = GetComponent<Rigidbody2D>();
    }

    private void Start () {
        groundFilter.SetLayerMask(groundLayer);
        groundFilter.useLayerMask = true;
        groundFilter.useTriggers = false;

    }

    public void OnJump () {
        jumpInput = true;
        animator.SetBool("IsJumping", true);
    }

    
    public void OnMove (InputValue input) {
        movementInput = input.Get<Vector2>();
    }

    public void OnInteraction() {
        interactionInput = true;
    }

    private void FixedUpdate () {
        GroundCheck();
        SlopeCheck();

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    interactionPressed = true;
        //    print("interactionPressed");
        //}

        //if (Input.GetKeyUp(KeyCode.E))
        //{
        //    interactionPressed = false;
        //    print("cancel interaction");
        //}


        animator.SetFloat("Speed", Mathf.Abs(movementInput.x));

        if (!isGrounded)
        {
            gravityVelocity = gravityVelocity - Vector2.down * Physics2D.gravity * gravity * Time.fixedDeltaTime;
        } else
        {
            gravityVelocity = Vector2.Max(Vector2.zero, gravityVelocity);
        }

        gravityVelocity.y = Mathf.Clamp(gravityVelocity.y, -20, 20);

        if (isGrounded)
        {
            extraJump = extraJumpValue;
            animator.SetBool("IsJumping", false);
        }

        if (jumpInput && isGrounded)
        {
            gravityVelocity = speedJump * Vector2.up;
            
        } else if (jumpInput && extraJump > 0)
        {
            gravityVelocity = speedJump * Vector2.up;
            extraJump--;
            
        }

        var movementDirection = movementInput;
        if (Mathf.Abs(movementInput.x) > 0.1f)
        {
            if (slopeNormal.HasValue)
            {
                movementDirection = -Mathf.Sign(movementInput.x) * Vector2.Perpendicular(slopeNormal.Value).normalized;
            }
        }

        jumpInput = false;
        animator.SetBool("IsJumping", false);
        interactionInput = false;//
        // Я изменил движение на изменение скорости обьекта, чтобы это работало нужно поставить
        // параметр Linear Damp в значение 50 у вашего RigidBody.
        body.velocity += (movementDirection * speed + gravityVelocity) * 10;
        // Если есть како RigidBodt2D под ногами то добавим его скорость к нашей.
        if (parent != null)
        {
            body.velocity += parent.velocity;
        }

        if (movementInput.x > 0 && !isFacingRight)
        {
            Flip();
        } else if (movementInput.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip () {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        isFacingRight = !isFacingRight;
    }

    private void GroundCheck () {
        var count = groundCollider.OverlapCollider(groundFilter, collidesBuffer);
        isGrounded = count > 0;
        // Помимо того, что узнаем стоим ли мы на земле, узнаем стоим ли мы на RigidBody
        // Я проверяю только первую коллизию, но в ваше случае может потребоваться проверить все
        if (count > 0 && collidesBuffer[0].attachedRigidbody != null)
        {
            // Если такая коллизия есть, то сохраняем 
            parent = collidesBuffer[0].attachedRigidbody;
        } else
        {
            // Если нет то сбрасываем ссылку
            parent = null;
        }
    }

    private void SlopeCheck () {
        var maxAngle = 0f;
        Vector2? normal = null;
        Vector2[] directions = { Vector2.down, Vector2.right, Vector2.left };
        foreach (var direction in directions)
        {
            var count = groundCollider.Cast(direction, groundFilter, raycastsBuffer, 0.1f);
            for (int index = 0; index < count; index++)
            {
                var hit = raycastsBuffer[index];
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
