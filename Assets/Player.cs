using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class Player : MonoBehaviour
{
    [SerializeField] private InputActionReference moveInput, camInput;
    [SerializeField] private Transform _orient;
    [SerializeField] private Vector2 _moveSpeed;
    [SerializeField] private float _height;
    [SerializeField] private float _spring, _damper;
    [SerializeField] private ConfigurableJoint _joint;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private LayerMask _groundLayer;

    private float _lookup = 0, _lookAround = 0;

    private void Start()
    {
        SetCursorLock(true);
    }

    private void OnEnable()
    {
        moveInput.action.Enable();
        camInput.asset.Enable();
    }

    private void Update()
    {
        var lookDir = camInput.action.ReadValue<Vector2>();
        _lookup = Mathf.Clamp(_lookup - lookDir.y, -90, 90);
        _lookAround += lookDir.x;
        _orient.transform.localEulerAngles = Vector3.right * _lookup + Vector3.up * _lookAround;
    }

    public void SetCursorLock(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    private void FixedUpdate()
    {
        var yVel = _rb.linearVelocity.y;
        var moveVal = moveInput.action.ReadValue<Vector2>();
        var orientation = Quaternion.Euler(Vector3.up * _lookAround);
        var vel = orientation * Vector3.forward * moveVal.y * _moveSpeed.y + orientation * Vector3.right * moveVal.x * _moveSpeed.x;
        vel.y = yVel;
        _rb.linearVelocity = vel;
        if (!Physics.Raycast(transform.position + Vector3.up, Vector3.down, out var hit, _height, _groundLayer))
        {
            _joint.yDrive = new()
            {
                positionSpring = 0,
                positionDamper = 0,
            };
            return;
        }
        _joint.yDrive = new()
        {
            positionSpring = _spring,
            positionDamper = _damper,
            maximumForce = float.PositiveInfinity,
        };
        _joint.targetPosition = (hit.point + Vector3.up * _height);

        
    }
}
