using UnityEngine;
using UnityEngine.Events;

public class PricedPickUp : Pickup
{
    public int price;
    public UnityEvent OnPurchase = new UnityEvent();

    internal override void OnPickup(Player player)
    {
        if(GameManager._GameManager.candyCornCount >= this.price){
            GameManager._GameManager.candyCornCount -= this.price;
            OnPurchase.Invoke();
            Destroy();
        }
    }

}