// Contains all player functionality including movement and interactions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Member objects and components
    Rigidbody2D rb;
    Animator animator;
    BoxCollider2D box;

    // The position of the camera follow 
    // Used to change camera position if the player is in certain parts of the scene
    [SerializeField] Transform cameraFollow;

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
    int staminaUsedHoe = 2;
    int staminaUsedPlanting = 10;
    int staminaUsedWatering = 3;
    int staminaUsedScythe = 5;

    // DEATH RELATED VARIABLES
    // Flag that the player is out of stamina and spaceship is out of energy
    bool resourcesDepleted;
    // Set the day, hour and minute until player death
    int deathDay;
    int deathHour;
    int deathMinute;

    // INTERRACTION/ACTION VARIABLES
    [Header("Interaction/Action Variables")]
    [SerializeField] Transform raycastEnd;
    [SerializeField] GameObject tileSelectYes;
    [SerializeField] GameObject tileSelectNo;
    [SerializeField] GameObject cropPlant;
    int energyCropA = 10;
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
        wateringCan = 2,
        scythe = 3,
        seedB = 4
    }

    // The currently equipped tool
    // Determines which action to perform
    Tools currentTool = Tools.hoe;

    // Reference to the pause screen, to enable and disable it
    [SerializeField] GameObject pauseScreen;

    // IN GAME TUTORIAL VARIABLES
    // Check if the player is in the tutorial and limit their actions if so
    [SerializeField] bool inTutorial = true;

    // The stage of the tutorial the player is currently in
    public int tutorialNumber { get; private set; } = 0;

    // Restricts the amount of tools the player can select during the tutorial
    int availableTools = 1;

    // The tiles with which the player can interact during the tutorial
    // This is a block of 3x3 tiles on the top left corner of the farming area
    // Must be a vector3int because that's what type Unity's tile system uses
    Vector3Int[] tutorialTiles = {
        new Vector3Int(-5, 2, 0), new Vector3Int(-4, 2, 0), new Vector3Int(-3, 2, 0),
        new Vector3Int(-5, 1, 0), new Vector3Int(-4, 1, 0), new Vector3Int(-3, 1, 0),
        new Vector3Int(-5, 0, 0), new Vector3Int(-4, 0, 0), new Vector3Int(-3, 0, 0)
    };

    //inventory
    public UI_Inventory ui_Inventory;
    public Inventory inventory;

    //SHOP
    private ShopManager shopManager;
    public Shop_UI ui_Shop; 
    // BASIC TEST UI
    // Mostly for debugging/checking things are working
/*    string cropAText;
    [SerializeField] TextMeshProUGUI cropAUI;
    string cropBText;
    [SerializeField] TextMeshProUGUI cropBUI;*/
    string currentToolString;
    [SerializeField] TextMeshProUGUI currentToolUI;
