using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainManager : MonoBehaviour
{
    public GameManager gameManager;
    public DialogueManager dialogueManager;
    public InitializationManager initializationManager;
    public SelectLocationManager selectLocationManager;
    public ContributionManager contributionManager;

    public Text nameText;
    public Text houseText;
    public Text nationalityText;
    public Text religionText;
    public Text ageText;

    public GameObject UIblurImg;

    public GameObject UIAlertSet;

    public GameObject UImAlertSet;
    public GameObject UImNext;
    public int bookmark;
    public Events gameEvent;

    public int eventNum;
    public string deadName;

    public GameObject UIMultiSelectSet;

    public Button selectButton;

    public Work selectedWork;
    public GameObject UIContributionSet;

    private void Awake()
    {
        dialogueManager.closeAlert();
        UIAlertSet.transform.DOScale(Vector3.zero, 0);
        UImNext.SetActive(false);
        if (gameManager.currClients.Count == 0)
        {
            initializationManager.InitializeGame();
            gameManager.UpdateSelectedOccupations();
            
            int ran = UnityEngine.Random.Range(1, 200);
            Land land = new Land();
            land.streetNum = ran;
            land.streetName = "Piazza de " + gameManager.surname;
            land.city = "Florence";
            land.price = 1000;
            land.buildingType = "Residential";

            gameManager.ownedLandProcess(land);
            gameManager.createBuilding(land, "Casa", "anonymous", 1);
        }
        gameManager.receiveLDB();
        gameManager.scoutRegion.Add("Italy");
    }

    private void Start()
    {
        gameManager = GameManager.instance;
        UpdateClientInfo();
        UpdateSelectButtonState();
    }

    private void Update()
    {
    }

    private void UpdateClientInfo()
    {
        nameText.text = gameManager.givenName;
        houseText.text = gameManager.surname;
        nationalityText.text = gameManager.residency;
    }

    public void onTurnBtnClick()
    {
        gameManager.year += 1;
        checkDeath();
        gameManager.UpdateProfitLoss();
        gameManager.currLands.Clear();
        gameManager.UpdateClients();
        gameManager.UpdateWorks();
        gameManager.UpdateRegions();
        gameManager.UpdateCourtier();
        gameManager.receiveLDB();
        gameManager.progressClients();
        gameManager.progressCourtiers();
        gameManager.createWorks();
        gameManager.shipConditionUpdate();
        gameManager.takeTurn();
        gameManager.UpdateEvents();
        checkEvents(0);
    }
    public void checkEvents(int i)
    {
        if (gameManager.currentEvents.Count == 0)
        {
            Debug.Log("no events.");
            return;
        }

        if (i > gameManager.currentEvents.Count - 1)
        {
            if (gameManager.currentEvents.Count > 0) {
                checkEvents(0);
            }
            return;
        }
        Events gameEvent = gameManager.currentEvents[i];
        eventNum = gameEvent.eventId;
        SetEventParameters(i, gameEvent.eventId, gameEvent);
        switch(eventNum)
        {
            case 0:
                break;
            // Offer Reject
            case 1:
                gameEvent.client.offers.Remove(gameManager.playerId);
                dialogueManager.offerDenied(gameEvent.client.name);
                break;
            // Offer Accept
            case 2:
                gameEvent.client.offers.Remove(gameManager.playerId);
                dialogueManager.offerAccepted(gameEvent.client.name, gameEvent.offeredValue);
                break;
            // Client Death
            case 3:
                dialogueManager.clientDeath(gameEvent.client.name);
                break;
            // New Work
            case 4:
                selectedWork = gameEvent.work;
                dialogueManager.newArtWork(gameEvent.client.name, selectedWork.name);
                break;
            // Build Ship
            case 5:
                gameManager.commissionedShips -= 1;
                gameManager.shipList.Add(gameEvent.ship);
                dialogueManager.newShipCompleted(gameEvent.ship.name);
                break;

        }
    }

    private void SetEventParameters(int index, int eventInt, Events thisEvent)
    {
        bookmark = index;
        eventNum = eventInt;
        gameEvent = thisEvent;
    }

    public void onOKBtnClick()
    {
        dialogueManager.closeAlert();
        gameManager.currentEvents.Remove(gameEvent);
        checkEvents(0);
    }
    public void onPosBtnClick()
    {
        dialogueManager.closeAlert();
        switch (eventNum)
        {
            // Transfer Reject
            case 1:
                dialogueManager.rejectionPos();
                break;
            // Transfer Accept
            case 2:
                if (gameManager.money >= gameEvent.offeredValue) {
                    gameManager.money -= gameEvent.offeredValue;
                    foreach (var client in gameManager.currClients)
                    {
                        if (client.id == gameEvent.id)
                        {
                            client.affiliation = gameManager.surname;
                        }
                    }
                    gameEvent.client.wage = gameEvent.offeredWage;
                    gameManager.protegeList.Add(gameEvent.client);
                    gameManager.currClients.Remove(gameEvent.client);
                    gameManager.UpdateSalary(gameEvent.offeredWage, true);
                    dialogueManager.dealPos(gameEvent.offeredValue);

                } else {
                    dialogueManager.offerInsufficientMoney(gameEvent.client.name);
                }
                break;
            // Client Death
            case 3:
                gameManager.money -= 10;
                dialogueManager.funeralPos(deadName);
                break;
            
            // New Work
            case 4:
                UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
                UIMultiSelectSet.SetActive(true);
                break;

            // Build Ship
            case 5:
                break;
        }
    }

    public void onNegBtnClick()
    {
        dialogueManager.closeAlert();
        switch (eventNum)
        {
            // Transfer Reject
            case 1:
                dialogueManager.rejectionNeg();
                break;
            // Transfer Accept
            case 2:
                dialogueManager.dealNeg();
                break;
            // Client Death
            case 3:
                dialogueManager.funeralNeg(deadName);
                break;
            // New Work
            case 4:
                int price = UnityEngine.Random.Range(1, 3) * 100;
                gameManager.money += price;
                dialogueManager.workNeg(price);
                break;
            // Build Ship
            case 5:
                break;
        }
    }

    public void checkDeath()
    {
        foreach (Client client in gameManager.protegeList)
        {
            if (client.death < gameManager.year)
            {
                gameManager.protegeList.Remove(client);
                gameManager.deathEventProcess(client);
                gameManager.UpdateSalary(gameEvent.offeredWage, false);
            }
        }
    }

    public void onSelectBtnClick()
    {
        selectedWork.owners.Add(gameManager.playerId);
        gameManager.ownedWorks.Add(selectedWork);
        foreach (Client client in gameManager.currClients)
        {
            if (client.id == selectedWork.creatorId) {
                client.ability += selectedWork.reputation;
                client.value = client.ability * 10;
                client.wage = client.ability / 10;
                break;
            }
        }
        gameManager.addWorktoLandProcess(selectLocationManager.selectedLand, selectedWork);
        UIMultiSelectSet.SetActive(false);
        dialogueManager.workPos();
    }

    public void onSellBtnClick()
    {
        int price = UnityEngine.Random.Range(1, 3) * 100;
        gameManager.money += price;
        UIMultiSelectSet.SetActive(false);
        dialogueManager.workNeg(price);
    }

    public void onBackgroundReset()
    {
        selectLocationManager.selectedLand = null; // Reset the selected land
        UpdateSelectButtonState(); // Update the button state
    }

    // Method to update the select button state
    public void UpdateSelectButtonState()
    {
        selectButton.interactable = selectLocationManager.selectedLand != null;
    }

    public void onContributionBtnClick()
    {
        dialogueManager.openBlurImg();
        contributionManager.receiveData();
        UIContributionSet.SetActive(true);
    }

    public void onCancelBtnClick()
    {
        UIblurImg.SetActive(false);
        UIContributionSet.SetActive(false);
    }

    public void onNextBtnClick()
    {
        checkEvents(bookmark + 1);
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

    public void onCourtierBtnClick()
    {
        Invoke("CourtierScene", 0.2f);
    }

    public void onAuctionBtnClick()
    {
        Invoke("AuctionScene", 0.2f);
    }

    public void onExpeditionBtnClick()
    {
        Invoke("ExpeditionScene", 0.2f);
    }

    private void BusinessScene()
    {
        SceneManager.LoadScene("Business");
    }
    private void PropertiesScene()
    {
        SceneManager.LoadScene("Properties");
    }
    private void ProtegeScene()
    {
        SceneManager.LoadScene("Protege");
    }
    private void CourtierScene()
    {
        SceneManager.LoadScene("Courtier");
    }
    private void AuctionScene()
    {
        SceneManager.LoadScene("Auction");
    }
    private void ExpeditionScene()
    {
        SceneManager.LoadScene("Expedition");
    }
}
