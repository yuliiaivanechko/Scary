using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangerScript : MonoBehaviour
{
    Animator fadeanimator;
    private int levelIndex;

    public void FadeIntoLevel(int index)
    {
        levelIndex = index;
        fadeanimator.SetBool("FadeOut", true);
    }
    // Start is called before the first frame update
    void Start()
    {
        fadeanimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void OnGameComplete()
    {
        //Application.Quit();
    }
}
