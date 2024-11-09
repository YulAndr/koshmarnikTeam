using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LevelSwitch : MonoBehaviour
{
    public int sceneBuildIndex;

    public bool interactionInput;

    public bool isInRange;

    
    private void Update () {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            interactionInput = true;
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
            print("interaction");
        }
    }
    
    private void OnTriggerEnter2D (Collider2D other) {
        print("Trigger Entered");
        
        if (other.tag == "Player")
        {
            isInRange = true;
            //print("Switching Scene to " + sceneBuildIndex);
            //SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }

    private void OnTriggerExit2D (Collider2D other) {
        if (other.tag == "Player")
        {
            isInRange = false;
            //print("Switching Scene to " + sceneBuildIndex);
            //SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}
