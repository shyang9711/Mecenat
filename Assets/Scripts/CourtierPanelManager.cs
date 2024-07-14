using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CourtierPanelManager : MonoBehaviour
{

    public GameManager gameManager;
    public CourtierManager courtierManager;

    public GameObject courtierItemPrefab;
    public Transform courtierContent;

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
        int tableSize = gameManager.hiredCourtiers.Count;

        courtierContent.GetChild(0).gameObject.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in courtierContent)
        {
            if (child != courtierContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }
        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {
            Courtier courtier = gameManager.hiredCourtiers[i];
            gameObj = Instantiate(courtierItemPrefab, courtierContent);
            gameObj.name = courtier.name;
            gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = courtier.name;
            gameObj.transform.GetChild(2).GetComponent<Text>().text = courtier.location;
            gameObj.transform.GetChild(4).GetComponent<Text>().text = courtier.action;

            gameObj.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }

        courtierItemPrefab.gameObject.SetActive(false);
    }

    void ItemClicked(int itemIndex)
    {
        Debug.Log("courtier " + gameManager.hiredCourtiers[itemIndex] + " selected.");
        courtierManager.onCourtierIndivClick(gameManager.hiredCourtiers[itemIndex]);
    }
}
