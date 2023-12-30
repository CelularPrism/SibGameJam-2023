using Assets.Scripts.Cheese;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheeseInventory : MonoBehaviour
{
    [SerializeField] private Transform _view;
    [SerializeField] private Material _cheeseMaterial;
    [SerializeField] private float _viewSize = 15;
    private RBMotionController _motionController;
    private HealthSystem _health;
    private float _defaultSpeed;
    private CheeseBar _bar;
    private readonly List<CheeseInstance> _views = new();
    private readonly List<CheeseInstance> _cheese = new();

    public float Fill { get; private set; } = 0;
    public bool IsFull => Fill >= 1;

    private void Awake()
    {
        _motionController = GetComponent<RBMotionController>();
        _health = GetComponent<HealthSystem>();
        _bar = FindObjectOfType<CheeseBar>();
    }

    private void OnEnable()
    {
        _health.OnDamage += OnDamage;
    }

    private void OnDamage(float damage)
    {
        if (_health.Health == 0)
        {
            RemoveAll();
            return;
        }

        Remove();
    }

    public bool TryPut(CheeseInstance cheese)
    {
        if (Fill + cheese.Size <= 1)
        {
            _cheese.Add(cheese);
            Fill = Mathf.Clamp01(Fill += cheese.Size);
            _motionController.Rigidbody.mass += cheese.Weight;

            if (_bar)
                _bar.Set(Fill);

            CheeseInstance view = _views.FirstOrDefault(view => view.Size == cheese.Size && view.gameObject.activeInHierarchy == false);

            if (view == null)
            {
                view = new GameObject($"Cheese [{cheese.Size}]").AddComponent<CheeseInstance>();
                view.Construct(cheese.Mesh, cheese.Size, cheese.Weight);
                view.transform.SetParent(_view);
                view.gameObject.AddComponent<MeshFilter>().mesh = cheese.Mesh;
                view.gameObject.AddComponent<MeshRenderer>().material = _cheeseMaterial;
                view.transform.localPosition = Vector3.zero;
                view.transform.localScale = Vector3.one * _viewSize;
                _views.Add(view);
            }
            else
                view.gameObject.SetActive(true);

            view.transform.localRotation = Quaternion.Euler(Vector3.forward * (45 * (Fill * 8)));
            return true;
        }

        return false;
    }

    public float RemoveAll()
    {
        float count = Fill;
        Fill = 0;
        _motionController.Rigidbody.mass = _motionController.DefaultMass;

        for (int i = 0; i < _view.childCount; i++)
        {
            _view.GetChild(i).gameObject.SetActive(false);
        }

        _cheese.Clear();
        return count;
    }

    public void Remove()
    {
        if (Fill > 0)
        {
            Fill = Mathf.Clamp01(Fill -= _cheese[^1].Size);
            _motionController.Rigidbody.mass -= _cheese[^1].Weight;
            IEnumerable<CheeseInstance> activeViews = _views.Where(view => view.gameObject.activeInHierarchy);
            activeViews.ElementAt(activeViews.Count() - 1).gameObject.SetActive(false);
            _cheese[^1].transform.position = transform.position;
            _cheese[^1].gameObject.SetActive(true);
            _cheese.RemoveAt(_cheese.Count - 1);
        }
    }

    private void OnDisable()
    {
        _health.OnDamage -= OnDamage;
    }
}
