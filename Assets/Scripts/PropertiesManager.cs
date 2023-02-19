using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PropertiesManager : MonoBehaviour
{
    public GameManager gameManager;
    public PropertiesPanelManager propertiesPanelManager;

    public GameObject UIPropertyMarketSet;

    public GameObject UIBlurImg;
    public GameObject UImAlertSet;
    public Text mAlertName;
    public Text mAlertDesc;
    public Text alertPosTxt;
    public Text alertNegTxt;

    public GameObject UIAlertSet;
    public Text alertName;
    public Text alertDesc;

    public GameObject UIInfoSet;
    public GameObject UIConstructSet;

    public int buildingPrice;
    public int workSlot;
    public int income;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.landbuyStatus = 0;
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onNewLandBtnClick()
    {
        UIPropertyMarketSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIPropertyMarketSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void onExitBtnClick()
    {
        if (UIInfoSet.transform.localScale == Vector3.one)
            UIInfoSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        if (UIPropertyMarketSet.transform.localScale == Vector3.one)
            UIPropertyMarketSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        if (UIBlurImg.transform.localScale == Vector3.one)
            UIBlurImg.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);

    }

    public void onIndividualBtnClick()
    {
        UIPropertyMarketSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        mAlertName.text = "Land Purchaes Agreement";
        mAlertDesc.text = "The Buyer: " + gameManager.givenName + 
            " agrees to purchase the property of " + gameManager.streetNum + " " + gameManager.streetName + 
            " in the city of " + gameManager.city + 
            " by payment of " + gameManager.landPrice + ".";
        alertPosTxt.text = "Sign";
        alertNegTxt.text = "Cancel the deal";
        UImAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UImAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        UIBlurImg.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIBlurImg.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void onPosBtnClick()
    {
        if (gameManager.money >= gameManager.landPrice)
        {
            gameManager.money = gameManager.money - gameManager.landPrice;
            gameManager.ownedLandProcess(gameManager.streetNum, gameManager.streetName, gameManager.city, gameManager.landPrice);
            gameManager.landbuyStatus = 1;

            UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UIAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
            UIAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            alertName.text = "Catching up with Jews";
            alertDesc.text = "You now own a place.";
        }
        else
        {
            UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UIAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
            UIAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            alertName.text = "Not Enough Money";
            alertDesc.text = "You tried to scam the seller but failed.";
        }

    }

    public void onNegBtnClick()
    {
        UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        UIAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        alertName.text = "Deal called off";
        alertDesc.text = "You wasted seller's time.";

    }

    public void onOKBtnClick()
    {
        UIAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        UIBlurImg.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        if (gameManager.landbuyStatus == 1)
        {
            onMeBtnClick();
        }
    }

    public void onOwnedLandBtnClick()
    {

        UIInfoSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIInfoSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        UIBlurImg.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIBlurImg.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void onNewConstructionBtnClick()
    {
        UIInfoSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        UIConstructSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIConstructSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

    }

    public void onBuildingCategoryClick()
    {
        gameManager.buildingCategory = gameObject.transform.name; 
    }

    public void onSelectBtnClick()
    {
        for (int i = 0; i < gameManager.ownedLands.Count; i++)
        {
            if (gameManager.ownedLands[i].streetNum == gameManager.streetNum && gameManager.ownedLands[i].streetName == gameManager.streetName)
            {
                Debug.Log(gameManager.ownedLands[i].streetNum);
                Debug.Log(gameManager.buildingCategory);

                if (gameManager.buildingCategory != "")
                {   
                    gameManager.ownedLands[i].buildingType = gameManager.buildingCategory;
                }
            }
            UIConstructSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UIBlurImg.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }


        gameManager.buildingCategory = "";
        onMeBtnClick();
    }

    public void setBuildingPrice(int price)
    {
        buildingPrice = price;
    }

    public void setWorkSlots(int slot)
    {
        workSlot = slot;
    }

    public void setIncome(int money)
    {
        income = money;
    }

    public void onBuildingItemClick()
    {
        if (gameManager.money >= buildingPrice)
        {
            gameManager.money -= buildingPrice;
            gameManager.createBuilding(gameManager.streetNum, gameManager.streetName, gameObject.transform.name, "anonymous", workSlot, income);
            onExitBtnClick();
        }
        else
        {
            UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UIAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
            UIAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            alertName.text = "Not Enough Money";
            alertDesc.text = "You tried to scam the seller but failed.";
        }
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
