using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HealthBarUI
{
    public class PlayerHealth : MonoBehaviour
    {

        public float health = 0f;
        private float lerpTimer;

        public float maxHealth  = 100f;
        public float chipSpeed = 2f;
        [Header("Image")]
        public Image frontHealthBar;
        public Image backHealthBar;
        [Header("Text")]
        public TextMeshProUGUI healthText;
        



        // Use this for initialization
        void Start()
        {
            health = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            health = Mathf.Clamp(health, 0, maxHealth);
            updateHealth();

            
            //change Code Here
            if(Input.GetKeyDown(KeyCode.L))
            {
                takeDamage(Random.Range(5, 10));
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                restoreHealth(Random.Range(5, 10));
            }
            
        }

        public void updateHealth()
        {
            //Debug.Log(health);
            float fillfront = frontHealthBar.fillAmount;
            float fillBack = backHealthBar.fillAmount;
            float hfraction = health / maxHealth;

            if(fillBack > hfraction)
            {
                frontHealthBar.fillAmount = hfraction;
                backHealthBar.color = Color.red;
                lerpTimer += Time.deltaTime;

                float percentComplete = lerpTimer / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                backHealthBar.fillAmount = Mathf.Lerp(fillBack, hfraction, percentComplete);
            }

            if (fillfront < hfraction)
            {
                backHealthBar.color = Color.green;
                backHealthBar.fillAmount = hfraction;
                lerpTimer += Time.deltaTime;

                float percentComplete = lerpTimer / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                frontHealthBar.fillAmount = Mathf.Lerp(fillfront, backHealthBar.fillAmount, percentComplete);
            }
            healthText.text = Mathf.Round(health) + "/" + Mathf.Round(maxHealth);
        }

        public void takeDamage(float damage)
        {
            health -= damage;
            lerpTimer = 0f;
        }

        public void restoreHealth(float healAmt)
        {
            health += healAmt;
            lerpTimer = 0f;
        }

        public void increaseHealth(int level)
        {
            maxHealth += (health * 0.0f) * ((100 - level) * 0.1f);
            health = maxHealth;
        }
        public void restoreToMaxHealth()
        {
            health = maxHealth;
        }
    }
}