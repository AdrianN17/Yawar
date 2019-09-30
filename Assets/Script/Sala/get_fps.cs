using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class get_fps : MonoBehaviour
{
    public Text fps_text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fps_text.text = "FPS : " + (int)(1.0f / Time.smoothDeltaTime);
    }
}
