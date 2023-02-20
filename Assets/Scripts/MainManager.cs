using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;
using System;
using static GameManager;

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
    public int eventCount;

    public string sDeath;
    public string cName;
    public string cArtname;
    public int cArtId;

    Work newWork;

    public Boolean openLocation = false;
    public Boolean locationAttempt = false;

    public GameObject UILocationsSet;

    public GameObject noplaceText;
    public GameObject selectBtn;

    public int noPlaceEventNum;

    private void Awake()
    {
        UIAlertSet.transform.DOScale(Vector3.zero, 0);
        gameManager.receiveCDB();
        gameManager.receiveWDB();
        gameManager.receiveLDB();
        gameManager.receiveDDB();
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
        pBookmark = 0;
        checkEvents();
    }

    public void calculateAge()
    {
        ageText.text = (gameManager.year - gameManager.dob).ToString();
    }

    public void checkEvents()
    {
        if (gameManager.events.Count > 0)
        {
            UImNext.SetActive(false);
            if (gameManager.eventCount > 1) {
                UImNext.SetActive(true);
            }
            if (gameManager.events[pBookmark].waitTime <= gameManager.year)
            {
                if (gameManager.events[pBookmark].alertType == 0)
                {
                    alertName.text = gameManager.events[pBookmark].alertName;
                    alertDesc.text = gameManager.events[pBookmark].alertDesc;
                    openAlert();
                } else
                {
                    mAlertName.text = gameManager.events[pBookmark].alertName;
                    mAlertDesc.text = gameManager.events[pBookmark].alertDesc;
                    mAlertPos.text = gameManager.events[pBookmark].posBtn;
                    mAlertNeg.text = gameManager.events[pBookmark].negBtn;
                    if (gameManager.events[pBookmark].eventId == 12)
                    {
                        newArtWorkProcess();
                        selectLocationManager.receiveLData();
                    }
                    openmAlert();

                }
                pEvents = gameManager.events[pBookmark];
            }

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
        UIAlertSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        UIblurImg.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
        UIblurImg.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void onOKBtnClick()
    {
        if (openLocation == true)
        {
            UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UILocationsSet.transform.DOMove(new Vector3(960, 540, 0), 0.1f).SetEase(Ease.OutBack);
            UILocationsSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            openLocation = false;
        } else
        {
            UIAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UILocationsSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            UIblurImg.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);

            gameManager.eventCount -= 1;
            gameManager.events.Remove(pEvents);
            onNextBtnClick();
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
        int posEventId = pEvents.eventId + 1;
        alertName.text = gameManager.dialogueList.dialogue[posEventId].alertName;
        alertDesc.text = gameManager.dialogueList.dialogue[posEventId].alertDesc;
        if (posEventId == 1)
        {
            gameManager.money = gameManager.money - pEvents.offeredValue;
            foreach (var clients in gameManager.currClients)
            {
                if (clients.Id == pEvents.id)
                {
                    clients.Affiliation = gameManager.surname;
                }
            }
            gameManager.protegeList.Add(pEvents.id);
            gameManager.payments = gameManager.payments + pEvents.offeredWage;

        } else if (posEventId == 10)
        {
            alertDesc.text = alertDesc.text + pEvents.name;
            gameManager.money = gameManager.money - 10;
        } else if (posEventId == 13)
        {
            if (noPlaceEventNum > 0)
            {
                locationAttempt = true;
                onNegBtnClick();
                return;
            }
            openLocation = true;
        }
        gameManager.events.Remove(pEvents);
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
        int negEventId = pEvents.eventId + 2;
        alertName.text = gameManager.dialogueList.dialogue[negEventId].alertName;
        alertDesc.text = gameManager.dialogueList.dialogue[negEventId].alertDesc;

        if (negEventId == 14)
        {
            if (locationAttempt)
            {
                alertDesc.text = "You have insufficient slot";
                locationAttempt = false;
            }
            workSold();
        }
        gameManager.events.Remove(pEvents);
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
            noPlaceEventNum = 0;
            workSold();
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
                        newWork);

                }
            }
        }
        UILocationsSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        noplaceText.SetActive(false);
    }

    public void newArtWorkProcess()
    {

        cArtId = gameManager.events[pBookmark].id;
        for (int j = 0; j < gameManager.ownedWorks.Count; j++)
        {
            if (gameManager.ownedWorks[j].Id == cArtId)
            {
                newWork = gameManager.mWork;
                cArtname = gameManager.ownedWorks[j].Name;
                Debug.Log(cArtname);

                newWork.Name = cArtname;
                newWork.Id = gameManager.ownedWorks[j].Id;
                newWork.CreatorId = gameManager.ownedWorks[j].CreatorId;
                newWork.Year = gameManager.ownedWorks[j].Year;
                newWork.Category = gameManager.ownedWorks[j].Category;
                newWork.Price = gameManager.ownedWorks[j].Price;
                newWork.Movement = gameManager.ownedWorks[j].Movement;
                newWork.Reputation = gameManager.ownedWorks[j].Reputation;
            }
        }
    }

    public void workSold()
    {
        int rnd = UnityEngine.Random.Range(1, 5);
        int price = rnd * 100;
        alertDesc.text =
        alertDesc.text = gameManager.dialogueList.dialogue[14].alertDesc + price + " due to insufficient slot." + Environment.NewLine + "(You gained " + price + ")";
        gameManager.money = gameManager.money + price;
        foreach (var works in gameManager.ownedWorks.ToList())
        {
            if (works.Id == cArtId)
            {
                Debug.Log("Artwork sold.");
                gameManager.ownedWorks.Remove(works);
            }
        }

    }

    public void onNextBtnClick()
    {

        Debug.Log("Next Button Clicked.");
        if (gameManager.events.Count > 0)
        {
            pBookmark = (pBookmark + 1) % gameManager.events.Count;
            checkEvents();
        }
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
