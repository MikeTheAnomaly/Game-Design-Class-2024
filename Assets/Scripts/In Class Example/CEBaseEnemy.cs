using UnityEngine;
using UnityEngine.AI;

public class CEBaseEnemy : MonoBehaviour, CEDamageable
{
    public Transform target;
    public float moveSpeed = 5.0f;

    private NavMeshAgent agent;

    private CEHealth _health = new(20);
    public CEHealth Health {get {return _health;} set{_health = value;}}

    [SerializeField]
    public CETeam _team = new(CETeamType.Team2);
    public CETeam Team {get{return _team;} set{_team = value;}}

    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //This directly updates the position of the enemy to the target
        //Does not account for physics
        agent.SetDestination(target.position);
    }

    void LateUpdate(){
        if(agent.hasPath && agent.remainingDistance <= agent.stoppingDistance){
            animator.SetTrigger("Attack");
        }
    }

    public void TakeDamage(float damage)
    {
        Health.TakeDamage(damage);
    }


}
