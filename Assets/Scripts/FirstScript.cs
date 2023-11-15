using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Perform actions when the spacebar is pressed

            GameObject.FindGameObjectWithTag("Load1Scene").GetComponent<LevelChangerScript>().FadeIntoLevel(1);
        }
    }
}
