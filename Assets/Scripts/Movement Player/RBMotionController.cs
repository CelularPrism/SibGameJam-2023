using Assets._Project.Gameplay.Ground_Check;
using Assets.Scripts.Movement_Player;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class RBMotionController : MonoBehaviour, IMovable
{
    [field: SerializeField] public float DefaultWalkForce { get; private set; }
    [field: SerializeField] public float DefaultMass { get; private set; }
    [SerializeField] private float _forceInterval;
    [SerializeField] private InputAction _moveInputAction, _jumpInputAction;
    [SerializeField] private float _rotationSmooth;
    [SerializeField] private float _jumpForce, _landForce;
    [SerializeField] private bool _canLookToCursor = true;
    [SerializeField] private int _cameraRotationSpeed = 300;
    [SerializeField] private float _lookIKWeight = 1;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    public Rigidbody Rigidbody { get; private set; }
    private CapsuleCollider _collider;
    private CinemachineOrbitalTransposer _orbitalTransposer;
    private float _rotationVelocity = 0.1f;
    private float _forceIntervalTime;
    private Vector3 _look;
    private Camera _camera;
    private IGroundChecker _groundChecker;
    private Animator _animator;
    private bool _isWalk;
    private readonly int _runAnimationHash = Animator.StringToHash("IsRun");
    private readonly int _cryAnimationHash = Animator.StringToHash("IsCry");
    private readonly int _animationSpeedMultiplierHash = Animator.StringToHash("SpeedMultiplier");

    public float SpeedFactor { get; set; } = 1;
    public float MassModifire { get; set; } = 1;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _orbitalTransposer = _virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        _groundChecker = new CapsuleGroundChecker(_collider, _groundMask);
        Rigidbody.mass = DefaultMass;
    }

    private void OnEnable()
    {
        _moveInputAction?.Enable();
        _jumpInputAction?.Enable();
        _jumpInputAction.performed += Jump;
    }

    private void Update()
    {
        if (_orbitalTransposer)
            _orbitalTransposer.m_XAxis.m_MaxSpeed = Input.GetMouseButton(1) ? _cameraRotationSpeed : 0;

        float cameraDistance = (transform.position - _camera.transform.position).magnitude;
        _look = Camera.main.ScreenToWorldPoint(new(Input.mousePosition.x, Input.mousePosition.y, cameraDistance));
        _isWalk = _moveInputAction.ReadValue<Vector2>() != Vector2.zero;
        _animator.SetBool(_runAnimationHash, _isWalk);
        _animator.SetBool(_cryAnimationHash, Input.GetMouseButton(0) && _isWalk == false);
        _animator.SetFloat(_animationSpeedMultiplierHash, new Vector2(Rigidbody.velocity.x, Rigidbody.velocity.z).magnitude);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_animator && layerIndex == 1)
        {
            if (_canLookToCursor)
            {
                _animator.SetLookAtWeight(_lookIKWeight);
                _animator.SetLookAtPosition(_look);
            }
            else
            {
                _animator.SetLookAtWeight(0);
            }
        }
    }

    private void FixedUpdate()
    {
        _groundChecker?.Check();
        Vector2 input = _moveInputAction.ReadValue<Vector2>();
        float targetAngle = Mathf.Atan2(input.normalized.x, input.normalized.y) * Mathf.Rad2Deg + _virtualCamera.transform.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, _rotationSmooth);
        Vector3 targetDirection = Quaternion.Euler(Vector3.up * targetAngle) * Vector3.forward;

        if (input != Vector2.zero || Input.GetMouseButton(0))
            transform.rotation = Quaternion.Euler(Vector3.up * smoothAngle);

        targetDirection *= DefaultWalkForce * Time.fixedDeltaTime;
        //float velocityMagnitude = Mathf.Abs(Rigidbody.velocity.magnitude);

        if (input != Vector2.zero && _groundChecker.IsGrounded && _forceIntervalTime >= _forceInterval)
        {
            _forceIntervalTime = 0;
            Rigidbody.AddForce(targetDirection/* / Mathf.Clamp(velocityMagnitude, 1, velocityMagnitude)*/, ForceMode.Force);
        }

        if (_groundChecker.IsGrounded == false)
            Rigidbody.AddForce(Physics.gravity * _landForce);

        _forceIntervalTime += Time.fixedDeltaTime;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (_groundChecker.IsGrounded)
            Rigidbody.AddForce((transform.up + transform.forward / 2) * _jumpForce, ForceMode.Impulse);
    }

    private void OnDisable()
    {
        _jumpInputAction.performed -= Jump;
        _moveInputAction?.Disable();
        _jumpInputAction?.Disable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_look, 0.5f);
        _groundChecker?.OnDrawGizmos();
    }

    public void Move(Vector3 direction) { }
}