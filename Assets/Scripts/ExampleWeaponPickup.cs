public class ExampleWeaponPickup : ExamplePickup
{
    public ExampleBaseWeapon WeaponPrefab;
    internal override void OnPickup(ExamplePlayer player)
    {
        ExampleBaseWeapon Weapon = Instantiate(WeaponPrefab);
        player.SetWeapon(Weapon);
        Destroy();
    }
}