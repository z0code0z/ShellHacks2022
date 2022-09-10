using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject YRotator; //use this to rotate on y axis
    public GameObject ActualCamera; //use this to rotate on x axis
    public GameObject Player;

    public float CameraHeight = 1f;
    public float MouseSensitivity = 1;

    float RotationX;

    
    void Update()
    {

        

        //this rotates up and down
        RotationX += Input.GetAxisRaw("Mouse Y") * MouseSensitivity;
        RotationX = Mathf.Clamp(RotationX, -80f, 80f);
        Quaternion XRotation = Quaternion.AngleAxis(RotationX, -Vector3.right);
        ActualCamera.transform.localRotation = XRotation;

        //this rotates left and right
        float RotationY = Input.GetAxisRaw("Mouse X") * MouseSensitivity;
        Player.transform.Rotate(Vector3.up, RotationY, Space.World);
        



    }


}
