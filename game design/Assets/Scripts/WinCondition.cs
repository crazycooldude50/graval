using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            string currentLevel = SceneManager.GetActiveScene().name;

            int levelNum = int.Parse(currentLevel.Substring(5));
            // Loads the next level
            SceneManager.LoadScene("Level " + (levelNum + 1));
        }
    }
}
