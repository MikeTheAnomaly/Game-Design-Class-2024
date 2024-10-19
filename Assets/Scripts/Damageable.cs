public interface Damageable{
    void TakeDamage(float damage);
    Health Health { get; }
    public Team Team { get;} 
}