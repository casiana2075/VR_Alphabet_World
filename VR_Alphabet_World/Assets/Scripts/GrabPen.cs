using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class GrabPen : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;
    [HideInInspector]
    public int penCount = 0;
    [SerializeField] AudioSource grabAudio;
    [SerializeField] NarratorSpeaking narratorSpeaking;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        bool ceva = narratorSpeaking.Test();
        Debug.Log("Ceva este:"+ceva);
        isGrabbed = true;
        if(penCount == 0 && ceva)
        {
            penCount++;
            grabAudio.Play();
        }
        else
        {
            Debug.Log("You already have a pen!");
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        Debug.Log($"{gameObject.name} has been released!");
    }

    public bool IsGrabbed()
    {
        return isGrabbed;
    }
}
