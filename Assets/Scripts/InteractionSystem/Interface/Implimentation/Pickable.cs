using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Pickable : MonoBehaviour, IInteractable
{

    public Transform playerHand;
    [SerializeField] private float pickUpSpeed = 20f;
    private Rigidbody rb;

    [SerializeField] bool isPickedup = false;
    private Coroutine pickupRoutine;
    private float forceSpeed = 5f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && isPickedup)
        //{

        //    Drop();

        //}
    }



    public void Interact()
    {
        if (isPickedup) return;
        isPickedup = true;
        pickupRoutine = StartCoroutine(PickupRoutine());

    }

    private IEnumerator PickupRoutine()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        if (rb) rb.isKinematic = true;
        rb.useGravity = false;

        while (Vector3.Distance(transform.position, playerHand.position) > 0.05f)
        {

            transform.position = Vector3.Lerp(transform.position, playerHand.position, pickUpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerHand.rotation, pickUpSpeed * Time.deltaTime);

            yield return null;

        }

        transform.position = playerHand.position;
        transform.rotation = playerHand.rotation;
        transform.SetParent(playerHand);

    }

    public void ExitInteract()
    {
        isPickedup = false;


        if (pickupRoutine != null)
        {
            StopCoroutine(pickupRoutine);
        }

        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(transform.right * forceSpeed, ForceMode.Impulse);


    }
}
