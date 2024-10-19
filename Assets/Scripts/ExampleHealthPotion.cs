public class ExampleHealthPotion : ExamplePickup{

    internal override void OnPickup(ExamplePlayer player)
    {
        player.Health.ResetHealth();
        Destroy();
    }
}