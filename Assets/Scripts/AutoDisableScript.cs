using UnityEngine;

public class AutoDisableScript : MonoBehaviour
{

	//Attachin this behaviour to a game object makes the GO autodisable itself.
    public float TimeOut;
    private float remainingTime;

	// Use this for initialization
	void OnEnable ()
	{
	    remainingTime = TimeOut;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (remainingTime < 0) gameObject.SetActive(false);
	    else remainingTime -= Time.deltaTime;
	}
}
