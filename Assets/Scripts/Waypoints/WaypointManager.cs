using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Waypoints
{
    public enum WaypointBehaviour
    {
        Idle,
        MoveTowardsLocation,
        MoveTowardsTarget,
        Patrol
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class WaypointManager : MonoBehaviour
    {
        private WaypointBehaviour _waypointBehaviour;

        private NavMeshAgent _navMeshAgent;
        private readonly List<Vector3> _waypoints = new List<Vector3>();
        private bool _patrolDirectionReverse = false;

        //Find next/previous waypoint index from the current, doesn't overwrite the current index value
        public int Next() => (_currentWaypointIndex + 1) % _waypoints.Count;
        public int Previous() => (_currentWaypointIndex - 1) < 0 ? _waypoints.Count - 1 : _currentWaypointIndex - 1;

        //Set current waypoint index
        public void SetIndex(int index) => _currentWaypointIndex = index;

        //Get the current waypoint index
        public List<Vector3> GetWaypoints => _waypoints;
        public int GetIndex() => _currentWaypointIndex;

        //Get Waypoint locations at specified index, if no index is specified it retunrs the current waypoint location
        public Vector3 GetWaypoint() => _waypoints[_currentWaypointIndex];
        public Vector3 GetWaypoint(int index) => _waypoints[index];

        //Get next/previous waypoint locations, doesn't overwrite the current index value
        public Vector3 NextWaypoint() => _waypoints[Next()];
        public Vector3 PreviousWaypoint() => _waypoints[Previous()];

        //Add waypoint locations to list
        public void Add(Vector3 location) => _waypoints.Add(location);
        public void AddRange(Vector3[] locations) => _waypoints.AddRange(locations);

        //Remove waypoint locations from list
        public void Remove(Vector3 waypoint) => _waypoints.Remove(waypoint);
        public void RemoveAt(int index) => _waypoints.RemoveAt(index);
        public void RemoveAt(int index, int count) => _waypoints.RemoveRange(index, count);

        //Set/Clear target to follow
        public void SetTarget(GameObject gameObject) => _target = gameObject;
        public void ClearTarget() => SetTarget(null);

        private Vector3 _targetLocation;
        public void Move(Vector3? targetLocation = null)
        {
            _targetLocation = GetWaypoint();

            if (targetLocation != null)
            {
                _targetLocation = targetLocation.Value;
            }

            _waypointBehaviour = WaypointBehaviour.MoveTowardsLocation;
        }

        //Start chasing target
        public void MoveTowardsTarget(GameObject gameObject = null)
        {
            if (gameObject != null)
            {
                SetTarget(gameObject);
            }

            _waypointBehaviour = WaypointBehaviour.MoveTowardsTarget;
        }

        /*TODO
            Follow path of waypoints, bezier curves ???
        */


        //Stop all behaviours
        public void Stop()
        {
            _waypointBehaviour = WaypointBehaviour.Idle;
        }

        //Start patroling in circles, TODO add different patrolling styles (rotate around, move back and front .. )
        public void Patrol(Vector3[] path = null, bool patrolDirectionReverse = false)
        {
            _patrolDirectionReverse = patrolDirectionReverse;

            if (path != null)
            {
                _waypoints.Clear();
                AddRange(path);
                _currentWaypointIndex = 0;
            }

            _waypointBehaviour = WaypointBehaviour.Patrol;
        }

        [SerializeField] private int _currentWaypointIndex = 0;

        private float _movementTolerance = 1f;

        [SerializeField] private GameObject _target;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            switch (_waypointBehaviour)
            {
                case WaypointBehaviour.MoveTowardsLocation:
                    {
                        UpdateLocation();
                        break;
                    }

                case WaypointBehaviour.MoveTowardsTarget:
                    {
                        if (_target != null)
                        {
                            UpdateTarget();
                        }

                        break;
                    }

                case WaypointBehaviour.Patrol:
                    UpdatePatrol();
                    break;
            }
        }

        private void UpdateLocation()
        {
            _navMeshAgent.SetDestination(_targetLocation);
        }

        private void UpdateTarget()
        {
            _navMeshAgent.SetDestination(_target.transform.position);
        }

        private void UpdatePatrol()
        {
            if (Vector3.Distance(transform.position, GetWaypoint()) > _movementTolerance)
            {
                _navMeshAgent.SetDestination(GetWaypoint());
            }
            else
            {
                SetIndex(_patrolDirectionReverse ? Previous() : Next());
            }
        }
    }
}