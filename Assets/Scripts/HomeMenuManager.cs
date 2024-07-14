using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class HomeMenuManager : MonoBehaviour
{
    public GameObject UICharacterSet;

    public GameManager gameManager;
    public InitializationManager initializationManager;

    [SerializeField] private InputField givenName;
    [SerializeField] private InputField houseName;
    public TMPro.TMP_Dropdown dropdownNationality;
    // public TMPro.TMP_Dropdown dropdownReligion;

    void Awake()
    {
        UICharacterSet.transform.DOScale(Vector3.zero, 0);
        nationalitySelector();
        // religionSelector();
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

    public void InitBtnState()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void nationalitySelector()
    {
       if (dropdownNationality.value == 0)
       {
           gameManager.residency = "United Kingdom";
       }
       if (dropdownNationality.value == 1)
       {
           gameManager.residency = "France";
       }
       if (dropdownNationality.value == 2)
       {
           gameManager.residency = "Germany";
       }
       if (dropdownNationality.value == 3)
       {
           gameManager.residency = "Italy";
       }
       if (dropdownNationality.value == 4)
       {
           gameManager.residency = "Spain";
       }
    }
    // public void religionSelector()
    // {
    //     if (dropdownReligion.value == 0)
    //     {
    //         gameManager.religion = "Catholic";
    //     }
    //     if (dropdownReligion.value == 1)
    //     {
    //         gameManager.religion = "Jewish";
    //     }
    //     if (dropdownReligion.value == 2)
    //     {
    //         gameManager.religion = "Muslim";
    //     }
    //     if (dropdownReligion.value == 3)
    //     {
    //         gameManager.religion = "Pagan";
    //     }
    //     if (dropdownReligion.value == 4)
    //     {
    //         gameManager.religion = "Non-religious";
    //     }
    // }

    public void onPlayBtnClick()
    {   
        UICharacterSet.transform.DOMove(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), 0.1f).SetEase(Ease.OutBack);
        UICharacterSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

    }

    public void onStartBtnClick()
    {
        gameManager.givenName = givenName.text.ToString();
        gameManager.surname = houseName.text.ToString();
        initializationManager.InitializeGame();
        Invoke("MainScene", 0.2f);
    }

    public void ExitBtnClick()
    {
        if (UICharacterSet.transform.localScale == Vector3.one)
            UICharacterSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);

    }

    void MainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
