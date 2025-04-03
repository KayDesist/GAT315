using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float jumpHeight = 2;
    [SerializeField] LayerMask layerMask = Physics.AllLayers;

    Rigidbody rb;
    Vector3 force;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Vector3 direction = Vector3.zero;
        direction.x = Input.GetAxis("Horizontal");
        direction.x = Input.GetAxis("Vertical");

        force = direction * speed;

        if (Input.GetKey(KeyCode.Space)) {
            rb.AddForce(Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * jumpHeight),ForceMode.Impulse);
        }

        var colliders= Physics.OverlapSphere(transform.position, 2, layerMask);
        foreach (var collider in colliders)
        { 
            Destroy(collider.gameObject);
        } 

        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 5, layerMask))
        {
            Destroy(hit.collider.gameObject.gameObject);
        }

    }


    private void FixedUpdate()
    {
        rb.AddForce(force, ForceMode.Force);
        rb.AddTorque(Vector3.up);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
    }
}