/*    string staminaText;
    [SerializeField] TextMeshProUGUI staminaTextUI;
    string spaceshipEnergyText;
    [SerializeField] TextMeshProUGUI spaceshipEnergyUI;*/

    private void Awake()
    {
        //DontDestroyOnLoad(ui_Inventory);
    }

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

        //instantiates inventory and sets inventory to player
        inventory = new Inventory(UseItem);
        ui_Inventory.SetPlayer(this);

        //Instantiates shop and sets it to player 
        shopManager = new ShopManager();
        ui_Shop.SetShop(shopManager, inventory, this); 
        
        //passing in the inventory object onto UI script
        ui_Inventory.SetInventory(inventory);

        // Initialize variables stored in data permanence
        if (DataPermanence.Instance != null)
        {
            // Set the player position when entering a new scene
            rb.MovePosition(DataPermanence.Instance.playerStartPosition);

            // Set the player member variables
            cropsHarvested = DataPermanence.Instance.cropsHarvested;
            stamina = DataPermanence.Instance.playerStamina;
            tutorialNumber = DataPermanence.Instance.tutorialNumber;
            availableTools = DataPermanence.Instance.availableTools;
            inTutorial = DataPermanence.Instance.playerTutorial;

            resourcesDepleted = DataPermanence.Instance.resourcesDepleted;
            deathDay = DataPermanence.Instance.deathDay;
            deathHour = DataPermanence.Instance.deathHour;
            deathMinute = DataPermanence.Instance.deathMinute;
            // If there are any crops held in the inventory in data permanence
            // add them to player inventory in this scene
            //Adds tools and seeds to the inventory 
            inventory.AddItem(new Item { itemType = Item.ItemType.hoe, amount = 1 });
            inventory.AddItem(new Item { itemType = Item.ItemType.wateringCan, amount = 1 });
            inventory.AddItem(new Item { itemType = Item.ItemType.scythe, amount = 1 });
            inventory.AddItem(new Item { itemType = Item.ItemType.seedA, amount = 1 });
            inventory.AddItem(new Item { itemType = Item.ItemType.seedB, amount = 1 });

            //SHOP TESTING - This called at start
            inventory.AddItem(new Item { itemType = Item.ItemType.cropA, amount = 10 });
            inventory.AddItem(new Item { itemType = Item.ItemType.cropB, amount = 10 });

            shopManager.UpdateStock(); 


            //Making sure the correct amount of items are being held across scenes 
            if (DataPermanence.Instance.cropA > 0)
            {
                inventory.AddItem(new Item { itemType = Item.ItemType.cropA, amount = DataPermanence.Instance.cropA });
                Debug.Log("inventory DataPermance ammout for crop A" + DataPermanence.Instance.cropA);
            }
            if (DataPermanence.Instance.cropB > 0)
            {
                inventory.AddItem(new Item { itemType = Item.ItemType.cropB, amount = DataPermanence.Instance.cropB });
                Debug.Log("inventory DataPermance ammout for crop B" + DataPermanence.Instance.cropB);
            }
            if (DataPermanence.Instance.hoe > 0)
            {
                inventory.AddItem(new Item { itemType = Item.ItemType.hoe, amount = DataPermanence.Instance.hoe });
                Debug.Log("inventory DataPermance ammout for hoe" + DataPermanence.Instance.hoe);
            }
            if (DataPermanence.Instance.wateringCan > 0)
            {
                inventory.AddItem(new Item { itemType = Item.ItemType.wateringCan, amount = DataPermanence.Instance.wateringCan });
                Debug.Log("inventory DataPermance ammout for wateringCan" + DataPermanence.Instance.wateringCan);
            }
            if (DataPermanence.Instance.scythe > 0)
            {
                inventory.AddItem(new Item { itemType = Item.ItemType.scythe, amount = DataPermanence.Instance.scythe });
                Debug.Log("inventory DataPermance ammout for scythe" + DataPermanence.Instance.scythe);
            }

            if (DataPermanence.Instance.seedA > 0)
            {
                inventory.AddItem(new Item { itemType = Item.ItemType.seedA, amount = DataPermanence.Instance.seedA });
                Debug.Log("inventory DataPermance ammout for seedA" + DataPermanence.Instance.seedA);
            }
            if (DataPermanence.Instance.seedB > 0)
            {
                inventory.AddItem(new Item { itemType = Item.ItemType.seedB, amount = DataPermanence.Instance.seedB });
                Debug.Log("inventory DataPermance ammout for seedB" + DataPermanence.Instance.seedB);
            }

        }

        if (!inTutorial)
            ChangeTutorialStage(5, 9);
    }

    // Update is called once per frame
    // Used to read input, perform calculations, set states etc
    void Update()
    {
        

        // Stop movement during certain animations
        // Get the name of the current animation
        string currentAnimation = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        // Check if the current animation should stop movement
        if (currentAnimation == "Player Hoe Down" || currentAnimation == "Player Hoe Sideways" || currentAnimation == "Player Hoe Up" ||
            currentAnimation == "Player Scythe Down" || currentAnimation == "Player Scythe Sideways" || currentAnimation == "Player Scythe Up" ||
            currentAnimation == "Player Watering Can Down" || currentAnimation == "Player Watering Can Sideways" || currentAnimation == "Player Watering Can Up")
            canMove = false;
        else
            canMove = true;

        //function to open inventory 
        if (Input.GetKeyDown(KeyCode.I))
            ui_Inventory.OpenInventory();

        //OPEN SHOP TEST
        if (Input.GetKeyDown(KeyCode.K))
            ui_Shop.OpenShop();  

        // Read directional input and set the movement vector
        // Normalize to reduce increased speed when moving diagonally
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        // Set the raycast corrector based on the previous and current direction that the player is facing
        if (Mathf.Abs(directionAnimatorParameter) == 1 && direction.x != 0)
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
        if (Input.GetButtonDown("Tools Left"))
        {
            // If the current weapon is the lowest tool in the enums
            // Then change it to the highest tool in the enums
            // Otherwise, go to the next one down
            if (currentTool <= 0)
                currentTool = (Tools)availableTools - 1;
            else
                currentTool--;
        }
        if (Input.GetButtonDown("Tools Right"))
        {
            // If the current weapon is the highest tool in the enums
            // Then change it to the lowest tool in the enums
            // Otherwise, go to the next one up
            if (currentTool >= (Tools)availableTools - 1)
                currentTool = 0;
            else
                currentTool++;
        }

        if (Input.GetButtonDown("Cancel"))
            PauseGame();
        //// When press escape, quit the game
        //if (Input.GetKeyDown(KeyCode.Escape))
        //    Application.Quit();

        // Set whether the player is carrying a crop or not
        if (Input.GetButtonDown("No Crops"))
            ChangeCarriedCrops(false, false);
/*        if (Input.GetButtonDown("Crop A") && cropsHarvested[0] > 0)
            ChangeCarriedCrops(true, false);
        if (Input.GetButtonDown("Crop B") && cropsHarvested[1] > 0)
            ChangeCarriedCrops(false, true);*/

        // Perform any actions and update the player variables in data permanence
        PerformAction();
        DataPermanence.Instance.playerStamina = stamina;
        DataPermanence.Instance.cropsHarvested = cropsHarvested;

        // Check if the player has no resources and if so, for how long this has been the case
        DeathCheck();

        // If we're in the tutorial, check what phase of the tutorial we're in
        // then check if the conditions to finish that phase have been met
        // These functions will move to the next phase of the tutorial of the conditions are met
        if (inTutorial)
        {
            switch (tutorialNumber)
            {
                case 0:

                    CheckTutorialOneOver();

                    break;

                case 1:

                    CheckTutorialTwoOver();

                    break;

                case 2:

                    CheckTutorialThreeOver();

                    break;

                case 3:

                    CheckTutorialFourOver();

                    break;

                /*                case 4:

                                    CheckTutorialFiveOver();

                                    break;*/

                case 5:

                    CheckTutorialSixOver();

                    break;

                case 6:

                    CheckTutorialSevenOver();

                    break;

                case 7:

                    CheckTutorialEightOver();

                    break;

                case 8:

                    CheckTutorialOver();

                    break;
            }
            Debug.Log("In tutorial: " + inTutorial);
        }

        // Move the camera position further above the player if they're near the top of the crop scene
        // To better admire the starry sky background
        if (transform.position.y >= 4)
            cameraFollow.transform.position = new Vector2(transform.position.x, transform.position.y + 3);
        else
            cameraFollow.transform.position = transform.position;

        // Animator parameters

        // Set the movement animation parameter to detect any movement of the rigidbody
        animator.SetFloat("Movement", rb.velocity.magnitude);
        // Set the player direction parameter
        animator.SetFloat("Vertical Direction", directionAnimatorParameter);
        // Set the crops that the player is or isn't carrying
        //animator.SetBool("Holding Crop A", carryCropA);
        //animator.SetBool("Holding Crop B", carryCropB);

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
        Vector2 raycastStart = new Vector2(transform.position.x, transform.position.y - 0.375f);
        RaycastHit2D[] rayCast = Physics2D.RaycastAll(raycastStart, raycastDirection, 0.125f);
        //RaycastHit2D[] rayCast = Physics2D.RaycastAll(raycastEnd.transform.position + raycastCorrector, raycastDirection, 0.2f);

        //Debug.DrawRay(raycastEnd.transform.position + raycastCorrector, raycastDirection, Color.cyan, 1f, false);
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
            if (carryCropA || carryCropB || tag == "Generator" || tag == "Grass Tile" || tag == "Untagged" || (tag == "Bed"))
            {
                DisplayCanInteract(false, false, false);

                // Generator related interactions
                if (Input.GetButtonDown("Action") && (tag == "Generator"))
                    SpaceshipInteraction(ref spaceshipController);
                // Bed related interractions
                if (Input.GetButtonDown("Action") && (tag == "Bed"))
                {
                    GoToSleep();
                    if (inTutorial)
                        CheckTutorialFiveOver();
                }
            }

            // Otherwise, perform an action based on equipped tool and what's being interacted with
            else
            {
                switch (currentTool)
                {
                    // Seed A is equipped
                    case Tools.seedA:

                        // Check if we're in the tutorial
                        if (inTutorial)
                        {
                            // If we're in the tutorial, can only plant crop A in the middle of the
                            // tutorial tile section during the second phase of the tutorial
                            if (positionInt == tutorialTiles[4] && tutorialNumber == 1)
                                UseSeed(displayPosition, 0);
                            // Otherwise, can't plant any crops
                            else
                                DisplayCanInteract(false, true, false);
                        }
                        else if (!inTutorial)
                        {
                            // Display where crop A will be planted and plant them
                            UseSeed(displayPosition, 0);
                        }

                        break;

                    // Seed B is equipped
                    case Tools.seedB:

                        // Display where crop B will be planted and plant them
                        UseSeed(displayPosition, 1);

                        break;

                    // Hoe is equipped 
                    case Tools.hoe:

                        // If interacting with a dirt tile, till it
                        if (tag == "Dirt Tile")
                        {
                            // Not in tutorial - interact as normal
                            if (!inTutorial)
                            {
                                Hoe(positionInt);
                                return;
                            }
                            // In tutorial - can only till certain tiles
                            else if (inTutorial)
                            {
                                for (int i = 0; i < tutorialTiles.Length; i++)
                                {
                                    if (positionInt == tutorialTiles[i])
                                    {
                                        Hoe(positionInt);
                                        //Debug.Log("PositionInt: " + positionInt);
                                        //Debug.Log("Tile position: " + tutorialTiles[i]);
                                        return;
                                    }
                                    else
                                        DisplayCanInteract(false, true, false);
                                }

                            }
                        }
                        else
                            DisplayCanInteract(false, true, false);

                        break;

                    // Watering can is equipped 
                    case Tools.wateringCan:

                        // If interacting with an unwatered crop, water it
                        if (tag == "Crop")
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
            if (Input.GetButtonDown("Action") && stamina >= staminaUsedWatering)
            {
                // Set the crop status to watereds
                crop.isWatered = true;

                // Trigger the watering animation
                animator.SetTrigger("Watering");

                // Play the watering sfx
                AudioManager.Instance.playWaterCrop();

                // Lower the stamina from the action
                stamina -= staminaUsedWatering;
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
            if (Input.GetButtonDown("Action") && stamina >= staminaUsedScythe)
            {
                // Destroy the game object
                Destroy(rayHit.transform.gameObject);
                // Increase the amount of harvested crops
                //cropsHarvested[cropType]++;
                switch(cropType)
                {
                    case 0:

                        inventory.AddItem(new Item { itemType = Item.ItemType.cropA, amount = 1 });
                        DataPermanence.Instance.cropA++;
                        break;

                    case 1:

                        inventory.AddItem(new Item { itemType = Item.ItemType.cropB, amount = 1 });
                        DataPermanence.Instance.cropB++;
                        break;
                }
                    

                // Reset the crop's tile to untilled dirt
                TilemapManager.Instance.ResetTile(pos);

                // Trigger the harvesting animation
                animator.SetTrigger("Harvesting");

                // Play the scythe sfx
                AudioManager.Instance.playHarvestCrop();

                // Lower the stamina from the action
                stamina -= staminaUsedScythe;
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
            if (Input.GetButtonDown("Action") && stamina >= staminaUsedHoe)
            {
                // Set the dirt tile to tilled
                TilemapManager.Instance.TillDirt(pos);

                // Trigged the tilling animation
                animator.SetTrigger("Tilling");

                // Play the tilling sfx
                AudioManager.Instance.playTillingSoil();

                // Show that we can't perform this interaction
                DisplayCanInteract(false, true, false);

                // Lower the stamina from the action
                stamina -= staminaUsedHoe;
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

    // Bring up the display to plant a 3x3 square of crops
    // And call the planting function
    void UseSeed(Vector2 pos, int cropElement)
    {
        // Show that we can perform this interaction
        DisplayCanInteract(false, true, false);

        // Create 9 boxcasts in a 3x3 square
        // This is to check if any of the required squares can have a crop planted
        RaycastHit2D[] cropBoxcasts = new RaycastHit2D[9];
        int boxcastElement = 0;

        Vector2 boxcastPosition;

        // This for loop makes a 3x3 square of tiles and sets a box cast on each square
        // With the selected tile in the center
        for (int i = -1; i < 2; i++)
            for (int j = -1; j < 2; j++)
            {
                // Set the position of the box cast to  the current tile in the 3x3 square
                boxcastPosition = new Vector2(pos.x + i, pos.y + j);

                // Set the size of the boxcast to half a tile
                Vector2 boxSize = new Vector2(0.5f, 0.5f);

                // Cast the boxcast
                cropBoxcasts[boxcastElement] = Physics2D.BoxCast(boxcastPosition, boxSize, 0, Vector2.zero, LayerMask.GetMask("Dirt Tile"));

                // Move to the next element in the array of boxcasts
                boxcastElement++;
            }

        // A flag to make sure stamina is taken once for each used seed
        // Instead of multiple times for each planted crop
        bool staminaTaken = false;

        // If any of the boxcasts from the 3x3 square hit a tilled tile
        // Then enable a crop to be planted here and plant one if the action button is pressed
        foreach (RaycastHit2D boxHit in cropBoxcasts)
            if (boxHit.collider != null)
            {
                // Used to correct for rounding down when converting position to ints
                // We want to round down, but setting a float to an int only removes after the decimal place
                // This causes the number to round up if the float value is negative
                int yCorrectorBox = boxHit.point.y < 0 ? -1 : 0;
                int xCorrectorBox = boxHit.point.x < 0 ? -1 : 0;

                // Set the raycast hit position to ints because that's what the SetTile function uses
                Vector3Int boxcastPositionInt = new Vector3Int((int)boxHit.point.x + xCorrectorBox, (int)boxHit.point.y + yCorrectorBox, 0);

                // If any of the selected tiles are tilled tile and can have a crop planted
                // Then display that this 3x3 box can have crops planted
                if (TilemapManager.Instance.IsTilled(boxcastPositionInt))
                    DisplayCanInteract(false, false, true);

                // Plant the crops for each tile in the 3x3 square that can have a crop planted there
                if (Input.GetButtonDown("Action") && stamina >= staminaUsedPlanting)
                {
                    // Plant the crop
                    PlantCrop(boxcastPositionInt, cropElement);

                    // Play the planting sfx
                    AudioManager.Instance.playPlantSeed();

                    if (!staminaTaken)
                    {
                        // Lower the stamina from the action
                        stamina -= staminaUsedPlanting;

                        staminaTaken = true;
                    }
                }
            }
    }

    // Interaction between the player and the spaceship
    // Either charge the spaceship using the crop the player is holding
    // Or charge the player form the spaceship's energy
    void SpaceshipInteraction(ref SpaceshipController spaceship)
    {
        // Charge the spaceship with crop A
        if (carryCropA)
        {
            // Make sure only one crop is fed into the generator at a certain tutorial stage
            if (inTutorial && (DataPermanence.Instance.cropA < 9 && tutorialNumber >= 7))
                return;

            // Charge the spaceship
            spaceship.ChargeSpaceship(energyCropA);

            // Play the crops into generator sfx
            AudioManager.Instance.playGeneratorFeedCrops();

            // Remove crop from the player's inventory
            //cropsHarvested[0]--;
            inventory.RemoveItem(new Item { itemType = Item.ItemType.cropA, amount = 1 });
            DataPermanence.Instance.cropA--;

            // If the crop goes below 0, no longer carrying it
            if (DataPermanence.Instance.cropA <= 0)
                carryCropA = false;
        }


        // Charge the spaceship with crop B
        else if (carryCropB)
        {
            spaceship.ChargeSpaceship(energyCropB);

            // Play the crops into generator sfx
            AudioManager.Instance.playGeneratorFeedCrops();

            // Remove crop from the player's inventory
            //cropsHarvested[1]--;
            inventory.RemoveItem(new Item { itemType = Item.ItemType.cropB, amount = 1 });
            DataPermanence.Instance.cropB--;

            // If the crop goes below 0, no longer carrying it
            if (DataPermanence.Instance.cropB <= 0)
                carryCropB = false;
        }

        // Charge the player from the spaceship
        else
        {
            spaceship.ChargePlayer(ref stamina);

            // Play the player recharge sfx
            if(DataPermanence.Instance.spaceshipEnergy > 0)
                AudioManager.Instance.playGeneratorRecharge();
        }
            
    }

    void GoToSleep()
    {
        // Figure out how long the crops grow between now and 7am next day
        // Then convert this into seconds and grow the crops that long
        int hoursGrowth;
        int minsGrowth;

        // If the time is not between midnight and 7am, stay on the current day
        if (TimeManager.Hour < 24 && TimeManager.Hour >= 7)
        {
            Debug.Log("Day increased. Old day: " + TimeManager.Day);
            // Increase the day count
            TimeManager.Day += 1;
            TimeManager.OnDayChanged?.Invoke();
            Debug.Log("Day increased. New day: " + TimeManager.Day);

            // Increase crop growth hours, adding the rest of today if we're sleeping until the next day
            // plus the 7 hour because we sleep until 7 the next day
            hoursGrowth = 7 + (24 - TimeManager.Hour);
        }
        // If we're not sleeping until the next day ie sleeping from 2am - 7am
        // Use that time slept to grow the crops instead
        else
            hoursGrowth = 7 - TimeManager.Hour;

        // The minutes past the hour needs to be subtracted from time grown
        minsGrowth = TimeManager.Minute;

        // Now this all needs to be converted to seconds
        // First convert hours to minutes game time
        float secondsGrowth = ((hoursGrowth * 60) - minsGrowth);
        // Now convert to seconds in real time - if 0.5 seconds real time is 1 minute game time
        secondsGrowth *= 0.5f;

        // Now add the real time growth to all the crops held in data permanence
        // Making sure to use their water multiplier if watered
        if (DataPermanence.Instance.allCrops.Count != 0)
            for (int i = 0; i < DataPermanence.Instance.allCrops.Count; i++)
            {
                DataPermanence.Instance.allCrops[i].timeAlive += secondsGrowth * DataPermanence.Instance.allCrops[i].wateredMultiplier;
            }

        TimeManager.Hour = 7;
        TimeManager.Minute = 0;

    }

    //adds a single crop to player upon mouse click and removes a single crop from the inventory
    public void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.cropA:

                if(carryCropA)
                    ChangeCarriedCrops(false, false);
                else
                    ChangeCarriedCrops(true, false);

                ui_Inventory.OpenInventory();
                //cropsHarvested[0]++;
                //DataPermanence.Instance.cropA--;
                //inventory.RemoveItem(new Item { itemType = Item.ItemType.cropA, amount = 1 });
                break;

            case Item.ItemType.cropB:

                if(carryCropB)
                    ChangeCarriedCrops(true, false);
                else
                    ChangeCarriedCrops(false, true);

                ui_Inventory.OpenInventory();
                //cropsHarvested[1]++;
                //DataPermanence.Instance.cropB--;
                //inventory.RemoveItem(new Item { itemType = Item.ItemType.cropB, amount = 1 });
                break;
        }
    }

    // Check if the player stamina and ship energy are empty
    // If they are, the player has one day to find some stamina or energy before death
    void DeathCheck()
    {
        // If resources are empty, set the flag to true and begin the death countdown
        if (DataPermanence.Instance.spaceshipEnergy <= 0 && stamina <= 0 && !resourcesDepleted)
        {
            resourcesDepleted = true;
            deathDay = TimeManager.Day + 1;
            deathHour = TimeManager.Hour;
            deathMinute = TimeManager.Minute;
            DataPermanence.Instance.deathDay = deathDay;
            DataPermanence.Instance.deathHour = deathHour;
            DataPermanence.Instance.deathMinute = deathMinute;
        }
        
        // If resources are gained before death, unset the bool
        if (resourcesDepleted && (DataPermanence.Instance.spaceshipEnergy > 0 || stamina > 0))
            resourcesDepleted = false;

        // If one day is passed with no resources, the player dies
        if (resourcesDepleted && TimeManager.Day >= deathDay && TimeManager.Hour >= deathHour && TimeManager.Minute >= deathMinute)
            SceneChanger.Instance.ChangeScene("Game Over", Vector2.zero);
            

        DataPermanence.Instance.resourcesDepleted = resourcesDepleted;
    }

    void PauseGame()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
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

    // Used to move to the next phase of the tutorial
    void ChangeTutorialStage(int toolsAvail, int tutNum)
    {
        availableTools = toolsAvail;
        tutorialNumber = tutNum;
        DataPermanence.Instance.tutorialNumber = tutNum;
        DataPermanence.Instance.availableTools = toolsAvail;
    }

    // Check if the first phase of the tutorial is over
    // This is done by checking if all the necessary tiles have been tilled
    void CheckTutorialOneOver()
    {
        // If any of the 9 tiles are untilled, leave this function - this stage of the tutorial is incomplete
        foreach (Vector3Int tile in tutorialTiles)
            if (TilemapManager.Instance != null && !TilemapManager.Instance.IsTilled(tile))
                return;

        // If all the tiles are tilled we reach this  line - move to the next tutorial stage
        ChangeTutorialStage(2, 1);
    }

    void CheckTutorialTwoOver()
    {
        // Get an array of all crops in the scene
        // This will only be the 3x3 tutorial square of crops at this point
        GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");

        // Check that the array is neither null nor empty
        // This means that the crops have been planted here
        if (crops?.Length > 0)
            ChangeTutorialStage(3, 2);
    }

    void CheckTutorialThreeOver()
    {
        // Get an array of all crops in the scene
        // This will only be the 3x3 tutorial square of crops at this point
        GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");

        // Access the crop game objects' crop controllers
        // If any of them are unwatered, leaving this function - this stage of the tutorial is incomplete
        foreach (GameObject crop in crops)
            if (!crop.GetComponent<CropController>().isWatered)
                return;

        // If all the crops are watered we reach this line - move to the next tutorial stage
        ChangeTutorialStage(3, 3);
    }

    void CheckTutorialFourOver()
    {
        // Check if the player has entered the spaceship in tutorial four
        // If so, move to the next tutorial stage
        if (tutorialNumber == 3 && SceneManager.GetActiveScene().name == "AlexTestScene SpaceShip")
            ChangeTutorialStage(3, 4);
    }

    void CheckTutorialFiveOver()
    {
        // TODO: add functionality here to sleep and move to the next day, grow crops, etc

        // TEMPORARY CODE TO FULLY GROW ALL CROPS AND MOVE TO NEXT PHASE OF TUTORIAL
        //if (Input.GetButtonDown("Action"))
        //{
        //    // Cycle through the list of crops in data permanence
        //    // Set them all to an age where they're fully grown
        //    for (int i = 0; i < DataPermanence.Instance.allCrops.Count; i++)
        //    {
        //        DataPermanence.Instance.allCrops[i].timeAlive = 100;
        //    }

        ChangeTutorialStage(4, 5);
        //}

    }

    void CheckTutorialSixOver()
    {
        if(SceneManager.GetActiveScene().name == "AlexTestScene")
        {
            GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");

            if (crops.Length == 0)
                ChangeTutorialStage(4, 6);
        }
        
    }

    void CheckTutorialSevenOver()
    {
        // Check if the player has entered the spaceship in tutorial seven
        // If so, move to the next tutorial stage
        if (tutorialNumber == 6 && SceneManager.GetActiveScene().name == "AlexTestScene SpaceShip")
            ChangeTutorialStage(4, 7);
    }

    void CheckTutorialEightOver()
    {
        if (DataPermanence.Instance.cropA == 8)
            ChangeTutorialStage(4, 8);
    }

    void CheckTutorialOver()
    {
        if (tutorialNumber == 8 && DataPermanence.Instance.spaceshipEnergy == 0)
        {
            ChangeTutorialStage(5, 9);
            inTutorial = false;
            DataPermanence.Instance.playerTutorial = false;
        }

    }

    // A seperate function to call this audio manager function
    // Used to call the function as an animation event
    // Therefore can not be called directly from the audio manager
    // Because the player doesn't contain an audio manage component
    public void footstepsAudio()
    {
        AudioManager.Instance.playFootsteps();
    }

    // Used to display any variables to the screen in place of UI for now
    // Can add more variables to this as/when we need them and delete it when we have something better
    void TestUI()
    {
        // Display the test variable as UI
/*        cropAText = "Crop A Held: " + cropsHarvested[0].ToString();
        cropAUI.text = cropAText;

        cropBText = "Crop B Held: " + cropsHarvested[1].ToString();
        cropBUI.text = cropBText;*/

        currentToolString = "Tool Equipped: " + currentTool.ToString();
        currentToolUI.text = currentToolString;

/*        staminaText = "Player Stamina: " + stamina.ToString();
        staminaTextUI.text = staminaText;

        Debug.Log("In player, spaceship energy: " + DataPermanence.Instance.spaceshipEnergy);
        // Set the spaceship energy to a string
        spaceshipEnergyText = "Spaceship Energy: " + DataPermanence.Instance.spaceshipEnergy.ToString();
        spaceshipEnergyUI.text = spaceshipEnergyText;*/
    }
}