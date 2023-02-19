using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEditor.PackageManager;
using UnityEditorInternal.Profiling;
using UnityEngine;
using static ClientManager;

public class GameManager : MonoBehaviour
{

    static public GameManager instance;

    public int money;
    public int reputation;
    public int year;

    public string givenName;
    public string surname;
    public string nationality;
    public string religion;
    public int dob;

    public int id;

    public int selectedCV;
    public int selectedWage;

    public int recursionNum;

    public int payments;

    public int streetNum;
    public string streetName;
    public string city;
    public int landPrice;

    public int landbuyStatus;

    public string buildingCategory;

    public TextAsset clientDB;
    public TextAsset workDB;
    public TextAsset landDB;
    [System.Serializable]
    public class Client
    {
        public int Id;
        public string Name;
        public string Occupation;
        public int Birth;
        public int Start;
        public int Death;
        public int Potential;
        public string Nationality;
        public string Affiliation;

        public int Value;
        public int Wage;
        public int Age;

    }

    [System.Serializable]
    public class ClientList
    {
        public Client[] client;
    }

    public ClientList clientList = new ClientList();

    public Client mClient;

    [System.Serializable]
    public class Work
    {
        public string Name;
        public int Id;
        public int CreatorId;
        public int Year;
        public string Category;
        public int Price;
        public string Movement;
        public int Reputation;
    }

    [System.Serializable]
    public class WorkList
    {
        public Work[] work;
    }

    public WorkList workList = new WorkList();

    public Work mWork;

    [System.Serializable]
    public class Land
    {
        public string name;
        public string city;
    }

    [System.Serializable]
    public class LandList
    {
        public Land[] land;
    }

    public LandList landList = new LandList();

    public Land mLand;

    //Event List
    [System.Serializable]
    public class Events
    {
        public string name;
        public int year;
        public int eventId;
        public int offeredValue;
        public int id;
        public int offeredWage;
        public int creatorId;
        public string category;
        public int price;
        public string movement;
        public int reputation;
    }

    [System.Serializable]
    public class CurrClients
    {
        public int Id;
        public string Name;
        public string Occupation;
        public int Birth;
        public int Start;
        public int Death;
        public int Potential;
        public string Nationality;
        public string Affiliation;

        public int Value;
        public int Wage;
        public int Age;

        public List<int> ctdWork = new List<int>();
    }

    [System.Serializable]
    public class CurrWorks
    {
        public string Name;
        public int Id;
        public int CreatorId;
        public int Year;
        public string Category;
        public int Price;
        public string Movement;
        public int Reputation;
    }

    [System.Serializable]
    public class CreatedWorks
    {
        public string Name;
        public int Id;
        public int CreatorId;
        public int Year;
        public string Category;
        public int Price;
        public string Movement;
        public int Reputation;
    }

    [System.Serializable]
    public class OwnedWorks
    {
        public string Name;
        public int Id;
        public int CreatorId;
        public int Year;
        public string Category;
        public int Price;
        public string Movement;
        public int Reputation;
        public int locNum;
        public string locName;
    }

    [System.Serializable]
    public class OwnedLandWorks
    {
        public string Name;
        public int Id;
        public int CreatorId;
        public int Year;
        public string Category;
        public int Price;
        public string Movement;
        public int Reputation;
        public int locNum;
        public string locName;
    }

    [System.Serializable]
    public class OwnedBuildings
    {
        public string name;
        public string creator;
        public int slots;
        public int income;
    }

    [System.Serializable]
    public class CurrLands
    {
        public int streetNum;
        public string streetName;
        public string city;
        public int price;
    }

    [System.Serializable]
    public class OwnedLands
    {
        public int streetNum;
        public string streetName;
        public string city;
        public int price;
        public int year;
        public string buildingType;
        public int workSlots;
        public List<OwnedLandWorks> oLWorks;
        public List<OwnedBuildings> oBuildings;
    }

