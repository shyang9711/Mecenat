using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LandManager : MonoBehaviour
{

    public GameManager gameManager;
    public PropertiesManager propertiesManager;

    public Text landName;
    public Text landCity;
    public Text landPrice;

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

    void receiveLData()
    {
        int tableSize = gameManager.currLands.Count;

        GameObject indivBtn = transform.GetChild(0).gameObject;
        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {

            gameObj = Instantiate(indivBtn, transform);
            gameObj.name = gameManager.currLands[i].streetNum + " " + gameManager.currLands[i].streetName;
            gameObj.transform.GetChild(0).GetComponent<Text>().text = gameManager.currLands[i].streetNum + " " + gameManager.currLands[i].streetName;
            gameObj.transform.GetChild(1).GetComponent<Text>().text = gameManager.currLands[i].city;
            gameObj.transform.GetChild(2).GetComponent<Text>().text = gameManager.currLands[i].price.ToString();

            gameObj.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }

        Destroy(indivBtn);
    }

    void ItemClicked(int itemIndex)
    {
        gameManager.streetNum = gameManager.currLands[itemIndex].streetNum;
        gameManager.streetName = gameManager.currLands[itemIndex].streetName;
        gameManager.city = gameManager.currLands[itemIndex].city;
        gameManager.landPrice = gameManager.currLands[itemIndex].price;
        propertiesManager.onIndividualBtnClick();

    }
}
