using FMODUnity;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class UseEnvironments : MonoBehaviour
{
    [SerializeField] private float _raduis;
    private MovementInput _input;
    private Collider[] _usables = new Collider[5];
    private bool _canUse = false;

    public bool CanUse() => _canUse;

    private void OnEnable()
    {
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Disable();
    }

    void Awake()
    {
        _input = new MovementInput();
        _input.Player.Use.performed += Use;
    }

    private void FixedUpdate()
    {
        if (Physics.OverlapSphereNonAlloc(transform.position, _raduis, _usables, LayerMask.GetMask("Usable Item")) > 0)
        {
            if (_usables[0].GetComponent<IItem>() != null)
                _canUse = true;
        } else
        {
            _canUse = false;
        }
    }

    private void Use(CallbackContext calbackContext)
    {
        Debug.Log(_usables[0]);
        var muted = false;
        RuntimeManager.GetBus("bus:/SFX").getMute(out muted);
        if (muted)
            RuntimeManager.GetBus("bus:/SFX").setMute(false);

        if (_canUse)
        {
            Debug.Log(_usables[0].GetComponent<IItem>());
            _usables[0].GetComponent<IItem>()?.Use();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _raduis);
    }
}