    public List<Events> events;
    public List<CurrClients> currClients;
    public List<CurrWorks> currWorks;
    public List<CreatedWorks> createdWorks;
    public List<OwnedWorks> ownedWorks;
    public List<CurrLands> currLands;
    public List<OwnedLands> ownedLands;
    public List<OwnedLandWorks> oLWorks;
    public List<OwnedBuildings> oBuildings;
    public List<int> protegeList = new List<int>();
    public List<int> creatorIDList = new List<int>();
    public List<int> createdWIDList = new List<int>();
    public List<int> currentYClient = new List<int>();

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void transferProcess(string name, int value, int wage)
    {

        if (selectedCV > value || selectedWage > wage)
        {
            Events e = new Events();
            e.name = name;
            e.year = year;
            e.eventId = 1;
            e.offeredValue = value;
            e.id = id;
            e.offeredWage = wage;
            events.Add(e);
        }
        else if (selectedCV <= value)
        {
            Events e = new Events();
            e.name = name;
            e.year = year;
            e.eventId = 0;
            e.offeredValue = value;
            e.id = id;
            e.offeredWage = wage;
            events.Add(e);
        }
    }

    public void eventProcess(string name)
    {
        Events e = new Events();
        e.name = name;
        e.year = year;
        e.eventId = 2;
        e.id = id;
        events.Add(e);
    }

    public void workEventProcess(string name, int Id, int creatorId, string category, int price, string movement, int reputation)
    {
        Events e = new Events();
        e.name = name;
        e.year = year;
        e.eventId = 3;
        e.id = Id;
        e.creatorId = creatorId;
        e.category = category;
        e.price = price;
        e.movement = movement;
        e.reputation = reputation;
        events.Add(e);
    }

