// Contains all player functionality including movement and interactions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
{
    // Member objects and components
    Rigidbody2D rb;
    Animator animator;
    BoxCollider2D box;

    // MOVEMENT VARIABLES
    [Header("Movement Variables")]
    // Control player speed
    [SerializeField] float movementSpeed = 20f;
    [SerializeField] float maximumSpeed = 15f;
    // Read the movement direction from player input
    Vector2 direction;
    // Used to flip certain animations depending on the player's direction
    bool facingRight = true;
    // Used in the animator blend tree to determine the direction the player faces
    float directionAnimatorParameter;
    // Restrict player movement in certain situations

    // COLLISIONS
    // A boxcast that returns all objects the player is touching
    // Boxcast is twice the size of the player. Will almost certainly change this to a raycast in the direction the player is facing
    //RaycastHit2D[] boxCast;
    public bool canMove { set; private get; } = true;

    //STAMINA & ENERGY RELATED VARIABLES & FUNCTIONS
    public const int MAX_STAMINA = 100;
    public int stamina
    {set;get;}

    


    // CROP RELATED VARIABLES
    [Header("Crop Variables")]
    [SerializeField] GameObject crop;
    int testCropsHarvested = 0;

    // BASIC TEST UI
    // Mostly for debugging/checking things are working
    string testUIText;
    //[SerializeField] TextMeshProUGUI testUI;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize member objects and components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();

        // Initialize variables stored in data permanence
        if (DataPermanence.Instance != null)
        {
            // Set the player position when entering a new scene
            rb.MovePosition(DataPermanence.Instance.playerStartPosition);
            // Set the player crops harvested
            testCropsHarvested = DataPermanence.Instance.testCropsHarvested;
        }
        //Debugging
        stamina = 0; 
    }

    // Update is called once per frame
    // Used to read input, perform calculations, set states etc
    void Update()
    {
        // Read directional input and set the movement vector
        // Normalize to reduce increased speed when moving diagonally
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Set the direction parameter based on the directional input
        // Only uses whole numbers to change the parameter - because the directional input is normalized, this ensures that diagonal movement (0.7, 0.7)
        // will keep the player facing in the current direction
        if (direction.y == 1)
            directionAnimatorParameter = 1;
        else if (direction.y == -1)
            directionAnimatorParameter = -1;
        else if (Mathf.Abs(direction.x) == 1)
            directionAnimatorParameter = 0;

        // Plant a new crop at the player current location
        if (Input.GetButtonDown("Plant Crop"))
            Instantiate(crop, transform.position, Quaternion.identity);

        // Water a crop
        if (Input.GetButtonDown("Use Item"))
            WaterCrop();

        // Harvest a crop
        if (Input.GetButtonDown("Harvest Crop"))
            HarvestCrop();

        
        // Animator parameters

        // Set the movement animation parameter to detect any movement of the rigidbody
        animator.SetFloat("Movement", rb.velocity.magnitude);
        // Set the player direction parameter
        animator.SetFloat("Vertical Direction", directionAnimatorParameter);

        // Set the variables for the test UI
        //TestUI();

       
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
        if (canMove)
            rb.AddForce(inputDirection * movementSpeed);
        // Stop movement when unable to move
        else
            rb.velocity = new Vector2(0f, 0f);

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

        if ((inputDirection.x < 0 && facingRight) || (inputDirection.x > 0 && !facingRight))
            FlipPlayer();
    }

    // Flip player on x axis to face left or right
    // Used to avoid duplicating sideways animations
    void FlipPlayer()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    // Harvest a crop if the player is in contact with it
    void HarvestCrop()
    {
   
        //// Cycle through all hits from the boxcast
        //// check to see if any have a crop tag
        //foreach (RaycastHit2D hit in boxCast)
        //    if (hit.collider.tag.Equals("Crop"))
        //    {
        //        // If the raycast hits a crop, check if it is fully grown
        //        CropController cropController = hit.transform.gameObject.GetComponent<CropController>();
        //        if(cropController.isGrown)
        //        {
        //            // If it's fully grown, destroy the game object and harvest the crop
        //            Destroy(hit.transform.gameObject);
        //            testCropsHarvested++;
        //            DataPermanence.Instance.testCropsHarvested++;
        //        }
        //    }
    }

    // Water a crop if the player is in contact with it
    void WaterCrop()
    {

        //// Cycle through all hits from the boxcast
        //// check to see if any have a crop tag
        //foreach (RaycastHit2D hit in boxCast)
        //    if (hit.collider.tag.Equals("Crop"))
        //    {
        //        // If the raycast hits a crop, water it
        //        CropController cropController = hit.transform.gameObject.GetComponent<CropController>();
        //        cropController.isWatered = true;

        //        // Trigger the watering animation
        //        animator.SetTrigger("Watering");
        //    }
    }

    //######################################################### SPACESHIP RECHARGE INTERACTION #######################################
    


    // Used to display any variables to the screen in place of UI for now
    //// Can add more variables to this as/when we need them and delete it when we have something better
    //void TestUI()
    //{
    //    // Display the test variable as UI
    //    testUIText = "Crops Held: " + testCropsHarvested.ToString();
    //    testUI.text = testUIText;
    //}
}