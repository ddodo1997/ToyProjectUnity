using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombies : MonoBehaviour
{
    [Range(0f, 360f)][SerializeField] float ViewAngle = 0f;
    [SerializeField] float ViewRadius = 1f;
    [SerializeField] LayerMask TargetMask;
    [SerializeField] LayerMask ObstacleMask;
    [SerializeField] List<Collider> hitTargetList = new List<Collider>();

    public List<Vector3> points = new List<Vector3>();
    public GameManager manager;
    private readonly int moveSpeed = Animator.StringToHash("Movement");
    private readonly int targetDistance = Animator.StringToHash("TargetDistance");
    private readonly int targetIsDead = Animator.StringToHash("PlayerIsDead");
    private Animator animator;
    private NavMeshAgent agent;
    private int currnetWayPoint = 0;
    public int cntWayPoint;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        SetWayPoint();
    }

    private void OnEnable()
    {
        currnetWayPoint = 0;
        hitTargetList.Clear();
    }

    private void Update()
    {
        if (hitTargetList.Count != 0 && hitTargetList[0].GetComponent<Player>().isDead)
        {
            animator.SetBool(targetIsDead, hitTargetList[0].GetComponent<Player>().isDead);
            return;
        }

        animator.SetFloat(moveSpeed, agent.velocity.magnitude / agent.speed);

        Vector3 myPos = transform.position + Vector3.up * 0.5f;

        float lookingAngle = transform.eulerAngles.y;
        Vector3 lookDir = AngleToDir(lookingAngle);

        if (hitTargetList.Count <= 0)
        {
            Collider[] Targets = Physics.OverlapSphere(myPos, ViewRadius, TargetMask);

            foreach (Collider EnemyColli in Targets)
            {
                Vector3 targetPos = EnemyColli.transform.position;
                Vector3 targetDir = (targetPos - myPos).normalized;
                float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
                if (targetAngle <= ViewAngle && !Physics.Raycast(myPos, targetDir, ViewRadius, ObstacleMask))
                {
                    hitTargetList.Add(EnemyColli);
                }
            }
        }
        else
        {
            agent.speed = 10f;
            agent.SetDestination(hitTargetList[0].transform.position);
            animator.SetFloat(targetDistance, (hitTargetList[0].transform.position - transform.position).magnitude);
        }
    }

    private void FixedUpdate()
    {
        if (hitTargetList.Count != 0 && hitTargetList[0].GetComponent<Player>().isDead)
            return;

        if (!agent.pathPending && agent.remainingDistance < 1)
            SetWayPoint();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            manager.OnGameOver();
            var player = other.GetComponent<Player>();
            player.OnDie();
            player.transform.forward = (transform.position - player.transform.position).normalized; 
        }
    }

    private void SetWayPoint()
    {
        currnetWayPoint = (currnetWayPoint + 1) % points.Count;
        agent.SetDestination(points[currnetWayPoint]);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

}
