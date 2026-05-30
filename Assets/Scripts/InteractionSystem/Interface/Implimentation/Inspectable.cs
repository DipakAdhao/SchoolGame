using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static PlayerStateManager;

public class Inspectable : MonoBehaviour, IInteractable
{

    // [SerializeField]Camera cam;
    public Transform inspectPos;
    bool isInspecting = false;
    bool isReturning;

    Vector3 originPos;
    Quaternion originRot;
    [SerializeField] float rotateSpeed = 0.1f;
    float moveSpeed = 10f;

    PlayerState previousState;
    private void Awake()
    {
        //cam = Camera.main;
        originPos = transform.position;
        originRot = transform.rotation;
    }
    void Start()
    {
    }

    void Update()
    {
        if (isInspecting)
        {
            HandleInspectRotation();
            transform.position = Vector3.Lerp(transform.position, inspectPos.position, moveSpeed * Time.deltaTime);
        }

        if (isReturning)
        {
            transform.position = Vector3.Lerp(transform.position, originPos, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Slerp(transform.rotation, originRot, Time.deltaTime * 10f);

            float distance = Vector3.Distance(transform.position, originPos);

            if (distance < 0.05f)
            {
                isReturning = false;
                isInspecting = false;
                transform.SetParent(null);

            }


        }


    }

    public void Interact()
    {
        Inspect();
        // PlayerStateManager.Instance.LockInput(); //
        // PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Inspecting);
        previousState = PlayerStateManager.Instance.currentState;
        PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Inspecting);
    }


    private void Inspect()
    {
        isInspecting = true;
        isReturning = false;
        transform.SetParent(null);

    }


    public void ExitInteract()
    {
        Debug.Log("droping shit");
        isInspecting = false;
        isReturning = true;
        //PlayerStateManager.Instance.UnLockInput(); //
        // PlayerStateManager.Instance.SetState(PlayerStateManager.PlayerState.Normal);
        PlayerStateManager.Instance.SetState(previousState);
    }
    //# issue happend here 1

    private void HandleInspectRotation()
    {

        Vector2 delta = Mouse.current.delta.ReadValue();

        float mouseX = delta.x;
        float mouseY = delta.y;
        

        //float mouseX = Input.GetAxis("Mouse X");  //new 
        //float mouseY = Input.GetAxis("Mouse Y");  //new

        transform.Rotate(Camera.main.transform.up, -mouseX * rotateSpeed , Space.World);
        transform.Rotate(Camera.main.transform.right, mouseY * rotateSpeed , Space.World);

    }

}
