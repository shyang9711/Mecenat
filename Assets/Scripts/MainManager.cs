using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;
using System;
using static GameManager;
using Newtonsoft.Json.Linq;
using UnityEditor.PackageManager;

public class MainManager : MonoBehaviour
{

    public GameManager gameManager;
    public SelectLocationManager selectLocationManager;

    public Button turnBtn;
    public Text nameText;
    public Text houseText;
    public Text nationalityText;
    public Text religionText;
    public Text ageText;

    public GameObject UIblurImg;

    public GameObject UIAlertSet;
    public Text alertName;
    public Text alertDesc;

    public GameObject UImAlertSet;
    public Text mAlertName;
    public Text mAlertDesc;
    public GameObject UImNext;
    public Text mAlertPos;
    public Text mAlertNeg;
    public int pBookmark;
    GameManager.Events pEvents;

    public int mAlertNum;
    public string sDeath;
    public string cName;
    public string cArtname;
    public int cArtId;

    public GameObject UILocationsSet;

    public GameObject noplaceText;
    public GameObject selectBtn;

    public int noPlaceEventNum;

    [System.Serializable]
    public class SelectedWorks
    {
        public string Name;
        public int Id;
        public int CreatorId;
        public int Year;
        public string Category;
        public int Price;
        public string Movement;
        public int Reputation;
    }

    public List<SelectedWorks> selectedWorks;

