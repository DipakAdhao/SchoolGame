using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    // # Issue happed here 2

    public enum PlayerState
    {
        Normal,
        Inspecting,
        Crouching,
        Hiding
    }

    //public bool isInputLocked { get; private set; }

    public PlayerState currentState { get; private set; }
    public Transform PlayerTransform { get; private set; }
    void Awake()
    {
        Instance = this;
        currentState = PlayerState.Normal;
        PlayerTransform = transform;

    }

    void Start()
    {
    }
    public void SetState(PlayerState newState)
    {
        currentState = newState;

    }

    //public void LockInput()
    //{

    //   isInputLocked = true;

    //}
  
    //public void UnLockInput()
    //{

    //    isInputLocked = false;
    //}


}
