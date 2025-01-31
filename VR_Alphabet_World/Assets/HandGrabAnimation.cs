using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandGrabAnimation : MonoBehaviour
{
    public Animator animator;
    public XRNode controllerNode; // Assign LeftHand or RightHand in Inspector
    private InputDevice targetDevice;

    [Header("Grabbable Objects")]
    public List<GameObject> grabbableObjects = new List<GameObject>(); // Assign in Unity
    public float grabRange = 4.0f; // Distance to check for objects

    public GameObject positionPoke;

    private GameObject heldObject = null; // Stores the object currently held

    void Start()
    {
        animator = GetComponent<Animator>();
        InitializeXRController();
    }

    void Update()
    {
        if (!targetDevice.isValid)
        {
            InitializeXRController();
        }

        // Detect if grip button is pressed
        bool isGripping = false;
        if (targetDevice.isValid)
        {
            targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out isGripping);
        }

        // Fallback for keyboard testing
        if (Input.GetKey(KeyCode.G))
        {
            isGripping = true;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            isGripping = false;
        }

        // Debug grip button state
        Debug.Log($"Grip button pressed: {isGripping}");

        // Check if the hand is actually holding an object when gripping
        if (isGripping && CheckForNearbyObject())
        {
            animator.SetBool("isGrabbing", true);
            Debug.Log("[HandAnimation] Grabbing object.");
        }
        else
        {
            animator.SetBool("isGrabbing", false);
            Debug.Log("[HandAnimation] Not grabbing.");
        }
    }

    // Detect and assign the correct XR controller
    void InitializeXRController()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(controllerNode, devices);
        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            Debug.Log($"[HandAnimation] Controller found: {targetDevice.name}");
        }
        else
        {
            Debug.LogWarning("[HandAnimation] No controller found.");
        }
    }

    // Function to check if a grabbable object is within range
    private bool CheckForNearbyObject()
    {
        foreach (GameObject obj in grabbableObjects)
        {
            // Calculate distance and check if it's within range
            float distance = Vector3.Distance(positionPoke.transform.position, obj.transform.position);
            Debug.Log($"[HandAnimation] Checking object: {obj.name}, Distance: {distance}");
            Debug.Log($"Grab range: {grabRange}");

            if (distance <= grabRange)
            {
                heldObject = obj;
                Debug.Log($"[HandAnimation] Object within range: {obj.name}");
                return true;
            }
        }

        heldObject = null;
        Debug.Log("[HandAnimation] No objects in range.");
        return false;
    }
}