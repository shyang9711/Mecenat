using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class ProtegeManager : MonoBehaviour
{

    //Manager Scripts
    public GameManager gameManager;
    public DialogueManager dialogueManager;
    public ClientManager clientManager;

    public Client selectedClient;
    
    public GameObject UIClientSet;
    public GameObject MarketBtn;
    public GameObject ProtegeBtn;
    public GameObject ScoutBtn;
    public bool isProtege;

    public GameObject SendBtn;
    public Text SendBtnTxt;

    public GameObject UIInfoSet;
    public GameObject UITransferSet;

    public Text CreatedWorksCountInfo;

    [SerializeField] private InputField offerValue;
    [SerializeField] private InputField offerWage;

    //Alert UI
    public GameObject UIAlertSet;
    public Text alertName;
    public Text alertDesc;
    private void Awake()
    {
        dialogueManager.closeAlert();

        MarketBtn.SetActive(true);
        ProtegeBtn.SetActive(false);

        SendBtn.SetActive(false);

        checkWorks();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        clientManager.receivePData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onMarketBtnClick()
    {
        isProtege = false;
        clientManager.onCloseButtonClick();
        clientManager.receiveCData();

        MarketBtn.SetActive(false);
        ProtegeBtn.SetActive(true);
    }
    public void onProtegeBtnClick()
    {
        isProtege = true;
        clientManager.onCloseButtonClick();
        clientManager.receivePData();

        MarketBtn.SetActive(true);
        ProtegeBtn.SetActive(false);
    }
    
    public void onIndividualBtnClick(Client client)
    {
        selectedClient = client;
        clientManager.onCloseButtonClick();

        if (selectedClient != null)
        {
            if (!isProtege)
            {
                // Update CreatedWorksCountInfo
                CreatedWorksCountInfo.text = selectedClient.ctdWork.Count > 0 ? selectedClient.ctdWork.Count.ToString() : "0";
                SendBtn.SetActive(true);
                if (selectedClient.offers.Contains(gameManager.playerId)) {
                    SendBtnTxt.text = "Resend Offer";
                } else {
                    SendBtnTxt.text = "Offer";
                }
            }
            else
            {
                SendBtn.SetActive(false);
            }
            infoSetMove();
        }
    }

    public void infoSetMove()
    {
        dialogueManager.openBlurImg();
        UIInfoSet.SetActive(true);
    }

    public void onScoutBtnClick()
    {
        UIInfoSet.SetActive(false);
        UITransferSet.SetActive(true);
    }

    public void onExitBtnClick()
    {
        dialogueManager.closeAlert();
    }

    public void onSendBtnClick()
    {
        dialogueManager.closeAlert();
        
        if (!string.IsNullOrEmpty(offerValue.text) && !string.IsNullOrEmpty(offerWage.text))
        {
            int.TryParse(offerValue.text, out int value);
            int.TryParse(offerWage.text, out int wage);
            string name = selectedClient.name;
            offerValue.text = "";
            offerWage.text = "";

            if (value > gameManager.money)
            {
                dialogueManager.notEnoughMoney();
            } else
            {
                if (gameManager.events.Count > 0)
                {
                    foreach (Events e in gameManager.events.ToList())
                    {
                        if (e.client != null)
                        {
                            if (e.client.Equals(selectedClient))
                            {
                                gameManager.events.Remove(e);
                            }
                        }
                    }
                }
                selectedClient.offers.Add(gameManager.playerId);
                gameManager.transferProcess(selectedClient, value, wage);
                
                dialogueManager.offerSent(clientManager.infoName.text);
            }
        }
    }

    public void checkWorks()
    {

    }


    public void onMeBtnClick()
    {
        Invoke("MainScene", 0.2f);
    }

    void MainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
