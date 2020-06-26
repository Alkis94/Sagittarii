using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;


public class UIManager : MonoBehaviour
{

    public static UIManager Instance { get; private set; } = null;

    [SerializeField]
    private TextMeshProUGUI energyText;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI ammoText;
    [SerializeField]
    private TextMeshProUGUI goldText;
    [SerializeField]
    private Image healthImage;
    [SerializeField]
    private Image energyImage;

    private Coroutine energyCoroutine = null;
    private Coroutine healthCoroutine = null;
    private Coroutine bossHealthCoroutine = null;

    [SerializeField]
    private Image bossHealthImage;
    [SerializeField]
    private GameObject bossHealthBar;

    private int bossMaxHealth;
    private int bossCurrentHealth;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start ()
    {
        AudioListener.pause = false;
    }

    public void UpdateHealth(int health, int maxHealth)
    {
        UpdateText(health,maxHealth,healthText);
        UpdateBar(health, maxHealth,healthImage,ref healthCoroutine);
    }

    public void UpdateEnergy(int exhaustion, int maxExhastion)
    {
        UpdateText(exhaustion, maxExhastion, energyText);
        UpdateBar(exhaustion, maxExhastion, energyImage,ref energyCoroutine);
    }

    public void UpdateAmmo(int ammo)
    {
        ammoText.text = ammo.ToString();
    }

    public void UpdateGold(int gold)
    {
        goldText.text = gold.ToString();
    }

    private void UpdateText(int current,int max, TextMeshProUGUI text)
    {
        current = 0 > current ? 0 : current;
        current = max < current ? max : current;
        text.text = current + "/" + max;
    }

    private void UpdateBar(int current, int max, Image image,ref Coroutine coroutine)
    {
        float imageFillAmount;
        if (current <= 0)
        {
            imageFillAmount = 0;
        }
        else if (current >= max)
        {
            imageFillAmount = 1;
        }
        else
        {
            imageFillAmount = (float)current / max;
        }

        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        
        if(imageFillAmount > image.fillAmount)
        {
            coroutine = StartCoroutine(FillBar(imageFillAmount,image));
        }
        else
        {
            coroutine = StartCoroutine(DepleteBar(imageFillAmount,image));
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

    public void EnableBossHealth(int health)
    {
        bossCurrentHealth += health;
        bossMaxHealth += health;
        bossHealthBar.SetActive(true);
    }

    public void UpdateBossHealth(int damage)
    {
        bossCurrentHealth -=  damage;
        UpdateBar(bossCurrentHealth, bossMaxHealth, bossHealthImage,ref bossHealthCoroutine);
        if(bossCurrentHealth <= 0)
        {
            bossCurrentHealth = 0;
            bossMaxHealth = 0;
            StartCoroutine(OnBossDeathDeactiveBar());
        }
    }

    IEnumerator OnBossDeathDeactiveBar()
    {
        yield return new WaitForSeconds(5f);
        bossHealthBar.SetActive(false);
        bossHealthImage.fillAmount = 1f;
    }

}
