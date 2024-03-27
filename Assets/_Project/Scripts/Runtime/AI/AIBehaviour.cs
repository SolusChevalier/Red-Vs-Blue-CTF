using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace CTF
{
    public class AIBehaviour : MonoBehaviour
    {
        #region FIELDS

        private NavMeshAgent agent;
        private NodeAbs behaviorTree;
        private bool isHostile = false;

        public Transform AISpawn;
        public Transform AIFlagTrans;
        public Transform AIFlagSpawn;
        public Transform AIFlagZone;
        public Transform PlayerTrans;

        public float EvalDelay = 2.0f;
        private float EvalTime = 0f;
        public float PickupRange = 1.0f;

        public GameObject BulletPrefab;
        public Transform AIBulletSpawn;
        public float AttackRange = 10f;
        public float shootingCooldown = 2f;
        public float CombatSeperation = 10f;

        #endregion FIELDS

        #region UNITY METHODS

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            InitBehaviorTree();
        }

        private void Update()
        {
            if (Time.time >= EvalTime)
            {
                behaviorTree.Eval();
                EvalTime = Time.time + EvalDelay;
            }
        }

        #endregion UNITY METHODS

        #region Initialization

        private void InitBehaviorTree()
        {
            behaviorTree = new Selector(new List<NodeAbs> { CreateFlagCarrierBehavior(), CreateFlagSeekerBehavior() });
        }

        #endregion Initialization

        #region Behavior Tree Nodes

        private NodeAbs CreateFlagCarrierBehavior()
        {
            return new Sequence(new List<NodeAbs> { new ConditionNode(HasFlag), new Selector(new List<NodeAbs> { CreateBlockedPathBehavior(), CreateClearPathBehavior() }) });
        }

        private NodeAbs CreateBlockedPathBehavior()
        {
            return new Sequence(new List<NodeAbs> { new ConditionNode(() => CheckPath(AISpawn.position)), new ActionNode(EngagePlayerOrReevaluate) });
        }

        private NodeAbs CreateClearPathBehavior()
        {
            return new Sequence(new List<NodeAbs> { new ConditionNode(() => !CheckPath(AISpawn.position)), new ActionNode(ReturnFlag) });
        }

        private NodeAbs CreateFlagSeekerBehavior()
        {
            return new Sequence(new List<NodeAbs> { new ConditionNode(() => !HasFlag()), new Selector(new List<NodeAbs> { CreateFlagCaptureBehavior(), CreateEngagementBehavior() }) });
        }

        private NodeAbs CreateFlagCaptureBehavior()
        {
            return new Sequence(new List<NodeAbs> { new ConditionNode(() => !CheckPath(AIFlagTrans.position)), new ActionNode(DefendFlag) });
        }

        private NodeAbs CreateEngagementBehavior()
        {
            return new Sequence(new List<NodeAbs> { new ConditionNode(() => CheckPath(AIFlagTrans.position) || CanShootPlayer()), new ActionNode(BecomeHostile) });
        }

        #endregion Behavior Tree Nodes

        #region AI Actions

        private bool HasFlag() => AIFlagZone.childCount > 0;

        private bool ReturnFlag()
        {
            if (!HasFlag()) return false;
            agent.SetDestination(AISpawn.position);
            if (WithinRange(AISpawn.position, PickupRange))
            {
                ResetFlag();
                return true;
            }
            return false;
        }

        private bool DefendFlag()
        {
            agent.SetDestination(AIFlagTrans.position);
            if (WithinRange(AIFlagTrans.position, PickupRange))
            {
                GameObject flag = GameObject.FindGameObjectWithTag("RedFlag");
                flag.transform.SetParent(AIFlagZone);
                return true;
            }
            return false;
        }

        private bool EngagePlayerOrReevaluate()
        {
            isHostile = true;
            FollowPlayer();
            if (!HasFlag())
            {
                isHostile = false;
                return false;
            }
            if (!CheckPath(AISpawn.position))
            {
                isHostile = false;
                return ReturnFlag();
            }

            return true;
        }

        private bool BecomeHostile()
        {
            isHostile = true;
            if (CanShootPlayer())
                Shoot();
            FollowPlayer();
            return true;
        }

        #endregion AI Actions

        #region Helper Methods

        private bool WithinRange(Vector3 pos, float dis) => Vector3.Distance(transform.position, pos) < dis;

        private bool CanShootPlayer() => Vector3.Distance(transform.position, PlayerTrans.position) <= AttackRange;

        private void ResetFlag()
        { foreach (Transform child in AIFlagZone) { child.SetParent(null); } }

        private void AttackPlayer()
        {
            if (CanShootPlayer() && isHostile)
            {
                GameObject Bullet = Instantiate(BulletPrefab, AIBulletSpawn.position, Quaternion.LookRotation(PlayerTrans.position - AIBulletSpawn.position));
                Bullet.GetComponent<Rigidbody>().velocity = (PlayerTrans.position - AIBulletSpawn.position).normalized * 20f;
            }
        }

        private bool CheckPath(Vector3 Target)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(Target, path);
            return path.status != NavMeshPathStatus.PathComplete;
        }

        private void Shoot()
        {
            if (CanShootPlayer())
            {
                Vector3 directionToPlayer = PlayerTrans.position - transform.position;
                directionToPlayer.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 10f);
                StartCoroutine(Fire());
            }
        }

        private IEnumerator Fire()
        {
            agent.isStopped = true;
            AttackPlayer();
            yield return new WaitForSeconds(0.2f);
            agent.isStopped = false;
        }

        private void FollowPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, PlayerTrans.position);
            if (distanceToPlayer > CombatSeperation)
            {
                Vector3 closerPosition = Vector3.MoveTowards(transform.position, PlayerTrans.position, 2f);
                agent.SetDestination(closerPosition);
            }
            else if (distanceToPlayer < CombatSeperation)
            {
                Vector3 backAwayPosition = Vector3.MoveTowards(transform.position, PlayerTrans.position, -2f);
                agent.SetDestination(backAwayPosition);
            }
        }

        #endregion Helper Methods
    }
}