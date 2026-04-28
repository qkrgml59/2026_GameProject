using UnityEngine;
using TMPro;
using Unity.UI;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public string charcterName;
    public int maxHealth = 100;
    public int currentHealth;

    //UI 요소
    public Slider healthBar;
    public TextMeshProUGUI healthText;

    public int maxMana = 10;
    public int currentMana;
    public Slider manaBar;
    public TextMeshProUGUI manaText;

    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        UpdateUI();
    }

    public void TakeDamage(int dmaamge)
    {
        currentHealth -= dmaamge; 
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
    }

    public void UseMana(int amount)
    {
        currentMana -= amount;
        if (currentMana <0)
        {
            currentMana = 0;
        }
        UpdateUI();
    }

    public void GainMana(int amount)
    {
        currentMana += amount;

        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }

        if (manaBar != null)
        {
            manaBar.value = (float)currentMana / maxMana;
        }

        if (manaBar != null)
        {
            manaText.text = $"{currentMana} / {maxMana}";
        }
    }
}
