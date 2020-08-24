using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private int maxHealth;

    private int currentHealth;
    [SerializeField]
    private Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(int amount)
    {
        currentHealth = maxHealth = amount;
        OnHealthChange();
    }

    void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChange();
    }

    private void OnHealthChange()
    {
        healthBar.value = (float)currentHealth / (float)maxHealth;
    }

    public IEnumerator TakeDamageDelayed(int amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        if (currentHealth == 0)
        {
            SetHealth(20);
        }
        OnHealthChange();
    }
}
