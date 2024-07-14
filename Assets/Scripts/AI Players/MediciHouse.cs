using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediciHouse : Player
{
    public MediciHouse(int id, string name, string residency, int foundationYear, int money) : base(id, name, residency, foundationYear, money) { }

    public override void performAction()
    {
        base.performAction(); // Call the default action
        CollectTaxes();
        PatronizeArt();
    }

    private void CollectTaxes()
    {
        int taxCollected = 120; // Example logic
        money += taxCollected;
        Debug.Log($"{name} collected {taxCollected} in taxes.");
    }

    private void PatronizeArt()
    {
        
    }
}
