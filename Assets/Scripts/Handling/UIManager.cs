using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;


public class UIManager : MonoBehaviour
{

    private static UIManager instance = null;



    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI goldText;
    public Image healthImage;

    private float imageFillAmountNew;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    

    private void OnEnable()
    {
        PlayerStats.OnPlayerHealthChanged += UpdateHealth;
        PlayerFireProjectile.OnPlayerFiredProjectile += UpdateAmmo;
        EnemyGotShot.OnDeathNotify += UpdateGold;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerHealthChanged -= UpdateHealth;
        PlayerFireProjectile.OnPlayerFiredProjectile -= UpdateAmmo;
        EnemyGotShot.OnDeathNotify -= UpdateGold;
    }

    private void Start ()
    {
        AudioListener.pause = false;
        healthText.text =  PlayerStats.CurrentHealth + "/" + PlayerStats.MaximumHealth;
        ammoText.text = PlayerStats.ammo.ToString();
        goldText.text = PlayerStats.gold.ToString();
    }


    private void UpdateHealth()
    {
        UpdateHealthText();
        UpdateHealthBar();
    }

    private void UpdateHealthText()
    {
        int health;
        health = 0 > PlayerStats.CurrentHealth ? 0 : PlayerStats.CurrentHealth;
        health = PlayerStats.MaximumHealth < health ? PlayerStats.MaximumHealth : health;
        healthText.text = health + "/" + PlayerStats.MaximumHealth;
    }

    private void UpdateHealthBar()
    {
        imageFillAmountNew = (float)PlayerStats.CurrentHealth / PlayerStats.MaximumHealth;
        if(imageFillAmountNew > healthImage.fillAmount)
        {
            StartCoroutine(FillHealthBar());
        }
        else
        {
            StartCoroutine(DepleteHealthBar());
        }
    }

    IEnumerator FillHealthBar()
    {
        float fillAmountDifference = imageFillAmountNew - healthImage.fillAmount;
        while(imageFillAmountNew > healthImage.fillAmount)
        {
            healthImage.fillAmount += 0.05f * fillAmountDifference;
            yield return null;
        }
    }

    IEnumerator DepleteHealthBar()
    {
        float fillAmountDifference = healthImage.fillAmount - imageFillAmountNew;
        while (imageFillAmountNew < healthImage.fillAmount)
        {
            healthImage.fillAmount -= 0.05f * fillAmountDifference;
            yield return null;
        }
    }

    private void UpdateAmmo()
    {
        ammoText.text = PlayerStats.ammo.ToString();
    }

    private void UpdateGold(Vector3 enemyPosition)
    {
        goldText.text = PlayerStats.gold.ToString();
    }

    //public void PressMute()
    //{
    //    if (AudioListener.pause)
    //    {
    //        AudioListener.pause = false;
    //        MuteText.text = "Mute : Off";
    //    }
    //    else
    //    {
    //        AudioListener.pause = true;
    //        MuteText.text = "Mute : On";
    //    }
    //}
}
