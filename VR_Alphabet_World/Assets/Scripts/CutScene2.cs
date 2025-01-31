using UnityEngine;
using UnityEngine.Playables;

public class CutScene2 : MonoBehaviour
{
    public PlayableDirector timeline;

    private bool timelineStarted = false;

    void Start()
    {
        if (timeline != null)
        {
            timeline.Pause();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !timelineStarted)
        {
            StartTimeline();
        }
    }

    private void StartTimeline()
    {
        if (timeline != null)
        {
            timeline.Play();
            timelineStarted = true;
        }
        else
        {
            Debug.LogWarning("Timeline-ul nu este asociat în inspector!");
        }
    }
}
