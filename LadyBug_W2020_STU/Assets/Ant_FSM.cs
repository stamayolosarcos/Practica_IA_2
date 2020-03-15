using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Pathfinding;
using Steerings;


namespace FSM
{
    [RequireComponent(typeof(Ant_Blackboard))]
    [RequireComponent(typeof(PathFeeder))]
    [RequireComponent(typeof(PathFollowing))]
    [RequireComponent(typeof(Seeker))]


    public class Ant_FSM : FiniteStateMachine
    {
        public enum State { INITIAL, GO_TO_TARGET, GO_TO_EXIT }
        public State currentState;

        private Ant_Blackboard blackboard;

        PathFeeder pathFeeder;


        // Start is called before the first frame update
        void Start()
        {
            blackboard = GetComponent<Ant_Blackboard>();
            pathFeeder = GetComponent<PathFeeder>();

        }

        // Update is called once per frame
        void Update()
        {

            switch (currentState)
            {

                case State.INITIAL:

                    ChangeState(State.GO_TO_TARGET);

                    break;

                case State.GO_TO_TARGET:

                    if (SensingUtils.DistanceToTarget(gameObject, pathFeeder.target) <= blackboard.CloseEnoughRadius)
                    {
                        ChangeState(State.GO_TO_EXIT);
                    }

                    break;

                case State.GO_TO_EXIT:

                    if (SensingUtils.DistanceToTarget(gameObject, pathFeeder.target) <= blackboard.CloseEnoughRadius)
                        Destroy(this);

                    break;

            }

        }

        void ChangeState(State newState)
        {

            switch (currentState)
            {

                case State.INITIAL:

                    break;

                case State.GO_TO_TARGET:

                    if (transform.GetChild(0).tag == "SEED_ON_ANT")
                        transform.GetChild(0).tag = "SEED";
                    else if (transform.GetChild(0).tag == "EGG_ON_ANT")
                        transform.GetChild(0).tag = "EGG";

                        GraphNode node = AstarPath.active.GetNearest(transform.GetChild(0).transform.position,NNConstraint.Default).node;
                        transform.GetChild(0).transform.position = (Vector3)node.position;
                        transform.GetChild(0).transform.parent = null;

                    break;

                case State.GO_TO_EXIT:

                    break;

            }

            switch (newState)
            {
                case State.INITIAL:

                    break;

                case State.GO_TO_TARGET:
                    pathFeeder.enabled = true;
                    pathFeeder.target = GetTarget("waypoint");

                    break;

                case State.GO_TO_EXIT:
                    pathFeeder.target = GetTarget("exit");
                    break;

            }

            currentState = newState;

        }

        GameObject GetTarget(string type)
        {
            GameObject target = null;
            int randomTarget;
            switch (type) 
            {
                case "waypoint":

                    randomTarget = Random.Range(0, blackboard.Waypoints.Length);
                    target = blackboard.Waypoints[randomTarget];

                    break;

                case "exit":

                    randomTarget = Random.Range(0, blackboard.ExitPoints.Length);
                    target = blackboard.ExitPoints[randomTarget];

                    break;
            }

            return target;
        }
    }
}
