using UnityEngine;
using TMPro;
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
    private Image healthImage;
    [SerializeField]
    private Image energyImage;
    [SerializeField]
    private Image specialImage;

    private Coroutine energyCoroutine = null;
    private Coroutine healthCoroutine = null;
    private Coroutine bossHealthCoroutine = null;

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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Town")
        {
            gameObject.SetActive(true);
        }
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

    public void CallDeathUI()
    {
        StartCoroutine(ActivateDeathUI());
    }

    IEnumerator ActivateDeathUI()
    {
        float delay = Time.time + 2f;
        float alpha = 0;
        blackDeathImage.enabled = true;

        while(delay > Time.time)
        {
            yield return new WaitForSeconds(0.025f);
            alpha += 0.015f;
            blackDeathImage.color = new Color(0, 0, 0, alpha);
        }

        GameManager.GameState = GameStateEnum.paused;
        defeatMenu.SetActive(true);
    }

}
