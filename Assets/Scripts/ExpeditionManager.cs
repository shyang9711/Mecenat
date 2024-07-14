using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ExpeditionManager : MonoBehaviour
{
    public GameManager gameManager;
    public DialogueManager dialogueManager;
    public ExpeditionSelectionManager expeditionSelectionManager;

    public int eventNum;
    public bool isShipChangeable = false;

    public Button slot1;
    public Button slot2;
    public Button slot3;
    public Button slot4;
    public Button slot5;
    public Button slot6;
    public Button slot7;
    public Button slot8;

    public GameObject infoPanel;
    public Text shipNameText;
    public Text builtYearText;
    public Text actionText;
    public Text cityText;
    public Text explorerText;
    public Text conditionText;

    public GameObject multiPanel;
    public Button selectBtn;

    public GameObject actionPanel;
    public Text nameTxt;
    public Text locationTxt;
    public Text conditionTxt;
    public Button exploreBtn;
    public Button tradeBtn;
    public Button hireBtn;
    public Button fireBtn;
    public Button cancelActionBtn;

    public GameObject UIRenameSet;
    public bool isBuilding = false;
    [SerializeField] private InputField renameField;


    public Ship selectedShip;
    public City selectedCity;
    public Explorer selectedExplorer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        dialogueManager.closeAlert();
        selectedShip = null;
        UpdateSelectButtonState();
        // Initialize buttons
        InitializeButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeButton()
    {
        UpdateButton(slot1, 0);
        UpdateButton(slot2, 1);
        UpdateButton(slot3, 2);
        UpdateButton(slot4, 3);
        UpdateButton(slot5, 4);
        UpdateButton(slot6, 5);
        UpdateButton(slot7, 6);
        UpdateButton(slot8, 7);
    }

    private void UpdateButton(Button button, int index)
    {
        // Remove all previous listeners to ensure no old listeners are called
        button.onClick.RemoveAllListeners();

        TextMeshProUGUI nameText = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI cityText = button.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI actionText = button.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        if (index < gameManager.shipList.Count && gameManager.shipList[index] != null)
        {
            Ship ship = gameManager.shipList[index];
            nameText.text = ship.name;
            cityText.text = "In Cargo";
            if (!ship.inCargo)
            {
                cityText.text = ship.city.name;
            }
            actionText.text = ship.action;
            cityText.gameObject.SetActive(true);
            actionText.gameObject.SetActive(true);
            button.onClick.AddListener(() => onSlotBtnClick(index));
        }
        else if (gameManager.shipList.Count + gameManager.commissionedShips > index)
        {
            nameText.text = "TBD";
            cityText.text = "In Cargo";
            actionText.text = "Commissioned";
            cityText.gameObject.SetActive(true);
            actionText.gameObject.SetActive(true);
            button.interactable = false;
            return;
        }
        else
        {
            nameText.text = "Empty Slot";
            cityText.gameObject.SetActive(false); // Hide the city text
            actionText.gameObject.SetActive(false); // Hide the action text
            button.onClick.AddListener(() => onNewShipBtnClick());
        }
    }
    public void onRenameBtnClick()
    {
        isShipChangeable = false;
        dialogueManager.openBlurImg();
        UIRenameSet.SetActive(true);
    }
    public void onRenameConfirmBtnClick()
    {
        if (!string.IsNullOrEmpty(renameField.text))
        {
            selectedShip.name = renameField.text;
            renameField.text = "";
            if (isBuilding)
            {
                isBuilding = false;
                gameManager.shpiBuildProcess(selectedShip);
            }
            UIRenameSet.SetActive(false);
            dialogueManager.closeAlert();
        }
        InitializeButton();
    }
    public void onOKBtnClick()
    {
        dialogueManager.closeAlert();
        if (isBuilding)
        {
            eventNum = 3;
            onRenameBtnClick();
        }
        else
        {
            eventNum = 0;
            InitializeButton();
        }
    }
    public void onPosBtnClick()
    {
        isShipChangeable = false;
        dialogueManager.closeAlert();
        switch (eventNum)
        {
            case 0:
                break;

            // Build New Ship
            case 1:
                Debug.Log("money : " + gameManager.money);
                Debug.Log("cost: " + selectedShip.cost);
                if (gameManager.money >= selectedShip.cost) {
                    gameManager.money -= selectedShip.cost;
                    Ship newShip = new Ship();
                    newShip.builtYear = gameManager.year;
                    newShip.condition = 100;
                    newShip.size = selectedShip.size;
                    newShip.cost = (int) Math.Round((double)(selectedShip.size * 10 * 99) / 100);
                    newShip.inCargo = true;
                    newShip.action = "None";
                    newShip.trade = "None";
                    newShip.constructionTime = selectedShip.constructionTime;
                    newShip.hired = false;
                    selectedShip = newShip;
                    isBuilding = true;
                    gameManager.commissionedShips += 1;
                    dialogueManager.buildShipPos();
                } else {
                    eventNum = 0;
                    dialogueManager.shipInsufficientMoney();
                }
                break;
            // Buy New Ship
            case 2:
                if (gameManager.money >= selectedShip.cost) {
                    gameManager.money -= selectedShip.cost;
                    gameManager.shipList.Add(selectedShip);
                    dialogueManager.buyShipPos();

                } else {
                    dialogueManager.shipInsufficientMoney();
                }
                break;
            // Case 3 skipped due to ship renaming event

            // Move Ship
            case 4:
                UpdateCancelAction(selectedShip);
                selectedShip.city = selectedCity;
                selectedShip.inCargo = false;
                dialogueManager.moveShipPos();
                break;
            
            // Maintenance Ship
            case 5:
                selectedShip.hasMaintanence = true;
                dialogueManager.maintanenceShipPos();
                break;

            // Trading
            case 6:
                selectedShip.isTrading = true;
                selectedShip.trade = selectedShip.city.trade;
                selectedShip.action = "Trading " + selectedShip.trade;
                gameManager.IncreaseTradeItem(selectedShip.trade);
                gameManager.UpdateTradingIncome();
                dialogueManager.tradePos();
                break;

            // Hiring
            case 7:
                selectedShip.explorer = selectedExplorer;
                selectedExplorer.ship = selectedShip;
                selectedShip.hired = true;
                dialogueManager.hireExplorerPos();
                break;

            // Searching
            case 8:
                break;

            // Cancel Action
            case 9:
                UpdateCancelAction(selectedShip);
                dialogueManager.cancelActionPos();
                break;

            // Sell Ship
            case 10:
                UpdateCancelAction(selectedShip);
                selectedShip.city = null;
                selectedShip.inCargo = true;
                if (selectedShip.hired)
                {
                    fireExplorer();
                }
                gameManager.money += selectedShip.cost;
                gameManager.shipList.Remove(selectedShip);
                selectedShip = null;
                dialogueManager.sellShipPos();
                break;
            
            // Exploring
            case 11:
                break;

            // Firing explorer
            case 12:
                fireExplorer();
                dialogueManager.fireExplorerPos();
                break;
        }
    }

    public void onNegBtnClick()
    {
        isShipChangeable = false;
        dialogueManager.closeAlert();
        switch (eventNum)
        {
            // Build New Ship
            case 1:
                eventNum = 0;
                dialogueManager.buildShipNeg();
                break;
            // Buy New Ship
            case 2:
                dialogueManager.buyShipNeg();
                break;
            // Case 3 skipped due to ship renaming event

            // Move Ship
            case 4:
                dialogueManager.moveShipNeg();
                break;

            // Maintanence Ship
            case 5:
                dialogueManager.maintanenceShipNeg();
                break;

            // Trading
            case 6:
                dialogueManager.tradeNeg();
                break;

            // Hiring
            case 7:
                dialogueManager.hireExplorerNeg();
                break;

            // Searching
            case 8:
                break;

            // Cancel Action
            case 9:
                dialogueManager.cancelActionNeg();
                break;

            // Sell Ship
            case 10:
                dialogueManager.sellShipNeg();
                break;
            
            // Exploring
            case 11:
                break;

            // Firing explorer
            case 12:
                dialogueManager.fireExplorerNeg();
                break;
        }
    }

    public void onSlotBtnClick(int index)
    {
        selectedShip = gameManager.shipList[index];

        shipNameText.text = selectedShip.name;
        builtYearText.text = selectedShip.builtYear.ToString();
        actionText.text = selectedShip.action;
        // tradeText.text = selectedShip.trade;
        cityText.text = "In Cargo";
        if (!selectedShip.inCargo) {
            cityText.text = selectedShip.city.name; // Assuming City has a name property
        }
        explorerText.text = "None";
        if (selectedShip.hired) {
            explorerText.text = selectedShip.explorer.name;
        }
        conditionText.text = selectedShip.condition.ToString();

        dialogueManager.openBlurImg();
        infoPanel.SetActive(true);
        multiPanel.SetActive(false);
        actionPanel.SetActive(false);
    }

    public void onNewShipBtnClick()
    {
        eventNum = 1;
        dialogueManager.closeAlert();
        dialogueManager.openBlurImg();
        UpdateSelectButtonState();
        expeditionSelectionManager.buildShipData();
        multiPanel.SetActive(true);
    }

    public void onActionBtnClick()
    {
        nameTxt.text = selectedShip.name;
        locationTxt.text = "In Cargo";
        conditionTxt.text = "Condition : " + selectedShip.condition.ToString();
        if (!selectedShip.inCargo)
        {
            locationTxt.text = selectedShip.city.name;
        }
        hireBtn.gameObject.SetActive(true);
        fireBtn.gameObject.SetActive(false);
        if (selectedShip.hired)
        {
            hireBtn.gameObject.SetActive(false);
            fireBtn.gameObject.SetActive(true);
        }
        dialogueManager.closeAlert();
        dialogueManager.openBlurImg();
        UpdateActionButtonsState();
        actionPanel.SetActive(true);
    }

    public void onInfoBtnClick()
    {
        shipNameText.text = selectedShip.name;
        builtYearText.text = selectedShip.builtYear.ToString();
        actionText.text = selectedShip.action;
        // tradeText.text = selectedShip.trade;
        cityText.text = "In Cargo";
        if (!selectedShip.inCargo) {
            cityText.text = selectedShip.city.name; // Assuming City has a name property
        }
        explorerText.text = "None";
        if (selectedShip.hired) {
            explorerText.text = selectedShip.explorer.name;
        }
        conditionText.text = selectedShip.condition.ToString();
        dialogueManager.closeAlert();
        dialogueManager.openBlurImg();
        infoPanel.SetActive(true);
        multiPanel.SetActive(false);
        actionPanel.SetActive(false);
    }

    public void onMaintenanceClick()
    {
        eventNum = 5;
        dialogueManager.closeAlert();
        dialogueManager.maintanenceShip(selectedShip);
    }

    public void onTradeBtnClick()
    {
        eventNum = 6;
        dialogueManager.closeAlert();
        dialogueManager.trade(selectedShip.city.trade);
    }

    public void onHireBtnClick()
    {
        eventNum = 7;
        dialogueManager.closeAlert();
        dialogueManager.openBlurImg();
        UpdateSelectButtonState();
        selectedExplorer = null;
        expeditionSelectionManager.explorerData();
        multiPanel.SetActive(true);
    }

    public void onMoveBtnClick()
    {
        eventNum = 4;
        dialogueManager.closeAlert();
        dialogueManager.openBlurImg();
        expeditionSelectionManager.citiesData();
        selectedCity = null;
        multiPanel.SetActive(true);
    }

    public void onCancelActionClick()
    {
        eventNum = 9;
        dialogueManager.closeAlert();
        dialogueManager.cancelAction();
    }

    public void onSellBtnClick()
    {
        eventNum = 10;
        dialogueManager.closeAlert();
        dialogueManager.sellShip();
    }

    public void onExitBtnClick()
    {
        expeditionSelectionManager.currentlySelectedButton = null;
        expeditionSelectionManager.SetAllButtonsHighlight(false);
        selectedShip = null;
        dialogueManager.closeAlert();
    }

    public void onSelectBtnClick()
    {
        dialogueManager.closeAlert();
        switch(eventNum)
        {
            // Build New Ship
            case 1:
                dialogueManager.buildShip(selectedShip);
                break;
            // Buy New Ship
            case 2:
                dialogueManager.buildShip(selectedShip);
                break;
            // Move Ship
            case 4:
                dialogueManager.moveShip(selectedShip, selectedCity);
                break;
            case 7:
                dialogueManager.hireExplorer(selectedShip, selectedExplorer);
                break;
        }

    }

    public void onFireBtnClick()
    {
        eventNum = 12;
        dialogueManager.closeAlert();
        dialogueManager.fireExplorer();
    }

    public void fireExplorer()
    {
        selectedShip.hired = false;
        selectedShip.explorer.isHired = false;
        selectedShip.explorer.ship = null;
        selectedShip.explorer = null;
    }

    public void UpdateCancelAction(Ship ship)
    {
        if(ship.isTrading)
        {
            gameManager.DecreaseTradeItem(ship.trade);
            ship.isTrading = false;
            ship.trade = "None";
            gameManager.UpdateTradingIncome();
        }
        ship.isExploring = false;
        ship.isSearching = false;
        ship.action = "None";
    }
    
    public void UpdateSelectButtonState()
    {
        selectBtn.interactable = expeditionSelectionManager.currentlySelectedButton != null;
    }

    public void UpdateActionButtonsState()
    {
        exploreBtn.interactable = !selectedShip.inCargo && selectedShip.hired && !selectedShip.isTrading && !selectedShip.isExploring && !selectedShip.isSearching;
        tradeBtn.interactable = !selectedShip.inCargo && !selectedShip.isTrading && !selectedShip.isExploring && !selectedShip.isSearching;
        cancelActionBtn.interactable = !string.IsNullOrEmpty(selectedShip.action) && selectedShip.action != "None";
    }

    public void onBackgroundReset()
    {
        expeditionSelectionManager.SetAllButtonsHighlight(false);
        selectedCity = null;
        if (isShipChangeable)
        {
            isShipChangeable = false;
            selectedShip = null;
        }
        expeditionSelectionManager.currentlySelectedButton = null;
        UpdateSelectButtonState();
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
