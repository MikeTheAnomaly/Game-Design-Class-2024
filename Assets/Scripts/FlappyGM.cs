using UnityEngine;
public class FlappyGM : MonoBehaviour
{
    public GameObject hurdle;
    public Vector2 randomDistanceOnY = new Vector2(-3,3);
    public float spawnTime = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        float yPos = transform.position.y + Random.Range(randomDistanceOnY.x, randomDistanceOnY.y);
        Vector3 spawnPos = new Vector3(transform.position.x, yPos , transform.position.z);
        Instantiate(hurdle, spawnPos, Quaternion.identity);
    }
}
