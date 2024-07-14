using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class DialogueManager : MonoBehaviour
{
    public GameManager gameManager;

    public Button MainButton;

    public GameObject UIBlurImg;

    public GameObject UIAlertSet;
    public Text alertName;
    public Text alertDesc;

    public GameObject UImAlertSet;
    public GameObject UImNext;
    public Text mAlertName;
    public Text mAlertDesc;
    public Text mAlertPos;
    public Text mAlertNeg;
    public GameObject UIWorkSet;
    public GameObject UIHireSet;
    public GameObject UIActionSet;
    public GameObject UIMultiSelectSet;
    public GameObject UIInfoSet;
    public GameObject UITransferSet;
    public GameObject UIPropertyMarketSet;
    public GameObject UICollectionSet;
    public GameObject UIWorkInfoSet;
    public GameObject UIConstructionSet;
    public GameObject UINewShipSet;
    public GameObject UIRenameSet;
    public GameObject UIGoldInfoSet;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //alert
    public void notEnoughMoney()
    {
        alertName.text = "Not Enough Money";
        alertDesc.text = "You do not have the amount of money you entered and your bluffing did not work.";
        openAlert();
    }

    public void offerSent(string name)
    {
        alertName.text = "Offer Sent";
        alertDesc.text = "You have successfully sent an offer to " + name;
        openAlert();
    }

    public void offerInsufficientMoney(string name)
    {
        alertName.text = "Broke";
        alertDesc.text = "You are not able to pay " + name + " the amount of money you have offered.";

        openAlert();
    }

    public void rejectionPos()
    {
        alertName.text = "Offer Rejected";
        alertDesc.text = "You accepted the rejection.";

        openAlert();
    }

    public void rejectionNeg()
    {
        alertName.text = "I Reject Your Rejection";
        alertDesc.text = "You raged, but nothing has changed. Your offer was rejected.";

        openAlert();
    }

    public void dealPos(int value)
    {
        alertName.text = "Done Deal";
        alertDesc.text = $"You have reached a deal. You paid {value}";

        openAlert();
    }

    public void dealNeg()
    {
        alertName.text = "Second Thought";
        alertDesc.text = "You decided not to continue with the deal.";

        openAlert();
    }

    public void funeralPos(string name)
    {
        alertName.text = "Live and Let Die";
        alertDesc.text = $"You have paid respects to {name}. You paid 10 golds";

        openAlert();
    }

    public void funeralNeg(string name)
    {
        alertName.text = "I Should See Dead People?";
        alertDesc.text = $"You did not attend {name}'s funeral. People started gossiping about you";

        openAlert();
    }

    public void workPos()
    {
        alertName.text = "This is a masterpiece";
        alertDesc.text = "You decided to keep the work.";

        openAlert();
    }

    public void workNeg(int price)
    {
        alertName.text = "We Are Not Amused";
        alertDesc.text = $"You decided to not to keep the artwork and sold the heart and souls of your client. The artwork was auctioned off at the price of {price}. You gained {price}";

        openAlert();
    }

    public void landPos()
    {
        alertName.text = "Mazel Tov";
        alertDesc.text = "You now own a place, one step closer to Jews.";

        openAlert();
    }

    public void landInsufficientMoney()
    {
        alertName.text = "You Are Not Nigerian Prince";
        alertDesc.text = "You tried to scam the seller but failed.";

        openAlert();
    }

    public void landNeg()
    {
        alertName.text = "Simon Says 'Run Away'";
        alertDesc.text = "You ran away before signing the contract.";

        openAlert();
    }

    public void hirePos()
    {
        alertName.text = "Here's Looking At You Kid";
        alertDesc.text = "You have hired a new courtier.";

        openAlert();
    }

    public void hireNeg()
    {
        alertName.text = "Important Message";
        alertDesc.text = "Unfortunately, we have decided to pursue other candidates that are stronger match.\n\nRegards,\nDoNotReply";

        openAlert();
    }
    public void firePos()
    {
        alertName.text = "You're Fired!";
        alertDesc.text = "You have fired your courtier and now needs to look for a job for the family.";

        openAlert();
    }

    public void fireNeg()
    {
        alertName.text = "My Mistake";
        alertDesc.text = "You told your courtier to get back to work.";

        openAlert();
    }

    public void courtierMovePos()
    {
        alertName.text = "Pawn to C3";
        alertDesc.text = "You placed your courtier to a new location.";

        openAlert();
    }

    public void courtierMoveNeg()
    {
        alertName.text = "S.T.A.Y.";
        alertDesc.text = "You told your courtier to stay in spot.";

        openAlert();
    }

    public void courtierScoutPos()
    {
        alertName.text = "Moneyball";
        alertDesc.text = "You assigned your courtier to scout for new talent.";

        openAlert();
    }

    public void courtierScoutNeg()
    {
        alertName.text = "No Scout November";
        alertDesc.text = "You did not assign your courtier to scout the region.";

        openAlert();
    }

    public void addBuildingPos()
    {
        alertName.text = "Another Great Erection";
        alertDesc.text = "You have constructed a new buiding.";

        openAlert();
    }

    public void addBuildingNeg()
    {
        alertName.text = "Postpone the Plan";
        alertDesc.text = "Your grand plan did not match the numbers of stone blocks and pickaxes needed. You ordered the workers to postpone it.";

        openAlert();
    }

    public void newShipCompleted(string shipName)
    {
        alertName.text = "Ships Ahoy";
        alertDesc.text = "Your commissioned ship " + shipName + " has been completed and is ready to sail.";

        openAlert();
    }

    public void discoverEtemenanki()
    {
        alertName.text = "Tower of Babel";
        alertDesc.text = "Come, let us build ourselves a city and a tower with its top in the heavens, and let us make a name for ourselves. You have discovered ancient architecture Etamenanki.";

        openAlert();
    }

    public void shipInsufficientMoney()
    {
        alertName.text = "Not Enough Money";
        alertDesc.text = "You tried to scam the seller but failed.";

        openAlert();
    }

    public void buildShipPos()
    {
        alertName.text = "Il est temps! levons l'ancre!";
        alertDesc.text = "You have commissioned to build a new ship.";

        openAlert();
    }

    public void buildShipNeg()
    {
        alertName.text = "Next Time";
        alertDesc.text = "You decided not to build a new ship.";

        openAlert();
    }

    public void buyShipPos()
    {
        alertName.text = "Il est temps! levons l'ancre!";
        alertDesc.text = "You have bought a new ship.";

        openAlert();
    }

    public void buyShipNeg()
    {
        alertName.text = "Next Time";
        alertDesc.text = "You decided not to buy a new ship.";

        openAlert();
    }

    public void moveShipPos()
    {
        alertName.text = "Ready to Sail";
        alertDesc.text = "You have moved your ship.";

        openAlert();
    }

    public void moveShipNeg()
    {
        alertName.text = "Next Time";
        alertDesc.text = "You decided not to move your ship.";

        openAlert();
    }

    public void maintanenceShipPos()
    {
        alertName.text = "Looking Good";
        alertDesc.text = "You have scheduled inspection for your ship.";

        openAlert();
    }

    public void maintanenceShipNeg()
    {
        alertName.text = "Let It Rust";
        alertDesc.text = "You decided not to call for inspection.";

        openAlert();
    }

    public void tradePos()
    {
        alertName.text = "For a Few Dollars More";
        alertDesc.text = "You have started trading goods.";

        openAlert();
    }

    public void tradeNeg()
    {
        alertName.text = "No Trading";
        alertDesc.text = "You decided not to do trading business.";

        openAlert();
    }

    public void hireExplorerPos()
    {
        alertName.text = "Hiring Successful";
        alertDesc.text = "You have hired explorer.";

        openAlert();
    }

    public void hireExplorerNeg()
    {
        alertName.text = "Sike";
        alertDesc.text = "You decided not to hire an explorer.";

        openAlert();
    }

    public void cancelActionPos()
    {
        alertName.text = "Action Canceled";
        alertDesc.text = "The ship aborted its current mission.";

        openAlert();
    }

    public void cancelActionNeg()
    {
        alertName.text = "Continue";
        alertDesc.text = "No new order given out.";

        openAlert();
    }

    public void sellShipPos()
    {
        alertName.text = "Sold";
        alertDesc.text = "The ship is now in the hand of the dealer";

        openAlert();
    }

    public void sellShipNeg()
    {
        alertName.text = "My Precious Time!";
        alertDesc.text = "You have successfully wated dealer's precious time.";

        openAlert();
    }

    public void fireExplorerPos()
    {
        alertName.text = "Walk the Plank";
        alertDesc.text = "You were about to make the explorer walk the plank but mercifully just let him get off the boat.";

        openAlert();
    }

    public void fireExplorerNeg()
    {
        alertName.text = "I Might Have Mixed Up";
        alertDesc.text = "You have decided to keep the explorer on the ship.";

        openAlert();
    }

    //multiple alert
    public void offerDenied(string name)
    {
        mAlertName.text = "Offer Denied.";
        mAlertDesc.text = name + " believes the amount you have offered is too low.";
        mAlertPos.text = "OK";
        mAlertNeg.text = "There's No Way";

        UImNext.SetActive(true);
        openmAlert();
    }

    public void offerAccepted(string name, int value)
    {        
        mAlertName.text = "Offer Accepted.";
        mAlertDesc.text = name + " has accepted your offer for " + value + ".";
        mAlertPos.text = "Accept";
        mAlertNeg.text = "Reject";

        UImNext.SetActive(true);
        openmAlert();
    }

    public void clientDeath(string name)
    {
        mAlertName.text = "Death of Your Client";
        mAlertDesc.text = "Your client " + name + " has passed away in " + gameManager.year + ". Would you attend the funeral?";
        mAlertPos.text = "Certainly";
        mAlertNeg.text = "I'm busy";

        UImNext.SetActive(true);
        openmAlert();

    }

    public void newArtWork(string name, string artName)
    {
        mAlertName.text = "New Artwork!";
        mAlertDesc.text = "Your client " + name + " has created a new artwork " +  artName + " in " + gameManager.year + ". Would you keep the artwork on the wall?";
        mAlertPos.text = "Keep it";
        mAlertNeg.text = "Sell it";

        UImNext.SetActive(true);
        openmAlert();

    }

    public void landPurchase(Land land)
    {
        
        mAlertName.text = "Land Purchase Agreement";
        mAlertDesc.text = "The Buyer: " + gameManager.givenName + 
            " agrees to purchase the property of " + land.streetNum + " " + land.streetName + 
            " in the city of " + land.city + 
            " by payment of " + land.price + ".";
        mAlertPos.text = "Sign";
        mAlertNeg.text = "Cancel the deal";
        openmAlert();
    }

    public void hireCourtier(Courtier courtier)
    {
        
        mAlertName.text = "Wir Suchen Dich!";
        mAlertDesc.text = gameManager.givenName + 
            " to hire " + courtier.name + " as of " + gameManager.year + 
            " with agreed on a salary of " + courtier.salary + ".";
        mAlertPos.text = "Hire";
        mAlertNeg.text = "Not!";
        openmAlert();

    }

    public void fireCourtier(Courtier courtier)
    {
        mAlertName.text = "See You in My Office, " + courtier.name;
        mAlertDesc.text = "You are only two words away from firing " + courtier.name + ".";
        mAlertPos.text = "Fire";
        mAlertNeg.text = "Never mind";
        openmAlert();
    }

    public void courtierScout(Courtier courtier, List<string> receivedOccupations)
    {
        string occupations = string.Join(", ", receivedOccupations);
        mAlertName.text = "Black Sheep Wall";
        mAlertDesc.text = "Will you send " + courtier.name + " to scout " + occupations.Trim() + " in " + courtier.location + "?";
        mAlertPos.text = "Yay";
        mAlertNeg.text = "Nay";
        openmAlert();
    }

    public void courtierMove(Courtier courtier, string region)
    {
        mAlertName.text = "I Like to Move It Move It";
        mAlertDesc.text = "You are planning to move your courtier, " + courtier.name + ", to " + region + ".";
        mAlertPos.text = "Proceed";
        mAlertNeg.text = "Call Off";
        openmAlert();
    }

    public void addBuilding(string building, int buildingCost)
    {
        mAlertName.text = "New Building";
        mAlertDesc.text = "You plan to build " + building + " for " + buildingCost + ".";
        mAlertPos.text = "Build";
        mAlertNeg.text = "Scrap Plan";
        openmAlert();
    }
    
    public void buildShip(Ship ship)
    {
        mAlertName.text = "New Ship";
        mAlertDesc.text = "You plan to build a new ship of size " + ship.size + " for " + ship.cost + ".";
        mAlertPos.text = "Build";
        mAlertNeg.text = "Never Mind";
        openmAlert();
    }
    
    public void buyShip(Ship ship)
    {
        mAlertName.text = "Buy Ship";
        mAlertDesc.text = "You plan to buy a new ship " + ship.name + " of size " + ship.size + " for " + ship.cost + ".";
        mAlertPos.text = "Buy";
        mAlertNeg.text = "Never Mind";
        openmAlert();
    }

    public void moveShip(Ship ship, City city)
    {
        mAlertName.text = "Move Ship";
        mAlertDesc.text = "You plan to move your ship " + ship.name + " to " + city.name + ".";
        mAlertPos.text = "Proceed";
        mAlertNeg.text = "Cancel";
        openmAlert();
    }

    public void maintanenceShip(Ship ship)
    {
        mAlertName.text = "Ship Maintenance";
        mAlertDesc.text = "You plan to hire workers to inspect your ship " + ship.name + " and fix any problems.";
        mAlertPos.text = "Take Good Care";
        mAlertNeg.text = "Cancel";
        openmAlert();
    }

    public void trade(string tradeItem)
    {
        mAlertName.text = "The Spice Must Flow";
        mAlertDesc.text = "You plan to do a business of trading " + tradeItem + ".";
        mAlertPos.text = "Trade";
        mAlertNeg.text = "Cancel";
        openmAlert();
    }

    public void hireExplorer(Ship ship, Explorer explorer)
    {
        mAlertName.text = "MEN WANTED";
        mAlertDesc.text = "For hazardous journey, small wages, bitter cold, long months of complete darkness, constant danger, Safe return doubtful, Honour and recognition in event of success. You have hired " + explorer.name + " for your ship " + ship.name + ".";
        mAlertPos.text = "Hire";
        mAlertNeg.text = "Cancel";
        openmAlert();
    }

    public void cancelAction()
    {
        mAlertName.text = "Cancel Action";
        mAlertDesc.text = "Do you wish to cancel current action?";
        mAlertPos.text = "Yes";
        mAlertNeg.text = "No";
        openmAlert();
    }

    public void sellShip()
    {
        mAlertName.text = "Sell Ship";
        mAlertDesc.text = "You have decided to sell your ship and the dealer asks you to sign the agreement.";
        mAlertPos.text = "Sign It";
        mAlertNeg.text = "Rip It";
        openmAlert();
    }

    public void fireExplorer()
    {
        mAlertName.text = "Dream of Incineraton";
        mAlertDesc.text = "You had a dream of someone getting fired. Is it this one?";
        mAlertPos.text = "Fire Explorer";
        mAlertNeg.text = "Keep Explorer";
        openmAlert();
    }

    public void victory()
    {
        mAlertName.text = "Veni, Vidi, Vici";
        mAlertDesc.text = "And Caesar wept, for there were no more worlds to conquer. You have successfully contributed to the world for centuries in the name of arts, humanities, ideologies, and science. With your power, the world has shaped as it is of today.";
        mAlertPos.text = "Return";
        mAlertNeg.text = "Continue";
        openmAlert();
    }


    // opening UI
    public void openBlurImg()
    {
        UIBlurImg.gameObject.SetActive(true);
        if (MainButton != null)
        {
            MainButton.interactable = false;
        }
    }

    public void openAlert()
    {
        openBlurImg();
        UIAlertSet.transform.DOMove(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), 0.1f).SetEase(Ease.OutBack);
        UIAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void openmAlert()
    {
        openBlurImg();
        UImAlertSet.transform.DOMove(new Vector3(Screen.width / 2f, Screen.height / 2f, 0), 0.1f).SetEase(Ease.OutBack);
        UImAlertSet.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void closeAlert()
    {
        if (MainButton != null)
        {
            MainButton.interactable = true;
        }
        if (UIBlurImg.activeSelf)
        {
            UIBlurImg.SetActive(false);
        }
        if (UImAlertSet) {
            if (UImAlertSet.transform.localScale == Vector3.one) {
                UImAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            }
        }
        if (UIAlertSet.transform.localScale == Vector3.one) {
            UIAlertSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
        }
        if (UIWorkSet){
            if (UIWorkSet.transform.localScale == Vector3.one) {
                UIWorkSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            }
        }
        if (UIHireSet) {
            if (UIHireSet.transform.localScale == Vector3.one) {
                UIHireSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            }
        }
        if (UIActionSet) {
            UIActionSet.SetActive(false);
        }
        if (UIMultiSelectSet) {
            UIMultiSelectSet.SetActive(false);
        }
        if (UIInfoSet) {
            UIInfoSet.SetActive(false);
        }
        if (UITransferSet) {
            UITransferSet.SetActive(false);
        }
        if (UIPropertyMarketSet) {
            if (UIPropertyMarketSet.transform.localScale == Vector3.one) {
                UIPropertyMarketSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            }
        }
        if (UICollectionSet) {
            if (UICollectionSet.transform.localScale == Vector3.one) {
                UICollectionSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            }
        }
        if (UIWorkInfoSet) {
            if (UIWorkInfoSet.transform.localScale == Vector3.one) {
                UIWorkInfoSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            }
        }
        if (UIConstructionSet) {
            if (UIConstructionSet.transform.localScale == Vector3.one) {
                UIConstructionSet.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack);
            }
        }
        if (UINewShipSet) {
            UINewShipSet.SetActive(false);
        }
        if (UIRenameSet) {
            UIRenameSet.SetActive(false);
        }
        if (UIGoldInfoSet) {
            UIGoldInfoSet.SetActive(false);
        }
    }
}
