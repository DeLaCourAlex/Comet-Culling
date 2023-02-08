using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Member objects
    Rigidbody2D rb;

    // MOVEMENT VARIABLES
    [Header("Movement Variables")]
    // Control player speed
    [SerializeField] float movementSpeed = 20f;
    [SerializeField] float maximumSpeed = 15f;
    // Read the movement direction from player input
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize member objects
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    // Used to read input, perform calculations, set states etc
    void Update()
    {
        // Read directional input and set the movement vector
        // Normalize to reduce increased speed when moving diagonally
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        Debug.Log("Input Direction: " + direction);
    }

    // Use for all movement of physics bodies
    private void FixedUpdate()
    {
        CharacterMovement(direction);
    }

    // Move the player based on directional input
    void CharacterMovement(Vector2 inputDirection)
    {
        // Add force to move the player along both axes
        rb.AddForce(inputDirection * movementSpeed);

        // Stop the player exceeding maximum velocity
        // Because the normalized input direction returns around (.7, .7) when moving diagonally, diagonal movement is still faster
        // Multiplying by its absolute reduces this, keeping velocity consistent in all directions
        // Perhaps there's a more elegant solution to this but it works as needed for now
        if (Mathf.Abs(rb.velocity.magnitude) > maximumSpeed)
            rb.velocity = new Vector2(Mathf.Sign(inputDirection.x) * Mathf.Abs(inputDirection.x) * maximumSpeed, Mathf.Sign(inputDirection.y) * Mathf.Abs(inputDirection.y) * maximumSpeed);

        // Causes the movement to stop when releasing directional buttons
        if (inputDirection.x == 0)
            rb.velocity = new Vector2(0f, rb.velocity.y);
        if (inputDirection.y == 0)
            rb.velocity = new Vector2(rb.velocity.x, 0f);

        // TODO:
        // Animation for walk
        // Flip sprite direction between left and right

        // Debugging output
        Debug.Log("Speed magnitutde: " + rb.velocity.magnitude);
        //Debug.Log("Speed X: " + rb.velocity.x);
        //Debug.Log("Speed Y: " + rb.velocity.y);
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}