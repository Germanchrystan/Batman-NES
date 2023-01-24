using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthUI : MonoBehaviour {
    public int health;

    RectTransform rectTransform;
    private GameObject player;
    public string objectName = "Batman"; 
    public PlayerHealth playerHealthScript;

    public GameObject[] healthBars;
    public Sprite fullHealthBar;
    public Sprite emptyHealthBar;

    private int totalHealth, currentHealth;

    [SerializeField]
    private int barsGap = 16;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        player = GameObject.Find(objectName);
        playerHealthScript = player.GetComponent<PlayerHealth>();
        totalHealth = playerHealthScript.totalHealth;
        currentHealth = playerHealthScript.currentHealth;  

        Array.Resize(ref healthBars, totalHealth);
    }

    void Start()
    {
        for (int i = 0; i < totalHealth; i++) 
        {
            GameObject newHealthBar = new GameObject();
            newHealthBar.name = string.Format("Health Bar {0}", i+1);
            newHealthBar.transform.SetParent(rectTransform);

            Image NewImage = newHealthBar.AddComponent<Image>();
            NewImage.sprite = fullHealthBar;
            
            RectTransform imageRectTransform = newHealthBar.GetComponent<RectTransform>();
            imageRectTransform.sizeDelta = new Vector2(13, 20);
            imageRectTransform.anchoredPosition = new Vector2((barsGap * i), 0);

            newHealthBar.SetActive(true);
            healthBars[i] = newHealthBar;
            
        }
    }

    void Update()
    {
        if (currentHealth != playerHealthScript.currentHealth)
        {
            updateHealthBars(playerHealthScript.currentHealth);
            currentHealth = playerHealthScript.currentHealth;
        }
    }

    void updateHealthBars(int currentHealth)
    {
        for(int i = 0; i < healthBars.Length; i++)
        {
            Image healthBarImage = healthBars[i].GetComponent<Image>();
            if(i < currentHealth)
            {
                healthBarImage.sprite = fullHealthBar;
            }
            else
            {
                healthBarImage.sprite = emptyHealthBar;
            }
        }
    }
}