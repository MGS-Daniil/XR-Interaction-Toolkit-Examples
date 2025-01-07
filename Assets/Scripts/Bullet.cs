using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;

    public void Fire(int speed)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * speed);
        print("Fire");
        Destroy(gameObject, 10);
    }


    private void OnCollisionEnter(Collision other)
    {
        print("bullet collision");
        // Destroy(gameObject);
    }
}