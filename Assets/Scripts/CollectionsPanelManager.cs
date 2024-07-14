using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionsPanelManager : MonoBehaviour
{
    public GameManager gameManager;
    public PropertiesManager propertiesManager;

    public GameObject workItemPrefab;
    public Transform collectionContent;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void receiveData()
    {
        // List<Transform> children = new List<Transform>();
        // foreach (Transform child in workContent)
        // {
        //     children.Add(child);
        // }
        foreach (Transform child in collectionContent)
        {
            if (child.name != "Works Panel") {
                Destroy(child.gameObject);
            }
        }
        Land selectedLand = propertiesManager.selectedLand;
        List<Work> works = selectedLand.works;
        int tableSize = works.Count;

        if (tableSize > 0)
        {
            // GameObject indivBtn = transform.GetChild(0).gameObject;
            // GameObject gameObj;

            for (int i = 0; i < tableSize; i++)
            {
                Work work = works[i];

                // Instantiate work item and set its details
                GameObject gameObj = Instantiate(workItemPrefab, collectionContent);
                gameObj.name = work.name;
                gameObj.transform.GetChild(1).GetComponent<Text>().text = work.creator;
                gameObj.transform.GetChild(2).GetComponent<Text>().text = work.year.ToString();
                gameObj.transform.GetChild(3).GetComponent<Text>().text = work.name;

                // Add click listener
                int index = i; // Capture index for use in the lambda
                gameObj.GetComponent<Button>().onClick.AddListener(() => ItemClicked(index));
            }
        }
        collectionContent.GetChild(0).gameObject.SetActive(false);
    }


    void ItemClicked(int itemIndex)
    {
        propertiesManager.selectedWork = propertiesManager.selectedLand.works[itemIndex];
        propertiesManager.onViewWorkClick(propertiesManager.selectedWork);
        StartCoroutine(ActivateWorkPanel());
    }

    private IEnumerator ActivateWorkPanel()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(1f);

        // Set the game object active
        collectionContent.GetChild(0).gameObject.SetActive(true);
    }
}
