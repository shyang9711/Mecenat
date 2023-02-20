using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SelectLocationManager : MonoBehaviour
{
    public GameManager gameManager;
    public MainManager mainManager;
    public GameObject noPlaceText;
    public GameObject selectBtn;
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
        gameManager.streetName = "";
        int tableSize = gameManager.ownedLands.Count;
        GameObject indivBtn = transform.GetChild(0).gameObject;
        GameObject gameObj;
        int j = 0;
        for (int i = 0; i < tableSize; i++)
        {
            gameObj = Instantiate(indivBtn, transform);
            gameObj.name = gameManager.ownedLands[i].streetNum + " " + gameManager.ownedLands[i].streetName;
            gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = gameManager.ownedLands[i].streetNum + " " + gameManager.ownedLands[i].streetName;
            if (gameManager.ownedLands[i].oLWorks.Count >= gameManager.ownedLands[i].workSlots)
            {
                gameObj.SetActive(false);
                j++;
            }

            gameObj.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }
        Debug.Log(j + " / " + tableSize);
        if (j >= tableSize)
        {
            noPlaceText.SetActive(true);
            selectBtn.GetComponentInChildren<Text>().text = "Sell";
            mainManager.noPlaceEventNum++;
        }
        Destroy(indivBtn);
    }
    void ItemClicked(int itemIndex)
    {
        Debug.Log("item " + itemIndex + " cliked");
        gameManager.streetNum = gameManager.ownedLands[itemIndex].streetNum;
        gameManager.streetName = gameManager.ownedLands[itemIndex].streetName;
    }
}
