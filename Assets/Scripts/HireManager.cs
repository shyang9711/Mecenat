using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HireManager : MonoBehaviour
{

    public GameManager gameManager;
    public CourtierManager courtierManager;

    public GameObject hireItemPrefab;
    public Transform hireContent;
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
        int tableSize = gameManager.currCourtiers.Count;

        hireContent.GetChild(0).gameObject.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in hireContent)
        {
            if (child != hireContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }
        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {
            Courtier courtier = gameManager.currCourtiers[i];

            if (!gameManager.scoutRegion.Contains(courtier.nationality))
            {
                continue;
            }
            gameObj = Instantiate(hireItemPrefab, hireContent);
            gameObj.name = courtier.name;
            gameObj.transform.GetChild(0).GetComponent<Text>().text = courtier.name;
            gameObj.transform.GetChild(1).GetComponent<Text>().text = courtier.nationality;
            gameObj.transform.GetChild(2).GetComponent<Text>().text = courtier.salary.ToString();

            gameObj.GetComponent<Button>().AddEventListener(i, ItemClicked);
        }

        hireItemPrefab.gameObject.SetActive(false);
    }

    void ItemClicked(int itemIndex)
    {
        courtierManager.onHireIndivBtnClick(gameManager.currCourtiers[itemIndex]);
        courtierManager.courtierEventNum = 1;
    }
}
