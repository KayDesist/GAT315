using UnityEngine;

public class PointEffector : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField, Range(0f, 10f)] private float radius = 1f;

    void Start()
    {
        transform.localScale = new Vector3(radius, radius, radius);
    }

    private void OnTriggerStay(Collider other)
    {
        // Calculate direction from the point effector to the player
        Vector3 distance = other.transform.position - transform.position;
        Vector3 direction = distance.normalized;
        // Apply force away the point effector
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float powerScale = Mathf.Abs((1 - distance.magnitude) / radius);
            float relativeForce = force * powerScale;
            rb.AddForce(direction * relativeForce, ForceMode.Force);
        }
    }

}