    public void receiveCDB()
    {
        string[] data = clientDB.text.Split(new String[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 9 - 1;
        clientList.client = new Client[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            clientList.client[i] = new Client();
            clientList.client[i].Id = int.Parse(data[9 * (i + 1)]);
            clientList.client[i].Name = data[9 * (i + 1) + 1];
            clientList.client[i].Occupation = data[9 * (i + 1) + 2];
            clientList.client[i].Birth = int.Parse(data[9 * (i + 1) + 3]);
            clientList.client[i].Start = int.Parse(data[9 * (i + 1) + 4]);
            clientList.client[i].Death = int.Parse(data[9 * (i + 1) + 5]);
            clientList.client[i].Potential = int.Parse(data[9 * (i + 1) + 6]);
            clientList.client[i].Value = int.Parse(data[9 * (i + 1) + 6]) * 100;
            clientList.client[i].Wage = int.Parse(data[9 * (i + 1) + 6]);
            clientList.client[i].Nationality = data[9 * (i + 1) + 7];
            clientList.client[i].Affiliation = data[9 * (i + 1) + 8];
            clientList.client[i].Age = year - clientList.client[i].Birth;

            mClient = clientList.client[i];
            currClientProcess(mClient);

        }
    }

    public void currClientProcess(Client mClient)
    {
        if (year - mClient.Start >= 0 && year - mClient.Death <= 0)
        {
            CurrClients cClients = new CurrClients();
            cClients.Name = mClient.Name;
            cClients.Id = mClient.Id;
            cClients.Age = mClient.Age;
            cClients.Occupation = mClient.Occupation;
            cClients.Birth = mClient.Birth;
            cClients.Start = mClient.Start;
            cClients.Death = mClient.Death;
            cClients.Potential = mClient.Potential;
            cClients.Nationality = mClient.Nationality;
            cClients.Affiliation = mClient.Affiliation;
            cClients.Wage = mClient.Wage;
            cClients.Value = mClient.Value;

            currClients.Add(cClients);
            creatorIDList.Add(cClients.Id);
        }
    }

    public void receiveWDB()
    {
        string[] data = workDB.text.Split(new String[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 8 - 1;
        workList.work = new Work[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            workList.work[i] = new Work();
            workList.work[i].Name = data[8 * (i + 1)];
            workList.work[i].Id = int.Parse(data[8 * (i + 1) + 1]);
            workList.work[i].CreatorId = int.Parse(data[8 * (i + 1) + 2]);
            workList.work[i].Year = int.Parse(data[8 * (i + 1) + 3]);
            workList.work[i].Category = data[8 * (i + 1) + 4];
            workList.work[i].Price = int.Parse(data[8 * (i + 1) + 5]);
            workList.work[i].Movement = data[8 * (i + 1) + 6];
            workList.work[i].Reputation = int.Parse(data[8 * (i + 1) + 7]);

            if (creatorIDList.Contains(workList.work[i].CreatorId))
            {
                if (!createdWIDList.Contains(workList.work[i].Id))
                {

                    mWork = workList.work[i];
                    currWorkProcess(mWork);

                }

            }
        }
    }

    public void currWorkProcess(Work mWork)
    {
        CurrWorks cWorks = new CurrWorks();
        cWorks.Name = mWork.Name;
        cWorks.Id = mWork.Id;
        cWorks.CreatorId = mWork.CreatorId;
        cWorks.Year = mWork.Year;
        cWorks.Category = mWork.Category;
        cWorks.Price = mWork.Price;
        cWorks.Movement = mWork.Movement;
        cWorks.Reputation = mWork.Reputation;

        if (cWorks.Year <= year)
        {
            currWorks.Add(cWorks);
        }

    }

    public void createWorks()
    {
        int maxC = currClients.Count() + 1;
        int rec = UnityEngine.Random.Range(0, maxC);
        recursionNum = rec;

        if (recursionNum > 0)
        {
            while (recursionNum > 0)
            {
                createWs();
                recursionNum -= 1;
            }
        }

        checkCtdWorks();

    }

    public void createWs()
    {
        if (currWorks.Count() != 0)
        {
            int maxW = currWorks.Count();
            int rnd = UnityEngine.Random.Range(0, maxW);

            Debug.Log(currWorks[rnd].Name);
            CreatedWorks cdWorks = new CreatedWorks();
            cdWorks.Name = currWorks[rnd].Name;
            cdWorks.Id = currWorks[rnd].Id;
            cdWorks.CreatorId = currWorks[rnd].CreatorId;
            cdWorks.Year = year;
            cdWorks.Category = currWorks[rnd].Category;
            cdWorks.Price = currWorks[rnd].Price;
            cdWorks.Movement = currWorks[rnd].Movement;
            cdWorks.Reputation = currWorks[rnd].Reputation;
            if (!currentYClient.Contains(currWorks[rnd].CreatorId))
            {
                createdWorks.Add(cdWorks);
                if (protegeList.Contains(cdWorks.CreatorId))
                {
                    OwnedWorks odWorks = new OwnedWorks();
                    odWorks.Name = currWorks[rnd].Name;
                    odWorks.Id = currWorks[rnd].Id;
                    odWorks.CreatorId = currWorks[rnd].CreatorId;
                    odWorks.Year = year;
                    odWorks.Category = currWorks[rnd].Category;
                    odWorks.Price = currWorks[rnd].Price;
                    odWorks.Movement = currWorks[rnd].Movement;
                    odWorks.Reputation = currWorks[rnd].Reputation;

                    ownedWorks.Add(odWorks);

                    foreach (var clients in currClients)
                    {
                        if (clients.Id == currWorks[rnd].CreatorId)
                        {
                            string creatorName = clients.Name;

                            workEventProcess(creatorName, currWorks[rnd].Id, currWorks[rnd].CreatorId, currWorks[rnd].Category, currWorks[rnd].Price, currWorks[rnd].Movement, currWorks[rnd].Reputation);
                        }
                    }
                }
                createdWIDList.Add(cdWorks.Id);
                currentYClient.Add(cdWorks.CreatorId);
                currWorks.Remove(currWorks[rnd]);
            }
            else
            {
                createWs();
            }
        }

    }

    public void checkCtdWorks()
    {
        for (int i = 0; i < createdWorks.Count(); i++)
        {
            for (int j = 0; j < currClients.Count(); j++)
            {
                if (createdWorks[i].CreatorId == currClients[j].Id)
                {
                    currClients[j].ctdWork.Add(createdWorks[i].Id);
                }
            }
        }
    }
    public void receiveLDB()
    {
        string[] data = landDB.text.Split(new String[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 2 - 1;
        landList.land = new Land[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            landList.land[i] = new Land();
            landList.land[i].name = data[2 * (i + 1)];
            landList.land[i].city = data[2 * (i + 1) + 1];

            mLand = landList.land[i];
            currLandList(mLand);
        }
    }

    public void currLandList(Land mLand)
    {
        CurrLands cLands = new CurrLands();
        int ran = UnityEngine.Random.Range(1, 300);
        for (int i = 0; i < ownedLands.Count(); i++)
        {
            while (ownedLands[i].streetNum == ran && ownedLands[i].streetName == mLand.name)
            {
                ran = UnityEngine.Random.Range(1, 200);
            }
        }
        cLands.streetNum = ran;
        cLands.streetName = mLand.name;
        cLands.city = mLand.city;

        int rec = UnityEngine.Random.Range(10, 25);
        int price = rec * 100;
        cLands.price = price;

        currLands.Add(cLands);

    }

    public void ownedLandProcess(int streetNum, string streetName, string city, int landPrice, string buildingType = "No Buildings")
    {

        OwnedLands o = new OwnedLands();
        o.streetNum = streetNum;
        o.streetName = streetName;
        o.city = city;
        o.price = landPrice;
        o.year = year;
        o.buildingType = buildingType;
        o.workSlots = 0;
        o.oLWorks = new List<OwnedLandWorks>();
        o.oBuildings = new List<OwnedBuildings>();
        ownedLands.Add(o);
        for (int i = 0; i < currLands.Count(); i++)
        {
            if (currLands[i].streetNum == streetNum && currLands[i].streetName == streetName)
            {
                currLands.Remove(currLands[i]);
            }
        }
    }

    public void addWorktoLandProcess(int streetNum, string streetName, string name, int id, int creatorid, int year, string category, int price, string movement, int reputation)
    {

        int olData = ownedLands.Count;
        int owData = ownedWorks.Count;

        for (int i = 0; i < owData; i++)
        {
            Debug.Log(" working through owned works . ");
            if (id == ownedWorks[i].Id)
            {
                ownedWorks[i].locNum = streetNum;
                ownedWorks[i].locName = streetName;
            }
        }

        for (int j = 0; j < olData; j++)
        {
            if (streetNum == ownedLands[j].streetNum && streetName == ownedLands[j].streetName)
            {
                OwnedLandWorks ow = new OwnedLandWorks();

                ow.Name = name;
                ow.Id = id;
                ow.CreatorId = creatorid;
                ow.Year = year;
                ow.Category = category;
                ow.Price = price;
                ow.Movement = movement;
                ow.Reputation = reputation;
                ow.locNum = streetNum;
                ow.locName = streetName;

                Debug.Log(ow.Name);
                ownedLands[j].oLWorks.Add(ow);
            }
        }
        oLWorks.Clear();
    }

    public void createBuilding(int streetNum, string streetName, string name, string creator, int slots, int income)
    {
        int olData = ownedLands.Count;
        for (int i = 0; i < olData; i++)
        {
            if (streetNum == ownedLands[i].streetNum && streetName == ownedLands[i].streetName)
            {
                OwnedBuildings ob = new OwnedBuildings();
                ob.name = name;
                ob.creator = creator;
                ob.slots = slots;
                ob.income = income;

                ownedLands[i].oBuildings.Add(ob);

                ownedLands[i].workSlots += slots;
            }

        }
    }
}
