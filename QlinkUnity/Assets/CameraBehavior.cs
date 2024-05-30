using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraBehavior : MonoBehaviour
{
    [Header("References")] 
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody playerBody;

    public float rotationSpeed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 viewDirection =
            player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDirection.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDirection = orientation.forward * horizontalInput + orientation.right * horizontalInput;

        if (inputDirection != Vector3.zero)
        {
            playerObj.forward =
                Vector3.Slerp(playerObj.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
