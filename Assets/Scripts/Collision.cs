using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    // Start is called before the first frame update


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("House"))
        {
            Debug.Log("Collided with a House object.");
            GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopMusic();
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<LevelChangerScript>().FadeIntoLevel(2);

        }
    }
}
