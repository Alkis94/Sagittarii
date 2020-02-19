using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;


public class UIManager : MonoBehaviour
{

    private static UIManager instance = null;


    public TextMeshProUGUI energyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI goldText;
    public Image healthImage;
    public Image energyImage;

    


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
        PlayerStats.OnPlayerEnergyChanged += UpdateEnergy;
        PlayerStats.OnPlayerGoldChanged += UpdateGold;
        PlayerStats.OnPlayerAmmoChanged += UpdateAmmo;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerHealthChanged -= UpdateHealth;
        PlayerStats.OnPlayerEnergyChanged -= UpdateEnergy;
        PlayerStats.OnPlayerGoldChanged -= UpdateGold;
        PlayerStats.OnPlayerAmmoChanged -= UpdateAmmo;
    }

    private void Start ()
    {
        AudioListener.pause = false;
    }


    private void UpdateHealth(int health, int maxHealth)
    {
        UpdateText(health,maxHealth,healthText);
        UpdateBar(health, maxHealth,healthImage);
    }

    private void UpdateEnergy(int exhaustion, int maxExhastion)
    {
        UpdateText(exhaustion, maxExhastion, energyText);
        UpdateBar(exhaustion, maxExhastion, energyImage);
    }

    private void UpdateText(int current,int max, TextMeshProUGUI text)
    {
        current = 0 > current ? 0 : current;
        current = max < current ? max : current;
        text.text = current + "/" + max;
    }

    private void UpdateBar(int current, int max, Image image)
    {
        float imageFillAmount = (float)current / max;
        if(imageFillAmount > image.fillAmount)
        {
            StartCoroutine(FillBar(imageFillAmount,image));
        }
        else
        {
            StartCoroutine(DepleteBar(imageFillAmount,image));
        }
    }

    IEnumerator FillBar(float imageFillAmount,Image image)
    {
        float fillAmountDifference = imageFillAmount - image.fillAmount;
        while(imageFillAmount > image.fillAmount)
        {
            image.fillAmount += 0.05f * fillAmountDifference;
            yield return null;
        }
    }

    IEnumerator DepleteBar(float imageFillAmount, Image image)
    {
        float fillAmountDifference = image.fillAmount - imageFillAmount;
        while (imageFillAmount < image.fillAmount)
        {
            image.fillAmount -= 0.05f * fillAmountDifference;
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
