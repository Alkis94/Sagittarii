using UnityEngine;
using UnityEngine.UI;

public class TimeCountDown : MonoBehaviour
{

    private float TimePassed;
    private string MinutesPassed = "3";
    private string SecondsPassed = "0";
    private Text TimeText;


    // Use this for initialization
    void Start ()
    {
        TimeText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        TimeCount();

    }

    private void TimeCount()
    {
        TimePassed += Time.deltaTime;
        MinutesPassed = Mathf.Floor(3 - TimePassed / 60).ToString("00");
        SecondsPassed = Mathf.Floor(60 - TimePassed % 60).ToString("00");
        TimeText.text = "Time : " + MinutesPassed + ":" + SecondsPassed;
    }
}
