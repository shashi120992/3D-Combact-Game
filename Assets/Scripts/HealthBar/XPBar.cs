using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace HealthBarUI
{
    public class XPBar : MonoBehaviour
    {
        //[HideInInspector] public PlayerHealth playerHealth;
        public int level;
        public float currentXP;
        public float reqXP;

        private float LerpTimer;
        private float delayedTimer;

        [Header("UI")]
        public Image fronXPBar;
        public Image backXPBar;

        [Header("Multifliers")]
        [Range(1f, 300f)] public float aditionMultifliyer = 300;
        [Range(2f, 4f)] public float powerMultipliyer = 2;
        [Range(7f, 14f)] public float divitionMultipliyer = 7;

        [Header("Text")]
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI XPText;
        public TextMeshProUGUI killCountText;

        public int killcount = 0;


        // Use this for initialization
        void Start()
        {
            fronXPBar.fillAmount = currentXP / reqXP;
            backXPBar.fillAmount = currentXP / reqXP;
            reqXP = calculateReqXP();
            levelText.text = "Level " + level;
        }

        // Update is called once per frame
        void Update()
        {
            updateXPUI();
            
            
            if(Input.GetKeyDown(KeyCode.LeftControl))
                gainExp(20);
            
            if (currentXP > reqXP)
                levelUp();
            
        }

        public void updateXPUI()
        {
            float xpFraction = currentXP / reqXP;
            float fxp = fronXPBar.fillAmount;
            if(fxp < xpFraction)
            {
                delayedTimer += Time.deltaTime;
                backXPBar.fillAmount = xpFraction;
                if(delayedTimer > 3)
                {
                    LerpTimer += Time.deltaTime;
                    float percentComplete = LerpTimer / 4;
                    fronXPBar.fillAmount = Mathf.Lerp(fxp, backXPBar.fillAmount, percentComplete);
                }
            }
            XPText.text = currentXP + "/" + reqXP;
        }

        public void gainExp(float xpGain)
        {
            currentXP += xpGain;
            LerpTimer = 0f;
        }

        public void gainExpScalable(float xpGained, int passedLevel)
        {
            if(passedLevel < level)
            {
                float multiplayer = 1 + (level - passedLevel) * 0.1f;
                currentXP += xpGained * multiplayer;
            }
            else
            {
                currentXP += xpGained;
            }
            LerpTimer = 0f;
            delayedTimer = 0f;
        }

        public void levelUp()
        {
            level++;
            fronXPBar.fillAmount = 0f;
            backXPBar.fillAmount = 0f;
            currentXP = Mathf.RoundToInt(currentXP - reqXP);
            GetComponent<PlayerHealth>().increaseHealth(level);
            reqXP = calculateReqXP();
            GetComponent<PlayerHealth>().restoreToMaxHealth();
            levelText.text = "Level " + level;
        }

        private int calculateReqXP()
        {
            int solveForReqXP = 0;

            for (int levelcycle = 1; levelcycle < level; levelcycle++)
            {
                solveForReqXP += (int) Mathf.Floor(levelcycle + aditionMultifliyer * Mathf.Pow(powerMultipliyer, levelcycle / divitionMultipliyer));
            }
            return solveForReqXP / 4;
        }

        public void killcountUI()
        {
            killcount++;
            gainExp(50);
            killCountText.text = "Kills : " + killcount;
        }

    
    }
}   