using Assets.Scripts.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Cheese
{
    [RequireComponent(typeof(LineRenderer))]
    public class SmellViewer : MonoBehaviour
    {
        [SerializeField] private float _detectRadius;
        [SerializeField] private float _updateInterval = 0.1f;
        [SerializeField] private float _height;
        [SerializeField] private bool _enableGizmos;
        [SerializeField] private Transform _agent;
        private LineRenderer _lineRenderer;
        private readonly PathSmoother _pathSmoother = new();
        private float _pathUpdateTime;
        private Transform _target;
        private NavMeshPath _path;
        private Collider[] _detectable = new Collider[10];

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _path = new();
        }

        private void Update()
        {
            if (_pathUpdateTime >= _updateInterval)
            {
                _pathUpdateTime = 0;

                if (_target == null)
                    _lineRenderer.positionCount = 0;
                else
                {
                    if (NavMesh.CalculatePath(GetAgentPosition(), GetTargetPosition(), NavMesh.AllAreas, _path))
                    {
                        Vector3[] path = _path.corners.Select(corner => new Vector3(corner.x, _height, corner.z)).ToArray();
                        //Vector3[] smoothPath = _pathSmoother.SmoothPath(path);
                        _lineRenderer.positionCount = path.Length;
                        _lineRenderer.SetPositions(path);
                    }
                }
            }

            _pathUpdateTime += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (Physics.OverlapSphereNonAlloc(_agent.position, _detectRadius, _detectable, LayerMask.GetMask("Cheese")) == 0)
            {
                _target = null;
                return;
            }

            float minDistance = float.MaxValue;

            for (int i = 0; i < _detectable.Length; i++)
            {
                if (_detectable[i])
                {
                    if (_detectable[i].gameObject.activeInHierarchy)
                    {
                        float targetDistance = Vector3.Distance(_agent.position, _detectable[i].transform.position);

                        if (targetDistance < minDistance)
                        {
                            minDistance = targetDistance;
                            _target = _detectable[i].transform;
                        }
                    }
                }
            }
        }

        private Vector3 GetAgentPosition()
        {
            if (NavMesh.SamplePosition
                (_agent.transform.position, out NavMeshHit navMeshInfo, 100, NavMesh.AllAreas))
                return navMeshInfo.position;

            return _agent.position;
        }

        private Vector3 GetTargetPosition()
        {
            if (NavMesh.SamplePosition
                (_target.transform.position, out NavMeshHit navMeshInfo, 100, NavMesh.AllAreas))
                return navMeshInfo.position;

            return _target.position;
        }

        private void OnDrawGizmos()
        {
            if (_enableGizmos == false)
                return;

            Vector3 center = _agent == null ? transform.position : _agent.transform.position;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(center, _detectRadius);
        }
    }
}