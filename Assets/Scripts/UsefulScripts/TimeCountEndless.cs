using UnityEngine;
using UnityEngine.UI;

public class TimeCountEndless: MonoBehaviour
{

    private float timePassed;
    private string minutesPassed = "3";
    private string secondsPassed = "0";
    private Text timeText;


    // Use this for initialization
    void Start()
    {
        timeText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeCount();

    }

    private void TimeCount()
    {
        timePassed += Time.deltaTime;
        minutesPassed = Mathf.Floor(timePassed / 60).ToString("00");
        secondsPassed = Mathf.Floor(timePassed % 60).ToString("00");
        timeText.text = "Time : " + minutesPassed + ":" + secondsPassed;
    }
}
