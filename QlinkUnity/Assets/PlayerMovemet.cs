using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovemet : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed;
    public float groundDrag;

    [Header("Ground Check")] 
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    public Transform orientation;

    private float horizontalInput;
    private float verticalinput;

    private Vector3 moveDirection;

    private Rigidbody playerBody;

    // Start is called before the first frame update
    private void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        playerBody.freezeRotation = true;
    }

    private void playerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalinput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        //calculate movement direction -- always facing towards where player body moves
        moveDirection = orientation.forward * verticalinput + orientation.right * horizontalInput;

        playerBody.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Update is called once per frame
    private void Update()
    {
        //grounded check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        playerInput();

        //handle drag
        if (grounded)
        {
            playerBody.drag = groundDrag;
            
        }
        else
        {
            playerBody.drag = 0;
        }
    }
}
