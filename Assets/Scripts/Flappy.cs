using UnityEngine;

public class Flappy : MonoBehaviour
{
    public Rigidbody rb;

    public float forceUp = 100;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 force = new Vector3(0, forceUp, 0);
            rb.AddForce(force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Hurdle")
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
