using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertiesPanelManager : MonoBehaviour
{
    public GameManager gameManager;
    public PropertiesManager propertiesManager;

    public Text infoName;
    public Text infoCity;
    public Text infoOwnedYears;
    public Text infoValue;
    public Text infoTax;
    public Text infoReputation;
    public Text infoCollections;
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
    public GameObject rline1;
    public GameObject rline2;
    public GameObject rline3;
    public GameObject rline4;
    public GameObject rline5;

    public GameObject storeBtn;
    public GameObject officeBtn;
    public GameObject financialcenterBtn;
    public GameObject headquartersBtn;
    public GameObject foundationBtn;
    public GameObject cline1;
    public GameObject cline2;
    public GameObject cline3;
    public GameObject cline4;   

    public GameObject galleryBtn;
    public GameObject concerthallBtn;
    public GameObject museumBtn;
    public GameObject clline1;
    public GameObject clline2;

    public GameObject workshopBtn;
    public GameObject academyBtn;
    public GameObject libraryBtn;
    public GameObject universityBtn;
    public GameObject techinstituteBtn;
    public GameObject aline1;
    public GameObject aline2;
    public GameObject aline3;
    public GameObject aline4;
    public GameObject aline5;

    public GameObject templeBtn;
    public GameObject churchBtn;
    public GameObject chapelBtn;
    public GameObject cathedralBtn;
    public GameObject basilicaBtn;
    public GameObject rlline1;
    public GameObject rlline2;
    public GameObject rlline3;
    public GameObject rlline4;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        receiveLData();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void receiveLData()
    {
        int tableSize = gameManager.ownedLands.Count;

        GameObject indivBtn = transform.GetChild(0).gameObject;
        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {

            gameObj = Instantiate(indivBtn, transform);
            gameObj.name = gameManager.ownedLands[i].streetNum + " " + gameManager.ownedLands[i].streetName;
            gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = gameManager.ownedLands[i].streetNum + " " + gameManager.ownedLands[i].streetName;
            gameObj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = gameManager.ownedLands[i].buildingType;

            gameObj.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }

        Destroy(indivBtn);
    }


    void ItemClicked(int itemIndex)
    {
        Debug.Log("item " + itemIndex + " cliked");
        gameManager.streetNum = gameManager.ownedLands[itemIndex].streetNum;
        gameManager.streetName = gameManager.ownedLands[itemIndex].streetName;
        int ownedY = gameManager.year - gameManager.ownedLands[itemIndex].year;
        infoName.text = gameManager.ownedLands[itemIndex].streetNum + " " + gameManager.ownedLands[itemIndex].streetName;
        infoCity.text = gameManager.ownedLands[itemIndex].city;
        infoOwnedYears.text = ownedY.ToString();
        infoValue.text = gameManager.ownedLands[itemIndex].price.ToString();
        infoTax.text = "15%";
        if (gameManager.ownedLands[itemIndex].price / 1000 * (1 + (ownedY / 100)) > 10)
        {
            infoReputation.text = "10";
        }
        else
        {
            infoReputation.text = (gameManager.ownedLands[itemIndex].price / 1000 * (1 + (ownedY / 100))).ToString();
        }
        int count = gameManager.ownedLands[itemIndex].oLWorks.Count;
        int slots = gameManager.ownedLands[itemIndex].workSlots;
        infoCollections.text = count + " / " + slots;

        checkBuildingType(gameManager.ownedLands[itemIndex]);
        checkOwnedBuildings(gameManager.ownedLands[itemIndex]);

        propertiesManager.onOwnedLandBtnClick();
    }

    public void checkBuildingType(GameManager.OwnedLands item)
    {

        if (item.buildingType == "Residential")
        {
            newConstructBtn.SetActive(false);
            residentialTree.SetActive(true);
            casaBtn.SetActive(true);
        }
        if (item.buildingType == "Commercial")
        {
            newConstructBtn.SetActive(false);
            commercialTree.SetActive(true);
            storeBtn.SetActive(true);
        }
        if (item.buildingType == "Cultural")
        {
            newConstructBtn.SetActive(false);
            culturalTree.SetActive(true);
            galleryBtn.SetActive(true);
        }
        if (item.buildingType == "Academic")
        {
            newConstructBtn.SetActive(false);
            academicTree.SetActive(true);
            workshopBtn.SetActive(true);
        }
        if (item.buildingType == "Religious")
        {
            newConstructBtn.SetActive(false);
            religiousTree.SetActive(true);
            templeBtn.SetActive(true);
        }

    }

    public void checkOwnedBuildings(GameManager.OwnedLands item)
    {
        int buildingList = item.oBuildings.Count;
        for (int i = 0; i < buildingList; i++)
        {
            Debug.Log(item.oBuildings[i].name);
            if (item.oBuildings[i].name == "Casa")
            {
                casaBtn.GetComponent<Button>().enabled = false;
                villaBtn.SetActive(true);
                rline1.SetActive(true);
            }
            if (item.oBuildings[i].name == "Villa")
            {
                villaBtn.GetComponent<Button>().enabled = false;
                courtyardBtn.SetActive(true);
                palaceBtn.SetActive(true);
                rline2.SetActive(true);
                rline4.SetActive(true);
            }
            if (item.oBuildings[i].name == "Courtyard")
            {
                courtyardBtn.GetComponent<Button>().enabled = false;
                if ( i + 1 < buildingList)
                {
                    if (item.oBuildings[i + 1].name == "Palace")
                    {
                        rline3.SetActive(true);
                        rline5.SetActive(true);
                        gardenBtn.SetActive(true);
                    }
                }
            }
            if (item.oBuildings[i].name == "Palace")
            {
                palaceBtn.GetComponent<Button>().enabled = false;
                if (i + 1 < buildingList)
                {
                    if (item.oBuildings[i + 1].name == "Courtyard")
                    {
                        rline3.SetActive(true);
                        rline5.SetActive(true);
                        gardenBtn.SetActive(true);
                    }
                }
            }
            if (item.oBuildings[i].name == "Garden")
            {
                gardenBtn.GetComponent<Button>().enabled = false;
            }
            if (item.oBuildings[i].name == "Store")
            {
                storeBtn.GetComponent<Button>().enabled = false;
                officeBtn.SetActive(true);
                cline1.SetActive(true);
            }
            if (item.oBuildings[i].name == "Office")
            {
                officeBtn.GetComponent<Button>().enabled = false;
                financialcenterBtn.SetActive(true);
                cline2.SetActive(true);
            }
            if (item.oBuildings[i].name == "Financial Center")
            {
                financialcenterBtn.GetComponent<Button>().enabled = false;
                headquartersBtn.SetActive(true);
                cline3.SetActive(true);
            }
            if (item.oBuildings[i].name == "Headquarters")
            {
                headquartersBtn.GetComponent<Button>().enabled = false;
                foundationBtn.SetActive(true);
                cline4.SetActive(true);
            }
            if (item.oBuildings[i].name == "Foundation")
            {
                foundationBtn.GetComponent<Button>().enabled = false;
            }
            if (item.oBuildings[i].name == "Gallery")
            {
                galleryBtn.GetComponent<Button>().enabled = false;
                concerthallBtn.SetActive(true);
                museumBtn.SetActive(true);
                clline1.SetActive(true);
                clline2.SetActive(true);
            }
            if (item.oBuildings[i].name == "Concert Hall")
            {
                concerthallBtn.GetComponent<Button>().enabled = false;
            }
            if (item.oBuildings[i].name == "Museum")
            {
                museumBtn.GetComponent<Button>().enabled = false;
            }
            if (item.oBuildings[i].name == "Workshop")
            {
                workshopBtn.GetComponent<Button>().enabled = false;
                academyBtn.SetActive(true);
                libraryBtn.SetActive(true);
                aline1.SetActive(true);
                aline2.SetActive(true);
            }
            if (item.oBuildings[i].name == "Academy")
            {

                academyBtn.GetComponent<Button>().enabled = false;
                if (i + 1 < buildingList)
                {
                    if (item.oBuildings[i + 1].name == "Library")
                    {
                        aline3.SetActive(true);
                        aline4.SetActive(true);
                        universityBtn.SetActive(true);
                    }
                }
            }
            if (item.oBuildings[i].name == "Library")
            {
                libraryBtn.GetComponent<Button>().enabled = false;
                if (i + 1 < buildingList)
                {
                    if (item.oBuildings[i + 1].name == "Academy")
                    {
                        aline3.SetActive(true);
                        aline4.SetActive(true);
                        universityBtn.SetActive(true);
                    }
                }
            }
            if (item.oBuildings[i].name == "University")
            {
                universityBtn.GetComponent<Button>().enabled = false;
                techinstituteBtn.SetActive(true);
                aline5.SetActive(true);
            }
            if (item.oBuildings[i].name == "Tech Institute")
            {
                techinstituteBtn.GetComponent<Button>().enabled = false;
            }
            if (item.oBuildings[i].name == "Temple")
            {
                templeBtn.GetComponent<Button>().enabled = false;
                churchBtn.SetActive(true);
                rlline1.SetActive(true);
            }
            if (item.oBuildings[i].name == "Church")
            {
                churchBtn.GetComponent<Button>().enabled = false;
                chapelBtn.SetActive(true);
                rlline2.SetActive(true);
            }
            if (item.oBuildings[i].name == "Chapel")
            {
                chapelBtn.GetComponent<Button>().enabled = false;
                cathedralBtn.SetActive(true);
                rlline3.SetActive(true);
            }
            if (item.oBuildings[i].name == "Cathedral")
            {
                cathedralBtn.GetComponent<Button>().enabled = false;
                basilicaBtn.SetActive(true);
                rlline4.SetActive(true);
            }
            if (item.oBuildings[i].name == "Basilica")
            {
                basilicaBtn.GetComponent<Button>().enabled = false;
            }
        }
    }
}
