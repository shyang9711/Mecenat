using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CourtierManager : MonoBehaviour
{

    public GameManager gameManager;
    public HireManager hireManager;
    public CourtierPanelManager courtierPanelManager;
    public CourtierSelectionManager courtierSelectionManager;
    public DialogueManager dialogueManager;

    public GameObject hireBtn;
    public Button selectBtn;

    public GameObject UIHireSet;
    public GameObject UIActionSet;
    public Text actionName;
    public Text actionLocation;
    public Text actionAction;
    public Button scoutBtn;
    public Button fireBtn;
    public GameObject UIMultiSelectSet;

    public Courtier selectedCourtier;

    public int courtierEventNum;
    public string selectionData;
    public List<string> selectedOccupations = new List<string>();
    public string selectedRegion;
    public int cost = 0;
    public Text costText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        dialogueManager.closeAlert();
        UpdateHireButtonState();
        UpdateSelectionState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onNewHireBtnClick()
    {
        if (gameManager.hiredCourtiers.Count < 10)
        {
            dialogueManager.openBlurImg();
            UIHireSet.transform.DOMove(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), 0.1f).SetEase(Ease.OutBack);
            UIHireSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }

    public void onExitBtnClick()
    {
        dialogueManager.closeAlert();
    }

    public void onHireIndivBtnClick(Courtier courtier)
    {
        selectedCourtier = courtier;
        UIHireSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        dialogueManager.hireCourtier(selectedCourtier);
    }

    public void onCourtierIndivClick(Courtier courtier)
    {
        selectedCourtier = courtier;
        actionName.text = courtier.name;
        actionLocation.text = courtier.location;
        actionAction.text = courtier.action;
        scoutBtn.interactable = courtier.inAction == 0;
        fireBtn.interactable = courtier.inAction == 0;
        dialogueManager.openBlurImg();
        UIActionSet.SetActive(true);
    }

    public void onPosBtnClick()
    {
        dialogueManager.closeAlert();
        switch (courtierEventNum)
        {
            // Hire Courtier
            case 1:
                dialogueManager.hirePos();
                selectedCourtier.action = "None";
                gameManager.hiredCourtiers.Add(selectedCourtier);
                gameManager.currCourtiers.Remove(selectedCourtier);
                gameManager.UpdateSalary(selectedCourtier.salary, true);
                break;
            // Move Courtier
            case 2:
                dialogueManager.courtierMovePos();
                gameManager.money -= cost;
                selectedCourtier.newLocation = selectedRegion;
                selectedCourtier.action = "Moving";
                selectedCourtier.inAction = 1;
                break;

            // Courtier Action Scout
            case 3:
                dialogueManager.courtierScoutPos();
                gameManager.money -= cost;
                selectedCourtier.scoutOccupation = selectedOccupations;
                selectedCourtier.action = "Preparing to Scout";
                selectedCourtier.inAction = 3;
                break;


            // Courtier Action Fire
            case 4:
                dialogueManager.firePos();
                gameManager.UpdateSalary(selectedCourtier.salary, false);
                gameManager.hiredCourtiers.Remove(selectedCourtier);
                break;

        }
    }

    public void onNegBtnClick()
    {
        dialogueManager.closeAlert();
        switch (courtierEventNum)
        {
            // Hire Courtier
            case 1:
                dialogueManager.hireNeg();
                break;
            // Move Courtier
            case 2:
                dialogueManager.courtierMoveNeg();
                selectedRegion = "";
                break;

            // Courtier Action Scout
            case 3:
                dialogueManager.courtierScoutNeg();
                selectedOccupations.Clear();
                break;
            

            // Courtier Action Fire
            case 4:
                dialogueManager.fireNeg();
                break;

        }
    }

    public void onOKBtnClick()
    {
        courtierEventNum = 0;
        selectionData = "";
        selectedCourtier = null;
        cost = 0;
        dialogueManager.closeAlert();
        hireManager.receiveData();
        courtierPanelManager.receiveData();
        UpdateHireButtonState();
    }

    public void onMoveBtnClick()
    {
        courtierEventNum = 2;
        selectionData = "Region";
        courtierSelectionManager.receiveRData();
        dialogueManager.closeAlert();
        dialogueManager.openBlurImg();
        UIMultiSelectSet.SetActive(true);
    }

    public void onScoutBtnClick()
    {
        courtierEventNum = 3;
        selectionData = "Occupation";
        courtierSelectionManager.receiveOData();
        dialogueManager.closeAlert();
        dialogueManager.openBlurImg();
        UIMultiSelectSet.SetActive(true);
    }

    public void onFireBtnClick()
    {
        courtierEventNum = 4;
        dialogueManager.closeAlert();
        dialogueManager.fireCourtier(selectedCourtier);
    }

    public void onSelectBtnClick()
    {
        dialogueManager.closeAlert();
        if (selectionData == "Region")
        {
            courtierEventNum = 2;
            dialogueManager.courtierMove(selectedCourtier, selectedRegion);
        }
        else if (selectionData == "Occupation")
        {
            courtierEventNum = 3;
            dialogueManager.courtierScout(selectedCourtier, selectedOccupations);
        }
    }

    public void onCancelBtnClick()
    {
        courtierEventNum = 0;
        selectionData = "";
        selectedOccupations.Clear();
        selectedRegion = "";
        dialogueManager.closeAlert();
    }
    public void onBackgroundReset()
    {
        selectedRegion = ""; // Reset the selected land
        if (selectedOccupations.Count == 0)
        {
            cost = 0;
        }
        courtierSelectionManager.SetAllRegionButtonsHighlight(false);
        UpdateSelectionState(); // Update the button state
    }

    // Method to update the select button state
    public void UpdateSelectionState()
    {
        costText.text = cost.ToString();
        selectBtn.interactable = !string.IsNullOrEmpty(selectedRegion) || selectedOccupations.Count > 0;
    }

    // Method to update the select button state
    private void UpdateHireButtonState()
    {
        hireBtn.SetActive(true);
        if (gameManager.hiredCourtiers.Count >= 10)
        {
            hireBtn.SetActive(false);
        }
    }

    public void onMeBtnClick()
    {
        Invoke("MainScene", 0.2f);
    }

    void MainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