    private void Awake()
    {
        UIAlertSet.transform.DOScale(Vector3.zero, 0);
        UImNext.SetActive(false);
        gameManager.receiveCDB();
        gameManager.receiveWDB();
        gameManager.receiveLDB();
        int ran = UnityEngine.Random.Range(1, 200);
        gameManager.ownedLandProcess(ran, "Piazza de " + gameManager.surname, "Florence", 1000, "Residential");
        gameManager.createBuilding(ran, "Piazza de " + gameManager.surname, "Casa", "anonymous", 1, -10);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;

        nameText.text = gameManager.givenName;
        houseText.text = gameManager.surname;
        nationalityText.text = gameManager.nationality;
        religionText.text = gameManager.religion;
        ageText.text = (gameManager.year - gameManager.dob).ToString();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onTurnBtnClick()
    {
        gameManager.year = gameManager.year + 1;
        checkEventD();
        gameManager.money -= gameManager.payments;
        gameManager.currClients.Clear();
        gameManager.creatorIDList.Clear();
        gameManager.currWorks.Clear();
        gameManager.currLands.Clear();
        gameManager.receiveCDB();
        gameManager.receiveWDB();
        gameManager.receiveLDB();
        calculateAge();
        gameManager.currentYClient.Clear();
        gameManager.recursionNum = 1;
        gameManager.createWorks();
        checkEvents(0);
    }

    public void calculateAge()
    {
        ageText.text = (gameManager.year - gameManager.dob).ToString();
    }

    public void checkEvents(int bookmark)
    {
        if (gameManager.events.Count() != 0)
        {
            Debug.Log("event list counted");
            if (bookmark > gameManager.events.Count - 1)
            {
                Debug.Log("reached the limit");
                checkEvents(0);
            }
            else
            {
                Debug.Log("not reached the limit and the limit is " + gameManager.events.Count());
                for (int i = bookmark; i < gameManager.events.Count(); i++)
                {
                    Debug.Log("i is " + i);
                    if (gameManager.events[i].year == gameManager.year - 2 && (gameManager.events[i].eventId == 1 || gameManager.events[i].eventId == 0))
                    {
                        Debug.Log("received respond");

                        if (gameManager.events[i].eventId == 1)
                        {
                            mAlertName.text = "Offer Denied.";
                            mAlertDesc.text = gameManager.events[i].name + " has denied your offer.";
                            mAlertPos.text = "Fine";
                            mAlertNeg.text = "OK";

                            UImNext.SetActive(true);
                            pBookmark = i;
                            openmAlert();
                            mAlertNum = 0;
                            pEvents = gameManager.events[i];
                            break;
                        }
                        else if (gameManager.events[i].eventId == 0 && gameManager.money >= gameManager.events[i].offeredValue)
                        {
                            mAlertName.text = "Offer Accepted.";
                            mAlertDesc.text = gameManager.events[i].name + " has accepted your offer for " + gameManager.events[i].offeredValue + ".";
                            mAlertPos.text = "Accept";
                            mAlertNeg.text = "Reject";

                            UImNext.SetActive(true);
                            pBookmark = i;
                            openmAlert();
                            for (int j = 0; j < gameManager.currClients.Count; j++)
                            {
                                if (gameManager.currClients[j].Id == gameManager.events[i].id)
                                {
                                    gameManager.currClients[j].Wage = gameManager.events[i].offeredWage;
                                }
                            }
                            mAlertNum = 1;
                            pEvents = gameManager.events[i];
                            break;

                        }
                        else if (gameManager.money < gameManager.events[i].offeredValue)
                        {
                            alertName.text = "Offer Denied";
                            alertDesc.text = "You are not able to pay " + gameManager.events[i].name + " the amount of money you have offered.";
                            mAlertPos.text = "Fine";
                            mAlertNeg.text = "OK";

                            UImNext.SetActive(true);
                            openmAlert();
                            pBookmark = i;
                            mAlertNum = 0;
                            pEvents = gameManager.events[i];
                            break;
                        }


                    }
                    else if (gameManager.events[i].eventId == 2)
                    {
                        sDeath = gameManager.events[i].name;
                        mAlertName.text = "Death of Your Client";
                        mAlertDesc.text = "Your client " + sDeath + " has passed away in " + gameManager.year + "." + Environment.NewLine + "Would you attend the funeral?";
                        mAlertPos.text = "Certainly";
                        mAlertNeg.text = "I'm busy";

                        UImNext.SetActive(true);
                        pBookmark = i;
                        openmAlert();
                        mAlertNum = 2;
                        pEvents = gameManager.events[i];
                        break;
                    }
                    else if (gameManager.events[i].eventId == 3)
                    {
                        cArtId = gameManager.events[i].id;
                        for (int j = 0; j < gameManager.ownedWorks.Count; j++)
                        {
                            if (gameManager.ownedWorks[j].Id == cArtId)
                            {
                                cArtname = gameManager.ownedWorks[j].Name;
                                Debug.Log(cArtname);

                                SelectedWorks s = new SelectedWorks();
                                s.Name = cArtname;
                                s.Id = gameManager.ownedWorks[j].Id;
                                s.CreatorId = gameManager.ownedWorks[j].CreatorId;
                                s.Year = gameManager.ownedWorks[j].Year;
                                s.Category = gameManager.ownedWorks[j].Category;
                                s.Price = gameManager.ownedWorks[j].Price;
                                s.Movement = gameManager.ownedWorks[j].Movement;
                                s.Reputation = gameManager.ownedWorks[j].Reputation;
                                selectedWorks.Add(s);
                            }
                        }

                        mAlertName.text = "New Artwork!";
                        mAlertDesc.text = "Your client " + gameManager.events[i].name + " has created a new artwork " +  cArtname + " in " + gameManager.year + "." 
                            + Environment.NewLine + "Would you keep the artwork on the wall?";
                        mAlertPos.text = "Keep it";
                        mAlertNeg.text = "Sell it";

                        selectLocationManager.receiveLData();
                        UImNext.SetActive(true);
                        pBookmark = i;
                        openmAlert();
                        mAlertNum = 3;
                        pEvents = gameManager.events[i];
                        break;

                    }
                }
            }
        } else
        {
            Debug.Log("no offers sent.");
        }
    }

    public void openmAlert()
    {
        UImAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UImAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        UIblurImg.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIblurImg.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }
    public void openAlert()
    {
        UImAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UImAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        UIblurImg.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIblurImg.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void onOKBtnClick()
    {

        if (mAlertNum == 4)
        {
            UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UILocationsSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
            UILocationsSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        }
        else
        {

            UIAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UILocationsSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UIblurImg.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);

            gameManager.events.Remove(pEvents);
            checkEvents(0);

        }

    }

    public void onPosBtnClick()
    {
        if (UIAlertSet.transform.localScale == Vector3.one)
        {

            UIAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }
        else if (UImAlertSet.transform.localScale == Vector3.one)
        {
            UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }

        if (mAlertNum == 0)
        {
            alertName.text = "Offer Rejected";
            alertDesc.text = "You accepted the rejection.";
            gameManager.events.Remove(pEvents);

        }

        else if (mAlertNum == 1)
        {
            alertName.text = "Offer Accepted";
            alertDesc.text = "You have reached a deal." + Environment.NewLine + "(You paid " + pEvents.offeredValue + ")";

            gameManager.money = gameManager.money - pEvents.offeredValue;
            foreach(var clients in gameManager.currClients)
            {
                if (clients.Id == pEvents.id)
                {
                    clients.Affiliation = gameManager.surname;
                }
            }
            gameManager.protegeList.Add(pEvents.id);

            gameManager.events.Remove(pEvents);
            gameManager.payments = gameManager.payments + pEvents.offeredWage;

        }
        else if (mAlertNum == 2)
        {
            alertName.text = "At the Funeral";
            alertDesc.text = "You have paid respects to " + sDeath + Environment.NewLine + "(You paid 10 golds)";
            gameManager.events.Remove(pEvents);
            gameManager.money = gameManager.money - 10;

        }
        else if (mAlertNum == 3)
        {
            alertName.text = "This is masterpiece";
            alertDesc.text = "You decided to keep the work.";
            mAlertNum = 4;
            gameManager.events.Remove(pEvents);

        }

        UIAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

    }

    public void onNegBtnClick()
    {
        if (UIAlertSet.transform.localScale == Vector3.one)
        {

            UIAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }
        else if (UImAlertSet.transform.localScale == Vector3.one)
        {
            UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }

        if (mAlertNum == 0)
        {
            alertName.text = "Offer Rejected";
            alertDesc.text = "You accepted the rejection.";
            gameManager.events.Remove(pEvents);

        }

        else if (mAlertNum == 1)
        {
            alertName.text = "Offer Denied";
            alertDesc.text = "You decided not to continue with the deal.";

            gameManager.events.Remove(pEvents);

        }
        else if (mAlertNum == 2)
        {
            alertName.text = "I Don't Have Time";
            alertDesc.text = "You did not attend " + sDeath + "'s funeral." + Environment.NewLine + "(People started gossiping about you)";
            gameManager.events.Remove(pEvents);

        }
        else if (mAlertNum == 3)
        {
            int rnd = UnityEngine.Random.Range(1, 5);
            int price = rnd * 100;
            alertName.text = "Sold the hearts and soul of your client";
            alertDesc.text = "You decided to sell the artwork and auctioned off at the price of " + price + Environment.NewLine + "(You gained " + price + ")";
            gameManager.money = gameManager.money + price;
            foreach (var works in gameManager.ownedWorks.ToList())
            {
                if (works.Id == cArtId)
                {
                    Debug.Log("Artwork sold.");
                    gameManager.ownedWorks.Remove(works);
                }
            }
            gameManager.events.Remove(pEvents);
        }

        UIAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void checkEventD()
    {
        int tableSize = gameManager.currClients.Count;
        for (int i = 0; i < tableSize; i++)
        {
            if (gameManager.protegeList.Contains(gameManager.currClients[i].Id))
            {
                if (gameManager.currClients[i].Death < gameManager.year)
                {
                    gameManager.eventProcess(gameManager.currClients[i].Name);  
                    gameManager.payments -= gameManager.currClients[i].Wage;
                }
            }
        }
    }

    public void onSelectBtnClick()
    {
        if (noPlaceEventNum == 1)
        {

            selectedWorks.Clear();
            mAlertNum = 3;
            noplaceText.SetActive(false);

            noPlaceEventNum = 0;
            UILocationsSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            onNegBtnClick();
        }
        else
        {

            int data = gameManager.ownedLands.Count;
            for (int i = 0; i < data; i++)
            {
                if (gameManager.streetNum == gameManager.ownedLands[i].streetNum && gameManager.streetName == gameManager.ownedLands[i].streetName)
                {
                    gameManager.addWorktoLandProcess(gameManager.streetNum,
                        gameManager.streetName,
                        selectedWorks[0].Name,
                        selectedWorks[0].Id,
                        selectedWorks[0].CreatorId,
                        selectedWorks[0].Year,
                        selectedWorks[0].Category,
                        selectedWorks[0].Price,
                        selectedWorks[0].Movement,
                        selectedWorks[0].Reputation);

                }
            }
            selectedWorks.Clear();
            mAlertNum = 0;
            noplaceText.SetActive(false);
            onOKBtnClick();
        }

    }

    public void onNextBtnClick()
    {

        Debug.Log("Next Button Clicked.");
        checkEvents(pBookmark + 1);
    }

    public void onBusinessBtnClick()
    {
        Invoke("BusinessScene", 0.2f);
    }
    public void onPropertiesBtnClick()
    {
        Invoke("PropertiesScene", 0.2f);
    }

    public void onProtegeBtnClick()
    {
        Invoke("ProtegeScene", 0.2f);
    }

    void BusinessScene()
    {
        SceneManager.LoadScene("Business");
    }
    void PropertiesScene()
    {
        SceneManager.LoadScene("Properties");
    }

    void ProtegeScene()
    {
        SceneManager.LoadScene("Protege");
    }
}
