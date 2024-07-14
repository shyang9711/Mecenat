using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InitializationManager : MonoBehaviour
{
    public GameManager gameManager;

    // Databases
    public TextAsset clientDB;
    public TextAsset workDB;
    public TextAsset courtierDB;
    public TextAsset cityDB;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeGame()
    {
        InitializeClients();
        InitializeWorks();
        InitializeCourtiers();
        InitializeRegions();
        InitializeCities();
        InitializeShips();
        InitializeTradePrice();
        InitializeAIPlayers();
    }
    private void InitializeClients()
    {
        string[] data = clientDB.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 10 - 1;
        gameManager.clientList.client = new Client[tableSize];

        int currentYear = gameManager.year;

        for (int i = 0; i < tableSize; i++)
        {
            int rowData = 10 * i;
            string occupation = data[rowData + 2];
            Client client;

            if (occupation == "Artist")
            {
                client = new Artist();
            }
            else if (occupation == "Explorer")
            {
                client = new Explorer();
            }
            else if (occupation == "Scholar")
            {
                client = new Scholar();
            }
            else
            {
                client = new Client();
            }
            client.id = int.Parse(data[rowData]);
            client.name = data[rowData + 1];
            client.occupation = occupation;
            client.birth = int.Parse(data[rowData + 3]);
            client.start = int.Parse(data[rowData + 4]);
            client.death = int.Parse(data[rowData + 5]);
            client.potential = int.Parse(data[rowData + 6]);
            client.ability = client.potential - UnityEngine.Random.Range(20, 30);
            client.value = client.ability * 10;
            client.wage = client.ability / 10;
            client.nationality = data[rowData + 7];
            client.affiliation = data[rowData + 8];
            client.residency = data[rowData + 9].Trim();
            client.age = client.start - client.birth;
            client.progress = UnityEngine.Random.Range(1, 10);

            gameManager.clientList.client[i] = client;

            // Initialize active clients
            if (currentYear - client.start >= 0 && currentYear - client.death <= 0 && !gameManager.creatorIdList.Contains(client.id))
            {
                client.age = currentYear - client.birth;
                gameManager.currClients.Add(client);
                gameManager.creatorIdList.Add(client.id);
            }
        }
    }


    private void InitializeWorks()
    {
        string[] data = workDB.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 8 - 1;
        gameManager.workList.work = new Work[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            int rowData = 8 * i;
            gameManager.workList.work[i] = new Work();
            gameManager.workList.work[i].id = int.Parse(data[rowData]);
            gameManager.workList.work[i].name = data[rowData + 1];
            gameManager.workList.work[i].creator = data[rowData + 2];
            gameManager.workList.work[i].creatorId = int.Parse(data[rowData + 3]);
            gameManager.workList.work[i].year = int.Parse(data[rowData + 4]);
            gameManager.workList.work[i].category = data[rowData + 5];
            gameManager.workList.work[i].movement = data[rowData + 6];
            gameManager.workList.work[i].reputation = int.Parse(data[rowData + 7]);
            gameManager.workList.work[i].price = UnityEngine.Random.Range(500 * (gameManager.workList.work[i].reputation - 1), 500 * gameManager.workList.work[i].reputation);
        }
        gameManager.workBeforeGameplay();
    }

    private void InitializeCourtiers()
    {
        string[] data = courtierDB.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 2 - 1;
        gameManager.courtierList.courtier = new Courtier[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            int rowData = 2 * (i + 1);
            gameManager.courtierList.courtier[i] = new Courtier();
            gameManager.courtierList.courtier[i].name = data[rowData];
            gameManager.courtierList.courtier[i].nationality = data[rowData + 1].Trim();
        }
        gameManager.UpdateCourtier();
    }

    private void InitializeRegions()
    {

        // Initialize Regions
        gameManager.adjacencyRegion["America"] = new List<(string, int)> { ("Caribbean", 1), ("Britain", 1), ("West Europe", 1) };
        gameManager.adjacencyRegion["Britain"] = new List<(string, int)> { ("America", 1), ("Caribbean", 1), ("Scandinavia", 1), ("West Europe", 1), ("East Europe", 1) };
        gameManager.adjacencyRegion["Scandinavia"] = new List<(string, int)> { ("Britain", 1), ("West Europe", 1), ("East Europe", 1) };
        gameManager.adjacencyRegion["Caribbean"] = new List<(string, int)> { ("America", 1), ("West Europe", 1), ("Latin America", 1), ("Britain", 1) };
        gameManager.adjacencyRegion["West Europe"] = new List<(string, int)> { ("Britain", 1), ("Caribbean", 1), ("East Europe", 1), ("North Africa", 1), ("America", 1), ("Latin America", 1), ("Scandinavia", 1) };
        gameManager.adjacencyRegion["East Europe"] = new List<(string, int)> { ("West Europe", 1), ("Britain", 1), ("North Africa", 1), ("Arabia", 1), ("Scandinavia", 1) };
        gameManager.adjacencyRegion["Central Asia"] = new List<(string, int)> { ("East Asia", 1), ("Arabia", 1), ("India", 1), ("East Indies", 1) };
        gameManager.adjacencyRegion["East Asia"] = new List<(string, int)> { ("Central Asia", 1), ("India", 1), ("East Indies", 1) };
        gameManager.adjacencyRegion["Latin America"] = new List<(string, int)> { ("Caribbean", 1), ("West Europe", 1), ("West Africa", 1), ("Oceania", 2) };
        gameManager.adjacencyRegion["North Africa"] = new List<(string, int)> { ("West Europe", 1), ("East Europe", 1), ("West Africa", 1), ("East Africa", 1), ("Arabia", 1) };
        gameManager.adjacencyRegion["Arabia"] = new List<(string, int)> { ("North Africa", 1), ("East Europe", 1), ("Central Asia", 1), ("India", 1), ("East Africa", 1) };
        gameManager.adjacencyRegion["India"] = new List<(string, int)> { ("Arabia", 1), ("Central Asia", 1), ("East Asia", 1), ("East Indies", 1) };
        gameManager.adjacencyRegion["East Indies"] = new List<(string, int)> { ("India", 1), ("East Asia", 1), ("Central Asia", 1), ("Oceania", 1) };
        gameManager.adjacencyRegion["West Africa"] = new List<(string, int)> { ("North Africa", 1), ("East Africa", 1), ("South Africa", 1), ("Latin America", 1) };
        gameManager.adjacencyRegion["East Africa"] = new List<(string, int)> { ("West Africa", 1), ("Arabia", 1), ("South Africa", 1), ("North Africa", 1) };
        gameManager.adjacencyRegion["Oceania"] = new List<(string, int)> { ("East Indies", 1), ("Latin America", 1) };
        gameManager.adjacencyRegion["South Africa"] = new List<(string, int)> { ("East Africa", 1), ("West Africa", 1) };

        // Initialize Nations
        gameManager.nationRegion["America"] = new List<string> { "Canada", "United States", "Mexico", "Panama", };
        gameManager.nationRegion["Britain"] = new List<string> { "Britain", "Ireland", "Iceland", "Scotland", };
        gameManager.nationRegion["Scandinavia"] = new List<string> { "Norway", "Sweden", "Denmark", "Finland" };
        gameManager.nationRegion["Caribbean"] = new List<string> { "Dominican Republic", "Jamaica", "Cuba", "Haiti" };
        gameManager.nationRegion["West Europe"] = new List<string> { "Italy", "France", "Germany", "Netherlands", "Belgium", "Spain", "Portugal", "Switzerland" };
        gameManager.nationRegion["East Europe"] = new List<string> { "Poland", "Czech Republic", "Austria", "Hungary", "Greece", "Croatia", "Serbia", "Bosnia and Herzegovina", "Romania", "Albania", "Russia", "Ukraine", "Lithuania", "Bulgaria" };
        gameManager.nationRegion["Central Asia"] = new List<string> { "Uzbekistan", "Afghanistan", "Mongolia", };
        gameManager.nationRegion["East Asia"] = new List<string> { "China", "Taiwan", "Korea", "Japan", };
        gameManager.nationRegion["Latin America"] = new List<string> { "Brazil", "Argentina", "Bolivia", "Colombia", "Chile", "Venezuela", "Peru" };
        gameManager.nationRegion["North Africa"] = new List<string> { "Morocco", "Algeria", "Tunisia", "Egypt", "Sudan", };
        gameManager.nationRegion["West Africa"] = new List<string> { "Mali", "Senegal", "Ghana", "Cote d'Ivoire", "Cameroon", "Congo" };
        gameManager.nationRegion["East Africa"] = new List<string> { "Ethiopia", "Sudan", "Somalia", "Kenya", "Uganda", "Tanzania", "Mozambique", "Zimbabwe" };
        gameManager.nationRegion["South Africa"] = new List<string> { "South Africa", "Angola", "Madagascar", };
        gameManager.nationRegion["East Indies"] = new List<string> { "Vietnam", "Cambodia", "Laos", "Thailand", "Indonesia", "Malaysia", "Singapore", "Papua New Guinea" };
        gameManager.nationRegion["Arabia"] = new List<string> { "Israel", "Saudi Arabia", "Syria", "Iraq", "Iran", "Lebanon", "Turkey", "Jordan", "Yemen", "Oman", "Kuwait", "Qatar" };
        gameManager.nationRegion["India"] = new List<string> { "India", "Pakistan", "Sri Lanka", "Myanmar", "Bangladesh", "Nepal", };
        gameManager.nationRegion["Oceania"] = new List<string> { "Australia", "New Zealand", "Fiji", "Tuvalu", };
    }

    private void InitializeCities()
    {
        string[] data = cityDB.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        int tableSize = data.Length / 7 - 1;
        gameManager.citiesList.city = new City[tableSize];

        for (int i = 0; i < tableSize; i++)
        {
            int rowData = 7 * (i + 1);
            gameManager.citiesList.city[i] = new City();
            gameManager.citiesList.city[i].name = data[rowData];
            gameManager.citiesList.city[i].nation = data[rowData + 1];
            gameManager.citiesList.city[i].region = data[rowData + 2];
            gameManager.citiesList.city[i].wonder = data[rowData + 3];
            gameManager.citiesList.city[i].legend = data[rowData + 4];
            gameManager.citiesList.city[i].artifact = data[rowData + 5];
            gameManager.citiesList.city[i].trade = data[rowData + 6];
        }
        
        // Initialize the reverse mapping from nation to region
        foreach (var region in gameManager.nationRegion)
        {
            foreach (var nation in region.Value)
            {
                gameManager.nationToRegion[nation] = region.Key;
            }
        }

        string nationalityRegion = gameManager.nationToRegion[gameManager.residency];
        foreach (var nation in gameManager.nationRegion[nationalityRegion])
        {
            gameManager.exploredNations.Add(nation);
        }
        gameManager.explorableRegion = gameManager.GetExplorableRegions(gameManager.exploredNations);
        gameManager.placedRegion.Add(gameManager.residency);
    }

    private void InitializeShips()
    {
        gameManager.shipBuildList.Add(new Ship { name = "Tiny", size = 100, cost = 1000, constructionTime = 1 });
        gameManager.shipBuildList.Add(new Ship { name = "Small", size = 500, cost = 5000, constructionTime = 2 });
        gameManager.shipBuildList.Add(new Ship { name = "Medium", size = 1000, cost = 10000, constructionTime = 5 });
        gameManager.shipBuildList.Add(new Ship { name = "Large", size = 3000, cost = 30000, constructionTime = 7 });
        gameManager.shipBuildList.Add(new Ship { name = "Huge", size = 5000, cost = 50000, constructionTime = 10 });
        gameManager.shipBuildList.Add(new Ship { name = "Gigantic", size = 10000, cost = 100000, constructionTime = 15 });
    }
    private void InitializeTradePrice()
    {
        string[] items = {
            "Chilly", "Tobacco", "Silver", "Cotton", "Coal", "Rubber", "Gold", "Oil", "Maize", "Coffee", "Stone", 
            "Carpet", "Wool", "Iron", "Rose", "Timber", "Marble", "Olive", "Dye", "Honey", "Whiskey", "Sugar", 
            "Potato", "Silk", "Copper", "Ivory", "Ceramic", "Herb", "Tea", "Rice", "Fur", "Wheat", "Vodka", 
            "Amber", "Jade", "Banana", "Cinnamon", "Pearl", "Wine", "Truffle", "Beer", "Salt", "Cheese", 
            "Chicken", "Fish", "Mercury", "Pepper", "Velvet", "Cacao", "Glass", "Platinum", "Diamond", "Incense"
        };

        foreach (var item in items)
        {
            gameManager.trades.Add(new TradeData { itemName = item, allocation = 0, situation = 0 });
        }
    }

    void InitializeAIPlayers()
    {
        gameManager.aiPlayers.Add(new MediciHouse(900, "Medici", "Italy", 1230, 5000));

    }
}
