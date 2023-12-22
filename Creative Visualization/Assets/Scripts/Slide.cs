using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    private bool advancing = false;
    private int dialogIndex = -1;

    [SerializeField] private List<string> gooberLines;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(slideFunctionality());
    }

    protected virtual IEnumerator slideFunctionality()
    {
        // Wait to get tweened onscreen
        yield return new WaitForSeconds(MainSceneManager.current.slideTransitionTime);

        while (dialogIndex+1 < gooberLines.Count)
        {
            // Do dialogue
            MainSceneManager.current.Dialogue(GetNextDialogue());

            // Wait until advancing
            yield return StartCoroutine(WaitForAdvance());
        }

        // Out of dialogue
        MainSceneManager.current.NextSlide();

    }
    
    private string superSecretMessage = "Hello World!";


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
}
