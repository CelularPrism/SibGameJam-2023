using Assets.Scripts.Movement_Player;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class CharacterMotionController : MonoBehaviour, IMovable
{
    public float MoveSpeed;
    [SerializeField] private InputAction _moveInputAction, _jumpInputAction;
    [SerializeField] float _rotationSmooth;
    [SerializeField] private float _jumpTime;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private bool _canLookToCursor = true;
    [SerializeField] private int _cameraRotationSpeed = 300;
    [SerializeField] private float _lookIKWeight = 1;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private CharacterController _characterController;
    private CinemachineOrbitalTransposer _orbitalTransposer;
    private Vector3 _motion;
    private float _rotationVelocity = 0.1f;
    private float _defaultSpeed;
    private Vector3 _look;
    private float _gravity;
    private float _jumpVelocity;
    private Camera _camera;
    private Animator _animator;
    private readonly int _runAnimationHash = Animator.StringToHash("IsRun");
    private readonly int _cryAnimationHash = Animator.StringToHash("IsCry");
    private readonly int _animationSpeedMultiplierHash = Animator.StringToHash("SpeedMultiplier");

    public float SpeedFactor { get; set; } = 1;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _orbitalTransposer = _virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _animator = GetComponent<Animator>();
        _defaultSpeed = MoveSpeed;
        _camera = Camera.main;
    }

    private void Start() => CalculateJump();

    private void CalculateJump()
    {
        float timeToApex = _jumpTime / 2;
        _gravity = -_jumpHeight * 2 / Mathf.Pow(timeToApex, 2);
        _jumpVelocity = 2 * _jumpHeight / timeToApex;
    }

    private void OnEnable()
    {
        _moveInputAction?.Enable();
        _jumpInputAction?.Enable();
    }

    private void Update()
    {
        if (_orbitalTransposer)
            _orbitalTransposer.m_XAxis.m_MaxSpeed = Input.GetMouseButton(1) ? _cameraRotationSpeed : 0;

        //if (_orbitalTransposer)
        //{
        //    if (Input.GetKeyDown(KeyCode.E))
        //        _orbitalTransposer.m_XAxis.Value += 90;

        //    if (Input.GetKeyDown(KeyCode.Q))
        //        _orbitalTransposer.m_XAxis.Value -= 90;
        //}

        Vector2 input = _moveInputAction.ReadValue<Vector2>();
        Vector2 direction = input.normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + _virtualCamera.transform.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, _rotationSmooth);
        Vector3 targetDirection = Quaternion.Euler(Vector3.up * targetAngle) * Vector3.forward;
        direction = new(targetDirection.x, targetDirection.z);
        float cameraDistance = (transform.position - _camera.transform.position).magnitude;
        _look = Camera.main.ScreenToWorldPoint(new(Input.mousePosition.x, Input.mousePosition.y, cameraDistance));

        if (input != Vector2.zero || Input.GetMouseButton(0))
            transform.rotation = Quaternion.Euler(Vector3.up * smoothAngle);

        if (input == Vector2.zero)
            direction = Vector2.zero;

        direction *= MoveSpeed * SpeedFactor * Time.deltaTime;
        _motion.x = direction.x;
        _motion.z = direction.y;

        if (_characterController.isGrounded == false)
            _motion.y += _gravity * Time.deltaTime;

        if (_jumpInputAction.WasPressedThisFrame())
        {
            if (_characterController.isGrounded)
                _motion.y = _jumpVelocity;
        }

        _characterController.Move(_motion);
        _animator.SetBool(_runAnimationHash, direction != Vector2.zero);
        _animator.SetBool(_cryAnimationHash, Input.GetMouseButton(0) && direction == Vector2.zero);
        _animator.SetFloat(_animationSpeedMultiplierHash, MoveSpeed * SpeedFactor / _defaultSpeed);
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
        //Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast(ray, out RaycastHit lookRayHitInfo, maxDistance: 200, LayerMask.GetMask("Ground")))
        //{
        //    _look = Vector3.Distance(lookRayHitInfo.point, transform.position) < _lookdeadZone 
        //        ? transform.position + transform.forward * _defaultTargetDistance
        //        : lookRayHitInfo.point;
        //}
        //else
        //{
        //    _look = transform.position + transform.forward * _defaultTargetDistance;
        //}
    }

    private void OnDisable()
    {
        _moveInputAction?.Disable();
        _jumpInputAction?.Disable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_look, 0.5f);
    }

    public void Move(Vector3 direction) => _characterController.Move(direction);
}
