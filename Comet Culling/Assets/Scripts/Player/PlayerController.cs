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
    public bool canMove { set; private get; } = true;

    // CROP RELATED VARIABLES
    [Header("Crop Variables")]
    [SerializeField] GameObject crop;
    int testCropsHarvested = 0;

    // STAMINA RELATED VARIABLES
    public int stamina;
    int MAX_STAMINA = 10; 

    // INTERRACTION/ACTION VARIABLES
    [SerializeField] Transform raycastEnd;

    // Store the tools as an enum to know which one to use
    enum Tools
    {
        hoe = 0,
        seed = 1,
        wateringCan = 2,
        scythe = 3
    }

    // The currently equipped tool
    // Determines which action to perform
    Tools currentTool = Tools.hoe;

    // BASIC TEST UI
    // Mostly for debugging/checking things are working
    string testUIText;
    [SerializeField] TextMeshProUGUI testUI;
    string currentToolString;
    [SerializeField] TextMeshProUGUI currentToolUI;
    string staminaText;
    [SerializeField] TextMeshProUGUI staminaTextUI;
    string spaceshipEnergyText;
    [SerializeField] TextMeshProUGUI spaceshipEnergyUI;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize member objects and components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();

        stamina = MAX_STAMINA; 
        // Initialize variables stored in data permanence
        if (DataPermanence.Instance != null)
        {
            // Set the player position when entering a new scene
            rb.MovePosition(DataPermanence.Instance.playerStartPosition);
            // Set the player crops harvested
            testCropsHarvested = DataPermanence.Instance.testCropsHarvested;
        }

        // Can delete this once figured out a way to access spaceship from the start
        spaceshipEnergyText = "Spaceship Energy: " + 100;
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

        // Cycle through the enum of tools
        // If at either end of the list, cycle around to the other end
        if(Input.GetButtonDown("Tools Left"))
        {
            // If the current weapon is the lowest tool in the enums
            // Then change it to the highest tool in the enums
            // Otherwise, go to the next one down
            if (currentTool <= 0)
                currentTool = (Tools)System.Enum.GetValues(typeof(Tools)).Length - 1;
            else
                currentTool--;
        }
        if (Input.GetButtonDown("Tools Right"))
        {
            // If the current weapon is the highest tool in the enums
            // Then change it to the lowest tool in the enums
            // Otherwise, go to the next one up
            if (currentTool >= (Tools)System.Enum.GetValues(typeof(Tools)).Length - 1)
                currentTool = 0;
            else
                currentTool++;
        }

        // Perform an action
        if (Input.GetButtonDown("Action"))
            PerformAction();

        // Animator parameters

        // Set the movement animation parameter to detect any movement of the rigidbody
        animator.SetFloat("Movement", rb.velocity.magnitude);
        // Set the player direction parameter
        animator.SetFloat("Vertical Direction", directionAnimatorParameter);

        // Set the variables for the test UI
        TestUI();
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

    // Perform an action depending on the tool equipped and 
    // the type of object interracted with
    void PerformAction()
    {
        Vector2 raycastDirection = raycastEnd.transform.position - transform.position;
        RaycastHit2D[] rayCast = Physics2D.RaycastAll(transform.position, raycastDirection, 0.6f);

        // Cycle through all hits from the boxcast
        // check to see if any the object hit has any tag
        // Use this to determine what actions to perform depending on what tool the player has activated
        foreach (RaycastHit2D hit in rayCast)
        {
            string tag = hit.transform.gameObject.tag;

            switch(tag)
            {
                case "Crop":
                    // If the raycast hits a crop, access its crop controller
                    CropController cropController = hit.transform.gameObject.GetComponent<CropController>();

                    // Performs an action on the crop depending on what tool is equipped
                    switch(currentTool)
                    {
                        // If the watering can is equipped, water the crop
                        case Tools.wateringCan:

                            // Set the crop status to watereds
                            cropController.isWatered = true;

                            // Trigger the watering animation
                            animator.SetTrigger("Watering");

                            // Lower the stamina from the action
                            stamina -= 10;

                            break;

                        // If the scythe is equipped, harvest the crop
                        case Tools.scythe:

                            // If the crop is fully  grown, harvest it
                            if (cropController.isGrown)
                            {
                                // Destroy the game object
                                Destroy(hit.transform.gameObject);
                                // Increase the amount of harvested crops
                                testCropsHarvested++;
                                DataPermanence.Instance.testCropsHarvested++;

                                // Reset the crop's tile to untilled dirt
                                TilemapManager.Instance.ResetTile(transform.position);

                                // Trigger the harvesting animation
                                animator.SetTrigger("Harvesting");

                                // Lower the stamina from the action
                                stamina -= 10;

                            }
                            break;
                    }

                    break;
                case "Dirt Tile":

                    switch(currentTool)
                    {
                        // If the hoe is equipped, till the ground
                        case Tools.hoe:

                            // Check that the tile isn't already tilled
                            if (!TilemapManager.Instance.IsTilled(transform.position))
                            {
                                // Set the dirt tile to tilled
                                TilemapManager.Instance.TillDirt(transform.position);

                                // Trigged the tilling animation
                                animator.SetTrigger("Tilling");

                                // Lower the stamina from the action
                                stamina -= 10;
                            }

                            break;
                        
                        // If seeds are equipped, plant a crop
                        case Tools.seed:
                            
                            // Plant a crop on the tile at the location of the player
                            TilemapManager.Instance.PlantCrop(transform.position);

                            // Check that the tile is tilled, then trigger the planting animation
                            if (TilemapManager.Instance.IsTilled(transform.position))
                                animator.SetTrigger("Planting");

                            // Lower the stamina from the action
                            stamina -= 10;

                            break;
                    }

                    break;
                case "Generator":
                    
                    //You can initialize staminarecharge (or any object belonging to a specific class) by calling this statement
                    //Which essentially makes this instance be able to access the scripts attached to that object
                    //Think of it like a pointer 
                    //Tldr if i hit the generator and it has the staminarecharge component, this statement will be valid
                    SpaceshipController spaceshipController = hit.transform.gameObject.GetComponent<SpaceshipController>();

                    Debug.Log("Spaceship energy: " + spaceshipController.spaceshipEnergy);
                    Debug.Log("Stamina value: " + stamina);
                    spaceshipController.ChargePlayer(ref stamina);
                   
                    Debug.Log("Spaceship energy: " + spaceshipController.spaceshipEnergy);
                    Debug.Log("Stamina value: " + stamina);

                    // Set the spaceship energy to a string
                    spaceshipEnergyText = "Spaceship Energy: " + spaceshipController.spaceshipEnergy.ToString();

                    break;
                // TODO: Add engine/spaceship stuff for codependency system
            }
        }
    }



    // Used to display any variables to the screen in place of UI for now
    // Can add more variables to this as/when we need them and delete it when we have something better
    void TestUI()
    {
        // Display the test variable as UI
        testUIText = "Crops Held: " + testCropsHarvested.ToString();
        testUI.text = testUIText;

        currentToolString = "Tool Equipped: " + currentTool.ToString();
        currentToolUI.text = currentToolString;

        staminaText = "Player Stamina: " + stamina.ToString();
        staminaTextUI.text = staminaText;

        spaceshipEnergyUI.text = spaceshipEnergyText;
    }
}