using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public static class ButtonExtension
{
    public static void AddEventListener<T> (this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate ()
        {
            OnClick(param);
        });
    }
}

public class ClientManager : MonoBehaviour
{
    public MarketManager marketManager;
    public GameManager gameManager;

    //Info UI
    public Text infoName;
    public Text infoOccupation;
    public Text infoAge;
    public Text infoCV;
    public Text infoWage;
    public Text infoReputation;
    public Text infoCreations;

    //Transfer UI
    public Text transferCV;
    public Text transferWage;

    //Alert UI
    public GameObject UIAlertSet;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        receiveCData();

        
        UIAlertSet.transform.DOScale(Vector3.zero, 0);

    }
    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }

    void receiveCData()
    {
        int tableSize = gameManager.currClients.Count;

        GameObject indivBtn = transform.GetChild(0).gameObject;
        GameObject gameObj;

        for (int i = 0; i < tableSize; i++)
        {

            gameObj = Instantiate(indivBtn, transform);
            gameObj.name = gameManager.currClients[i].Name;
            gameObj.transform.GetChild(0).GetComponent<Text>().text = gameManager.currClients[i].Name;
            gameObj.transform.GetChild(1).GetComponent<Text>().text = gameManager.currClients[i].Occupation;
            gameObj.transform.GetChild(2).GetComponent<Text>().text = gameManager.currClients[i].Age.ToString();
            gameObj.transform.GetChild(3).GetComponent<Text>().text = gameManager.currClients[i].Potential.ToString();
            gameObj.transform.GetChild(4).GetComponent<Text>().text = gameManager.currClients[i].Value.ToString();
            gameObj.transform.GetChild(5).GetComponent<Text>().text = gameManager.currClients[i].Affiliation;

            gameObj.GetComponent<Button>().AddEventListener(i, ItemClicked);

            if (gameManager.year - gameManager.currClients[i].Start < 0)
            {
                gameObj.SetActive(false);
            } else if (gameManager.year - gameManager.currClients[i].Death > 0)
            {
                gameObj.SetActive(false);
            }

            foreach (var protege in gameManager.protegeList)
            {
                if (protege == gameManager.currClients[i].Id)
                {
                    gameObj.SetActive(false);
                }
            }

        }

        Destroy(indivBtn);
    }

    void ItemClicked(int itemIndex)
    {
        Debug.Log("item " + itemIndex + " cliked");
        infoName.text = gameManager.currClients[itemIndex].Name;
        infoOccupation.text = gameManager.currClients[itemIndex].Occupation;
        infoAge.text = gameManager.currClients[itemIndex].Age.ToString();
        infoCV.text = gameManager.currClients[itemIndex].Value.ToString();
        infoWage.text = gameManager.currClients[itemIndex].Wage.ToString();
        infoReputation.text = gameManager.currClients[itemIndex].Potential.ToString();
        infoCreations.text = 0.ToString();

        transferCV.text = gameManager.currClients[itemIndex].Value.ToString();
        transferWage.text = gameManager.currClients[itemIndex].Wage.ToString();

        gameManager.id = gameManager.currClients[itemIndex].Id;

        gameManager.selectedCV = gameManager.currClients[itemIndex].Value;
        gameManager.selectedWage = gameManager.currClients[itemIndex].Wage;

        marketManager.onIndividualBtnClick();
    }

    public void onAlertBtnClick()
    {
        if (UIAlertSet.transform.localScale == Vector3.one)
            UIAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);

    }

    public void onMeBtnClick()
    {
        Invoke("MainScene", 0.5f);
    }


}
