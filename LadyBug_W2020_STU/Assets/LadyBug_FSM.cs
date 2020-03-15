using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Steerings;
using Pathfinding;


namespace FSM
{

    [RequireComponent(typeof(LadyBug_BlackBoard))]
    [RequireComponent(typeof(PathFeeder))]
    [RequireComponent(typeof(PathFollowing))]
    [RequireComponent(typeof(Seeker))]

    public class LadyBug_FSM : FiniteStateMachine
    {

        public enum State { INITIAL, WANDER, GO_TO_SEED, BRING_SEED, GO_TO_EGG, BRING_EGG }
        public State currentState;

        private LadyBug_BlackBoard blackboard;

        private GameObject currentSEED, currentEGG;


        PathFeeder pathFeeder;




        // Start is called before the first frame update
        void Start()
        {
            blackboard = GetComponent<LadyBug_BlackBoard>();
            pathFeeder = GetComponent<PathFeeder>();

        }

        // Update is called once per frame
        void Update()
        {
            switch (currentState)
            {

                case State.INITIAL:

                    ChangeState(State.WANDER);

                    break;

                case State.WANDER:

                    if (SensingUtils.DistanceToTarget(gameObject, pathFeeder.target) <= blackboard.CloseEnoughRadius)
                    {
                        

                        ChangeState(State.WANDER);

                        break;
                    }

                    currentEGG = SensingUtils.FindInstanceWithinRadius(gameObject, "EGG", blackboard.CloseEggDetectedRadius);
                    if (currentEGG != null) 
                    {
                        ChangeState(State.GO_TO_EGG);
                        break;

                    }

                    currentEGG = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "EGG", blackboard.FarEggDetectedRadius);
                    if(currentEGG != null) 
                    {
                        ChangeState(State.GO_TO_EGG);
                        break;
                    }

                    currentSEED = SensingUtils.FindInstanceWithinRadius(gameObject, "SEED", blackboard.CloseSeedDetectedRadius);
                    if(currentSEED != null) 
                    {
                        ChangeState(State.GO_TO_SEED);
                        break;
                    }

                    currentSEED = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "SEED", blackboard.FarSeedDetectedRadius);
                    if(currentSEED != null) 
                    {
                        ChangeState(State.GO_TO_SEED);
                        break;
                    }


                    break;

                case State.GO_TO_SEED:
                    if(pathFeeder.target.tag != "SEED") 
                    {
                        ChangeState(State.WANDER);
                        break;
                    }

                    currentEGG = SensingUtils.FindInstanceWithinRadius(gameObject, "EGG", blackboard.CloseEggDetectedWithSeedRadius);
                    if(currentEGG != null) 
                    {
                        ChangeState(State.GO_TO_EGG);
                        break;
                    }

                    if(SensingUtils.DistanceToTarget(gameObject, pathFeeder.target)<= blackboard.CloseEnoughRadiusToItem) 
                    {
                        ChangeState(State.BRING_SEED);
                        break;
                    }

                    

                    break;

                case State.GO_TO_EGG:
                    if(pathFeeder.target.tag != "EGG")
                    {
                        ChangeState(State.WANDER);
                        break;
                    }

                    if (SensingUtils.DistanceToTarget(gameObject, pathFeeder.target) <= blackboard.CloseEnoughRadiusToItem)
                    {

                        ChangeState(State.BRING_EGG);
                        break;
                    }

                   
                    break;

                case State.BRING_SEED:

                    currentEGG = SensingUtils.FindInstanceWithinRadius(gameObject, "EGG", blackboard.CloseEggDetectedWithSeedRadius);

                    if (currentEGG != null) 
                    {
                        ChangeState(State.GO_TO_EGG);
                        break;
                    }

                    if(SensingUtils.DistanceToTarget(gameObject, pathFeeder.target)<= blackboard.CloseEnoughRadius) 
                    {
                        ChangeState(State.WANDER);
                        break;
                    }

                    break;

                case State.BRING_EGG:

                    if(SensingUtils.DistanceToTarget(gameObject, pathFeeder.target)<= blackboard.CloseEnoughRadius) 
                    {
                        ChangeState(State.WANDER);
                        break;
                    }

                    break;



            }
        }



        void ChangeState(State newState)
        {
            switch (currentState)
            {
                case State.INITIAL:
                    break;

                case State.WANDER:
                    break;

                case State.GO_TO_EGG:
                    break;

                case State.GO_TO_SEED:
                    break;

                case State.BRING_EGG:

                    transform.GetChild(0).transform.parent = null;

                    break;

                case State.BRING_SEED:

                    if(newState == State.GO_TO_EGG) 
                    {
                        GraphNode node = AstarPath.active.GetNearest(transform.GetChild(0).transform.position, NNConstraint.Default).node;
                        transform.GetChild(0).transform.position = (Vector3)node.position;
                        transform.GetChild(0).tag = "SEED";
                        transform.GetChild(0).transform.parent = null;

                        break;
                    }
                    
                    if(newState == State.WANDER) 
                    {
                        transform.GetChild(0).transform.parent = null;                    
                    }

                    break;


            }

            switch (newState)
            {
                case State.INITIAL:
                    break;

                case State.WANDER:

                    pathFeeder.enabled = true;
                    pathFeeder.target = GetTarget("wayPoint");

                    break;

                case State.GO_TO_EGG:

                    pathFeeder.target = currentEGG;

                    break;

                case State.GO_TO_SEED:

                    pathFeeder.target = currentSEED;

                    break;

                case State.BRING_EGG:

                    currentEGG.transform.parent = transform;
                    transform.GetChild(0).tag = "DONE";
                    pathFeeder.target = GetTarget("hatchingPoint");

                    break;

                case State.BRING_SEED:

                    currentSEED.transform.parent = transform;
                    transform.GetChild(0).tag = "DONE";
                    pathFeeder.target = GetTarget("storePoint");

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
                case "wayPoint":

                     randomTarget = Random.Range(0, blackboard.WayPoints.Length);
                    target = blackboard.WayPoints[randomTarget];

                    break;

                case "storePoint":

                    randomTarget = Random.Range(0, blackboard.StorePoints.Length);
                    target = blackboard.StorePoints[randomTarget];

                    break;

                case "hatchingPoint":

                    randomTarget = Random.Range(0, blackboard.HatchingPoints.Length);
                    target = blackboard.HatchingPoints[randomTarget];

                    break;
            }

            

            return target;
        }

    }
}
