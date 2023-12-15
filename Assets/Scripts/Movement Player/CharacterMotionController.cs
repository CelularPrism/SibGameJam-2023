using Assets.Scripts.Movement_Player;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMotionController : MonoBehaviour, IMovable
{
    public float MoveSpeed;
    [SerializeField] private InputAction _moveInputAction;
    [SerializeField] float _rotationSmooth;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private int _cameraRotationSpeed = 300;
    [SerializeField] private bool _canLookToCursor = true;
    [SerializeField] private float _lookIKWeight = 1;
    [SerializeField] private float _lookdeadZone;
    [SerializeField] private float _defaultTargetDistance;
    private CharacterController _characterController;
    private CinemachineOrbitalTransposer _orbitalTransposer;
    private float _rotationVelocity = 0.1f;
    private Animator _animator;
    private float _defaultSpeed;
    private Camera _camera;
    private Vector3 _look;
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

    private void OnEnable()
    {
        _moveInputAction.Enable();
    }

    private void Update()
    {
        //if (_orbitalTransposer)
        //    _orbitalTransposer.m_XAxis.m_MaxSpeed = Input.GetMouseButton(1) ? _cameraRotationSpeed : 0;

        if (_orbitalTransposer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                _orbitalTransposer.m_XAxis.Value += 90;

            if (Input.GetKeyDown(KeyCode.E))
                _orbitalTransposer.m_XAxis.Value -= 90;
        }

        Vector2 input = _moveInputAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _virtualCamera.transform.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, _rotationSmooth);
        direction = Quaternion.Euler(Vector3.up * targetAngle) * Vector3.forward;

        if (input != Vector2.zero || Input.GetMouseButton(0))
            transform.rotation = Quaternion.Euler(Vector3.up * smoothAngle);

        if (input == Vector2.zero)
            direction = Vector3.zero;

        _characterController.Move(MoveSpeed * SpeedFactor * Time.deltaTime * direction.normalized);
        _animator.SetBool(_runAnimationHash, direction != Vector3.zero);
        _animator.SetBool(_cryAnimationHash, Input.GetMouseButton(0) && direction == Vector3.zero);
        _animator.SetFloat(_animationSpeedMultiplierHash, MoveSpeed * SpeedFactor / _defaultSpeed);

        _characterController.Move(Physics.gravity * Time.deltaTime);
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
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit lookRayHitInfo, maxDistance: 200, LayerMask.GetMask("Ground")))
        {
            _look = Vector3.Distance(lookRayHitInfo.point, transform.position) < _lookdeadZone 
                ? transform.position + transform.forward * _defaultTargetDistance
                : lookRayHitInfo.point;
        }
        else
        {
            _look = transform.position + transform.forward * _defaultTargetDistance;
        }
    }

    private void OnDisable()
    {
        _moveInputAction.Disable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_look, 0.5f);
    }

    public void Move(Vector3 direction) => _characterController.Move(direction);
}
