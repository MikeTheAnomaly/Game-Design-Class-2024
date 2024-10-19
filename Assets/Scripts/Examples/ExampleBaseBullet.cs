using UnityEngine;
using UnityEngine.Events;

public class ExampleBaseBullet : MonoBehaviour
{

    public UnityEvent OnDestroy;

    public float lifeTime = 5.0f;
    private float spawnTime;

    public LayerMask collisionMask;
    internal GameObject owner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        spawnTime = Time.time;
        GetComponent<Rigidbody>().includeLayers = collisionMask;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time - spawnTime > lifeTime)
        {
            gameObject.SetActive(false);
            OnDestroy.Invoke();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != owner)
        {

            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
                OnDestroy.Invoke();
            }
            if (collision.gameObject != owner && collision.gameObject.TryGetComponent(out ExampleDamageable damageable))
            {
                damageable.TakeDamage(10.0f);
            }
        }
    }
}
