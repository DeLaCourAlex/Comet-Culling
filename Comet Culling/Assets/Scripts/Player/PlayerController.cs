// Contains all player functionality including movement and interactions

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Yarn;
using Yarn.Unity;
//using UnityEditor.SearchService;


public class PlayerController : MonoBehaviour
{
    // Member objects and components
    Rigidbody2D rb;
    Animator animator;

    //Tool sprite, display the currently equipped tool from an array of sprites
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] spriteArray;
    [SerializeField] GameObject toolSprite;

    //merchant 
    [SerializeField] GameObject Dialogue;
    private DialogueRunner dialogueRunner;
    DialogueUI dialogueUI;

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
    // Store how many crops of each type are harvested
    int[] cropsHarvested;
    // A flag to stop multiple stamina being taken when planting a seed
    bool planting;

    // STAMINA RELATED VARIABLES
    // The player current stamina
    public int stamina;

    // max stamina for recharging
    int MAX_STAMINA = 100;

    // Stamina used for the various actions
    int staminaUsedHoe = 2;
    int staminaUsedPlanting = 10;
    int staminaUsedWatering = 3;
    int staminaUsedScythe = 1;

    // DEATH RELATED VARIABLES
    // Flag that the player is out of stamina and spaceship is out of energy
    bool resourcesDepleted;
    // Set the day, hour and minute until player death
    int deathDay;
    int deathHour;
    int deathMinute;

    // INTERRACTION/ACTION VARIABLES
    // Set the raycast for interaction as well as the indicator of whether an action can be performed
    [Header("Interaction/Action Variables")]
    [SerializeField] Transform raycastEnd;
    [SerializeField] GameObject tileSelectYes;
    [SerializeField] GameObject tileSelectNo;
    [SerializeField] GameObject cropPlant;
    // A reference to the NPC
    [SerializeField] GameObject NPC;
    public bool npc_detection;

    //Check if screen has been interacted with today
    bool hasInteractedScreenToday;
    // Check if a captains log is currently open
    bool readingCaptainsLog;

    // The amount of energy replenished by the two seed types
    int energyCropA = 10;
    int energyCropB = 15;
    // Flags if a crop is equipped to feed into the generator
    bool carryCropA;
    bool carryCropB;

    // Store the tools as an enum to know which one is equipped
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
    [SerializeField] bool inTutorial;

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

    // Start is called before the first frame update
    void Start()
    {
        // Initialize member objects and components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dialogueRunner = GameObject.FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueUI = GetComponent<DialogueUI>();

        // Initialize member variables
        stamina = MAX_STAMINA;
        cropsHarvested = new int[2];

        //instantiates inventory and sets inventory to player
        inventory = new Inventory(UseItem);
        ui_Inventory.SetPlayer(this);

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
            currentTool = (Tools)DataPermanence.Instance.currentTool;
            resourcesDepleted = DataPermanence.Instance.resourcesDepleted;
            deathDay = DataPermanence.Instance.deathDay;
            deathHour = DataPermanence.Instance.deathHour;
            deathMinute = DataPermanence.Instance.deathMinute;
            hasInteractedScreenToday = DataPermanence.Instance.screenInteractedToday;

            // If there are any crops held in the inventory in data permanence
            // add them to player inventory in this scene
            //Adds tools and seeds to the inventory 

            inventory.AddItem(new Item { itemType = Item.ItemType.hoe, amount = 1 });

            if(inTutorial)
            {
                if (availableTools > 1)
                    inventory.AddItem(new Item { itemType = Item.ItemType.seedA, amount = 10 });

                if (availableTools > 2)
                    inventory.AddItem(new Item { itemType = Item.ItemType.wateringCan, amount = 1 });

                if (availableTools > 3)
                    inventory.AddItem(new Item { itemType = Item.ItemType.scythe, amount = 1 });
            }
            
            else
            {
                inventory.AddItem(new Item { itemType = Item.ItemType.wateringCan, amount = 1 });
                inventory.AddItem(new Item { itemType = Item.ItemType.scythe, amount = 1 });

                //Making sure the correct amount of items are being held across scenes 
                if (DataPermanence.Instance.cropA > 0)
                    inventory.AddItem(new Item { itemType = Item.ItemType.cropA, amount = DataPermanence.Instance.cropA });

                if (DataPermanence.Instance.cropB > 0)
                    inventory.AddItem(new Item { itemType = Item.ItemType.cropB, amount = DataPermanence.Instance.cropB });

                if (DataPermanence.Instance.seedA > 0)
                    inventory.AddItem(new Item { itemType = Item.ItemType.seedA, amount = DataPermanence.Instance.seedA });

                if (DataPermanence.Instance.seedB > 0)
                    inventory.AddItem(new Item { itemType = Item.ItemType.seedB, amount = DataPermanence.Instance.seedB });
            }

            
        }

        // set the sprite renderer to display the currently equipped tool
        spriteRenderer.sprite = spriteArray[(int)currentTool];
        toolSprite.SetActive(true);

        if (!inTutorial)
            ChangeTutorialStage(5, 12);
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
            currentAnimation == "Player Watering Can Down" || currentAnimation == "Player Watering Can Sideways" || currentAnimation == "Player Watering Can Up" ||
            currentAnimation == "Player Seed Down" || currentAnimation == "Player Seed Sideways" || currentAnimation == "Player Seed Up" ||
            readingCaptainsLog || dialogueUI.isTalking)
            canMove = false;
        else
            canMove = true;

        //function to open inventory 
        if (Input.GetButtonDown("Inventory") && !pauseScreen.activeSelf)
        {
            ui_Inventory.OpenInventory();
            // Stop movement when inventory is open
            direction = Vector2.zero;
        }

        // Gameplay action/movement can only happen when the inventory is closed and the game is not paused
        if (!ui_Inventory.isInventoryVisible && !pauseScreen.activeSelf)
        {
            // Read directional input and set the movement vector
            // Normalize to reduce increased speed when moving diagonally
            if (!readingCaptainsLog && canMove)
                direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            else
                direction = Vector2.zero;

            // Set the direction parameter based on the directional input
            // Only uses whole numbers to change the parameter - because the directional input is normalized, this ensures that diagonal movement (0.7, 0.7)
            // will keep the player facing in the current direction
            if (direction.y == 1)
                directionAnimatorParameter = 1;
            else if (direction.y == -1)
                directionAnimatorParameter = -1;
            else if (Mathf.Abs(direction.x) == 1)
                directionAnimatorParameter = 0;

            // Perform any actions and update the player variables in data permanence
            PerformAction();
        }

        // Pause the game
        if (Input.GetButtonDown("Cancel"))
            PauseGame();

        // Check if the player has no resources and if so, for how long this has been the case
        DeathCheck();

        // Store necessary variables in data permanence
        DataPermanence.Instance.playerStamina = stamina;
        DataPermanence.Instance.cropsHarvested = cropsHarvested;


        // If we're in the tutorial, check what phase of the tutorial we're in
        // then check if the conditions to finish that phase have been met
        // These functions will move to the next phase of the tutorial of the conditions are met
        if (inTutorial)
        {
            switch (tutorialNumber)
            {
                case 0:

                    CheckTutorial1Over();

                    break;

                case 1:

                    CheckTutorial2Over();

                    break;

                case 2:

                    CheckTutorial3Over();

                    break;

                case 3:

                    CheckTutorial4Over();

                    break;

                case 4:

                    CheckTutorial5Over();

                    break;

                case 6:

                    CheckTutorial7Over();

                    break;

                case 8:

                    CheckTutorial9Over();

                    break;

                case 9:

                    CheckTutorial10Over();

                    break;

                case 10:

                    CheckTutorial11Over();

                    break;

                case 11:

                    CheckTutorialOver();

                    break;

            }

            //skip tutorial upon key press
            if (Input.GetKeyDown(KeyCode.Q))
                SkipOverTutorial();
        }

        // Move the camera position further above the player if they're near the top of the crop scene
        // To better admire the starry sky background
        if (transform.position.y >= 4)
            cameraFollow.transform.position = new Vector2(transform.position.x, transform.position.y + 3);
        else
            cameraFollow.transform.position = transform.position;

        // Animator parameters

        // Set the movement animation parameter to detect any movement of the rigidbody
        animator.SetFloat("Movement", direction.magnitude);
        // Set the player direction parameter
        animator.SetFloat("Vertical Direction", directionAnimatorParameter);
        // Set the crops that the player is or isn't carrying
    }

    // Use for all movement of physics bodies
    private void FixedUpdate()
    {
        CharacterMovement(direction);
    }

    //collision with NPC
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("NPC") && (!inTutorial || tutorialNumber == 7))
        {
            npc_detection = true;
            Dialogue.SetActive(true);
            //Npc Detection
            if (npc_detection)
            {

                //starts a different node depending on the day 
                switch (TimeManager.Day)
                {

                    case 2:

                        dialogueRunner.StartDialogue("Day_2");

                        break;

                    case 4:

                        dialogueRunner.StartDialogue("Day_4");
                        break;

                    case 6:

                        dialogueRunner.StartDialogue("Day_6");
                        break;

                }
            }
        }
    }

    //exit collision 
    private void OnTriggerExit2D(Collider2D other)
    {
        npc_detection = false;

        Dialogue.SetActive(false);
        dialogueUI.HideTalksprites();
        dialogueRunner.Stop();
    }

    [YarnCommand("Trade")]
    public void Trade(bool TradeItem = true)
    {
        if (TradeItem)
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.seedA, amount = 2 });
            inventory.AddItem(new Item { itemType = Item.ItemType.seedB, amount = 2 });
        }
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
            // If the raycast hits the screen, access the captains log script
            CaptainLogs captainLogs = hit.transform.gameObject.GetComponent<CaptainLogs>();
            // If the raycast hits the bed, access the bed controller
            BedController bedController = hit.transform.gameObject.GetComponent<BedController>();

            // Call the various functions depending on what is being interacted with
            // Can not perform actions farming whilst carrying crops or interacting with grass tiles
            if (carryCropA || carryCropB || tag == "Generator" || tag == "Grass Tile" || tag == "Untagged" || tag == "Bed" || tag == "Screen" || tag == "Player" || tag == "NPC")
            {
                // Remove tile interaction indicator
                // Because we can't farm under these conditions
                DisplayCanInteract(false, false, false);

                // Generator related interactions
                if (Input.GetButtonDown("Action") && (tag == "Generator"))
                    SpaceshipInteraction(ref spaceshipController);

                // Bed related interractions
                if (Input.GetButtonDown("Action") && (tag == "Bed"))
                {
                    if (inTutorial && tutorialNumber != 5)
                        break;

                    GoToSleep(ref bedController);

                    if (inTutorial)
                        CheckTutorial6Over();
                }

                //Screen related interactions
                if (Input.GetButtonDown("Action") && (tag == "Screen"))
                {
                    if (inTutorial && tutorialNumber != 4)
                        break;

                    if (hasInteractedScreenToday && !readingCaptainsLog)
                    {
                        readingCaptainsLog = true;
                        captainLogs.logOpen = true;
                    }

                    else if (readingCaptainsLog)
                    {
                        readingCaptainsLog = false;
                        captainLogs.logOpen = false;
                    }


                    else if (!hasInteractedScreenToday)
                    {
                        captainLogs.hasBeenRead = true;
                        hasInteractedScreenToday = true;
                        DataPermanence.Instance.screenInteractedToday = true;
                        readingCaptainsLog = true;
                        captainLogs.logOpen = true;
                    }
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
                                        return;
                                    }
                                    else
                                        DisplayCanInteract(false, true, false);
                                }

                            }
                        }
                        // Otherwise, display that we can't interact
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
                        // Otherwise, display that we can't interact
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
                        // Otherwise, display that we can't interact
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
                if (AudioManager.Instance != null)
                    AudioManager.Instance.playWaterCrop();

                // Lower the stamina from the action
                stamina -= staminaUsedWatering;
            }
        }
        else
            // Otherwise, display that we can't interact
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

                // Increase the amount of harvested crops depending on the crop type
                switch (cropType)
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
                if (AudioManager.Instance != null)
                    AudioManager.Instance.playHarvestCrop();

                // Lower the stamina from the action
                stamina -= staminaUsedScythe;
            }
        }
        else
            // Otherwise, display that we can't interact
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
                if (AudioManager.Instance != null)
                    AudioManager.Instance.playTillingSoil();

                // Show that we can't perform this interaction
                DisplayCanInteract(false, true, false);

                // Lower the stamina from the action
                stamina -= staminaUsedHoe;
            }
        }
        else
            // Otherwise, display that we can't interact
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
            {
                planting = false;
                return;
            }


        // Check that the tile is tilled, then trigger the planting animation
        if (TilemapManager.Instance.IsTilled(pos))
        {
            // Plant a crop on the tile at the location of the player
            TilemapManager.Instance.PlantCrop(pos, cropPosition, cropElement);

            planting = true;
        }

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
                    planting = false;

                    // Plant the crop
                    PlantCrop(boxcastPositionInt, cropElement);

                    if (planting)
                    {
                        // Play the planting sfx
                        animator.SetTrigger("Planting");

                        if (AudioManager.Instance != null)
                            AudioManager.Instance.playPlantSeed();

                        if (!staminaTaken)
                        {
                            // Lower the stamina from the action
                            stamina -= staminaUsedPlanting;

                            // Remove the planted seed from inventory
                            if (cropElement == 0)
                            {
                                inventory.RemoveItem(new Item { itemType = Item.ItemType.seedA, amount = 1 });
                            }
                            else if (cropElement == 1)
                            {
                                inventory.RemoveItem(new Item { itemType = Item.ItemType.seedB, amount = 1 });
                            }

                            staminaTaken = true;
                        }
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
            if (AudioManager.Instance != null)
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
            if (AudioManager.Instance != null)
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
            if (spaceship.spaceshipEnergy <= 0 || stamina >= 100)
            {
                if (AudioManager.Instance != null)
                    AudioManager.Instance.playGeneratorError();

                return;
            }

            spaceship.ChargePlayer(ref stamina);

            // Play the player recharge sfx
            if (AudioManager.Instance != null)
                AudioManager.Instance.playGeneratorRecharge();
        }

    }

    void GoToSleep(ref BedController bed)
    {
        // Call the sleeping in bed animation
        bed.BedAnimation();

        // Figure out how long the crops grow between now and 7am next day
        // Then convert this into seconds and grow the crops that long
        int hoursGrowth;
        int minsGrowth;

        // If the time is not between midnight and 7am, stay on the current day
        if (TimeManager.Hour < 24 && TimeManager.Hour >= 7)
        {
            // Increase the day count
            TimeManager.Day += 1;
            TimeManager.OnDayChanged?.Invoke();

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

        hasInteractedScreenToday = false;
    }

    // Set the current equipped item from the inventory
    public void UseItem(Item item)
    {
        toolSprite.SetActive(true);
        switch (item.itemType)
        {
            case Item.ItemType.cropA:

                // If already carrying crop A, unequip it
                if (carryCropA)
                {
                    ChangeCarriedCrops(false, false);
                    spriteRenderer.sprite = spriteArray[(int)currentTool];
                    DataPermanence.Instance.currentTool = (int)currentTool;
                }

                // If not already carrying crop A, equip it
                else
                {
                    ChangeCarriedCrops(true, false);
                    spriteRenderer.sprite = spriteArray[5];
                    DataPermanence.Instance.currentTool = 5;
                }

                // Close inventory once an item is selected
                ui_Inventory.OpenInventory();

                break;

            case Item.ItemType.cropB:

                // If already carrying crop A, unequip it
                if (carryCropB)
                {
                    ChangeCarriedCrops(false, false);
                    spriteRenderer.sprite = spriteArray[(int)currentTool];
                    DataPermanence.Instance.currentTool = (int)currentTool;

                }

                // If not already carrying crop A, equip it
                else
                {
                    ChangeCarriedCrops(false, true);
                    spriteRenderer.sprite = spriteArray[6];
                    DataPermanence.Instance.currentTool = 6;
                }

                // Close inventory once an item is selected
                ui_Inventory.OpenInventory();

                break;

            case Item.ItemType.hoe:

                // set the current tool to hoe
                currentTool = Tools.hoe;
                spriteRenderer.sprite = spriteArray[0];
                DataPermanence.Instance.hoe--;
                DataPermanence.Instance.currentTool = (int)currentTool;
                // Any equipped crops are unequipped
                ChangeCarriedCrops(false, false);

                // Close inventory once an item is selected
                ui_Inventory.OpenInventory();

                break;

            case Item.ItemType.wateringCan:

                // Set the current tool to watering can
                currentTool = Tools.wateringCan;
                spriteRenderer.sprite = spriteArray[2];
                DataPermanence.Instance.wateringCan--;
                DataPermanence.Instance.currentTool = (int)currentTool;
                // Any equipped crops are unequipped
                ChangeCarriedCrops(false, false);

                // Close inventory once an item is selected
                ui_Inventory.OpenInventory();

                break;

            case Item.ItemType.scythe:

                // Set the current tool to scythe
                currentTool = Tools.scythe;
                spriteRenderer.sprite = spriteArray[3];
                DataPermanence.Instance.scythe--;
                DataPermanence.Instance.currentTool = (int)currentTool;
                // Any equipped crops are unequipped
                ChangeCarriedCrops(false, false);

                // Close inventory once an item is selected
                ui_Inventory.OpenInventory();

                break;

            case Item.ItemType.seedA:

                // Set the current tool to seed A
                currentTool = Tools.seedA;
                spriteRenderer.sprite = spriteArray[1];
                DataPermanence.Instance.seedA--;
                DataPermanence.Instance.currentTool = (int)currentTool;
                // Any equipped crops are unequipped
                ChangeCarriedCrops(false, false);

                // Close inventory once an item is selected
                ui_Inventory.OpenInventory();

                break;

            case Item.ItemType.seedB:

                // Set the current tool to seed B
                currentTool = Tools.seedB;
                spriteRenderer.sprite = spriteArray[4];
                DataPermanence.Instance.seedB--;
                DataPermanence.Instance.currentTool = (int)currentTool;
                // Any equipped crops are unequipped
                ChangeCarriedCrops(false, false);

                // Close inventory once an item is selected
                ui_Inventory.OpenInventory();

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
        // Enable the pause screen and menu
        pauseScreen.SetActive(true);

        // Pause the game's timescale
        Time.timeScale = 0f;

        // close the inventory when pausing if it's open
        if (ui_Inventory.isInventoryVisible)
            ui_Inventory.OpenInventory();
    }

    // Set if a crop is carried
    // Used to feed it into the generator when interacting with the generator
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
    void CheckTutorial1Over()
    {
        // If any of the 9 tiles are untilled, leave this function - this stage of the tutorial is incomplete
        foreach (Vector3Int tile in tutorialTiles)
            if (TilemapManager.Instance != null && !TilemapManager.Instance.IsTilled(tile))
                return;

        // Add seed A to inventory
        // THis condition stops it being constantly added every update
        if (tutorialNumber == 0)
            inventory.AddItem(new Item { itemType = Item.ItemType.seedA, amount = 10 });

        // If all the tiles are tilled we reach this  line - move to the next tutorial stage
        ChangeTutorialStage(2, 1);
    }

    // Check if the second phase of the tutorial is over
    // This is done by checking if a seed has been planted
    void CheckTutorial2Over()
    {
        // Get an array of all crops in the scene
        // This will only be the 3x3 tutorial square of crops at this point
        GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");

        // Check that the array is neither null nor empty
        // This means that the crops have been planted here
        if (crops?.Length > 0 && tutorialNumber == 1)
        {
            ChangeTutorialStage(3, 2);
            // Add the watering can to inventory for the next stage of the tutorial
            inventory.AddItem(new Item { itemType = Item.ItemType.wateringCan, amount = 1 });
        }
    }

    // Check if the third phase of the tutorial is over
    // This is done by checking if all the crops have been watered
    void CheckTutorial3Over()
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

    // Check if the fourth phase of the tutorial is over
    // This is done by checking if the spaceship is entered
    void CheckTutorial4Over()
    {
        // Check if the player has entered the spaceship in tutorial four
        // If so, move to the next tutorial stage
        if (tutorialNumber == 3 && (SceneManager.GetActiveScene().name == "Sangit SpaceShip 3" || SceneManager.GetActiveScene().name == "AlexTestScene SpaceShip"))
            ChangeTutorialStage(3, 4);
    }

    // Check if the fifth phase of the tutorial is over
    // This is done by checking if the screen is interacted with
    void CheckTutorial5Over()
    {
        if (tutorialNumber == 4 && hasInteractedScreenToday && !readingCaptainsLog)
            ChangeTutorialStage(4, 5);
    }

    // Check if the sixth phase of the tutorial is over
    // This is done by checking if the bed is interacted with
    void CheckTutorial6Over()
    {
        if (tutorialNumber == 5)
            inventory.AddItem(new Item { itemType = Item.ItemType.scythe, amount = 1 });

        ChangeTutorialStage(4, 6);
    }

    // Check if the seventh phase of the tutorial is over
    // This is done by checking if the crops are all harvested
    void CheckTutorial7Over()
    {
        if (SceneManager.GetActiveScene().name == "AlexTestScene" || SceneManager.GetActiveScene().name == "SangitTestScene3")
        {
            GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");

            if (crops.Length == 0)
                ChangeTutorialStage(4, 7);
        }

    }

    // Check if the eighth phase of the tutorial is over
    // This is called from the yarn spinner script
    [YarnCommand("TutSeven")]
    void CheckTutorial8Over()
    {
        // Check if the player has spoken to Vas
        // If so, move to the next tutorial stage
        ChangeTutorialStage(4, 8);
    }

    // Check if the ninth phase of the tutorial is over
    // This is done by checking if the player enters the ship
    void CheckTutorial9Over()
    {
        if (tutorialNumber == 6 && SceneManager.GetActiveScene().name == "Sangit SpaceShip 3" || SceneManager.GetActiveScene().name == "AlexTestScene SpaceShip")
            ChangeTutorialStage(4, 9);
    }

    // Check if the tenth phase of the tutorial is over
    // This is done by checking if the player feeds a crop into the generator
    void CheckTutorial10Over()
    {
        if (DataPermanence.Instance.cropA == 8)
            ChangeTutorialStage(4, 10);
    }

    // Check if the eleventh phase of the tutorial is over
    // This is done by checking if the player recharges their stamina from the spaceship
    void CheckTutorial11Over()
    {
        if (tutorialNumber == 10 && DataPermanence.Instance.spaceshipEnergy == 0)
            ChangeTutorialStage(4, 11);
    }
    void CheckTutorialOver()
    {
        if (tutorialNumber == 11 && (SceneManager.GetActiveScene().name == "AlexTestScene" || SceneManager.GetActiveScene().name == "SangitTestScene3"))
        {
            ChangeTutorialStage(5, 12);
            inTutorial = false;
            DataPermanence.Instance.playerTutorial = false;
            inventory.AddItem(new Item { itemType = Item.ItemType.seedB, amount = 10 });
        }
    }

    //Skips over tutorial when key is pressed
    void SkipOverTutorial()
    {
        TimeManager.Day++;
        TimeManager.OnDayChanged?.Invoke();
        ChangeTutorialStage(5, 12);
        inTutorial = false;
    }

    // A seperate function to call this audio manager function
    // Used to call the function as an animation event
    // Therefore can not be called directly from the audio manager
    // Because the player doesn't contain an audio manage component
    public void footstepsAudio()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.playFootsteps();
    }
}