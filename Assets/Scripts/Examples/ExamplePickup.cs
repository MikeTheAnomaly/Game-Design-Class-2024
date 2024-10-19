using UnityEngine;

public abstract class ExamplePickup : MonoBehaviour
{

    public bool hasTimeToLive = true;
    public float timeToLive = 10.0f;
    
    abstract internal void OnPickup(ExamplePlayer player);

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
        if (other.TryGetComponent(out ExamplePlayer player))
        {
            OnPickup(player);
        }
    }


}
