using UnityEngine;

public class ExampleBaseMeleeAttack : MonoBehaviour
{
    public float damage = 10.0f;

    public ExampleTeam team = new(TeamType.Team2);


    public ExampleRandomSoundPlayer attackSoundPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ExampleDamageable damageable))
        {
            if (damageable.Team.teamType != team.teamType)
            {
                damageable.TakeDamage(damage);
                attackSoundPlayer.PlayRandomSound();
            }
        }
    }
}
