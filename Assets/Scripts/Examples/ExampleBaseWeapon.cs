using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class ExampleBaseWeapon : MonoBehaviour
{
    //rate of fire for the weapon in bullets per second
    public string weaponName = "Base Weapon";
    public float fireRate = 4.0f;
    public float damage = 10.0f;
    public float bulletSpeed = 100.0f;
    public GameObject bulletPrefab;
    public Transform barrelEnd;
    
    public ObjectPool<ExampleBaseBullet> bulletPool;
    private float nextFireTime = 0.0f;
    private bool fireHeld;

    public LayerMask bulletCollisionMask;
    public GameObject owner;

    public UnityEvent OnFire = new UnityEvent();

    public void Awake(){
        bulletPool = new ObjectPool<ExampleBaseBullet>(() => {
            GameObject bullet = Instantiate(bulletPrefab);
            ExampleBaseBullet baseBullet = bullet.GetComponent<ExampleBaseBullet>();
            baseBullet.collisionMask = bulletCollisionMask;
            baseBullet.owner = owner;
            baseBullet.OnDestroy.AddListener(() => bulletPool.Release(baseBullet));
            bullet.SetActive(false);

            return bullet.GetComponent<ExampleBaseBullet>();
        }, defaultCapacity: 50);
    }

    public virtual void StartFire(){
        fireHeld = true;
        TryFire();
    }

    public virtual void StopFire(){
        fireHeld = false;
    }

    // Virtual method to fire the weapon
    internal virtual void TryFire()
    {
        if (Time.time > nextFireTime && bulletPrefab != null && barrelEnd != null && fireHeld)
        {
            OnFire.Invoke();
            GameObject bullet = bulletPool.Get().gameObject; 
            bullet.transform.position = barrelEnd.position;
            bullet.transform.rotation = barrelEnd.rotation;
            bullet.SetActive(true);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = barrelEnd.forward * bulletSpeed;
            }
            nextFireTime = Time.time + 1.0f / fireRate;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(fireHeld){
            TryFire();
        }
    }
}
