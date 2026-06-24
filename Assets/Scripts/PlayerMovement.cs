using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public ShipProperties properties;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Controls controls;
    private InputAction move;
    private InputAction rotate;
    private InputAction mouse;

    // 
    private bool isPlayerControlled = false;


    /// <summary>
    /// Called on loading.
    /// </summary>
    void Awake()
    {
        if (properties == null) //Null check
        {
            Debug.LogError($"{name}: ShipProperties not assigned!");
            enabled = false;
            return;
        }

        
        if (!TryGetComponent(out rb)) //Error Checking rb
        {
            Debug.LogError($"{name}: Rigidbody2D missing!");
            enabled = false;
            return;
        }

        rb.gravityScale = 0.0f;
        rb.linearDamping = 0.0f;
        rb.angularDamping = 0.0f;
        rb.mass = properties.shipMass; // in metric Tons

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None;

        mainCamera = Camera.main;

        controls = new Controls();
        move = controls.Player.Movement;
        rotate = controls.Player.Rotate;
        mouse = controls.Player.Mouse;

    }

    /// <summary>
    /// Enables input controls
    /// </summary>
    private void OnEnable()
    {
        if (controls == null)
            controls = new Controls();

        controls.Player.Enable();
    }

    /// <summary>
    /// Disables input controls
    /// </summary>
    private void OnDisable()
    {
        controls.Player.Disable();
    }


    /// <summary>
    /// Primary control loop for handling player input and applying forces to the ship. Called at a fixed time interval for consistent physics updates.
    /// </summary>
    /// <param name="dt"></param> fixed delta time
    void FixedUpdate()
    {
        if (!isPlayerControlled)
            return;

        float dt = Time.fixedDeltaTime;
        handleThrust(dt);
        handleManeuver(dt);
        handleConstraints(dt);
    }

    /// <summary>
    /// Handles applying thrust forces to player
    /// </summary>
    /// <param name="dt"></param> delta time
    void handleThrust(float dt)
    {
        float forwardInput = move.ReadValue<Vector2>().y;

        float thrustPower =
            forwardInput >= 0f
            ? properties.thrustPower
            : properties.retroThrustPower;

        rb.AddForce(transform.up * forwardInput * thrustPower, ForceMode2D.Force);
    }

    /// <summary>
    /// Handles applying rotational forces to player
    /// </summary>
    /// <param name="dt"></param> delta time
    void handleManeuver(float dt)
    {
        //Strafing
        float strafeInput = -move.ReadValue<Vector2>().x;
        rb.AddForce(transform.right * 1.0f * strafeInput * properties.rcsPower, ForceMode2D.Force);

        //Torque limit
        float maxTorque = properties.rcsPower * properties.leverArm / 50000f;
        //Rotation code
        if (properties.trackMouse)
        {
            Vector2 mousePos = mouse.ReadValue<Vector2>();
            Vector2 worldMouse = mainCamera.ScreenToWorldPoint(mousePos);
            Vector2 direction = worldMouse - (Vector2)transform.position;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            float angleDiff = Mathf.DeltaAngle(rb.rotation, targetAngle);

            if (Mathf.Abs(angleDiff) < 0.5f && Mathf.Abs(rb.angularVelocity) < 2f)
            {
                rb.angularVelocity = 0f;
                rb.rotation = targetAngle;
                return;
            }


            float kp = 0.25f; // Proportional gain
            float kd = 0.1f; // Derivative gain

            float torqueInput = kp * angleDiff - rb.angularVelocity * kd;
            torqueInput = Mathf.Clamp(torqueInput, -1f, 1f);
                
            rb.AddTorque(torqueInput * maxTorque, ForceMode2D.Force);
        }
        else
        {
            float rotateInput = rotate.ReadValue<float>();

            rb.AddTorque(rotateInput * maxTorque, ForceMode2D.Force);
        }

    }

    /// <summary>
    /// Handles applying clamping forces, and dampening forces
    /// </summary>
    /// <param name="dt"></param> delta time
    void handleConstraints(float dt)
    {
        Vector2 moveInput = move.ReadValue<Vector2>();
        bool noInput = moveInput.sqrMagnitude < 0.01f;

        float rotateInput = rotate.ReadValue<float>();
        bool noRotateInput = !properties.trackMouse && Mathf.Abs(rotateInput) < 0.01f;

        //linear speed damping
        float targetDrag = (rb.linearVelocity.magnitude < 5f && noInput) ? 3f : 0f;
        rb.linearDamping = Mathf.Lerp(rb.linearDamping, targetDrag, 5f * dt);


        //Clamp top linear velocity
        if (rb.linearVelocity.magnitude > properties.maxLinearSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * properties.maxLinearSpeed;
        }

        //Clean up linear velocity
        if (rb.linearVelocity.magnitude < 0.01f)
        {
            rb.linearVelocity = Vector2.zero;
        }


        //low angular speed dampening
        if (Mathf.Abs(rb.angularVelocity) < 90 && noRotateInput)
        {
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0f, 3f * dt);
        }
            
        // Clamp Top Angular Velocity (deg/s)
        if (Mathf.Abs(rb.angularVelocity) > properties.maxAngularSpeed)
        {
            rb.angularVelocity = Mathf.Sign(rb.angularVelocity) * properties.maxAngularSpeed;
        }

        //Clean up angular velocity
        if (Mathf.Abs(rb.angularVelocity) < 0.01f)
        {
            rb.angularVelocity = 0.0f;
        }

    }

    public void enableControl()
    {
        isPlayerControlled = true;
    }

    public void disableControl()
    {
        isPlayerControlled = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    
}
