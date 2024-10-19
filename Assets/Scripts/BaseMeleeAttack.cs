using UnityEngine;

public class BaseMeleeAttack : MonoBehaviour
{
    public float damage = 10.0f;

    public Team team = new(TeamType.Team2);


    public RandomSoundPlayer attackSoundPlayer;

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
        if (other.TryGetComponent(out Damageable damageable))
        {
            if (damageable.Team.teamType != team.teamType)
            {
                damageable.TakeDamage(damage);
                attackSoundPlayer.PlayRandomSound();
            }
        }
    }
}
