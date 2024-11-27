
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class CEPlayer : MonoBehaviour, CEDamageable
{

    public Rigidbody rb;

    public Vector2 moveDirection;
    public float moveSpeed = 5f;
    public Vector2 lookDirection;
    public bool isMouse;

    [SerializeField]
    private CEHealth _health = new(100);
    public CEHealth Health {get {return _health;} set{_health = value;}}


    [SerializeField]
    public CETeam _team = new(CETeamType.Team1);
    public CETeam Team {get{return _team;} set{_team = value;}}

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
    }

    public void TakeDamage(float damage)
    {
        Health.TakeDamage(damage);
    }

    private void HandleMovement()
    {

        if (this.moveDirection.magnitude > .1f)
        {
            rb.MovePosition(rb.position + new Vector3(this.moveDirection.x * moveSpeed, 0, this.moveDirection.y * moveSpeed) * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }

        rb.rotation = Quaternion.Euler(0, Mathf.Atan2(this.lookDirection.x, this.lookDirection.y) * Mathf.Rad2Deg, 0);
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        this.moveDirection = input.normalized;

    }

    public void OnLook(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (!this.isMouse)
        {
            input = input.normalized;
            this.lookDirection = input;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(input);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float hitInfo))
            {
                Vector3 lookPoint = ray.GetPoint(hitInfo);
                Vector3 direction = lookPoint - rb.position;
                this.lookDirection = new Vector2(direction.x, direction.z).normalized;
            }
        }
    }

    public void OnControlsChanged(PlayerInput input)
    {
        Debug.Log("Controls Changed");
        this.isMouse = input.currentControlScheme == "Keyboard&Mouse";
    }
}
