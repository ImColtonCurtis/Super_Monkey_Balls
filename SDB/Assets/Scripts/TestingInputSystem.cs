using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingInputSystem : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        //playerInputActions.Player.Jump.performed += Jump;

        // button rebinding
        /*
        playerInputActions.Player.Disable();
        playerInputActions.Player.Jump.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse") // not just mouse movements but clicks too
            .OnComplete(callback => {
                Debug.Log(callback);
                callback.Dispose();
                playerInputActions.Player.Enable();
            })
            .Start();
        */
        // save rebind
        // google how (save through a json file or something)

        playerInput.SwitchCurrentActionMap("UI");
        playerInputActions.Player.Disable();
        playerInputActions.UI.Enable();
    }

    private void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            playerInput.SwitchCurrentActionMap("UI");
            playerInputActions.Player.Disable();
            playerInputActions.UI.Enable();
        }
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            playerInput.SwitchCurrentActionMap("Player");
            playerInputActions.UI.Disable();
            playerInputActions.Player.Enable();
        }
    }

    private void FixedUpdate()
    {        
        //Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        //float speed = 5f;
        //sphereRigidbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (context.performed)
        {
            Debug.Log("jump" + context.phase);
        }
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Submit " + context);
        }
    }
}
