public interface ExampleDamageable{
    void TakeDamage(float damage);
    ExampleHealth Health { get; }
    public ExampleTeam Team { get;} 
}