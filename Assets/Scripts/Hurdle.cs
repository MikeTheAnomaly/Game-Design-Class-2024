using UnityEngine;

public class Hurdle : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 velocity = new Vector3(-5,0,0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = velocity;
    }
}
