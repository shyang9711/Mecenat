using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourtierSelectionManager : MonoBehaviour
{
    public GameManager gameManager;
    public CourtierManager courtierManager;

    public Transform selectionContent;
    public GameObject itemPrefab;
    public Text title;
    public List<string> occupations;

    private List<GameObject> instantiatedButtons = new List<GameObject>(); // Track instantiated buttons
    private List<int> buttonToNationIndexMap = new List<int>();
    private List<GameObject> regionButtons = new List<GameObject>(); // Track instantiated region buttons

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void receiveOData()
    {
        occupations = new List<string>{"Artist", "Writer", "Scholar", "Explorer"};
        courtierManager.selectedOccupations.Clear();
        int tableSize = occupations.Count;
        Debug.Log("occputations.Count == " + tableSize);
        clearPreviousButtons();
        // Clear the list of region buttons
        instantiatedButtons.Clear();

        selectionContent.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < tableSize; i++)
        {

            string occupation = occupations[i];

            GameObject gameObj = Instantiate(itemPrefab, selectionContent);
            gameObj.name = occupation;
            gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = occupation;

            gameObj.GetComponent<Button>().AddEventListener(i, occupationItemClicked);

            instantiatedButtons.Add(gameObj); // Track instantiated buttons

            Debug.Log($"Occupation '{occupation}' processed and added to instantiated buttons.");
        }

        itemPrefab.gameObject.SetActive(false);
    }

    void occupationItemClicked(int itemIndex)
    {
        GameObject buttonObj = selectionContent.GetChild(itemIndex + 1).gameObject;
        if (courtierManager.selectedOccupations.Contains(occupations[itemIndex]))
        {
            courtierManager.selectedOccupations.Remove(occupations[itemIndex]);
            SetButtonHighlight(buttonObj, false);
        }
        else
        {
            courtierManager.selectedOccupations.Add(occupations[itemIndex]);
            SetButtonHighlight(buttonObj, true);
        }
        courtierManager.cost = courtierManager.selectedOccupations.Count * 50;
        courtierManager.UpdateSelectionState();
    }

    void SetButtonHighlight(GameObject buttonObj, bool isHighlighted)
    {
        Color color = isHighlighted ? ColorExtensions.FromHex("#E9D18E") : ColorExtensions.FromHex("#A9A9A9"); // Set your desired highlight color
        buttonObj.GetComponent<Image>().color = color;
    }

    // Method to set all buttons' highlight state
    public void SetAllButtonsHighlight(bool isHighlighted)
    {
        foreach (var buttonObj in instantiatedButtons)
        {
            SetButtonHighlight(buttonObj, isHighlighted);
        }
    }

    void clearPreviousButtons()
    {
        // Clear existing buttons
        foreach (Transform child in selectionContent)
        {
            if (child != selectionContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }
        instantiatedButtons.Clear();
        regionButtons.Clear();
    }

    public void receiveRData()
    {
        int tableSize = gameManager.exploredNations.Count;
        clearPreviousButtons();
        // Clear the list of region buttons
        regionButtons.Clear();
        buttonToNationIndexMap.Clear();

        selectionContent.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < tableSize; i++)
        {
            string region = gameManager.exploredNations[i];
            if (region != courtierManager.selectedCourtier.location)
            {
                GameObject gameObj = Instantiate(itemPrefab, selectionContent);
                gameObj.name = region;
                gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = region;

                // Capture the current index in a local variable
                int currentIndex = regionButtons.Count;
                buttonToNationIndexMap.Add(i);
                gameObj.GetComponent<Button>().onClick.AddListener(() => regionItemClicked(currentIndex));

                regionButtons.Add(gameObj); // Track instantiated region buttons
            }
        }

        itemPrefab.gameObject.SetActive(false);
    }

    void regionItemClicked(int itemIndex)
    {
        int nationIndex = buttonToNationIndexMap[itemIndex];
        courtierManager.selectedRegion = gameManager.exploredNations[nationIndex];
        SetAllRegionButtonsHighlight(false); // Reset all region buttons to non-highlighted state
        GameObject buttonObj = regionButtons[itemIndex]; // Use the correct button from the list
        SetButtonHighlight(buttonObj, true); // Highlight the selected region button
        courtierManager.cost = 50;
        courtierManager.UpdateSelectionState();
    }

    // Method to set all region buttons' highlight state
    public void SetAllRegionButtonsHighlight(bool isHighlighted)
    {
        foreach (var buttonObj in regionButtons)
        {
            SetButtonHighlight(buttonObj, isHighlighted);
        }
    }
}