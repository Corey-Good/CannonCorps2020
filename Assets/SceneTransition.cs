using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public RectTransform panel;
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.alpha(panel, 0, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
