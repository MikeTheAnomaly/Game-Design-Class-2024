public class HealthPotion : Pickup{

    internal override void OnPickup(Player player)
    {
        player.Health.ResetHealth();
        Destroy();
    }
}