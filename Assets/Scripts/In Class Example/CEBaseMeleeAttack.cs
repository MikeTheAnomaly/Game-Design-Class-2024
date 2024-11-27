using UnityEngine;

public class CEBaseMeleeAttack : MonoBehaviour
{
    public float damage = 10.0f;

    public CETeam team = new(CETeamType.Team2);


    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CEDamageable damageableTarget))
        {
            if (damageableTarget.Team.teamType != team.teamType)
            {
                damageableTarget.TakeDamage(damage);
            }
        }
    }

}