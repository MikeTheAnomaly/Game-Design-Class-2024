using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour, Damageable
{

    [SerializeField]
    private Health _health = new(100);
    public Health Health { get { return _health; } set { _health = value; } }


    [SerializeField]
    public Team _team = new(TeamType.Team1);
    public Team Team { get { return _team; } set { _team = value; } }

    public float speed = 5.0f;
    private Vector2 moveDirection = Vector2.zero;
    //look direction is a vector2 because we only care about x and z
    //goes from -1 to 1 in both directions
    private Vector2 lookDirection = Vector2.zero;
    private Rigidbody rb;

    public BaseWeapon weapon;
    private bool isMouse;

    public Animator anim;
    public Transform weaponPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Health.OnDeath.AddListener(OnDeath);
        if (weapon != null)
        {
            weapon.owner = gameObject;
        }

        this.Team = new Team(this.Team.teamType, this.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMovement();
        UpdateAnimation();
    }

    void HandleMovement()
    {
        //Bad doesnt account for other physics objects
        //rb.linearVelocity = new Vector3(this.moveDirection.x * this.speed, rb.linearVelocity.y, this.moveDirection.y * this.speed);
        //Good
        if (this.moveDirection.magnitude > 0.1f) // Deadzone check
        {
            rb.MovePosition(rb.position + new Vector3(this.moveDirection.x * this.speed, 0, this.moveDirection.y * this.speed) * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
        rb.rotation = Quaternion.Euler(0, Mathf.Atan2(this.lookDirection.x, this.lookDirection.y) * Mathf.Rad2Deg, 0);

    }

    void UpdateAnimation()
    {
        if (anim != null)
        {
            //convert world space move direction to local space
            //since the animation will be in local space (locomotion)
            //this method uses the transform of the object to convert the world space vector to local space
            //alternatively you could calculate by rotating the vector by angle of the rotation vector
            Vector3 localMoveDirection = transform.InverseTransformDirection(new Vector3(moveDirection.x, 0, moveDirection.y));
            anim.SetFloat("SpeedZ", localMoveDirection.z);
            anim.SetFloat("SpeedX", localMoveDirection.x);
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        //normalize for diagonal movement
        input = input.normalized;
        this.moveDirection = input;
    }

    public void OnLook(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (isMouse)
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
        else if (input != Vector2.zero)
        {
            this.lookDirection = input;
        }
    }

    public void OnControlsChanged(PlayerInput playerInput)
    {
        //Debug.Log("Controls Changed " + playerInput.currentControlScheme);
        this.isMouse = playerInput.currentControlScheme == "Keyboard&Mouse";
    }

    public void SetWeapon(BaseWeapon weapon)
    {
        //if its the same weapon we dont want to do anything
        if (this.weapon.name == weapon.name){
            return;
        }

        if(this.weapon != null){
            Destroy(this.weapon.gameObject);
        }
        this.weapon = weapon;
        this.weapon.transform.parent = this.weaponPosition;
        this.weapon.transform.localPosition = Vector3.zero;
        this.weapon.transform.localRotation = Quaternion.identity;
        weapon.owner = gameObject;
    }

    public void OnFire(InputValue inputValue)
    {
        float val = inputValue.Get<float>();
        if (weapon != null)
        {
            if (val > 0.0f)
            {
                weapon.StartFire();
            }
            else
            {
                weapon.StopFire();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Health.TakeDamage(damage);
    }

    public void OnDeath()
    {
        Debug.Log("Player Died");
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickup")
        {
            other.GetComponent<Pickup>().OnPickup(this);
        }
    }

}
