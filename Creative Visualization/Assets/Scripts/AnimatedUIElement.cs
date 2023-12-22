using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedUIElement : MonoBehaviour
{
    [SerializeField] private float degreeSway = 0f;
    [SerializeField] private float swayTime = 2f;
    [SerializeField] private Vector2 bobAmount = Vector2.zero;
    [SerializeField] private float bobTime = 2f;

    private float c_swayTime = 0f;
    private float c_bobTime = 0f;

    // Animated ui elements you wish to move through other means should be parented to something else!

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update timers and transforms
        if (swayTime != 0)
        {
            c_swayTime = (c_swayTime + Time.deltaTime) % swayTime;
            transform.localRotation = Quaternion.Euler(0, 0, degreeSway * Mathf.Sin((c_swayTime / swayTime) * 2f * Mathf.PI));
        }

        if (bobTime != 0)
        {
            c_bobTime = (c_bobTime + Time.deltaTime) % bobTime;
            transform.localPosition = bobAmount * Mathf.Sin((c_bobTime / bobTime) * 2f * Mathf.PI);
        }
    }
}
