using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;

    //player input map
    PlayerInput playerInput;

    //events
    public event EventHandler OnJump;
    public event EventHandler OnInteract;
    public event EventHandler OnPlantEquipped;
    public event EventHandler OnToolEquipped;

    //camera
    float mouseSensitivity = 25;

    private void Awake()
    {
        playerInput = new PlayerInput();

        playerInput.player.move.Enable();
        playerInput.player.jump.Enable();
        playerInput.player.interact.Enable();
        playerInput.player.mouse.Enable();
        playerInput.player.equipPlant.Enable();
        playerInput.player.equipTool.Enable();

        Instance = this;
    }

    private void Start()
    {
        playerInput.player.jump.performed += Jump_performed;
        playerInput.player.interact.performed += Interact_performed;
        playerInput.player.equipPlant.performed += EquipPlant_performed;
        playerInput.player.equipTool.performed += EquipTool_performed;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void EquipTool_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnToolEquipped?.Invoke(this,EventArgs.Empty);
    }

    private void EquipPlant_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlantEquipped?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJump?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetInputVectorNormalized()
    {
        Vector2 MoveInputVector = playerInput.player.move.ReadValue<Vector2>();

        return new Vector3(MoveInputVector.x,0,MoveInputVector.y);
    }

    public Vector2 GetMouseDelta()
    {
        Vector2 mouseInputVector = playerInput.player.mouse.ReadValue<Vector2>();
        return new Vector2(mouseInputVector.x * mouseSensitivity * Time.deltaTime,mouseInputVector.y * mouseSensitivity * Time.deltaTime);
    }
}
