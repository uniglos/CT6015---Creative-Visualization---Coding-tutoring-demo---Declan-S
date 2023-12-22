using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAnimator : MonoBehaviour
{
    [SerializeField] GameObject bg1;
    [SerializeField] GameObject bg2;
    [SerializeField] GameObject bg3;

    [SerializeField] float bg2Speed = 300f;
    [SerializeField] float bg3Speed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bg2.transform.position = new Vector3((bg2.transform.position.x + Time.deltaTime * bg2Speed) % Screen.width, (bg2.transform.position.y + Time.deltaTime * -bg2Speed * 0.66f) % Screen.height, 0);
        bg3.transform.position = new Vector3(bg3.transform.position.x, (bg3.transform.position.y + Time.deltaTime * -bg3Speed) % Screen.height, 0);
    }
}
