using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLocationManager : MonoBehaviour
{
    public GameManager gameManager;
    public MainManager mainManager;

    public Transform locationContent;

    public Land selectedLand;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        selectedLand = null;
        mainManager.UpdateSelectButtonState();
        receiveLData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void receiveLData()
    {
        int tableSize = gameManager.ownedLands.Count;

        GameObject indivBtn = locationContent.GetChild(0).gameObject;
        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {

            Land land = gameManager.ownedLands[i];
            if (land.workSlots > land.works.Count)
            {
                gameObj = Instantiate(indivBtn, locationContent);
                gameObj.name = land.streetNum + " " + land.streetName;
                gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = land.streetNum + " " + land.streetName;

                gameObj.GetComponent<Button>().AddEventListener(i, ItemClicked);
            }
        }

        Destroy(indivBtn);
    }

    void ItemClicked(int itemIndex)
    {
        Debug.Log("item " + itemIndex + " cliked");
        selectedLand = gameManager.ownedLands[itemIndex];
        mainManager.UpdateSelectButtonState();
    }
}
