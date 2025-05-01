using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;//camera movement speed 
    public Vector3 offset; //the offset of the camera from the player 

    private void LateUpdate()
    {
        //Determine the desired position based on the players position and the offset
        Vector3 desiredPosition= player.position + offset;  

        //smootly move the camera torwards the desired position
        Vector3 smoothedPosition= Vector3.Lerp(desiredPosition, desiredPosition,smoothSpeed);

       smoothedPosition.z= transform.position.z; 
        transform.position= smoothedPosition;

    }
}
