using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameManager gameManager;
    public DialogueManager dialogueManager;
    public GameObject menuPanel;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onMenuBtnClick()
    {
        dialogueManager.openBlurImg();
        menuPanel.gameObject.SetActive(true);
    }

    public void onBackToGameBtnClick()
    {
        dialogueManager.closeAlert();
        menuPanel.gameObject.SetActive(false);
    }
}
