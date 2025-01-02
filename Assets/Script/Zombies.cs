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
    private readonly int moveSpeed = Animator.StringToHash("Movement");
    private readonly int targetDistance = Animator.StringToHash("TargetDistance");
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
    }

    private void Update()
    {

        animator.SetFloat(moveSpeed, agent.velocity.magnitude / agent.speed);
        //SetSight();
        Vector3 myPos = transform.position + Vector3.up * 0.5f;

        float lookingAngle = transform.eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + ViewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - ViewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);

        if (hitTargetList.Count <= 0)
        {
            Collider[] Targets = Physics.OverlapSphere(myPos, ViewRadius, TargetMask);

            if (Targets.Length == 0) return;
            foreach (Collider EnemyColli in Targets)
            {
                Vector3 targetPos = EnemyColli.transform.position;
                Vector3 targetDir = (targetPos - myPos).normalized;
                float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
                if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, ViewRadius, ObstacleMask))
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
        if (!agent.pathPending && agent.remainingDistance < 1)
            SetWayPoint();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().isDead = true;
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
