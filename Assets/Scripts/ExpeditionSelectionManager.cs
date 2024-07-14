using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionSelectionManager : MonoBehaviour
{
    public GameManager gameManager;
    public ExpeditionManager expeditionManager;

    public Transform selectionContent;
    public GameObject selectionPrefab;
    
    public GameObject currentlySelectedButton;

    private List<City> movableCities;

    private List<Explorer> explorerList;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buildShipData()
    {
        int tableSize = gameManager.shipBuildList.Count;

        selectionContent.GetChild(0).gameObject.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in selectionContent)
        {
            if (child != selectionContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }

        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {

            Ship ship = gameManager.shipBuildList[i];
            gameObj = Instantiate(selectionPrefab, selectionContent);
            gameObj.name = ship.name;
            gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ship.name;

            gameObj.GetComponent<Button>().AddEventListener(i, buildShipClick);
        }

        selectionPrefab.gameObject.SetActive(false);
    }

    void buildShipClick(int itemIndex)
    {
        GameObject clickedButton = selectionContent.GetChild(itemIndex + 1).gameObject;

        if (expeditionManager.selectedShip != gameManager.shipBuildList[itemIndex])
        {
            // De-highlight the previously selected button
            if (currentlySelectedButton != null)
            {
                SetButtonHighlight(currentlySelectedButton, false);
            }

            // Highlight the new button
            SetButtonHighlight(clickedButton, true);
            currentlySelectedButton = clickedButton;
            expeditionManager.isShipChangeable = true;
            expeditionManager.selectedShip = gameManager.shipBuildList[itemIndex];
        }
        else
        {
            // De-highlight if the same ship is clicked again
            SetButtonHighlight(clickedButton, false);
            currentlySelectedButton = null;
            expeditionManager.selectedShip = null;
        }
        expeditionManager.UpdateSelectButtonState();
    }

    public void buyShipData()
    {
        int tableSize = gameManager.shipBuyList.Count;

        selectionContent.GetChild(0).gameObject.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in selectionContent)
        {
            if (child != selectionContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }

        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {

            Ship ship = gameManager.shipBuyList[i];
            gameObj = Instantiate(selectionPrefab, selectionContent);
            gameObj.name = ship.name;
            gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ship.name;

            gameObj.GetComponent<Button>().AddEventListener(i, buyShipClick);
        }

        selectionPrefab.gameObject.SetActive(false);
    }

    void buyShipClick(int itemIndex)
    {
        GameObject clickedButton = selectionContent.GetChild(itemIndex + 1).gameObject;

        if (expeditionManager.selectedShip != gameManager.shipBuyList[itemIndex])
        {
            // De-highlight the previously selected button
            if (currentlySelectedButton != null)
            {
                SetButtonHighlight(currentlySelectedButton, false);
            }

            // Highlight the new button
            SetButtonHighlight(clickedButton, true);
            currentlySelectedButton = clickedButton;
            expeditionManager.selectedShip = gameManager.shipBuyList[itemIndex];
        }
        else
        {
            // De-highlight if the same ship is clicked again
            SetButtonHighlight(clickedButton, false);
            currentlySelectedButton = null;
            expeditionManager.selectedShip = null;
        }
        expeditionManager.UpdateSelectButtonState();
    }

    public void citiesData()
    {
        int tableSize = gameManager.citiesList.city.Length;

        selectionContent.GetChild(0).gameObject.SetActive(true);

        // Clear existing buttons
        movableCities = new List<City>();
        foreach (Transform child in selectionContent)
        {
            if (child != selectionContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }

        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {

            City city = gameManager.citiesList.city[i];
            if (!gameManager.placedRegion.Contains(city.nation))
            {
                continue;
            }
            if (expeditionManager.selectedShip.city != null && expeditionManager.selectedShip.city.name == city.name)
            {
                continue;
            }
            movableCities.Add(city);
            gameObj = Instantiate(selectionPrefab, selectionContent);
            gameObj.name = city.name;
            gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = city.name;

            int currentIndex = movableCities.Count - 1;
            gameObj.GetComponent<Button>().AddEventListener(currentIndex, cityClick);
        }

        selectionPrefab.gameObject.SetActive(false);
    }

    void cityClick(int itemIndex)
    {
        Debug.Log("item Index clicked : " + itemIndex);
        GameObject clickedButton = selectionContent.GetChild(itemIndex + 1).gameObject;

        if (expeditionManager.selectedCity != movableCities[itemIndex])
        {
            // De-highlight the previously selected button
            if (currentlySelectedButton != null)
            {
                SetButtonHighlight(currentlySelectedButton, false);
            }

            // Highlight the new button
            SetButtonHighlight(clickedButton, true);
            currentlySelectedButton = clickedButton;
            expeditionManager.selectedCity = movableCities[itemIndex];
        }
        else
        {
            // De-highlight if the same ship is clicked again
            SetButtonHighlight(clickedButton, false);
            currentlySelectedButton = null;
            expeditionManager.selectedCity = null;
        }
        expeditionManager.UpdateSelectButtonState();
    }
    public void explorerData()
    {
        int tableSize = gameManager.protegeList.Count;

        selectionContent.GetChild(0).gameObject.SetActive(true);

        // Clear existing buttons
        explorerList = new List<Explorer>();
        foreach (Transform child in selectionContent)
        {
            if (child != selectionContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }

        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {

            if (gameManager.protegeList[i] is Explorer explorer)
            {
                explorerList.Add(explorer);
                gameObj = Instantiate(selectionPrefab, selectionContent);
                gameObj.name = explorer.name;
                gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = explorer.name;

                int currentIndex = explorerList.Count - 1;
                gameObj.GetComponent<Button>().AddEventListener(currentIndex, explorerClick);
            }
        }

        selectionPrefab.gameObject.SetActive(false);
    }

    void explorerClick(int itemIndex)
    {
        Debug.Log("item Index clicked : " + itemIndex);
        GameObject clickedButton = selectionContent.GetChild(itemIndex + 1).gameObject;

        if (expeditionManager.selectedExplorer != explorerList[itemIndex])
        {
            // De-highlight the previously selected button
            if (currentlySelectedButton != null)
            {
                SetButtonHighlight(currentlySelectedButton, false);
            }

            // Highlight the new button
            SetButtonHighlight(clickedButton, true);
            currentlySelectedButton = clickedButton;
            expeditionManager.selectedExplorer = explorerList[itemIndex];
        }
        else
        {
            // De-highlight if the same ship is clicked again
            SetButtonHighlight(clickedButton, false);
            currentlySelectedButton = null;
            expeditionManager.selectedExplorer = null;
        }
        expeditionManager.UpdateSelectButtonState();
    }

    void SetButtonHighlight(GameObject buttonObj, bool isHighlighted)
    {
        if (buttonObj != null)
        {
            Color color = isHighlighted ? ColorExtensions.FromHex("#E9D18E") : ColorExtensions.FromHex("#A9A9A9"); // Set your desired highlight color
            buttonObj.GetComponent<Image>().color = color;
        }
    }

    // Method to set all buttons' highlight state
    public void SetAllButtonsHighlight(bool isHighlighted)
    {
        SetButtonHighlight(currentlySelectedButton, isHighlighted);
    }
}
