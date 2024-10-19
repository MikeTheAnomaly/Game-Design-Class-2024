public class ExampleCandyCorn : ExamplePickup{
    internal override void OnPickup(ExamplePlayer player){
        ExampleGameManager._GameManager.AddCandyCorn();
        Destroy();
    }
}