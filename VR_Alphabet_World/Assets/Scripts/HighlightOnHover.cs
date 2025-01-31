/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HighlightOnHover : MonoBehaviour
{
    private Outline outline;

    void Awake()
    {
        // Get the Outline component
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogWarning("No Outline component found on " + gameObject.name);
        }
    }

    public void OnHoverEnter()
    {
        if (outline != null)
        {
            outline.OutlineColor = Color.yellow; // Set the desired highlight color
            outline.OutlineWidth = 5.0f; // Set the outline width
        }
    }

    public void OnHoverExit()
    {
        if (outline != null)
        {
            outline.OutlineColor = Color.clear;  // Remove the highlight
            outline.OutlineWidth = 0f;          // Reset the outline width
        }
    }
}
*/

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HighlightOnHover : MonoBehaviour
{
    private Outline outline;
    private Rigidbody rb;
    private Vector3 initialPosition=new Vector3(0.158350423f, 0.0149999997f, 0.039779f);
    private Quaternion initialRotation;

    void Awake()
    {
        // Get the Outline component
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogWarning("No Outline component found on " + gameObject.name);
        }

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody component found on " + gameObject.name);
        }
        Debug.Log("Initial Position: " + initialPosition);
        // Store the initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void OnHoverEnter()
    {
        if (outline != null)
        {
            outline.OutlineColor = Color.yellow; // Set the desired highlight color
            outline.OutlineWidth = 5.0f; // Set the outline width
        }
        LockHighlightAndPosition(true);
    }

    public void OnHoverExit()
    {
        if (outline != null)
        {
            outline.OutlineColor = Color.clear;  // Remove the highlight
            outline.OutlineWidth = 0f;          // Reset the outline width
        }
        LockHighlightAndPosition(true);
    }

    public void LockHighlightAndPosition(bool isLocked)
    {
        if (outline != null)
        {
            if (isLocked)
            {

                if (rb != null)
                {
                    rb.isKinematic = true; // Prevent movement
                }

                // Reset to initial position and rotation
                transform.position = initialPosition;
                transform.rotation = initialRotation;
            }
            else
            {
                outline.OutlineColor = Color.clear; // Remove highlight when unlocked
                outline.OutlineWidth = 0f;         // Reset the outline width

                if (rb != null)
                {
                    rb.isKinematic = false; // Allow movement
                }
            }
        }
    }
}
