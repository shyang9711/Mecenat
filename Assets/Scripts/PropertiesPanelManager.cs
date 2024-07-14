using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PropertiesPanelManager : MonoBehaviour
{
    public GameManager gameManager;
    public PropertiesManager propertiesManager;

    public Transform propertiesContent;
    public GameObject propertiesItemPrefab;
    public GameObject newConstructBtn;

    public GameObject residentialTree;
    public GameObject commercialTree;
    public GameObject culturalTree;
    public GameObject academicTree;
    public GameObject religiousTree;

    //Building buttons
    public GameObject casaBtn;
    public GameObject villaBtn;
    public GameObject courtyardBtn;
    public GameObject palaceBtn;
    public GameObject gardenBtn;

    public GameObject storeBtn;
    public GameObject officeBtn;
    public GameObject financialcenterBtn;
    public GameObject headquartersBtn;
    public GameObject foundationBtn;

    public GameObject exhibitionhallBtn;
    public GameObject galleryBtn;
    public GameObject concerthallBtn;
    public GameObject museumBtn;
    public GameObject expositionpavilionBtn;

    public GameObject workshopBtn;
    public GameObject academyBtn;
    public GameObject libraryBtn;
    public GameObject universityBtn;
    public GameObject techinstituteBtn;

    public GameObject templeBtn;
    public GameObject churchBtn;
    public GameObject chapelBtn;
    public GameObject cathedralBtn;
    public GameObject basilicaBtn;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        receiveData();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void receiveData()
    {
        int tableSize = gameManager.ownedLands.Count;

        propertiesContent.GetChild(0).gameObject.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in propertiesContent)
        {
            if (child != propertiesContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }

        if (tableSize > 0) {
            if (propertiesContent.childCount == 0) {
                return;
            }

            GameObject gameObj;
            for (int i = 0; i < tableSize; i++)
            {
                Land land = gameManager.ownedLands[i];
                gameObj = Instantiate(propertiesItemPrefab, propertiesContent);
                gameObj.name = land.streetNum + " " + land.streetName;
                gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = land.streetNum + " " + land.streetName;
                string buildingTypeText = string.IsNullOrEmpty(land.buildingType) ? "No Building" : land.buildingType;
                gameObj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = buildingTypeText;

                gameObj.GetComponent<Button>().AddEventListener(i, ItemClicked);
            }

            propertiesItemPrefab.gameObject.SetActive(false);
        }
    }


    void ItemClicked(int itemIndex)
    {
        propertiesManager.onPropertyItemClick(gameManager.ownedLands[itemIndex]);
    }

    public void checkBuildingType(Land item)
    {

        if (item.buildingType == "Residential")
        {
            newConstructBtn.SetActive(false);
            residentialTree.SetActive(true);
            culturalTree.SetActive(false);
            academicTree.SetActive(false);
        }
        else if (item.buildingType == "Commercial")
        {
            newConstructBtn.SetActive(false);
            commercialTree.SetActive(true);
        }
        else if (item.buildingType == "Cultural")
        {
            newConstructBtn.SetActive(false);
            residentialTree.SetActive(false);
            culturalTree.SetActive(true);
            academicTree.SetActive(false);
        }
        else if (item.buildingType == "Academic")
        {
            newConstructBtn.SetActive(false);
            residentialTree.SetActive(false);
            culturalTree.SetActive(false);
            academicTree.SetActive(true);
        }
        else if (item.buildingType == "Religious")
        {
            newConstructBtn.SetActive(false);
            religiousTree.SetActive(true);
        }
        else{
            newConstructBtn.SetActive(true);
            residentialTree.SetActive(false);
            culturalTree.SetActive(false);
            academicTree.SetActive(false);
        }

    }

    public void checkOwnedBuildings(Land item)
    {
        int buildingList = item.buildings.Count;
        for (int i = 0; i < buildingList; i++)
        {
            if (item.buildings[i].name == "Casa")
            {
                casaBtn.GetComponent<Button>().enabled = false;
                casaBtn.SetActive(true);
                villaBtn.SetActive(true);
            }
            if (item.buildings[i].name == "Villa")
            {
                villaBtn.GetComponent<Button>().enabled = false;
                courtyardBtn.SetActive(true);
                palaceBtn.SetActive(true);
            }
            if (item.buildings[i].name == "Courtyard")
            {
                courtyardBtn.GetComponent<Button>().enabled = false;
                gardenBtn.SetActive(true);
            }
            if (item.buildings[i].name == "Palace")
            {
                palaceBtn.GetComponent<Button>().enabled = false;
                gardenBtn.SetActive(true);
            }
            if (item.buildings[i].name == "Garden")
            {
                gardenBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Store")
            {
                storeBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Office")
            {
                officeBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Financial Center")
            {
                financialcenterBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Headquarters")
            {
                headquartersBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Foundation")
            {
                foundationBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Exhibition Hall")
            {
                exhibitionhallBtn.GetComponent<Button>().enabled = false;
                exhibitionhallBtn.SetActive(true);
                galleryBtn.SetActive(true);
            }
            if (item.buildings[i].name == "Gallery")
            {
                galleryBtn.GetComponent<Button>().enabled = false;
                concerthallBtn.SetActive(true);
                museumBtn.SetActive(true);
            }
            if (item.buildings[i].name == "Concert Hall")
            {
                concerthallBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Museum")
            {
                museumBtn.GetComponent<Button>().enabled = false;
                if (item.buildings.Any(building => building.name == "Concert Hall"))
                {
                    expositionpavilionBtn.SetActive(true);
                }
            }
            if (item.buildings[i].name == "Exposition Pavilion")
            {
                expositionpavilionBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Workshop")
            {
                workshopBtn.GetComponent<Button>().enabled = false;
                workshopBtn.SetActive(true);
                academyBtn.SetActive(true);
            }
            if (item.buildings[i].name == "Academy")
            {
                academyBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Library")
            {
                libraryBtn.GetComponent<Button>().enabled = false;
                if (item.buildings.Any(building => building.name == "Academy"))
                {
                    universityBtn.SetActive(true);
                }
            }
            if (item.buildings[i].name == "University")
            {
                universityBtn.GetComponent<Button>().enabled = false;
                techinstituteBtn.SetActive(true);
            }
            if (item.buildings[i].name == "Tech Institute")
            {
                techinstituteBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Temple")
            {
                templeBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Church")
            {
                churchBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Chapel")
            {
                chapelBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Cathedral")
            {
                cathedralBtn.GetComponent<Button>().enabled = false;
            }
            if (item.buildings[i].name == "Basilica")
            {
                basilicaBtn.GetComponent<Button>().enabled = false;
            }
        }
    }
}
