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
        PlayerStats.OnPlayerGoldChanged += UpdateGold;
        PlayerFireProjectile.OnPlayerFiredProjectile += UpdateAmmo;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerHealthChanged -= UpdateHealth;
        PlayerStats.OnPlayerGoldChanged -= UpdateGold;
        PlayerFireProjectile.OnPlayerFiredProjectile -= UpdateAmmo;
    }

    private void Start ()
    {
        AudioListener.pause = false;
    }


    private void UpdateHealth(int health, int maxHealth)
    {
        UpdateHealthText(health,maxHealth);
        UpdateHealthBar(health, maxHealth);
    }

    private void UpdateHealthText(int health,int maxHealth)
    {
        
        health = 0 > health ? 0 : health;
        health = maxHealth < health ? maxHealth : health;
        healthText.text = health + "/" + maxHealth;
    }

    private void UpdateHealthBar(int health, int maxHealth)
    {
        imageFillAmountNew = (float)health / maxHealth;
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

    private void UpdateAmmo(int ammo)
    {
        ammoText.text = ammo.ToString();
    }

    private void UpdateGold(int gold)
    {
        goldText.text = gold.ToString();
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
