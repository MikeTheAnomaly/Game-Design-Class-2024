using UnityEngine;
using UnityEngine.AI;

public class ExampleEnemy : MonoBehaviour, ExampleDamageable
{
    public float moveSpeed = 5.0f;
    public Transform target;
    private NavMeshAgent agent;

    [SerializeField]
    private ExampleHealth _health = new(20); 
    public ExampleHealth Health {get{return _health;} set{_health = value;}}

    [SerializeField]
    public ExampleTeam _team = new(TeamType.Team2);
    public ExampleTeam Team {get{return _team;} set{_team = value;}}


    //Animation
    Animator anim;
    public Animator characterAnim;

    //sound
    public ExampleRandomSoundPlayer deathSoundPlayer;
    public ExampleRandomSoundPlayer hitSoundPlayer;
    void OnEnable(){
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        Health.OnDeath.AddListener(OnDeath);

        this.Team = new ExampleTeam(this.Team.teamType, this.gameObject);

        this.Health.OnHealthChanged.AddListener((ExampleHealth health) => {
            hitSoundPlayer.PlayRandomSound();
        });

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //This directly updates the position of the enemy to the target
        //Does not account for physics
        agent.SetDestination(target.position);
        UpdateAnimation();
        

        transform.LookAt(target);
    }

    void LateUpdate(){
        if (agent.hasPath && agent.remainingDistance < agent.stoppingDistance)
        {
            anim.SetTrigger("Attack");
        }
    }

    public void TakeDamage(float damage)
    {
        Health.TakeDamage(damage);
    }

    public void OnDeath()
    {
        //spawn a drop
        Drop randomDrop = ExampleGameManager._GameManager.GetWeightedRandomDrop();
        GameObject drop = Instantiate(randomDrop.dropPrefab, transform.position, Quaternion.identity);

        deathSoundPlayer.PlayRandomSound();
        //want to make sure any code resets for the next time this object is used
        agent.ResetPath();
        gameObject.SetActive(false);
        anim.ResetTrigger("Attack");
        anim.SetTrigger("Reset");
    }

    public void UpdateAnimation(){
        if(characterAnim != null){
            //create a normalized vector from the agent velocity in object space
            Vector3 localMoveDirection = transform.InverseTransformDirection(agent.velocity);
            //normalize based on agent speed
            localMoveDirection = localMoveDirection.normalized * (localMoveDirection.magnitude / moveSpeed);
            characterAnim.SetFloat("SpeedZ", localMoveDirection.z);
            characterAnim.SetFloat("SpeedX", localMoveDirection.x);

        }
    }

}
