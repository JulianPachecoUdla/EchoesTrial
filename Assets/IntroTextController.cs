using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroTextController : MonoBehaviour
{
    public string sceneToLoad = "SampleScene";
    public float waitTime = 8f;

    void Start()
    {
        StartCoroutine(WaitAndLoadScene());
    }

    IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}