using UnityEngine;

public abstract class Pickup : MonoBehaviour
{

    public bool hasTimeToLive = true;
    public float timeToLive = 10.0f;
    
    abstract internal void OnPickup(Player player);

    internal void Destroy()
    {
        Destroy(gameObject);
    }

    void OnEnable()
    {
        if (hasTimeToLive)
        {
            Invoke("Destroy", timeToLive);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            OnPickup(player);
        }
    }


}
