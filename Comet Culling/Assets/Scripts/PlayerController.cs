using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    // A variable to test data permanence
    int testVariable;
    string testVariableText;
    [SerializeField] TextMeshProUGUI testVariableUI;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize member objects
        rb = GetComponent<Rigidbody2D>();

        if (DataPermanence.Instance != null)
            testVariable = DataPermanence.Instance.testVariablePlayer;
        else
            testVariable = 0;

    }

    // Update is called once per frame
    // Used to read input, perform calculations, set states etc
    void Update()
    {
        // Read directional input and set the movement vector
        // Normalize to reduce increased speed when moving diagonally
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Increment and display the test variable for data permanence
        DebugDisplayTestVariable();
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

    // Used to display the test variable when running a build of the game to check for data permanence between scenes
    void DebugDisplayTestVariable()
    {
        // Increment test variable 
        if (Input.GetButtonDown("Debug Increment"))
            testVariable++;

        // Save the test variable to the data permanence instance
        DataPermanence.Instance.testVariablePlayer = testVariable;

        // Display the test variable as UI
        testVariableText = testVariable.ToString();
        testVariableUI.text = testVariableText;
    }
}