using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutScene1 : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(NextScene());
    }

    void Update()
    {
        
    }

    private IEnumerator NextScene()
    {
        yield return new WaitForSeconds(32);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
