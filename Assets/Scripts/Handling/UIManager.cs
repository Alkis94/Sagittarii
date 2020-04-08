using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;


public class UIManager : MonoBehaviour
{

    private static UIManager instance = null;

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
    [SerializeField]


    private Image bossHealthImage;
    [SerializeField]
    private GameObject bossHealthBar;

    private int bossMaxHealth;
    private int bossCurrentHealth;


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

        BossHealth.BossEngaged += OnBossEnganged;
        BossHealth.BossDamaged += OnBossDamaged;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerHealthChanged -= UpdateHealth;
        PlayerStats.OnPlayerEnergyChanged -= UpdateEnergy;
        PlayerStats.OnPlayerGoldChanged -= UpdateGold;
        PlayerStats.OnPlayerAmmoChanged -= UpdateAmmo;

        BossHealth.BossEngaged -= OnBossEnganged;
        BossHealth.BossDamaged -= OnBossDamaged;
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

    private void OnBossEnganged(int health)
    {
        bossCurrentHealth += health;
        bossMaxHealth += health;
        bossHealthBar.SetActive(true);
    }

    private void OnBossDamaged(int damage)
    {
        bossCurrentHealth -=  damage;
        UpdateBar(bossCurrentHealth, bossMaxHealth, bossHealthImage);
        if(bossCurrentHealth <= 0)
        {
            bossCurrentHealth = 0;
            bossMaxHealth = 0;
            StartCoroutine(OnBossDeathDeactiveBar());
        }
    }

    IEnumerator OnBossDeathDeactiveBar()
    {
        yield return new WaitForSeconds(2f);
        bossHealthBar.SetActive(false);
        bossHealthImage.fillAmount = 1f;
    }

}
