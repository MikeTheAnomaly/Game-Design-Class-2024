public interface CEDamageable{
    void TakeDamage(float damage);
    CEHealth Health { get; }
    public CETeam Team { get;} 
}