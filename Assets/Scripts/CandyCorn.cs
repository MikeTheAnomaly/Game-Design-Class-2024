public class CandyCorn : Pickup{
    internal override void OnPickup(Player player){
        GameManager._GameManager.AddCandyCorn();
        Destroy();
    }
}