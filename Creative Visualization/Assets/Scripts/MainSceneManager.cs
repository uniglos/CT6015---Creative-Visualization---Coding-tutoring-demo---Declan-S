using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    /* Main scene manager - 
     
     * Singleton class, of course, just what each element calls back to.
     * There's always a character portrait which we can swap between. Public function for setCharacterSprite
     * We also need to be able to advance to the next "window" or segment of information. Going back should also be an option. Maybe we can set checkpoints we can warp backwards to?
     * 
     * Slides work by being in a list of a serialized custom class, containing a prefab (the slide), the string ID of that slide, as well as the string ID of the previous and next slides
     * Contained within that slide prefab is a slide class which contains the functionality of that slide. 
     * Pressing advance just moves to the next stage of a coroutine inside said slide class that just shows elements/changes the dialogue box's contents as required.
     * 
     */

    public static MainSceneManager current;
    public bool isAdvancing = false;

    [Header("General")]
    [SerializeField] public float slideTransitionTime = 1f;

    [Header("Goober")]
    [SerializeField] public AudioClip speechSound;
    [SerializeField] private float speechTimePerCharacter = 0.05f;
    [SerializeField] private Sprite mouthShutSprite;
    [SerializeField] private Sprite mouthOpenSprite;

    [Header("Scene Objects")]
    [SerializeField] private TMP_Text dialogueTextBox;
    [SerializeField] private Image characterSprite;

    [Header("Lecture Content")]
    [SerializeField] private List<SlideRegisterInfo> presentationStructure;

    private Slide currentSlide;
    private string currentSlideID = "Introduction";

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        JumpToSlide("Introduction");
    }

    // Update is called once per frame
    void Update()
    {
        // When the advance key is just pressed call the advance function in the current slide.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentSlide.Advance();
        }
    }

    // ======== Character Speech ======== //
    // This prompts the character (thinking of calling them Goober at the request of Sky) to say whatever is passed as a parameter.
    public void Dialogue(string dialogue)
    {
        StartCoroutine(Coro_Dialogue(dialogue));
    }

    private IEnumerator Coro_Dialogue(string dialogue)
    {
        // Empty box
        dialogueTextBox.text = "";
        // Fill it in again
        foreach (char character in dialogue)
        {
            dialogueTextBox.text += character;

            // Sound
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = speechSound;
            audioSource.pitch = Random.Range(4f, 7f) / 10f;
            audioSource.volume = 0.05f;
            audioSource.Play();

            // Sprite
            if (characterSprite.sprite == mouthOpenSprite)
            {
                characterSprite.sprite = mouthShutSprite;
            } 
            else
            {
                characterSprite.sprite = mouthOpenSprite;
            }

            yield return new WaitForSeconds(speechTimePerCharacter);
        }
        characterSprite.sprite = mouthShutSprite;
    }

    // ======== Slide Switching ======== //
    public void JumpToSlide(string slideID)
    {
        StartCoroutine(Coro_JumpToSlide(slideID));
    }

    public void NextSlide()
    {
        // Find where current slide is in presentation
        int currentIndex = presentationStructure.IndexOf(GetSlideRegisterInfo(currentSlideID));

        // Increase index by 1
        if (currentIndex + 1 >= presentationStructure.Count)
        {
            Debug.Log("End of presentation, bye loser!");
            Application.Quit();
        }
        string nextSlideID = presentationStructure[currentIndex + 1].slideID;

        // Jump to slide of the next ID
        Debug.Log("Loading slide: " + nextSlideID);
        JumpToSlide(nextSlideID);
    }

    public void PreviousSlide() 
    {
        // Find where current slide is in presentation
        int currentIndex = presentationStructure.IndexOf(GetSlideRegisterInfo(currentSlideID));

        // Decrease index by 1
        if (currentIndex - 1 < 0)
        {
            Debug.Log("You can't go back dummy");
            return;
        }
        string priorSlideID = presentationStructure[currentIndex - 1].slideID;

        // Jump to slide of the prior ID
        JumpToSlide(priorSlideID);
    }

    private IEnumerator Coro_JumpToSlide(string slideID)
    {
        // Check since this may be first time (slide will be null)
        if (currentSlide != null)
        {
            yield return StartCoroutine(TweenElementPosition(currentSlide.gameObject, currentSlide.transform.position + new Vector3(0, 999f,0 ), slideTransitionTime));
            Debug.Log("Destorying " + currentSlideID);
            Destroy(currentSlide.gameObject);
        } 
        else
        {
            currentSlideID = "Introduction";
        }

        // Find what our slide is
        SlideRegisterInfo requiredSlideInfo = GetSlideRegisterInfo(slideID);

        // Instantiate new slide prefab as current slide
        currentSlide = Instantiate(requiredSlideInfo.slidePrefab, transform).GetComponent<Slide>();
        currentSlideID = slideID;
        Vector3 standardPosition = currentSlide.transform.position;
        currentSlide.transform.position += new Vector3(0, -999f, 0);

        // Tween slide onscreen
        yield return StartCoroutine(TweenElementPosition(currentSlide.gameObject, standardPosition, slideTransitionTime));
    }

    // ======== Element manipulation ======== //
    public IEnumerator TweenElementPosition(GameObject elementToMove, Vector2 destination, float t)
    {
        float progress = 0;
        Vector2 originalPosition = elementToMove.transform.position;
        while (progress < slideTransitionTime)
        {
            progress += Time.deltaTime;
            elementToMove.transform.position = Vector2.Lerp(originalPosition, destination, progress / t) ;
            yield return new WaitForEndOfFrame();
        }
    }

    // Gets the data class of the id you put in.
    private SlideRegisterInfo GetSlideRegisterInfo(string slideID)
    {
        foreach (SlideRegisterInfo slideInfo in presentationStructure)
        {
            if (slideInfo.slideID == slideID)
            {
                return slideInfo;
            }
        }

        return null;
    }

    // ======== Data class definitions: ======= //
    [System.Serializable] public class SlideRegisterInfo
    {
        public GameObject slidePrefab;
        public string slideID;
    }

}
