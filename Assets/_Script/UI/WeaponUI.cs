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

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        player = GameObject.Find(objectName);
        playerMovementScript = player.GetComponent<PlayerMovement>();

        currentFireState = playerMovementScript.currentFireState;
        
        imageComponent = gameObject.AddComponent<Image>();

    }

    void Start()
    {
        setSprite(currentFireState);
    }

    void Update()
    {
        if (currentFireState != playerMovementScript.currentFireState)
        {
            setSprite(playerMovementScript.currentFireState);
            currentFireState = playerMovementScript.currentFireState;
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
}
