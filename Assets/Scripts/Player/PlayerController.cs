using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    private CharacterController controller;


    [Header("inputsystem Thingy")]
    [SerializeField] private PlayerInputActions input;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 lookInput;


    [Header("Rotation")]
    [SerializeField] public float mouseSensitivity = 20f;
    [SerializeField] private float xRotation = 0f;
    public Transform cameraPivot;

    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2f;

    [SerializeField] private float crouchSpeed = 5f;
    [SerializeField] private float standSpeed = 10f;


    [SerializeField] private float crouchTransformY = 0.5f;
    [SerializeField] private float standingTransformY = 1f;

    //private bool isCrouching;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = new PlayerInputActions();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Input__________________
    private void OnEnable()
    {

        input.Enable();

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;


        input.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        input.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        input.Player.Crouch.performed += ctx => StartCrouch();
        input.Player.Crouch.canceled += ctx => StopCrouch();

    }


    private void OnDisable()
    {
        input.Disable();
    }


    void Update()
    {
        PlayerStateManager.PlayerState state =
        PlayerStateManager.Instance.currentState;

        if (state == PlayerStateManager.PlayerState.Inspecting ||
            state == PlayerStateManager.PlayerState.Hiding)
        {
            return;
        }

        // if (PlayerStateManager.Instance.currentState == PlayerStateManager.PlayerState.Inspecting) return;
        // if (PlayerStateManager.Instance.currentState != PlayerStateManager.PlayerState.Normal) return;
        //if (PlayerStateManager.Instance.isInputLocked) return;

        PlayerMovement();
        PlayerRotate();
        HandleCrouching();

    }

    // Movement-----
    private void PlayerMovement()
    {

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    // Rotating-----
    private void PlayerRotate()
    {
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;  // old way
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;  // old way

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;  // new way ( can clean but later)
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;  //new way

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    // Crouching-----
    private void StartCrouch()
    {

        if (PlayerStateManager.Instance.currentState != PlayerStateManager.PlayerState.Normal)
            return;
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Crouching);
        moveSpeed = crouchSpeed;

        //isCrouching = true;

        //controller.height = Mathf.Lerp(controller.height, crouchHeight, 2f * Time.deltaTime);
        //moveSpeed = crouchspeed;

        //Vector3 camPos = cameraPivot.localPosition;
        //camPos.y = crouchTransformY;
        //cameraPivot.localPosition = camPos;

    }

    private void StopCrouch()
    {

        if (PlayerStateManager.Instance.currentState != PlayerStateManager.PlayerState.Crouching)
            return;
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);
        moveSpeed = standSpeed;
       // isCrouching = false;

        //controller.height = Mathf.Lerp(standingHeight, controller.height, 2f * Time.deltaTime);
        //moveSpeed = standspeed;

        //Vector3 camPos = cameraPivot.localPosition;
        //camPos.y = standingHeight;
        //cameraPivot.localPosition = camPos;

    }

    private void HandleCrouching()
    {
        if(PlayerStateManager.Instance.currentState == PlayerStateManager.PlayerState.Crouching)
        {
            controller.height = Mathf.Lerp(controller.height, crouchHeight, 2f * Time.deltaTime);
            //moveSpeed = crouchspeed;

            Vector3 camPos = cameraPivot.localPosition;
            camPos.y = crouchTransformY;
            cameraPivot.localPosition = camPos;

        }
        else if (PlayerStateManager.Instance.currentState == PlayerStateManager.PlayerState.Normal)
        {
            controller.height = Mathf.Lerp(standingHeight, controller.height, 2f * Time.deltaTime);
            //moveSpeed = standspeed;

            Vector3 camPos = cameraPivot.localPosition;
            // camPos.y = standingHeight;
            camPos.y = standingTransformY;
            cameraPivot.localPosition = camPos;

        }
    }
}
