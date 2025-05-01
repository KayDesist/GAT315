using UnityEngine;

public class CollisionInfo2D : MonoBehaviour
{
    Material material;
    Color color;

    void Start()
    {
        material = GetComponent<Renderer>().material; 
        color= material.color;

    }

    private void OnTriggerEnter(Collider other)
    {
        //if(CompareTag)
    }


}

