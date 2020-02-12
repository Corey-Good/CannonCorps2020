
using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscores : MonoBehaviour
{
    private List<HighscoreEntry> highscoreEntryList;
    HighscoresJson loadedHighscoresJson;
    public GameObject ffaScrollView;
    public GameObject smScrollView;
    public GameObject tbScrollView;
    public GameObject playerScoreListing;
    public List<GameObject>[] allListings = new List<GameObject>[3]
        {
            new List<GameObject>(),
            new List<GameObject>(),
            new List<GameObject>(),
        };
    private Player playerInstance;


    private void FirstTimeLoad(string tableKey)
    {

        if (!PlayerPrefs.HasKey(tableKey))
        {
            HighscoresJson firstSave = new HighscoresJson();
            firstSave.highscoreEntryList.Add(new HighscoreEntry { score = 10, name = "test" });
            Save(firstSave, tableKey);
            loadedHighscoresJson = Load(tableKey);
        }

        loadedHighscoresJson = Load(tableKey);
        highscoreEntryList = loadedHighscoresJson.highscoreEntryList;
        PopulateScoreListings(highscoreEntryList, tableKey);
    }
    private void Awake()
    {
        //PlayerPrefs.DeleteKey("FFA");
        //PlayerPrefs.DeleteKey("SM");
        //PlayerPrefs.DeleteKey("TB");
        FirstTimeLoad("FFA");
        FirstTimeLoad("SM");
        FirstTimeLoad("TB");

    }
    private void OnEnable()
    {
        Player.OnPlayerReturnsToMenu += UpdatePermanentTable;
    }
    private void OnDisable()
    {
        Player.OnPlayerReturnsToMenu -= UpdatePermanentTable;
    }
    private void UpdatePermanentTable(Player player) // do we want to have multiple copies of the same name? yes
    {
        AddHighscoreEntry(player.ScoreCurrent, player.PlayerName, player.gameState.ToString());
        highscoreEntryList = loadedHighscoresJson.highscoreEntryList;
        PopulateScoreListings(highscoreEntryList, player.gameState.ToString());
        player.ResetPlayerStats();
    }

    private void AddHighscoreEntry(int score, string name, string tableKey)
    {
        HighscoreEntry newHighscoreEntry = new HighscoreEntry { score = score, name = name };

        loadedHighscoresJson = Load(tableKey);

        loadedHighscoresJson.highscoreEntryList.Add(newHighscoreEntry);

        Sort(loadedHighscoresJson.highscoreEntryList);

        Save(loadedHighscoresJson, tableKey);
    }

    private HighscoresJson Load(string tableKey)
    {
        loadedHighscoresJson = JsonUtility.FromJson<HighscoresJson>(PlayerPrefs.GetString(tableKey));
        return loadedHighscoresJson;
    }

    private void Save(HighscoresJson loadedHighscoresJson, string tableKey)
    {
        PlayerPrefs.SetString(tableKey, JsonUtility.ToJson(loadedHighscoresJson));
        PlayerPrefs.Save();
    }

    private void PopulateScoreListings(List<HighscoreEntry> highscoreEntries, string tableKey)
    {
        GameObject tableType = ffaScrollView;
        string rankString; //21TH is a problem
        int count = 0;
        int gameMode = 0;
        #region SetTable

        switch (tableKey)
        {
            case "FFA":
                tableType = ffaScrollView;
                gameMode = 0;
                break;
            case "SM":
                tableType = smScrollView;
                gameMode = 1;
                break;
            case "TB":
                tableType = tbScrollView;
                gameMode = 2;
                break;
        }

        #endregion

        if (allListings[gameMode] != null)
        {
            foreach (GameObject listing in allListings[gameMode])
            {
                Destroy(listing);
            }
            allListings[gameMode].Clear();
        }
        // Note this list will delete all listings off of all the tables------------------------

        foreach (HighscoreEntry entry in highscoreEntries)
        {
            switch (++count)
            {
                default:
                    rankString = count + "TH"; break;
                case 1:
                    rankString = "1ST"; break;
                case 2:
                    rankString = "2ND"; break;
                case 3:
                    rankString = "3RD"; break;
            }


            int score = entry.score;
            string name = entry.name;

            // Create and add a player listing
            GameObject tempListing = Instantiate(playerScoreListing);
            tempListing.transform.SetParent(tableType.transform, false);
            allListings[gameMode].Add(tempListing);

            // Set the players name and score
            Text[] tempText = tempListing.GetComponentsInChildren<Text>();
            tempText[0].text = rankString + " " + name;
            tempText[1].text = score.ToString();
        }

    }

    // class used to save the game
    private class HighscoresJson
    {
        public List<HighscoreEntry> highscoreEntryList = new List<HighscoreEntry>();
    }

    // class that just contains a score and a name
    [Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }

    // commented out until testing is complete

    private void Sort(List<HighscoreEntry> highscoreEntryList)
    {
        // Sorting 
        for (int i = 0; i < highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.Count; j++)
            {
                if (highscoreEntryList[j].score > highscoreEntryList[i].score)
                {
                    // Swap
                    HighscoreEntry tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tmp;

                }
            }
        }
    }
}