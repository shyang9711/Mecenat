using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LandManager : MonoBehaviour
{

    public GameManager gameManager;
    public PropertiesManager propertiesManager;

    public Transform landContent;
    public GameObject landItemPrefab;

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
        int tableSize = gameManager.currLands.Count;

        landContent.GetChild(0).gameObject.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in landContent)
        {
            if (child != landContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }

        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {

            gameObj = Instantiate(landItemPrefab, landContent);
            Land land = gameManager.currLands[i];
            gameObj.name = land.streetNum + " " + land.streetName;
            gameObj.transform.GetChild(0).GetComponent<Text>().text = land.streetNum + " " + land.streetName;
            gameObj.transform.GetChild(1).GetComponent<Text>().text = land.city;
            gameObj.transform.GetChild(2).GetComponent<Text>().text = land.price.ToString();

            gameObj.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }

        landItemPrefab.gameObject.SetActive(false);
    }

    void ItemClicked(int itemIndex)
    {
        propertiesManager.onLandBtnClick(gameManager.currLands[itemIndex]);

    }
}
