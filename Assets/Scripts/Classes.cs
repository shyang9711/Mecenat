using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Player
{
    public int id;
    public string name;
    public string residency;
    public int foundationYear;
    public int money;

    public List<Client> protegeList;
    public List<Work> ownedWork;
    public List<Courtier> hiredCourtier;

    public Player(int id, string name, string residency, int foundationYear, int money)
    {
        this.id = id;
        this.name = name;
        this.residency = residency;
        this.foundationYear = foundationYear;
        this.money = money;
        this.protegeList = new List<Client>();
        this.ownedWork = new List<Work>();
        this.hiredCourtier = new List<Courtier>();
    }

    public virtual void performAction()
    {
        // Default actions
        Debug.Log($"{name} is performing default actions.");
    }
}

[System.Serializable]
public class Client : ISerializationCallbackReceiver
{
    public int id;
    public string name;
    public string occupation;
    public int birth;
    public int start;
    public int death;
    public int ability;
    public int potential;
    public string nationality;
    public string affiliation;
    public string residency;

    public int value;
    public int wage;
    public int age;

    public int progress;

    [NonSerialized]
    public List<int> offers = new List<int>();

    [NonSerialized]
    public List<Work> potWork = new List<Work>();

    [NonSerialized]
    public List<Work> ctdWork = new List<Work>();

    // Temporary serialized fields
    [SerializeField]
    private List<int> serializedOffers;

    [SerializeField]
    private List<Work> serializedPotWork;

    [SerializeField]
    private List<Work> serializedCtdWork;

    public void OnBeforeSerialize()
    {
        // Serialize only necessary fields
        serializedOffers = new List<int>(offers);
        serializedPotWork = new List<Work>(potWork);
        serializedCtdWork = new List<Work>(ctdWork);
    }

    public void OnAfterDeserialize()
    {
        // Deserialize the fields
        offers = new List<int>(serializedOffers);
        potWork = new List<Work>(serializedPotWork);
        ctdWork = new List<Work>(serializedCtdWork);
    }
}

[System.Serializable]
public class Artist : Client
{
    public bool isGrandMaster;
}

[System.Serializable]
public class Explorer : Client
{
    public bool isHired;
    public Ship ship;
}

[System.Serializable]
public class Scholar : Client
{
    public string school;
}

[System.Serializable]
public class ClientList
{
    public Client[] client;
}

[System.Serializable]
public class Work
{
    public int id;
    public string name;
    public string creator;
    public int creatorId;
    public int year;
    public string category;
    public int price;
    public string movement;
    public int reputation;
    public int locNum;
    public string locName;
    public List<int> owners = new List<int>();
}

[System.Serializable]
public class WorkList
{
    public Work[] work;
}

[System.Serializable]
public class Land
{
    public string name;
    public string city;
    public int streetNum;
    public string streetName;
    public int price;
    public int workSlots;
    public List<Work> works = new List<Work>();
    public List<Building> buildings = new List<Building>();
    public string buildingType;
    public int year;
}

[System.Serializable]
public class LandList
{
    public Land[] land;
}

[System.Serializable]
public class Events
{
    public Client client;
    public Work work;
    public Ship ship;
    public int year;
    public int eventId;
    public int offeredValue;
    public int id;
    public int offeredWage;
}

[System.Serializable]
public class Building
{
    public string name;
    public string creator;
    public int slots;
}

[System.Serializable]
public class Courtier
{
    public string name;
    public string nationality;
    public int salary;
    public string location;
    public List<string> scoutOccupation;
    public string newLocation;
    public string action;
    public int inAction;
}

[System.Serializable]
public class CourtierList
{
    public Courtier[] courtier;
}


[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new Exception("There are keys and values not matching in length");

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}
[System.Serializable]
public class MovementEntry
{
    public string movement;
    public SerializableDictionary<int, int> reputationDict;
    public int totalReputation;

    public MovementEntry(string movement, SerializableDictionary<int, int> reputationDict, int totalReputation)
    {
        this.movement = movement;
        this.reputationDict = reputationDict;
        this.totalReputation = totalReputation;
    }
}

[Serializable]
public class ScoutOccupationEntry
{
    public string Key;
    public List<string> Value;
}

[System.Serializable]
public class City
{
    public string name;
    public string nation;
    public string region;
    public string wonder;
    public string legend;
    public string artifact;
    public string trade;
}

[System.Serializable]
public class CitiesList
{
    public City[] city;
}

public static class ColorExtensions
{
    public static Color FromHex(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        return Color.white; // Fallback to white if parsing fails
    }
}

[System.Serializable]
public class Ship
{
    public string name;
    public int builtYear;
    public int constructionTime;
    public int size;
    public int cost;
    public int condition;
    public string action;
    public int inAction;
    public bool hasMaintanence;
    public bool isTrading;
    public bool isExploring;
    public bool isSearching;
    public string trade;
    public bool inCargo;
    public bool hired;
    public City city;
    [NonSerialized]
    public Explorer explorer;
    public List<string> addOns;
}

[System.Serializable]
public class ShipList
{
    public Ship[] ship;
}

[System.Serializable]
public class TradeData
{
    public string itemName;
    public int allocation;
    public int situation;
}