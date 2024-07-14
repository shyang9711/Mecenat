using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PropertiesManager : MonoBehaviour
{
    // Managers
    public GameManager gameManager;
    public PropertiesPanelManager propertiesPanelManager;
    public DialogueManager dialogueManager;
    public CollectionsPanelManager collectionsPanelManager;
    public LandManager landManager;

    // Sets
    public GameObject UIPropertyMarketSet;

    public GameObject UIInfoSet;
    public GameObject UIConstructSet;

    public GameObject UICollectionSet;

    // Land Info Set
    public Text infoName;
    public Text infoCity;
    public Text infoOwnedYears;
    public Text infoValue;
    public Text infoTax;
    public Text infoReputation;
    public Text infoCollections;

    // Work Info Set
    public GameObject UIWorkInfoSet;
    public Text workName;
    public Text workYear;
    public Text workCreator;
    public Text workPrice;
    public Text workMovement;
    public Text workReputation;

    // Event Num
    public int propertiesEventNum;

    //Selected Objects
    public Button selectButton;
    public Land selectedLand;
    public string selectedBuildingType;
    public string selectedBuilding;

    // Collection Works
    public Transform workContent;

    public Work selectedWork;

    // Building Slots
    public Dictionary<string, int> buildingSlots = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        gameManager.landbuyStatus = 0;
        gameManager = GameManager.instance;
        dialogueManager.closeAlert();
        UpdateSelectButtonState();
        InitializeBuildingSlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onNewLandBtnClick()
    {
        dialogueManager.openBlurImg();
        UIPropertyMarketSet.transform.DOMove(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), 0.1f).SetEase(Ease.OutBack);
        UIPropertyMarketSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void onExitBtnClick()
    {
        onBackgroundReset();
        dialogueManager.closeAlert();
        StartCoroutine(ActivateWorkPanel());
    }

    public void onLandBtnClick(Land land)
    {
        selectedLand = land;
        UIPropertyMarketSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);

        propertiesEventNum = 1;
        dialogueManager.landPurchase(selectedLand);
    }

    public void onPosBtnClick()
    {
            
        dialogueManager.closeAlert();
        switch (propertiesEventNum)
        {
            // Buy Land
            case 1:
                if (checkSufficientMoney(selectedLand.price))
                {
                    gameManager.money = gameManager.money - selectedLand.price;
                    gameManager.ownedLandProcess(selectedLand);
                    gameManager.currLands.Remove(selectedLand);
                    gameManager.landbuyStatus = 1;
                    dialogueManager.landPos();
                }
                break; // Add break here to prevent fall-through
            
            // Build New Building
            case 2:
                int buildingCost = buildingSlots[selectedBuilding] * 100;
                if(checkSufficientMoney(buildingCost))
                {
                    dialogueManager.addBuildingPos();
                    gameManager.money -= buildingCost;
                    gameManager.createBuilding(selectedLand, selectedBuilding, "anonymous", buildingSlots[selectedBuilding]);
                }
                break;

        }

    }

    public void onNegBtnClick()
    {
        dialogueManager.closeAlert();
        switch (propertiesEventNum)
        {
            // Buy Land
            case 1:
                dialogueManager.landNeg();
                selectedLand = null;
                break;
            
            // Build New Building
            case 2:
                dialogueManager.addBuildingNeg();
                break;
        }
    }

    public void onOKBtnClick()
    {
        selectedLand = null;
        dialogueManager.closeAlert();
        propertiesPanelManager.receiveData();
        landManager.receiveData();
    }

    bool checkSufficientMoney(int cost)
    {
        if (gameManager.money < cost)
        {
            dialogueManager.landInsufficientMoney();
            return false;
        }
        return true;

    }

    public void onPropertyItemClick(Land land)
    {
        int ownedY = gameManager.year - land.year;
        infoName.text = land.streetNum + " " + land.streetName;
        infoCity.text = land.city;
        infoOwnedYears.text = ownedY.ToString();
        infoValue.text = land.price.ToString();
        infoTax.text = "15%";
        if (land.price / 1000 * (1 + (ownedY / 100)) > 10)
        {
            infoReputation.text = "10";
        }
        else
        {
            infoReputation.text = (land.price / 1000 * (1 + (ownedY / 100))).ToString();
        }
        int count = land.works.Count;
        int slots = land.workSlots;
        infoCollections.text = count + " / " + slots;

        propertiesPanelManager.checkBuildingType(land);
        propertiesPanelManager.checkOwnedBuildings(land);

        selectedLand = land;
        dialogueManager.openBlurImg();
        UIInfoSet.SetActive(true);
    }

    public void onNewConstructionBtnClick()
    {
        UIInfoSet.SetActive(false);
        UIConstructSet.transform.DOMove(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), 0.1f).SetEase(Ease.OutBack);
        UIConstructSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

    }

    public void onBuildingCategoryClick()
    {

        // Get the currently selected button
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

        // Find the Text component within the button and get its text
        Text buttonText = selectedButton.transform.GetChild(0).GetComponent<Text>();
        if (buttonText != null)
        {
            selectedBuildingType = buttonText.text;
        }
        else
        {
            Debug.LogError("No Text component found within the button");
        }
        UpdateSelectButtonState(); // Update the button state
    }

    public void onBuildingClick()
    {
        propertiesEventNum = 2;

        // Get the currently selected button
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

        // Find the Text component within the button and get its text
        Text buttonText = selectedButton.transform.GetChild(0).GetComponent<Text>();
        if (buttonText != null)
        {
            selectedBuilding = buttonText.text;
            dialogueManager.closeAlert();
            dialogueManager.addBuilding(selectedBuilding, buildingSlots[selectedBuilding] * 100);
        }
        else
        {
            Debug.LogError("No Text component found within the button");
        }
    }

    public void onBackgroundReset()
    {
        selectedBuildingType = "";
        selectedBuilding = "";
        UpdateSelectButtonState(); // Update the button state
    }

    // Method to update the select button state
    private void UpdateSelectButtonState()
    {
        selectButton.interactable = !string.IsNullOrEmpty(selectedBuildingType);
    }

    public void onSelectBtnClick()
    {
        selectedLand.buildingType = selectedBuildingType;
        addInitialBuilding();
        propertiesPanelManager.receiveData();
        dialogueManager.closeAlert();

        gameManager.buildingCategory = "";
    }

    public void InitializeBuildingSlots()
    {

        buildingSlots.Add("Casa", 5);
        buildingSlots.Add("Villa", 15);
        buildingSlots.Add("Courtyard", 5);
        buildingSlots.Add("Palace", 40);
        buildingSlots.Add("Garden", 10);

        buildingSlots.Add("Workshop", 5);
        buildingSlots.Add("Academy", 15);
        buildingSlots.Add("Library", 10);
        buildingSlots.Add("University", 30);
        buildingSlots.Add("Tech Institute", 40);
        
        buildingSlots.Add("Exhibition Hall", 10);
        buildingSlots.Add("Gallery", 20);
        buildingSlots.Add("Museum", 30);
        buildingSlots.Add("Concert Hall", 30);
        buildingSlots.Add("Exposition Pavilion", 40);
    }

    public void addInitialBuilding()
    {
        if (selectedLand.buildingType == "Residential") {
            gameManager.createBuilding(selectedLand, "Casa", "anonymous", buildingSlots["Casa"]);
        }
        else if (selectedLand.buildingType == "Cultural") {
            gameManager.createBuilding(selectedLand, "Exhibition Hall", "anonymous", buildingSlots["Exhibition Hall"]);
        }
        else if (selectedLand.buildingType == "Academic") {
            gameManager.createBuilding(selectedLand, "Workshop", "anonymous", buildingSlots["Workshop"]);
        }
    }

    public void onViewCollectionClick()
    {
        collectionsPanelManager.receiveData();
        dialogueManager.openBlurImg();
        UICollectionSet.transform.DOMove(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), 0.1f).SetEase(Ease.OutBack);
        UICollectionSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void onViewWorkClick(Work work)
    {
        Debug.Log(work.name);
        workName.text = work.name;
        workYear.text = $"{work.year}";
        workCreator.text = work.creator;
        workReputation.text = $"{work.reputation}";
        // workPrice.text = $"{work.price}";
        workMovement.text = work.movement;
        UIWorkInfoSet.transform.DOMove(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), 0.1f).SetEase(Ease.OutBack);
        UIWorkInfoSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void onAuctionBtnClick()
    {
        selectedLand.works.Remove(selectedWork);
        gameManager.auctionWorks.Add(selectedWork);
        dialogueManager.closeAlert();
    }

    private IEnumerator ActivateWorkPanel()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(1f);

        // Set the game object active
        workContent.GetChild(0).gameObject.SetActive(true);
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
