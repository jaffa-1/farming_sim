using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Movement WASD
    CharacterController characterController;
    float movementSpeed = 25f;
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
    ICanInteract equippedInteractable;
    [SerializeField] Transform spawnTransform;

    public static event EventHandler<OnInteractableChangedEventArgs> OnInteractableChanged;

    public class OnInteractableChangedEventArgs : EventArgs
    {
        public ICanInteract Interactable;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        //input events

        GameInput.Instance.OnJump += GameInput_OnJump;
        GameInput.Instance.OnInteract += GameInput_OnInteract;
    }

    private void GameInput_OnInteract(object sender, System.EventArgs e)
    {
        if (Interactable != null)
        {
            Interactable.Interact();
            Debug.Log("Interacted with " + Interactable);
        }
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
                Interactable = plantSite;
                OnInteractableChanged?.Invoke(this, new OnInteractableChangedEventArgs
                {
                    Interactable = plantSite
                });
            }

            if (interactHit.transform.TryGetComponent(out Plant plant))
            {
                //looking at plant
                Interactable = plant;
                OnInteractableChanged?.Invoke(this, new OnInteractableChangedEventArgs
                {
                    Interactable = plant
                });
            }
        }
        else
        {
            if (Interactable != null)
            {
                Interactable = null;
                OnInteractableChanged?.Invoke(this, new OnInteractableChangedEventArgs
                {
                    Interactable = null
                });
            }
        }
    }

    #endregion

}
