using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonSceneLoader : MonoBehaviour
{
    public string loadSceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hi() {
        SceneManager.LoadScene(loadSceneName);
    }
}
