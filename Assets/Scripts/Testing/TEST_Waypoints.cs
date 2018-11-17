using Assets.Scripts.Waypoints;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Waypoints : MonoBehaviour
{
    public WaypointBehaviour Behaviour;

    [SerializeField] public List<Vector3> PatrolRoute;
    public Vector3 TargetLocation;
    public GameObject Target;
    public bool PatrolReverse = false;

    public WaypointManager[] AIs;

    void Start()
    {
        AIs = FindObjectsOfType<WaypointManager>();

        foreach (var waypointManager in AIs)
        {
            waypointManager.AddRange(PatrolRoute.ToArray());
            waypointManager.SetTarget(Target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        foreach (var waypointManager in AIs)
        {
            switch (Behaviour)
            {
                case WaypointBehaviour.Patrol:
                    {
                        waypointManager.Patrol(patrolDirectionReverse: PatrolReverse);
                        break;
                    }
                case WaypointBehaviour.MoveTowardsTarget:
                    {
                        waypointManager.MoveTowardsTarget(Target);
                        break;
                    }
                case WaypointBehaviour.MoveTowardsLocation:
                {
                        waypointManager.Move(TargetLocation);
                    break;
                }
            }
        }
    }
}
