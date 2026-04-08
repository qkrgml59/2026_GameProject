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

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmaamge)
    {
        currentHealth -= dmaamge; 
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
    }
}
