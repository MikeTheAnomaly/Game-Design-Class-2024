public class WeaponPickup : Pickup
{
    public BaseWeapon WeaponPrefab;
    internal override void OnPickup(Player player)
    {
        BaseWeapon Weapon = Instantiate(WeaponPrefab);
        player.SetWeapon(Weapon);
        Destroy();
    }
}