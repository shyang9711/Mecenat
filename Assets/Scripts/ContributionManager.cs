using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContributionManager : MonoBehaviour
{
    public GameManager gameManager;

    public Transform contributionContent;
    public GameObject contributionItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void receiveData()
    {
        int tableSize = gameManager.movementList.Count;

        contributionContent.GetChild(0).gameObject.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in contributionContent)
        {
            if (child != contributionContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }

        GameObject gameObj;
        for (int i = tableSize - 1; i >= 0; i--)
        {
            MovementEntry movementEntry = gameManager.movementList[i];
            gameObj = Instantiate(contributionItemPrefab, contributionContent);
            gameObj.name = movementEntry.movement;
            gameObj.transform.GetChild(0).GetComponent<Text>().text = movementEntry.movement;
            int userContribution = 0;
            if (movementEntry.reputationDict.ContainsKey(gameManager.playerId)) {
                userContribution = movementEntry.reputationDict[gameManager.playerId] / movementEntry.totalReputation;
            }
            gameObj.transform.GetChild(1).GetComponent<Text>().text = userContribution + "%";
        }

        contributionItemPrefab.gameObject.SetActive(false);
    }
}
