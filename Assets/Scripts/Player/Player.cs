using System;
using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    //Movement WASD
    CharacterController characterController;
    float movementSpeed = 15f;
    bool isGrounded;
    float gravity = -9.8f;
    float jumpHeight = 10;
    float fallSpeed = 5;
    Vector3 upwardVelocity;

    //camera
    private float xRotation;
    private float yRotation;

    //interact

    float interactDistance = 1.5f;
    Ray interactRay;
    RaycastHit interactHit;
    
    ICanInteract Interactable;

    Plant equippedPlant;
    Tools equippedTool;
    [SerializeField] Transform interactSpawnTransform;

    public static event EventHandler<OnInteractableChangedEventArgs> OnInteractableChanged;

    public class OnInteractableChangedEventArgs : EventArgs
    {
        public ICanInteract Interactable;
    }

    [Serializable]
    class plantItem
    {
        public PlantSO plantSO;
        public int itemCount;

        public plantItem(PlantSO newplant,int count)
        {
            plantSO = newplant;
            itemCount = count;
        }
        public void IncreaseCount()
        {
            itemCount++;
        }
    }

    [SerializeField]
    List<plantItem> plantItemlist;

    private void Awake()
    {
        plantItemlist = new List<plantItem>();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        //input events

        GameInput.Instance.OnJump += GameInput_OnJump;
        GameInput.Instance.OnInteract += GameInput_OnInteract;
        GameInput.Instance.OnPlantEquipped += GameInput_OnPlantEquipped;
    }

    private void GameInput_OnPlantEquipped(object sender, EventArgs e)
    {
        
    }

    private void FixedUpdate()
    {
        HandleMovement();

    }
    void Update()
    {
        HandleCamera();
        HandleInteract();
    }

    #region |---movement---|
    private void GameInput_OnJump(object sender, System.EventArgs e)
    {
        Jump();
    }


    void HandleMovement()
    {
        Vector3 moveDir = GameInput.Instance.GetInputVectorNormalized();

        isGrounded = characterController.isGrounded;

        characterController.Move(transform.TransformDirection(moveDir) * movementSpeed * Time.deltaTime);

        if (isGrounded && upwardVelocity.y < 0)
        upwardVelocity.y = -2f;

        upwardVelocity.y += gravity * fallSpeed * Time.deltaTime;

        characterController.Move(upwardVelocity * Time.deltaTime);
    }


    void HandleCamera()
    {
        Vector2 mouseInput = GameInput.Instance.GetMouseDelta();

        // vertical look
        xRotation -= mouseInput.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseInput.x;
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // horizontal look
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void Jump()
    {
        if (isGrounded)
            upwardVelocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
    }

    #endregion

    #region |---Interaction---|

    private void HandleInteract()
    {
        interactRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.transform.TryGetComponent(out PlantSite plantSite))
            {
                // looking at a plant site 
                SetInteractable(plantSite);
            }

            if (interactHit.transform.TryGetComponent(out Plant plant))
            {
                //looking at plant
                SetInteractable(plant);
            }
            if (interactHit.transform.TryGetComponent(out Tools tools))
            {
                SetInteractable(tools);
            }
            if (interactHit.transform.TryGetComponent(out SeedStorage seedStorage))
            {
                SetInteractable(seedStorage);
            }
        }
        else
        {
            if (Interactable != null)
            {
                SetInteractable(null);
            }
        }
    }

    void SetInteractable(ICanInteract iCanInteract)
    {
        Interactable = iCanInteract;
        OnInteractableChanged?.Invoke(this, new OnInteractableChangedEventArgs
        {
            Interactable = iCanInteract
        });
    }

    private void GameInput_OnInteract(object sender, System.EventArgs e)
    {
        if (Interactable != null)
        {
            Interactable.Interact(this);
        }
    }

    public Transform GetInteractSpawn()
    {
        return interactSpawnTransform;
    }
    #endregion
    public bool UpdatePlantInventory(PlantSO plantSO)
    {
        for (int i = 0; i < plantItemlist.Count; i++)
        {
            if (plantItemlist[i].plantSO == plantSO)
            {
                //player has plant
                plantItemlist[i].IncreaseCount();
                Debug.Log(plantItemlist[i].itemCount);
                return true;
            }
        }
        //player does not have item 
        plantItem plantitem =new(plantSO,1);
        plantItemlist.Add(plantitem);
        return false;

    }

    public void SetEquippedPlant(Plant plant)
    {
        equippedPlant = plant;
    }

    public bool HasEquippedPlant()
    {
        if (equippedPlant != null)
        {
            return true;
        }
        return false;
    }

    public Plant GetEquippedPlant()
    {
        return equippedPlant;
    }

    public void SetEquippedTool(Tools tool)
    {
        equippedTool = tool;
    }
    public bool HasEquippedTool()
    {
        if (equippedTool != null)
        {
            return true;
        }
        return false;
    }

}
