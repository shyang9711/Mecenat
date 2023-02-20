using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{
    public GameManager gameManager;
    public PropertiesManager propertiesManager;
    public GameObject nameText;
    public GameObject yearText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void receiveCData(GameManager.OwnedLands selectedLand)
    {
        int tableSize = selectedLand.oLWorks.Count;
        GameObject indivBtn = transform.GetChild(0).gameObject;
        GameObject gameObj;
        for (int j = 0; j < tableSize; j++)
        {
            gameObj = Instantiate(indivBtn, transform);
            gameObj.name = selectedLand.oLWorks[j].Name;
            gameObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = selectedLand.oLWorks[j].Name;
            gameObj.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = selectedLand.oLWorks[j].CreatorName;
            gameObj.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = selectedLand.oLWorks[j].Year.ToString();
        }
        indivBtn.SetActive(false);
    }
}
