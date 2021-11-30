using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDrawer : MonoBehaviour
{
    public GameObject TextFieldObject;
    private Text text;
    public void Draw(int lives)
    {
        text.text = lives.ToString();
    }

    // Use this for initialization
    void OnEnable()
    {
        text = TextFieldObject.GetComponent<Text>();
    }
}
