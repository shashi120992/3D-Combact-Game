using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public bool isPlayer;
    public float health = 100.0f;
    public float maxHealth = 100f;
    public bool Dead { get; private set; }
    //public TextMeshProUGUI healthText;

    private new AnimationManager animation;
    private SlowMotion slowMotion;

    private MovementManager movement;
    private UIManager uiManager;

    private void Awake()
    {
        animation = GetComponentInChildren<AnimationManager>();
        slowMotion = GetComponent<SlowMotion>();

        Dead = false;

        if (isPlayer)
        {
            movement = GetComponent<MovementManager>();
            uiManager = GetComponent<UIManager>();
        }
    }
    /*
    private void Update()
    {
        UpdateHealthUI();
    }
    */

    public void ApplyDamage(float damage, bool knockDown)
    {
        if (Dead) return;
        int type = 0;

        health = Mathf.Max(health - damage, 0.0f);
        //healthText.text = Mathf.Round(health) + "/" + Mathf.Round(maxHealth);

        if (isPlayer)
        {
            uiManager.UpdateHealthBar(health);
            type = 1;
        }

        if (health == 0.0f)
        {
            OnCharacterDeath(isPlayer);
            ToggleEnemyManager(false);
            animation.Death();
        }
        else if (knockDown && Random.Range(0, 2) < 1)
        {
            animation.KnockDown();

            if (isPlayer)
            {
                movement.KnockDown = true;
                ToggleEnemyManager(false);
            }
        }
        else
        {
            animation.Hit(type);
        }
        
    }
    /*
    public void UpdateHealthUI()
    {
        healthText.text = Mathf.Round(health) + "/" + Mathf.Round(maxHealth);
    }
    */

    private void OnCharacterDeath(bool ko = false)
    {
        slowMotion.Play(ko);
        Dead = true;
    }

    public void ToggleEnemyManager(bool enable)
    {
        EnemyManager enemy = GameObject
            .FindWithTag(ObjectTag.ENEMY)
            .GetComponent<EnemyManager>();

        if (!isPlayer) enemy.Destroy();
        enemy.enabled = enable;
    }
}
