using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public static class ButtonExtension
{
    public static void AddEventListener<T> (this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate ()
        {
            OnClick(param);
        });
    }
}

public class ClientManager : MonoBehaviour
{
    public ProtegeManager protegeManager;
    public GameManager gameManager;

    public Transform clientContent;
    public GameObject clientItemPrefab;

    //Info UI
    public Text infoName;
    public Text infoOccupation;
    public Text infoAge;
    public Text infoCV;
    public Text infoWage;
    public Text infoReputation;
    public Text infoCreations;

    //Transfer UI
    public Text transferCV;
    public Text transferWage;
    
    //Alert UI
    public GameObject UIAlertSet;

    //Filter UI
    public GameObject filterSet;
    public Dropdown occupationDropdown;
    // public Dropdown affiliationDropdown;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        // Populate dropdowns with unique values from clients list
        PopulateDropdowns();
        // Add listener for dropdowns
        occupationDropdown.onValueChanged.AddListener(delegate { FilterClients(); });
        // affiliationDropdown.onValueChanged.AddListener(delegate { FilterClients(); });
        
        UIAlertSet.transform.DOScale(Vector3.zero, 0);

    }
    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }

    public void onFilterButtonClick()
    {
        filterSet.SetActive(true);
    }

    public void onCloseButtonClick()
    {
        filterSet.SetActive(false);
    }
    public void receivePData()
    {
        int tableSize = gameManager.protegeList.Count;
        clientContent.GetChild(0).gameObject.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in clientContent)
        {
            if (child != clientContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }

        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {
            Client client = gameManager.protegeList[i];

            gameObj = Instantiate(clientItemPrefab, clientContent);
            gameObj.name = client.name;
            gameObj.transform.GetChild(0).GetComponent<Text>().text = client.name;
            gameObj.transform.GetChild(1).GetComponent<Text>().text = client.occupation;
            gameObj.transform.GetChild(2).GetComponent<Text>().text = client.age.ToString();
            gameObj.transform.GetChild(3).GetComponent<Text>().text = (client.ability / 30).ToString();
            gameObj.transform.GetChild(4).GetComponent<Text>().text = client.value.ToString();
            gameObj.transform.GetChild(5).GetComponent<Text>().text = client.affiliation;

            gameObj.GetComponent<Button>().AddEventListener(i, ProtegeItemClicked);
        }

        clientContent.GetChild(0).gameObject.SetActive(false);
    }

    void ProtegeItemClicked(int itemIndex)
    {
        Debug.Log("item " + itemIndex + " cliked");
        Client client = gameManager.protegeList[itemIndex];
        infoName.text = client.name;
        infoOccupation.text = client.occupation;
        infoAge.text = client.age.ToString();
        infoCV.text = client.value.ToString();
        infoWage.text = client.wage.ToString();
        infoReputation.text = client.potential.ToString();
        infoCreations.text = "0";

        transferCV.text = client.value.ToString();
        transferWage.text = client.wage.ToString();

        protegeManager.onIndividualBtnClick(client);
    }

    public void receiveCData()
    {
        string selectedOccupation = occupationDropdown.options[occupationDropdown.value].text;
        // string selectedAffiliation = affiliationDropdown.options[affiliationDropdown.value].text;

        int tableSize = gameManager.currClients.Count;

        clientContent.GetChild(0).gameObject.SetActive(true);

        // Clear existing buttons
        foreach (Transform child in clientContent)
        {
            if (child != clientContent.GetChild(0))
            {
                Destroy(child.gameObject);
            }
        }

        GameObject gameObj;
        for (int i = 0; i < tableSize; i++)
        {
            Client client = gameManager.currClients[i];

            if (selectedOccupation != "All" && client.occupation != selectedOccupation)
            {
                continue;
            }

            var occupationsForResidency = GetOccupationsForResidency(client.residency);
            if (!gameManager.scoutRegion.Contains(client.residency) || 
                occupationsForResidency == null || 
                !occupationsForResidency.Contains(client.occupation))
            {
                continue;
            }

            gameObj = Instantiate(clientItemPrefab, clientContent);
            gameObj.name = client.name;
            gameObj.transform.GetChild(0).GetComponent<Text>().text = client.name;
            gameObj.transform.GetChild(1).GetComponent<Text>().text = client.occupation;
            gameObj.transform.GetChild(2).GetComponent<Text>().text = client.age.ToString();
            gameObj.transform.GetChild(3).GetComponent<Text>().text = (client.ability / 30).ToString();
            gameObj.transform.GetChild(4).GetComponent<Text>().text = client.value.ToString();
            gameObj.transform.GetChild(5).GetComponent<Text>().text = client.affiliation;

            gameObj.GetComponent<Button>().AddEventListener(i, ClientItemClicked);
        }

        clientContent.GetChild(0).gameObject.SetActive(false);
    }

    public List<string> GetOccupationsForResidency(string residency)
    {
        foreach (var entry in gameManager.scoutOccupation)
        {
            if (entry.Key == residency)
            {
                return entry.Value;
            }
        }
        return null;
    }

    void ClientItemClicked(int itemIndex)
    {
        Debug.Log("item " + itemIndex + " cliked");
        Client client = gameManager.currClients[itemIndex];
        infoName.text = client.name;
        infoOccupation.text = client.occupation;
        infoAge.text = client.age.ToString();
        infoCV.text = client.value.ToString();
        infoWage.text = client.wage.ToString();
        infoReputation.text = (client.ability / 30).ToString();
        infoCreations.text = "0";

        transferCV.text = client.value.ToString();
        transferWage.text = client.wage.ToString();

        protegeManager.onIndividualBtnClick(client);
    }

    void FilterClients()
    {
        // Clear existing buttons
        foreach (Transform child in clientContent)
        {
            if (child != clientContent.GetChild(0)) // Keep the original template button
            {
                Destroy(child.gameObject);
            }
        }
        occupationDropdown.RefreshShownValue();

        // Re-populate the list with filtered clients
        if (protegeManager.isProtege)
        {
            receivePData();
        }
        else {
            receiveCData();
        }
    }


    void PopulateDropdowns()
    {
        List<string> occupations = new List<string> { "All" };
        // List<string> affiliations = new List<string> { "All" };

        foreach (var client in gameManager.currClients)
        {
            if (!occupations.Contains(client.occupation))
                occupations.Add(client.occupation);
            // if (!affiliations.Contains(client.affiliation))
            //     affiliations.Add(client.affiliation);
        }

        occupationDropdown.AddOptions(occupations);
        // affiliationDropdown.AddOptions(affiliations);
    }

    public void onAlertBtnClick()
    {
        if (UIAlertSet.transform.localScale == Vector3.one)
            UIAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);

    }

    public void onMeBtnClick()
    {
        Invoke("MainScene", 0.5f);
    }


}
