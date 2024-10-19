using UnityEngine;
using UnityEngine.Events;

public class ExamplePricedPickUp : ExamplePickup
{
    public int price;
    public UnityEvent OnPurchase = new UnityEvent();

    internal override void OnPickup(ExamplePlayer player)
    {
        if(ExampleGameManager._GameManager.candyCornCount >= this.price){
            ExampleGameManager._GameManager.candyCornCount -= this.price;
            OnPurchase.Invoke();
            Destroy();
        }
    }

}