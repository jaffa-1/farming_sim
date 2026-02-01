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

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        //input events

        GameInput.Instance.OnJump += GameInput_OnJump;
        GameInput.Instance.OnInteract += GameInput_OnInteract;
    }

    private void GameInput_OnInteract(object sender, System.EventArgs e)
    {
        
    }
    void Update()
    {
        HandleMovement();
        HandleCamera();
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

    void Jump()
    {
        if (isGrounded)
            upwardVelocity.y = Mathf.Sqrt(-2 * gravity * jumpHeight);
    }

    #endregion

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
}
