using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    RectTransform rectTransform;
    
    private GameObject player;
    public string objectName = "Batman"; 
    public PlayerMovement playerMovementScript;
    public PlayerMovement.FireState currentFireState;

    public Image imageComponent;
    public Sprite batarangSprite;
    public Sprite pistolSprite;
    public Sprite tripleSprite;

    [SerializeField]
    private Font ammoCounterFont;
    private int ammo;
    private Text ammoCounterText;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        player = GameObject.Find(objectName);
        playerMovementScript = player.GetComponent<PlayerMovement>();

        currentFireState = playerMovementScript.currentFireState;
        
        imageComponent = gameObject.AddComponent<Image>();
        
        
        // Weapon Counter
        GameObject ammoCounter = new GameObject();
        ammoCounter.name = "Weapon Counter";
        ammoCounter.transform.SetParent(rectTransform);
        ammoCounterText = ammoCounter.AddComponent<Text>();
    
        RectTransform ammoCounterRectTransform = ammoCounter.GetComponent<RectTransform>();
        ammoCounterRectTransform.anchoredPosition = new Vector2(50, 0);
        ammoCounterRectTransform.sizeDelta = new Vector2(50, 16);
        
        ammoCounterText.font = ammoCounterFont;
        ammoCounterText.alignByGeometry = true;
        ammoCounterText.color = new Color(.94f, 0.75f, 0.53f, 1f);
         ammoCounterText.alignment = TextAnchor.MiddleCenter;
        ammo = playerMovementScript.ammo;
    }

    void Start()
    {
        setSprite(currentFireState);
        setAmmoCounterText();
        
    }

    void Update()
    {
        if (currentFireState != playerMovementScript.currentFireState)
        {
            setSprite(playerMovementScript.currentFireState);
            currentFireState = playerMovementScript.currentFireState;
        }
        if(ammo != playerMovementScript.ammo)
        {
            setAmmoCounterText();
            ammo = playerMovementScript.ammo;
        }
    }

    void setSprite(PlayerMovement.FireState fireState)
    {
        switch(fireState)
        {
            case PlayerMovement.FireState.BATARANG:
                imageComponent.sprite = batarangSprite;
                break;
            case PlayerMovement.FireState.PISTOL:
                imageComponent.sprite = pistolSprite;
                break;
            case PlayerMovement.FireState.TRIPLE:
                imageComponent.sprite = tripleSprite;
                break;
            default:
                imageComponent.sprite = batarangSprite;
                break;
        }
    }

    void setAmmoCounterText()
    {
        ammoCounterText.text =  string.Format("{0}", playerMovementScript.ammo);
    }
}
