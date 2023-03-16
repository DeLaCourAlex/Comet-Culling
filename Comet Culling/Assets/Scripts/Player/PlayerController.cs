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
    [SerializeField] GameObject[] crops;
    int[] cropsHarvested;

    // STAMINA RELATED VARIABLES
    public int stamina;
    int MAX_STAMINA = 100;
    int staminaPerAction = 5;

    // INTERRACTION/ACTION VARIABLES
    [Header("Interaction/Action Variables")]
    [SerializeField] Transform raycastEnd;
    [SerializeField] GameObject tileSelectYes;
    [SerializeField] GameObject tileSelectNo;
    [SerializeField] GameObject cropPlant;
    int energyCropA = 5;
    int energyCropB = 15;
    bool carryCropA;
    bool carryCropB;
    // Used to alter the position of the raycast depending on the previous directions of the player
    // Ensures that if the player was facing the crops below them if they turn sideways, they will now be 
    // facing the crop to the bottom right, not directly to the right
    // This is a vector3 instead of a vector 2 to more easily alter the transform positions which are naturally vector3s
    Vector3 raycastCorrector = Vector3.zero;

    UI staminaUI;

    // Store the tools as an enum to know which one to use
    enum Tools
    {
        hoe = 0,
        seedA = 1,
        seedB = 2,
        wateringCan = 3,
        scythe = 4
    }

    // The currently equipped tool
    // Determines which action to perform
    Tools currentTool = Tools.hoe;

    // BASIC TEST UI
    // Mostly for debugging/checking things are working
    string cropAText;
    [SerializeField] TextMeshProUGUI cropAUI;
    string cropBText;
    [SerializeField] TextMeshProUGUI cropBUI;
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

        // Initialize member variables
        stamina = MAX_STAMINA;
        cropsHarvested = new int[2];

        // Initialize variables stored in data permanence
        if (DataPermanence.Instance != null)
        {
            // Set the player position when entering a new scene
            rb.MovePosition(DataPermanence.Instance.playerStartPosition);

            // Set the player member variables
            cropsHarvested = DataPermanence.Instance.cropsHarvested;
            stamina = DataPermanence.Instance.playerStamina;
        }
    }

    // Update is called once per frame
    // Used to read input, perform calculations, set states etc
    void Update()
    {
        // Read directional input and set the movement vector
        // Normalize to reduce increased speed when moving diagonally
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Set the raycast corrector based on the previous and current direction that the player is facing
        if(Mathf.Abs(directionAnimatorParameter) == 1 && direction.x != 0)
        {
            if (directionAnimatorParameter == 1)
                raycastCorrector = new Vector3(0, 0.5f, 0);
            else if (directionAnimatorParameter == -1)
                raycastCorrector = new Vector3(0, -0.5f, 0);
        }

        if (directionAnimatorParameter == 0 && direction.y != 0)
        {
            if (facingRight)
                raycastCorrector = new Vector3(0.5f, 0, 0);
            else if (!facingRight)
                raycastCorrector = new Vector3(-0.5f, 0, 0);
        }

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

        // Set whether the player is carrying a crop or not
        if (Input.GetButtonDown("No Crops"))
            ChangeCarriedCrops(false, false);
        if (Input.GetButtonDown("Crop A") && cropsHarvested[0] > 0)
            ChangeCarriedCrops(true, false);
        if (Input.GetButtonDown("Crop B") && cropsHarvested[1] > 0)
            ChangeCarriedCrops(false, true);

        // Perform any actions and update the player variables in data permanence
        PerformAction();
        DataPermanence.Instance.playerStamina = stamina;
        DataPermanence.Instance.cropsHarvested = cropsHarvested;

        // Animator parameters

        // Set the movement animation parameter to detect any movement of the rigidbody
        animator.SetFloat("Movement", rb.velocity.magnitude);
        // Set the player direction parameter
        animator.SetFloat("Vertical Direction", directionAnimatorParameter);
        // Set the crops that the player is or isn't carrying
        animator.SetBool("Holding Crop A", carryCropA);
        animator.SetBool("Holding Crop B", carryCropB);

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
        // Set the raycast direction depending on where the player is facing
        // First set the transforms to vector 2s
        Vector2 raycastDirection = (raycastEnd.transform.position) - (transform.position);
        RaycastHit2D[] rayCast = Physics2D.RaycastAll(raycastEnd.transform.position + raycastCorrector, raycastDirection, 0.2f);

        Debug.DrawRay(raycastEnd.transform.position + raycastCorrector, raycastDirection, Color.cyan, 1f, false);
        // Cycle through all hits from the boxcast
        // check to see if any the object hit has any tag
        // Use this to determine what actions to perform depending on what tool the player has activated
        foreach (RaycastHit2D hit in rayCast)
        {
            // Use the tag of the object hit to determine what actions can be performed
            string tag = hit.transform.gameObject.tag;

            // Used to correct for rounding down when converting position to ints
            // We want to round down, but setting a float to an int only removes after the decimal place
            // This causes the number to round up if the float value is negative
            int yCorrector = hit.point.y < 0 ? -1 : 0;
            int xCorrector = hit.point.x < 0 ? -1 : 0;

            // Set the raycast hit position to ints because that's what the SetTile function uses
            Vector3Int positionInt = new Vector3Int((int)hit.point.x + xCorrector, (int)hit.point.y + yCorrector, 0);

            // Set the position to display if we can interact or not
            // This needs to be in the middle of the tile, so we add 0.5 on each axis
            Vector2 displayPosition = new Vector2((float)positionInt.x + 0.5f, (float)positionInt.y + 0.5f);
            tileSelectNo.transform.position = displayPosition;
            tileSelectYes.transform.position = displayPosition;
            cropPlant.transform.position = displayPosition;

            // Access the necessary scripts based one what the raycast might have hit
            // If the raycast hits a crop, access its crop controller
            CropController cropController = hit.transform.gameObject.GetComponent<CropController>();
            // If the raycast hit a generator/bed, access its spaceship controller
            SpaceshipController spaceshipController = hit.transform.gameObject.GetComponent<SpaceshipController>();

            // Can not perform actions whilst carrying crops or interacting with grass tiles
            if (carryCropA || carryCropB || tag == "Generator" || tag == "Grass Tile" || tag == "Untagged")
            {
                DisplayCanInteract(false, false, false);

                // Generator related interactions
                if (Input.GetButtonDown("Action") && tag == "Generator")
                    SpaceshipInteraction(ref spaceshipController);
            }
                
            // Otherwise, perform an action based on equipped tool and what's being interacted with
            else
            {
                switch (currentTool)
                {
                    // Seed A is equipped
                    case Tools.seedA:

                        // Display where crop A will be planted and plant them
                        UseSeed(displayPosition, 0);

                        break;

                    // Seed B is equipped
                    case Tools.seedB:

                        // Display where crop B will be planted and plant them
                        UseSeed(displayPosition, 1);

                        break;

                    // Hoe is equipped 
                    case Tools.hoe:

                        // If interacting with a dirt tile, till it
                        if(tag == "Dirt Tile")
                        {
                            Hoe(positionInt);
                            return;
                        }
                        else
                            DisplayCanInteract(false, true, false);

                        break;

                    // Watering can is equipped 
                    case Tools.wateringCan:

                        // If interacting with an unwatered crop, water it
                        if(tag == "Crop")
                        {
                            WateringCan(ref cropController);
                            return;
                        }
                        else
                            DisplayCanInteract(false, true, false);

                        break;

                    case Tools.scythe:

                        // If interacting with a grown crop, harvest it
                        if (tag == "Crop")
                        {
                            Scythe(ref cropController, hit, positionInt, cropController.elementNumber);
                            return;
                        }
                        else
                            DisplayCanInteract(false, true, false);

                        break;
                }
            }
        }
    }

    // Interaction using the watering can with a crop
    void WateringCan(ref CropController crop)
    {
        // If the crop is watered, we can water it
        if (!crop.isWatered && !crop.isGrown)
        {
            // Show that we can perform this interaction
            DisplayCanInteract(true, false, false);

            // Perform the interaction
            if (Input.GetButtonDown("Action") && stamina >= staminaPerAction)
            {
                // Set the crop status to watereds
                crop.isWatered = true;

                // Trigger the watering animation
                animator.SetTrigger("Watering");

                // Lower the stamina from the action
                stamina -= staminaPerAction;
            }
        }
        else
            // Show that we can't perform this interaction
            DisplayCanInteract(false, true, false);
    }

    // Interaction using the scythe with a crop
    void Scythe(ref CropController crop, RaycastHit2D rayHit, Vector3Int pos, int cropType)
    {
        // If the crop is fully  grown, we can harvest it
        if (crop.isGrown)
        {
            // Show that we can perform this interaction
            DisplayCanInteract(true, false, false);

            // Perform the interaction
            if (Input.GetButtonDown("Action") && stamina >= staminaPerAction)
            {
                // Destroy the game object
                Destroy(rayHit.transform.gameObject);
                // Increase the amount of harvested crops
                cropsHarvested[cropType]++;

                // Reset the crop's tile to untilled dirt
                TilemapManager.Instance.ResetTile(pos);

                // Trigger the harvesting animation
                animator.SetTrigger("Harvesting");

                // Lower the stamina from the action
                stamina -= staminaPerAction;
            }
        }
        else
            // Show that we can't perform this interaction
            DisplayCanInteract(false, true, false);
    }

    // Interaction using the hoe with a dirt tile
    void Hoe(Vector3Int pos)
    {
        // Check that the tile isn't already tilled
        if (!TilemapManager.Instance.IsTilled(pos))
        {
            // Show that we can perform this interaction
            DisplayCanInteract(true, false, false);

            // Perform the interaction
            if (Input.GetButtonDown("Action") && stamina >= staminaPerAction)
            {
                // Set the dirt tile to tilled
                TilemapManager.Instance.TillDirt(pos);

                // Trigged the tilling animation
                animator.SetTrigger("Tilling");

                // Show that we can't perform this interaction
                DisplayCanInteract(false, true, false);

                // Lower the stamina from the action
                //stamina -= staminaPerAction;
            }
        }
        else
            // Show that we can't perform this interaction
            DisplayCanInteract(false, true, false);
    }

    // Plant a crop
    void PlantCrop(Vector3Int pos, int cropElement)
    {
        // Check if there is a crop game object in the current position
        // Cycle through all the current crops that have been planted
        // And check their position against  where you are trying to plant a new crop
        GameObject[] allCrops = GameObject.FindGameObjectsWithTag("Crop");

        // Set the position to plant the crop in the center of the tile in question
        Vector2 cropPosition = new Vector2((float)pos.x + 0.5f, (float)pos.y + 0.5f);

        // If there is another crop in this position, display that we can't perform the interaction 
        // And leave the function
        for (int i = 0; i < allCrops.Length; i++)
            if (Vector2.Distance(cropPosition, allCrops[i].transform.position) == 0)
                return;

        // Check that the tile is tilled, then trigger the planting animation
        if (TilemapManager.Instance.IsTilled(pos))
            // Plant a crop on the tile at the location of the player
            TilemapManager.Instance.PlantCrop(pos, cropPosition, cropElement);
    }

    void UseSeed(Vector2 pos, int cropElement)
    {
        DisplayCanInteract(false, true, false);

        RaycastHit2D[] cropBoxcasts = new RaycastHit2D[9];
        int boxcastElement = 0;

        Vector2 boxcastPosition = Vector2.zero;

        for (int i = -1; i < 2; i++)
            for (int j = -1; j < 2; j++)
            {

                boxcastPosition = new Vector2(pos.x + i, pos.y + j);

                Vector2 boxSize = new Vector2(0.5f, 0.5f);
                cropBoxcasts[boxcastElement] = Physics2D.BoxCast(boxcastPosition, boxSize, 0, Vector2.zero, LayerMask.GetMask("Dirt Tile"));

                boxcastElement++;
            }

        foreach (RaycastHit2D boxHit in cropBoxcasts)
            if (boxHit.collider != null)
            {
                int yCorrectorBox = boxHit.point.y < 0 ? -1 : 0;
                int xCorrectorBox = boxHit.point.x < 0 ? -1 : 0;
                // Set the raycast hit position to ints because that's what the SetTile function uses
                Vector3Int boxcastPositionInt = new Vector3Int((int)boxHit.point.x + xCorrectorBox, (int)boxHit.point.y + yCorrectorBox, 0);
                if (TilemapManager.Instance.IsTilled(boxcastPositionInt))
                    DisplayCanInteract(false, false, true);

                if (Input.GetButtonDown("Action") && stamina >= staminaPerAction)
                    PlantCrop(boxcastPositionInt, cropElement);
            }
    }

    // Interaction using a seed with a dirt tile
    void Seed(Vector3Int pos, int cropElement)
    {
        // Check if there is a crop game object in the current position
        // Cycle through all the current crops that have been planted
        // And check their position against  where you are trying to plant a new crop
        GameObject[] allCrops = GameObject.FindGameObjectsWithTag("Crop");

        // Set the position to plant the crop in the center of the tile in question
        Vector2 cropPosition = new Vector2((float)pos.x + 0.5f, (float)pos.y + 0.5f);

        // If there is another crop in this position, display that we can't perform the interaction 
        // And leave the function
        for (int i = 0; i < allCrops.Length; i++)
            if (Vector2.Distance(cropPosition, allCrops[i].transform.position) == 0)
            {
                DisplayCanInteract(false, true, false);
                return;
            }

        // Check that the tile is tilled, then trigger the planting animation
        if (TilemapManager.Instance.IsTilled(pos))
        {
            // Show that we can perform this interaction
            DisplayCanInteract(true, false, false);

            // Perform the interaction
            if (Input.GetButtonDown("Action") && stamina >= staminaPerAction)
            {
                // Plant a crop on the tile at the location of the player
                TilemapManager.Instance.PlantCrop(pos, cropPosition, cropElement);

                animator.SetTrigger("Planting");

                // Lower the stamina from the action
                stamina -= staminaPerAction;
            }
        }

        else
            // Show that we can't perform this interaction
            DisplayCanInteract(false, true, false);
    }

    // Interaction between the player and the spaceship
    // Either charge the spaceship using the crop the player is holding
    // Or charge the player form the spaceship's energy
    void SpaceshipInteraction(ref SpaceshipController spaceship)
    {
        // Charge the spaceship with crop A
        if (carryCropA)
        {
            // Charge the spaceship
            spaceship.ChargeSpaceship(energyCropA);

            // Remove crop from the player's inventory
            cropsHarvested[0]--;

            // If the crop goes below 0, no longer carrying it
            if (cropsHarvested[0] <= 0)
                carryCropA = false;
        }
            

        // Charge the spaceship with crop B
        else if (carryCropB)
        {
            spaceship.ChargeSpaceship(energyCropB);

            // Remove crop from the player's inventory
            cropsHarvested[1]--;

            // If the crop goes below 0, no longer carrying it
            if (cropsHarvested[1] <= 0)
                carryCropB = false;
        }
            
        // Charge the player from the spaceship
        else
            spaceship.ChargePlayer(ref stamina);
    }

    void ChangeCarriedCrops(bool cropA, bool cropB)
    {
        carryCropA = cropA;
        carryCropB = cropB;
    }

    // Display if certain interractions can occur based on the tool selected and the nearest tile
    void DisplayCanInteract(bool displayYes, bool displayNo, bool displayCrop)
    {
        tileSelectYes.SetActive(displayYes);
        tileSelectNo.SetActive(displayNo);
        cropPlant.SetActive(displayCrop);
    }

    // Used to display any variables to the screen in place of UI for now
    // Can add more variables to this as/when we need them and delete it when we have something better
    void TestUI()
    {
        // Display the test variable as UI
        cropAText = "Crop A Held: " + cropsHarvested[0].ToString();
        cropAUI.text = cropAText;

        cropBText = "Crop B Held: " + cropsHarvested[1].ToString();
        cropBUI.text = cropBText;

        currentToolString = "Tool Equipped: " + currentTool.ToString();
        currentToolUI.text = currentToolString;

        staminaText = "Player Stamina: " + stamina.ToString();
        staminaTextUI.text = staminaText;

        // Set the spaceship energy to a string
        spaceshipEnergyText = "Spaceship Energy: " + DataPermanence.Instance.spaceshipEnergy.ToString();
        spaceshipEnergyUI.text = spaceshipEnergyText;
    }
}