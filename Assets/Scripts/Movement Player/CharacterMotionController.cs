using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMotionController : MonoBehaviour
{
    public float MoveSpeed;
    [SerializeField] private InputAction _moveInputAction;
    [SerializeField] float _rotationSmooth;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private int _cameraRotationSpeed = 300;
    private CharacterController _characterController;
    private CinemachineOrbitalTransposer _orbitalTransposer;
    private bool _stopped = false;
    private float _rotationVelocity = 0.1f;
    private Animator _animator;
    private int _runAnimationHash = Animator.StringToHash("IsRun");
    private int _cryAnimationHash = Animator.StringToHash("IsCry");

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _orbitalTransposer = _virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _moveInputAction.Enable();
    }

    private void Update()
    {
        if (_orbitalTransposer)
            _orbitalTransposer.m_XAxis.m_MaxSpeed = Input.GetMouseButton(1) ? _cameraRotationSpeed : 0;

        if (_stopped == false)
        {
            Vector2 input = _moveInputAction.ReadValue<Vector2>();
            Vector3 direction = new Vector3(input.x, 0, input.y).normalized;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _virtualCamera.transform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, _rotationSmooth);
            direction = Quaternion.Euler(Vector3.up * targetAngle) * Vector3.forward;

            if (input != Vector2.zero || Input.GetMouseButton(0))
                transform.rotation = Quaternion.Euler(Vector3.up * smoothAngle);

            if (input == Vector2.zero)
                direction = Vector3.zero;

            _characterController.Move(MoveSpeed * Time.deltaTime * direction.normalized);
            _animator.SetBool(_runAnimationHash, direction != Vector3.zero);
            _animator.SetBool(_cryAnimationHash, Input.GetMouseButton(0) && direction == Vector3.zero);
        }
    }

    private void OnDisable()
    {
        _moveInputAction.Disable();
    }

    public void Stop() => _stopped = true;
}
