using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{

    public GameManager gameManager;
    public DialogueManager dialogueManager;
    public Text moneyText;
    public Text reputationText;
    public Text yearText;

    public GameObject UIGoldInfoSet;
    public Text incomeText;
    public Text expenseText;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LateUpdate()
    {
        moneyText.text = gameManager.money.ToString();
        reputationText.text = gameManager.reputation.ToString();
        yearText.text = gameManager.year.ToString();
    }

    public void onGoldClick()
    {
        dialogueManager.openBlurImg();
        if (UIGoldInfoSet.activeSelf)
        { 
            UIGoldInfoSet.SetActive(false);
        }
        else
        {
            UIGoldInfoSet.SetActive(true);
            incomeText.text = gameManager.Income.ToString();
            expenseText.text = gameManager.Expense.ToString();
        }
    }

    public void onExitClick()
    {
        dialogueManager.closeAlert();
    }
}
