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

    public AIPlayerManager aiPlayerManager;

    public int money;
    public int reputation;
    public int year;
    public int playerId = 1;

    public string givenName;
    public string surname;
    public string residency;

    // Profit and Loss
    public int expense;
    public int income;
    private int tradingIncome;
    private int max_allocation = 8;
    private int initial_trade_price = 7;
    private int monopoly_bonus = 400;

    public int Income
    {
        get { return income; }
        private set { income = value; }
    }

    public int Expense
    {
        get { return expense; }
        private set { expense = value; }
    }

    public int landbuyStatus;

    public string buildingCategory;

    // Databases
    public TextAsset landDB;

    public ClientList clientList = new ClientList();

    public WorkList workList = new WorkList();

    public LandList landList = new LandList();

    public CourtierList courtierList = new CourtierList();
    public CitiesList citiesList = new CitiesList();

    // Events
    public List<Events> events;
    public List<Events> currentEvents;

    public List<Client> currClients = new List<Client>();
    public List<Work> ownedWorks;
    public List<Land> currLands;
    public List<Land> ownedLands;

    // Courtier
    public List<Courtier> currCourtiers;
    public List<Courtier> hiredCourtiers;


    public List<Client> protegeList = new List<Client>();
    public List<int> creatorIdList = new List<int>();

    // Regions
    public List<string> explorableRegion = new List<string>();
    public Dictionary<string, List<(string, int)>> adjacencyRegion = new Dictionary<string, List<(string, int)>>();

    public Dictionary<string, List<string>> nationRegion = new Dictionary<string, List<string>>();
    public Dictionary<string, string> nationToRegion = new Dictionary<string, string>();
    public List<string> exploredNations = new List<string>();
    public List<string> scoutRegion = new List<string>();
    public List<string> placedRegion = new List<string>();
    public List<ScoutOccupationEntry> scoutOccupation = new List<ScoutOccupationEntry>();

    // Auction
    public List<Work> auctionWorks = new List<Work>();

    // Movement
    public List<MovementEntry> movementList = new List<MovementEntry>();

    // Ships
    public int commissionedShips = 0;
    public List<Ship> shipList = new List<Ship>();
    public List<Ship> shipBuildList = new List<Ship>();
    public List<Ship> shipBuyList = new List<Ship>();

    // Trading
    public List<TradeData> trades = new List<TradeData>();

    // AI players
    public List<Player> aiPlayers = new List<Player>();
    public List<Player> currentPlayers = new List<Player>();

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

    public void transferProcess(Client client, int value, int wage)
    {
        Events e = new Events();
        e.client = client;
        e.id = client.id;
        e.year = 2;
        e.offeredValue = value;
        e.offeredWage = wage;
        // Reject
        if (client.value > value || client.wage > wage)
        {
            e.eventId = 1;
            events.Add(e);
        }
        // Accept
        else
        {
            e.eventId = 2;
            events.Add(e);
        }
    }

    public void deathEventProcess(Client client)
    {
        Events e = new Events();
        e.client = client;
        e.year = 0;
        e.eventId = 3;
        e.id = client.id;
        events.Add(e);
    }

    public void workEventProcess(Client client, Work work)
    {
        Events e = new Events();
        e.client = client;
        e.work = work;
        e.year = 0;
        e.eventId = 4;
        e.id = work.id;
        events.Add(e);
    }

    public void shpiBuildProcess(Ship ship)
    {
        Events e = new Events();
        e.ship = ship;
        e.year = ship.constructionTime;
        e.eventId = 5;
        events.Add(e);
    }

    public void UpdateProfitLoss()
    {
        money -= expense;
        money += income;
    }

    public void UpdateSalary(int salary, bool hired)
    {
        if (hired)
        {
            expense += salary;
        }
        else {
            expense -= salary;
        }
    }

    public void UpdateEvents()
    {
        for (int i = events.Count - 1; i >= 0; i--)
        {
            events[i].year -= 1;
            if (events[i].year <= 0)
            {
                currentEvents.Add(events[i]);
                events.RemoveAt(i);
            }
        }
    }

    public void UpdateClients()
    {
        List<Client> clientsToRemove = new List<Client>();
        foreach (var client in currClients)
        {
            client.age = year - client.birth;
            if (year - client.death > 0)
            {
                clientsToRemove.Add(client);
            }
        }

        foreach (var client in clientsToRemove)
        {
            currClients.Remove(client);
            creatorIdList.Remove(client.id);
        }

        foreach (var client in clientList.client)
        {
            if (year - client.start >= 0 && year - client.death <= 0 && !creatorIdList.Contains(client.id))
            {
                currClients.Add(client);
                creatorIdList.Add(client.id);
            }
        }
    }

    public void UpdateWorks()
    {
        // Create a dictionary for quick lookup of clients by creatorId
        Dictionary<int, Client> clientDictionary = currClients.ToDictionary(client => client.id);
        Dictionary<int, Client> protegeDictionary = protegeList.ToDictionary(client => client.id);

        foreach (Work work in workList.work)
        {
            if (creatorIdList.Contains(work.creatorId) && work.year == year)
            {
                // Use the dictionary to find the client with the same creatorId
                if (clientDictionary.TryGetValue(work.creatorId, out Client client))
                {
                    client.potWork.Add(work);
                }

                else if (protegeDictionary.TryGetValue(work.creatorId, out client))
                {
                    client.potWork.Add(work);
                }
            }
        }
    }

    public void createWorks()
    {
        foreach (Client client in currClients) {
            if (client.affiliation.Trim().ToLower() != "none" && client.progress == 0)
            {
                if (client.potWork.Count > 0)
                {
                    workHelper(client);
                }
                else
                {
                    client.progress = UnityEngine.Random.Range(1, 10);
                }
            }
            else
            {
                client.ability -= 1;
            }
        }
        foreach (Client protege in protegeList)
        {
            if (protege.potWork.Count > 0)
            {
                Work work = workHelper(protege);
                workEventProcess(protege, work);
                UpdateMovement(work);
            }
            else
            {
                protege.progress = UnityEngine.Random.Range(1, 10);
            }
        }
    }

    private Work workHelper(Client client)
    {
        int random = UnityEngine.Random.Range(0, client.potWork.Count);
        Work work = client.potWork[random];
        work.year = year;
        client.ctdWork.Add(work);
        client.potWork.Remove(work);
        client.progress = UnityEngine.Random.Range(1, 10);
        return work;
    }

    public void workBeforeGameplay()
    {
        // Add potWork
        foreach (Work work in workList.work)
        {
            if (creatorIdList.Contains(work.creatorId) && work.year <= year)
            {
                // Find the client with the same CreatorId and add the work to their potWork list
                foreach (Client client in currClients)
                {
                    if (client.id == work.creatorId)
                    {
                        client.potWork.Add(work);
                        break;
                    }
                }
            }
        }

        foreach (Client client in currClients)
        {
            var worksToRemove = new List<Work>();

            foreach (Work work in client.potWork)
            {
                if (work.year < year)
                {
                    worksToRemove.Add(work);
                    client.ctdWork.Add(work);
                    client.ability += work.reputation;
                }
            }
            client.value = client.ability * 10;
            client.wage = client.ability / 10;

            foreach (var work in worksToRemove)
            {
                client.potWork.Remove(work);
            }
        }

        // Initialize the list with data
        movementList.Add(new MovementEntry("Gothic", new SerializableDictionary<int, int>(), 100));
        movementList.Add(new MovementEntry("Byzantine", new SerializableDictionary<int, int>(), 100));
    }

    public void UpdateMovement(Work work)
    {
        bool updated = false;
        for (int i = 0; i < movementList.Count; i++)
        {
            if (movementList[i].totalReputation < 100)
            {
                if (movementList[i].movement == work.movement)
                {
                    var dict = movementList[i].reputationDict;
                    int oldValue = dict.ContainsKey(playerId) ? dict[playerId] : 0;
                    int newValue = oldValue + work.reputation;
                    dict[playerId] = newValue;

                    int newSum = Math.Min(movementList[i].totalReputation + work.reputation, 100);
                    movementList[i].totalReputation = newSum;
                    updated = true;
                    break;
                }
            }
        }

        if (!updated)
        {
            var newDict = new SerializableDictionary<int, int> { { playerId, work.reputation } };
            movementList.Add(new MovementEntry(work.movement, newDict, work.reputation));
        }
    }

    public void progressClients()
    {
        // Combine currClients and protegeList into a single collection
        var allClients = currClients.Concat(protegeList);

        // Iterate through the combined collection and call the helper method
        foreach (Client client in allClients)
        {
            progressClientsHelper(client);
        }
    }

    void progressClientsHelper(Client client)
    {
        client.progress -= 1;
        if (client.affiliation.Trim().ToLower() == "none" && client.progress <= 0) {
            client.progress = UnityEngine.Random.Range(1, 10);
        }
    }

    public void progressCourtiers()
    {
        HashSet<string> newScoutRange = new HashSet<string>();
        foreach (Courtier courtier in hiredCourtiers) {
            if (courtier.inAction > 1)
            {
                courtier.inAction -= 1;
            }
            else if (courtier.inAction == 1)
            {
                courtier.inAction -= 1;
                if (courtier.action == "Moving")
                {
                    courtier.location = courtier.newLocation;
                    courtier.newLocation = "";
                    courtier.action = "Moved to " + courtier.location;
                    UpdateMoveRegion();
                }
                if (courtier.action == "Preparing to Scout")
                {
                    string region = GetRegionByNation(courtier.location);
                    if (nationRegion.ContainsKey(region))
                    {
                        foreach (string nation in nationRegion[region])
                        {
                            newScoutRange.Add(nation);
                        }
                    }
                    UpdateSelectedOccupations();
                    courtier.action = "Scouting";
                }
            }
            else if (courtier.inAction == 0)
            {
                if (courtier.action == "Scouting")
                {
                    string region = GetRegionByNation(courtier.location);
                    if (nationRegion.ContainsKey(region))
                    {
                        foreach (string nation in nationRegion[region])
                        {
                            newScoutRange.Add(nation);
                        }
                    }
                }
                else
                {
                    courtier.action = "Idling";
                }
            }
        }
        newScoutRange.Add(residency);
        scoutRegion = new List<string>(newScoutRange);
    }

    public void UpdateMoveRegion()
    {
        HashSet<string> newMoveRegionSet = new HashSet<string>(hiredCourtiers.Select(courtier => courtier.location));
        newMoveRegionSet.Add(residency);

        placedRegion = newMoveRegionSet.ToList();
    }

    public void receiveLDB()
    {
        string[] data = landDB.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 2 - 1;
        landList.land = new Land[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            var land = new Land();
            land.name = data[2 * (i + 1)];
            land.city = data[2 * (i + 1) + 1];

            int ran;
            bool uniqueStreetNum;
            do
            {
                uniqueStreetNum = true;
                ran = UnityEngine.Random.Range(1, 300);
                foreach (var ownedLand in ownedLands)
                {
                    if (ownedLand.streetNum == ran && ownedLand.streetName == land.name)
                    {
                        uniqueStreetNum = false;
                        break;
                    }
                }
            } while (!uniqueStreetNum);

            land.streetNum = ran;
            land.streetName = land.name;

            int rec = UnityEngine.Random.Range(10, 25);
            land.price = rec * 100;

            landList.land[i] = land;
            currLands.Add(land);
        }
    }

    public void ownedLandProcess(Land land)
    {
        land.year = year;
        ownedLands.Add(land);
        for (int i = 0; i < currLands.Count; i++)
        {
            if (currLands[i].streetNum == land.streetNum && currLands[i].streetName == land.streetName)
            {
                currLands.Remove(currLands[i]);
            }
        }
    }

    public void addWorktoLandProcess(Land land, Work work)
    {
        work.locNum = land.streetNum;
        work.locName = land.streetName;

        land.works.Add(work);
    }

    public void createBuilding(Land land, string name, string creator, int slots)
    {
        Building building = new Building();
        building.creator = creator;
        building.name = name;

        land.buildings.Add(building);
        land.workSlots += slots;
        int olData = ownedLands.Count;
    }

    public void UpdateCourtier()
    {
        currCourtiers.Clear();
        int courtierCount = courtierList.courtier.Length;
        int count = 0;
        List<int> newCourtierList = new List<int>();
        while (currCourtiers.Count < 10 && count < 20)
        {
            int rand = UnityEngine.Random.Range(0, courtierCount);
            if (!newCourtierList.Contains(rand)) {
                newCourtierList.Add(rand);
                Courtier newCourtier = courtierList.courtier[rand];
                bool alreadyHired = false;
                foreach (Courtier courtier in hiredCourtiers)
                {
                    if (courtier.name == newCourtier.name)
                    {
                        alreadyHired = true;
                        break;
                    }
                }
                if (!alreadyHired)
                {
                    newCourtier.location = newCourtier.nationality;
                    newCourtier.salary = UnityEngine.Random.Range(2, 10);
                    currCourtiers.Add(newCourtier);
                }
                count++;
            }
        }
    }

    public string GetRegionByNation(string nation)
    {
        if (nationToRegion.ContainsKey(nation))
        {
            return nationToRegion[nation];
        }
        else
        {
            Debug.Log("Nation not found");
            return "";
        }
    }

    public List<string> GetExplorableRegions(List<string> exploredNations)
    {
        HashSet<string> explorableRegions = new HashSet<string>();

        foreach (string exploredNation in exploredNations)
        {
            string exploredRegion = GetRegionByNation(exploredNation);
            if (!string.IsNullOrEmpty(exploredRegion) && adjacencyRegion.ContainsKey(exploredRegion))
            {
                foreach ((string adjacentRegion, int weight) in adjacencyRegion[exploredRegion])
                {
                    explorableRegions.Add(adjacentRegion);
                }
            }
        }
        return new List<string>(explorableRegions);
    }

    public void UpdateRegions()
    {

    }

    public void UpdateSelectedOccupations()
    {
        List<ScoutOccupationEntry> newScoutOccupation = new List<ScoutOccupationEntry>();

        // Create a temporary dictionary to help with checking and adding occupations
        Dictionary<string, List<string>> tempOccupationDict = new Dictionary<string, List<string>>();

        foreach (Courtier courtier in hiredCourtiers)
        {
            if (courtier.action != "Scouting" || courtier.inAction == 0)
            {
                if (!tempOccupationDict.ContainsKey(courtier.location))
                {
                    tempOccupationDict[courtier.location] = new List<string>();
                }

                foreach (string occupation in courtier.scoutOccupation)
                {
                    if (!tempOccupationDict[courtier.location].Contains(occupation))
                    {
                        tempOccupationDict[courtier.location].Add(occupation);
                    }
                }
            }
        }

        // Add all occupations in scout region for player's region
        List<string> occupationsList = new List<string> { "Artist", "Writer", "Scholar", "Explorer" };

        if (!tempOccupationDict.ContainsKey(residency))
        {
            tempOccupationDict[residency] = new List<string>();
        }

        foreach (string occupation in occupationsList)
        {
            if (!tempOccupationDict[residency].Contains(occupation))
            {
                tempOccupationDict[residency].Add(occupation);
            }
        }

        // Convert the temporary dictionary to the list structure required
        foreach (var kvp in tempOccupationDict)
        {
            newScoutOccupation.Add(new ScoutOccupationEntry { Key = kvp.Key, Value = kvp.Value });
        }

        scoutOccupation = newScoutOccupation;
    }

    public void shipConditionUpdate()
    {
        List<Ship> shipsToRemove = new List<Ship>();
        if (shipList.Count > 0)
        {
            foreach (Ship ship in shipList)
            {
                ship.condition -= 5;
                if (ship.hasMaintanence)
                {
                    money -= ship.size / 10;
                    ship.condition += 3;
                }
                ship.cost = (int)Math.Round((double)(ship.size * 10 * ship.condition) / 100);
                ship.hasMaintanence = false;
                if (ship.condition <= 0)
                {
                    shipsToRemove.Add(ship);
                }
            }
        }

        foreach (Ship ship in shipsToRemove)
        {
            shipSink(ship);
        }
    }

    public void shipSink(Ship ship)
    {
        if (ship.isTrading)
        {
            DecreaseTradeItem(ship.trade);
            ship.isTrading = false;
            ship.trade = "None";
            UpdateTradingIncome();
        }
        ship.isExploring = false;
        ship.isSearching = false;
        ship.action = "None";
        ship.city = null;
        ship.inCargo = true;
        ship.hired = false;
        if (ship.explorer != null)
        {
            ship.explorer.ship = null;
            ship.explorer.isHired = false;
            ship.explorer = null;
        }
        shipList.Remove(ship);
    }
    public void UpdateTradingIncome()
    {
        income -= tradingIncome;
        tradingIncome = 0;
        foreach (Ship ship in shipList)
        {
            if (ship.isTrading)
            {
                foreach (TradeData trade in trades)
                {
                    if (trade.itemName == ship.trade.Trim())
                    {
                        double tradeIncome;

                        if (trade.allocation <= max_allocation)
                        {
                            tradeIncome = initial_trade_price * ship.size;
                        }
                        else if (trade.allocation > max_allocation)
                        {
                            tradeIncome = (80 / trade.allocation) * ship.size;
                        }
                        else if (trade.allocation == max_allocation && trade.situation == max_allocation)
                        {
                            tradeIncome = initial_trade_price * ship.size + monopoly_bonus;
                        }
                        else
                        {
                            tradeIncome = 0;
                        }

                        tradingIncome += (int)tradeIncome / 10;
                        Debug.Log(tradingIncome);
                    }
                }
            }

        }
        income += tradingIncome;
    }
    public void IncreaseTradeItem(string itemName)
    {
        itemName = itemName.Trim();
        var trade = trades.Find(t => t.itemName == itemName);
        if (trade != null)
        {
            Debug.Log($"Found trade item: {trade.itemName}");
            trade.allocation += 1;
            trade.situation += 1;
            Debug.Log($"Updated trade item: {trade.itemName}, Allocation: {trade.allocation}, Situation: {trade.situation}");
        }
        else
        {
            Debug.Log($"Trade item not found: {itemName}");
        }
    }

    public void DecreaseTradeItem(string itemName)
    {
        itemName = itemName.Trim();
        var trade = trades.Find(t => t.itemName == itemName);
        if (trade != null)
        {
            trade.allocation = Mathf.Max(trade.allocation - 1, 0);
            trade.situation = Mathf.Max(trade.situation - 1, 0);
            income -= 50;
        }
    }
    public void takeTurn()
    {
        List<Player> playersToMove = new List<Player>();

        foreach (Player player in aiPlayers)
        {
            if (player.foundationYear <= year)
            {
                playersToMove.Add(player);
            }
        }

        foreach (Player player in playersToMove)
        {
            currentPlayers.Add(player);
            aiPlayers.Remove(player);
        }

        int i = 0;
        while (i < currentPlayers.Count)
        {
            Player currentPlayer = currentPlayers[i];
            currentPlayer.performAction();
            i++;
        }
    }

}
