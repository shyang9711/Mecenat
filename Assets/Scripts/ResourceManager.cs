using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{

    public GameManager gameManager;
    public Text moneyText;
    public Text reputationText;
    public Text yearText;

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
}
