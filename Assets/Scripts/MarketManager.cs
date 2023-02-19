using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class MarketManager : MonoBehaviour
{
    public GameObject UIProtegeSet;
    public GameObject UIScoutSet;
    public GameObject UIProtegeBtn;
    public GameObject UIScoutBtn;

    public GameObject ScoutBtn;
    public Text ScoutBtnTxt;

    public GameObject UIBlurImg;
    public GameObject UIInfoSet;
    public GameObject UITransferSet;

    public Text CreatedWorksCountInfo;

    [SerializeField] private InputField offerValue;
    [SerializeField] private InputField offerWage;

    //Alert UI
    public GameObject UIAlertSet;
    public Text alertName;
    public Text alertDesc;

    //Manager Scripts
    public GameManager gameManager;
    public ClientManager clientManager;
    private void Awake()
    {

        UIInfoSet.transform.DOScale(Vector3.zero, 0);
        UITransferSet.transform.DOScale(Vector3.zero, 0);

        UIScoutSet.SetActive(false);
        UIProtegeBtn.SetActive(false);

        ScoutBtn.SetActive(false);

        checkWorks();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onMarketBtnClick()
    {
        UIProtegeSet.SetActive(false);
        UIScoutSet.SetActive(true);
        UIProtegeBtn.SetActive(true);
        UIScoutBtn.SetActive(false);

        ScoutBtn.SetActive(true);

    }
    public void onProtegeBtnClick()
    {
        UIProtegeSet.SetActive(true);
        UIScoutSet.SetActive(false);
        UIProtegeBtn.SetActive(false);
        UIScoutBtn.SetActive(true);

        ScoutBtn.SetActive(false);
    }
    
    public void onIndividualBtnClick()
    {
        foreach(var clients in gameManager.currClients)
        {
            if (clients.Id.Equals(gameManager.id))
            {
                if (clients.ctdWork.Count() != 0)
                {
                    CreatedWorksCountInfo.text = clients.ctdWork.Count().ToString();
                }
                else
                {
                    CreatedWorksCountInfo.text = "0";
                }
            }
        }

        if (gameManager.events.Count > 0)
        {
            foreach (var transfers in gameManager.events)
            {

                if (transfers.id.Equals(gameManager.id))
                {
                    ScoutBtnTxt.text = "Resend Offer";
                    infoSetMove();
                    break;
                }
                else
                {
                    ScoutBtnTxt.text = "Scout";
                    infoSetMove();
                }
            }
        }
        else
        {
            ScoutBtnTxt.text = "Scout";
            infoSetMove();
        }
    }

    public void infoSetMove()
    {
        UIInfoSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIInfoSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        UIBlurImg.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIBlurImg.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void onScoutBtnClick()
    {
        UIInfoSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        UITransferSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UITransferSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

    }

    public void onExitBtnClick()
    {
        if (UIInfoSet.transform.localScale == Vector3.one)
            UIInfoSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        if (UITransferSet.transform.localScale == Vector3.one)
            UITransferSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        if (UIBlurImg.transform.localScale == Vector3.one)
            UIBlurImg.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
    }

    public void onSendBtnClick()
    {
        int.TryParse(offerValue.text, out int value);
        int.TryParse(offerWage.text, out int wage);
        string name = clientManager.infoName.text;

        Debug.Log(value);
        if (value > gameManager.money)
        {
            notEnoughMoney();
        } else
        {
            if (gameManager.events.Count > 0)
            {
                foreach (var transfers in gameManager.events.ToList())
                {
                    if (transfers.id.Equals(gameManager.id))
                    {
                        Debug.Log(transfers.name);
                        gameManager.events.Remove(transfers);
                    }
                    
                }
                gameManager.transferProcess(name, value, wage);

                UIAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
                UIAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
                onExitBtnClick();

                alertName.text = "Offer Sent";
                alertDesc.text = "You have successfully sent an offer to " + clientManager.infoName.text;
            }

            else
            {
                gameManager.transferProcess(name, value, wage);

                UIAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
                UIAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
                onExitBtnClick();

                alertName.text = "Offer Sent";
                alertDesc.text = "You have successfully sent an offer to " + clientManager.infoName.text;
            }
        }
    }

    public void notEnoughMoney()
    {
        UIAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        onExitBtnClick();

        alertName.text = "Not Enough Money";
        alertDesc.text = "You do not have the amount of money you entered.";
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
