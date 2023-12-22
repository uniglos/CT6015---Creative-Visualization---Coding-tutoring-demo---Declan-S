using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionSlide : Slide
{
    private bool advancing = false;
    private int dialogIndex = -1;

    [SerializeField] private List<string> gooberLines;

    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;

    private bool button1Pressed = false;
    private bool button2Pressed = false;
    private bool button3Pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(slideFunctionality());
    }

    protected virtual IEnumerator slideFunctionality()
    {
        // Wait to get tweened onscreen
        yield return new WaitForSeconds(MainSceneManager.current.slideTransitionTime);

        MainSceneManager.current.Dialogue("Let's try it out, break this problem down into steps!");
        yield return new WaitForSeconds(6f);

        // Out of dialogue - Prompt Question!!
        // The correct answer requires defining a variable, increasing that variable, and then using that variable in the attack function parameter
        MainSceneManager.current.Dialogue("In which order would I write these if I wanted to use the attack function snippet here with the highest value?");

        yield return StartCoroutine(WaitForAdvance());
        MainSceneManager.current.Dialogue("That's right! You need to define your variable, after which you can modify it, and finally retrieve it.");
        yield return new WaitForSeconds(6f);
        MainSceneManager.current.Dialogue("I'm sorry, but I don't have time to go into more detail right now, but I hope this has helped you out!");
        yield return StartCoroutine(WaitForAdvance());
        Application.Quit();
        MainSceneManager.current.NextSlide();
    }

    protected virtual string GetNextDialogue()
    {
        dialogIndex += 1;
        if (dialogIndex + 1 > gooberLines.Count)
        {
            return "";
        } 
        else
        {
            return gooberLines[dialogIndex];
        }
    }

    protected IEnumerator WaitForAdvance()
    {
        yield return new WaitUntil(() => advancing);
        advancing = false;
    }

    public void Advance()
    {
        advancing = true;
    }

    // Setup of quiz buttons
    public void OnClickButton1()
    {
        button1Pressed = true;
        Debug.Log("button one clicked");
        MainSceneManager.current.GetComponent<AudioSource>().pitch = 1.0f;
        MainSceneManager.current.GetComponent<AudioSource>().Play();
    }

    public void OnClickButton2()
    {
        button2Pressed = true;
        Debug.Log("button two clicked");
        if (!button1Pressed)
        {
            MainSceneManager.current.Dialogue("Not quite that, you need to define your variables before you can refernece or use them.");
            button2Pressed = false;
        }
        else
        {
            MainSceneManager.current.GetComponent<AudioSource>().pitch = 1.0f;
            MainSceneManager.current.GetComponent<AudioSource>().Play();
        }
    }

    public void OnClickButton3()
    {
        button3Pressed = true;
        Debug.Log("button three clicked");
        if(!button1Pressed)
        {
            MainSceneManager.current.Dialogue("Not quite that, you need to define your variables before you can refernece or use them.");
        } 
        else if (!button2Pressed)
        {
            MainSceneManager.current.Dialogue("Almost! We can get the power value higher before we attack!");
            button1Pressed = false;
        }
        else
        {
            MainSceneManager.current.GetComponent<AudioSource>().pitch = 1.0f;
            MainSceneManager.current.GetComponent<AudioSource>().Play();
            advancing = true;
        }
    }
}
