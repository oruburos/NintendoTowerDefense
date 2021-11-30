using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveDrawer : MonoBehaviour
{
    public GameObject TextFieldCurrentWave;
    public GameObject TextFieldMaxWave;

    private Text textCurrentWave;
    private Text textMaxWave;

    public void DrawCurrentWave(int wave)
    {
        textCurrentWave.text = wave.ToString();
    }

    public void DrawMaxWave(int maxWave)
    {
        textMaxWave.text = maxWave.ToString();
    }

    // Use this for initialization
    void OnEnable()
    {
        textCurrentWave = TextFieldCurrentWave.GetComponent<Text>();
        textMaxWave = TextFieldMaxWave.GetComponent<Text>();

    }
}
