using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //   # # # # # # # # # # # # 
    //   #                     #
    //   #  SINGLETON CLASS    #
    //   #                     #
    //   # # # # # # # # # # # # 

    public static UIManager Instance = null;

    private int Score;


    public Text HealthText;
    public Text ScoreText;
    public Text MuteText;
   // public Text TimeText;

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }

        // Here we save our singleton instance
        Instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        
        AudioListener.pause = false;
        Score = 0;
        HealthText.text =  C.PLAYER_MAXIMUM_HEALTH + "/" + C.PLAYER_MAXIMUM_HEALTH;

    }
	
	// Update is called once per frame
	void Update ()
    {
       
    }

    public void UpdateHealth(int health, int maximumHealth)
    {
        health = 0 > health ? 0 : health;
        HealthText.text =  health.ToString() + "/" + maximumHealth.ToString();
    }


    public void UpdateScore()
    {
        Score += 10;
        ScoreText.text = "Score: " + Score.ToString();
    }

    public void PressMute()
    {
        if (AudioListener.pause)
        {
            AudioListener.pause = false;
            MuteText.text = "Mute : Off";
        }
        else
        {
            AudioListener.pause = true;
            MuteText.text = "Mute : On";
        }
    }
}
