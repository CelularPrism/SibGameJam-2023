using Assets.Scripts.Cheese;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheeseInventory : MonoBehaviour
{
    [SerializeField] private Transform _view;
    [SerializeField] private Material _cheeseMaterial;
    [SerializeField] private float _viewSize = 15;
    private CharacterMotionController _motionController;
    private float _defaultSpeed;
    private CheeseBar _bar;
    private readonly List<CheeseInstance> _views = new();

    public float Count { get; private set; } = 0;
    public bool IsFull => Count >= 1;

    private void Awake()
    {
        _motionController = GetComponent<CharacterMotionController>();
        _defaultSpeed = _motionController.MoveSpeed;
        _bar = FindObjectOfType<CheeseBar>();
    }

    public bool TryPut(Mesh mesh, float size, float weight)
    {
        if (Count + size <= 1)
        {
            Count = Mathf.Clamp01(Count += size);
            _motionController.MoveSpeed -= weight;

            if (_bar)
                _bar.Set(Count);

            CheeseInstance view = _views.FirstOrDefault(view => view.Size == size && view.gameObject.activeInHierarchy == false);

            if (view == null)
            {
                view = new GameObject($"Cheese [{size}]").AddComponent<CheeseInstance>();
                view.Construct(mesh, size, weight);
                view.transform.SetParent(_view);
                view.gameObject.AddComponent<MeshFilter>().mesh = mesh;
                view.gameObject.AddComponent<MeshRenderer>().material = _cheeseMaterial;
                view.transform.localPosition = Vector3.zero;
                view.transform.localScale = Vector3.one * _viewSize;
                _views.Add(view);
            }
            else
                view.gameObject.SetActive(true);

            view.transform.localRotation = Quaternion.Euler(Vector3.forward * (45 * (Count * 8)));
            return true;
        }

        return false;
    }

    public float RemoveAll()
    {
        float count = Count;
        Count = 0;
        _motionController.MoveSpeed = _defaultSpeed;

        for (int i = 0; i < _view.childCount; i++)
        {
            _view.GetChild(i).gameObject.SetActive(false);
        }
        return count;
    }
}
