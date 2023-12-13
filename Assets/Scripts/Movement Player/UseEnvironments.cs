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
        if (Physics.OverlapSphereNonAlloc(transform.position, _raduis, _usables, LayerMask.GetMask("Usable")) > 0)
        {
            if (_usables[0].GetComponent<IUsable>() != null)
            {
                var learns = FindObjectsOfType<LearnDestroyer>();
                foreach (var learn in learns)
                {
                    if (learn.gameObject.activeInHierarchy && learn.gameObject != transform.gameObject)
                        Destroy(learn.gameObject);
                }
                _canUse = true;
            }
        } else
        {
            _canUse = false;
        }
    }

    private void Use(CallbackContext calbackContext)
    {
        if (_canUse)
        {
            _usables[0].GetComponent<IUsable>()?.Use();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _raduis);
    }
}
