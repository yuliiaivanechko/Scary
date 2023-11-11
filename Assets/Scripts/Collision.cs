using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("House"))
        {
            Debug.Log("Collided with a House object.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
