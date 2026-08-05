using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    private TextMeshProUGUI goldGainedText;
    [SerializeField]
    private TextMeshProUGUI relicNameText;
    [SerializeField]
    private TextMeshProUGUI relicDescriptionText;
    [SerializeField]
    private GameObject relicInfoMenu;
    [SerializeField]
    private Image healthImage;
    [SerializeField]
    private Image energyImage;
    [SerializeField]
    private Image specialImage;
    [SerializeField]
    private TextMeshProUGUI locationText;
    [SerializeField]
    private CanvasGroup locationGroup;

    [SerializeField]
    private Image bossHealthImage;
    [SerializeField]
    private GameObject bossHealthBar;
    [SerializeField]
    private GameObject defeatMenu;
    [SerializeField]
    private Image blackDeathImage;

    private int bossMaxHealth;
    private int bossCurrentHealth;
    private int goldGained;

    private Coroutine relicCoroutine = null;
    private Coroutine energyCoroutine = null;
    private Coroutine healthCoroutine = null;
    private Coroutine bossHealthCoroutine = null;
    private Coroutine goldGainedCoroutine = null;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    public void SetHealth(int health, int maxHealth)
    {
        UpdateText(health, maxHealth, healthText);
        if (health <= 0)
        {
            healthImage.fillAmount = 0;
        }
        else if (health >= maxHealth)
        {
            healthImage.fillAmount = 1;
        }
        else
        {
            healthImage.fillAmount = (float)health / maxHealth;
        }
    }

    public void UpdateAmmo(int ammo)
    {
        ammoText.text = ammo.ToString();
    }

    public void UpdateGold(int gold)
    {
        goldText.text = gold.ToString();
    }

    public int GoldGained
    {
        get
        {
            return goldGained;
        }

        set
        {
            if (goldGainedCoroutine != null)
            {
                StopCoroutine(goldGainedCoroutine);
            }

            goldGained = value;
            goldGainedText.enabled = true;

            if (goldGained >= 0)
            {
                goldGainedText.text = "+" + GoldGained;
            }
            else
            {
                goldGainedText.text = "" + GoldGained;
            }

            goldGainedCoroutine = StartCoroutine(DisableGoldGained());
        }
    }

    private IEnumerator DisableGoldGained()
    {
        yield return new WaitForSeconds(3f);
        Int32.TryParse(goldText.text, out int gold);
        gold += GoldGained;
        goldText.text = gold.ToString();
        goldGainedText.enabled = false;
        goldGained = 0;
    }

    public void UpdateSpecial(float cooldown)
    {
        EmptyBar(specialImage);
        StartCoroutine(FillBarWithTime(cooldown, specialImage));
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
        var fillAmountDifference = imageFillAmount - image.fillAmount;
        while(imageFillAmount > image.fillAmount)
        {
            image.fillAmount += 0.05f * fillAmountDifference;
            yield return null;
        }
    }

    IEnumerator DepleteBar(float imageFillAmount, Image image)
    {
        var fillAmountDifference = image.fillAmount - imageFillAmount;
        while (imageFillAmount < image.fillAmount)
        {
            image.fillAmount -= 0.05f * fillAmountDifference;
            yield return null;
        }
    }

    IEnumerator FillBarWithTime(float coolDown, Image image)
    {
        while (image.fillAmount < 1)
        {
            image.fillAmount += Time.deltaTime / coolDown;
            yield return null;
        }
    }

    private void EmptyBar( Image image)
    {
        image.fillAmount = 0;
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
        UpdateBar(bossCurrentHealth, bossMaxHealth, bossHealthImage, ref bossHealthCoroutine);
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

    private void DeactiveBossHealthBar()
    {
        bossHealthBar.SetActive(false);
        bossHealthImage.fillAmount = 1f;
    }

    public void ShowItemText(string relicName, string relicDescription, RelicRarity relicRarity)
    {
        relicNameText.color = relicRarity switch
        {
            RelicRarity.Common => Color.green,
            RelicRarity.Rare => Color.blue,
            RelicRarity.Epic => Color.magenta,
            _ => Color.green,
        };

        // relicInfoMenu.transform.localPosition = new Vector3(450, relicInfoMenu.transform.localPosition.y, 0);

        relicNameText.text = relicName;
        relicDescriptionText.text = relicDescription;

        if(relicCoroutine != null)
        {
            StopCoroutine(relicCoroutine);
        }

        relicCoroutine = StartCoroutine(MoveItemTexts());
    }

    private IEnumerator MoveItemTexts()
    {
        LeanTween.moveLocalX(relicInfoMenu, 230, 0.5f).setEaseInOutCubic();
        yield return new WaitForSeconds(5f);
        LeanTween.moveLocalX(relicInfoMenu, 450, 0.5f).setEaseInOutBack();
    }

    public void ShowDeathUI()
    {
        StartCoroutine(ActivateDeathUI());
    }

    private IEnumerator ActivateDeathUI()
    {
        blackDeathImage.enabled = true;
        var delay = Time.time + 2f;
        var alpha = 0f;
        while (delay > Time.time)
        {
            yield return new WaitForSeconds(0.025f);
            alpha += 0.015f;
            blackDeathImage.color = new Color(0, 0, 0, alpha);
        }

        GameManager.GameState = GameStateEnum.Paused;
        defeatMenu.SetActive(true);
    }

    public void ShowLocation(string location)
    {
        StartCoroutine(ShowLocationCoroutine(location));
    }

    private IEnumerator ShowLocationCoroutine(string location)
    {
        yield return new WaitForSeconds(0.5f);
        locationText.text = location;
        LeanTween.alphaCanvas(locationGroup, 1f, 1.5f).setEase(LeanTweenType.easeInCirc);
        yield return new WaitForSeconds(3f);
        LeanTween.alphaCanvas(locationGroup, 0f, 0.25f).setEase(LeanTweenType.linear);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Town")
        {
            DeactiveBossHealthBar();
        }
    }
}
