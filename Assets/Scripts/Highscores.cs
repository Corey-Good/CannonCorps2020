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
    public List<GameObject> allListings = new List<GameObject>();
    private Player playerInstance;

    private void Awake()
    {
        //Get the player class to reference in this script
        playerInstance = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        loadedHighscoresJson = Load("FFA");
        highscoreEntryList = loadedHighscoresJson.highscoreEntryList;
        PopulateScoreListings(highscoreEntryList, ffaScrollView);

        loadedHighscoresJson = Load("SM");
        highscoreEntryList = loadedHighscoresJson.highscoreEntryList;
        PopulateScoreListings(highscoreEntryList, smScrollView);

        loadedHighscoresJson = Load("TB");
        highscoreEntryList = loadedHighscoresJson.highscoreEntryList;
        PopulateScoreListings(highscoreEntryList, tbScrollView);

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
        AddHighscoreEntry(player.ScoreCurrent, player.PlayerName);
        highscoreEntryList = loadedHighscoresJson.highscoreEntryList;
        PopulateScoreListings(highscoreEntryList);
    }

    private void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry newHighscoreEntry = new HighscoreEntry { score = score, name = name };

        loadedHighscoresJson = Load();

        if (loadedHighscoresJson is null)
        {
            HighscoresJson firstSave = new HighscoresJson();
            firstSave.highscoreEntryList.Add(new HighscoreEntry { score = 10, name = "test" });
            Save(firstSave);
            loadedHighscoresJson = Load();
        }
        

        loadedHighscoresJson.highscoreEntryList.Add(newHighscoreEntry);

        Sort(loadedHighscoresJson.highscoreEntryList);

        Save(loadedHighscoresJson);
    }

    private HighscoresJson Load()
    {
        //Debug.Log("This is the key: " + PlayerPrefs.GetString("TB"));
        loadedHighscoresJson = JsonUtility.FromJson<HighscoresJson>(PlayerPrefs.GetString("TB"));
        return loadedHighscoresJson;
    }

    private HighscoresJson Load(string tableKey)
    {
        loadedHighscoresJson = JsonUtility.FromJson<HighscoresJson>(PlayerPrefs.GetString(tableKey));
        return loadedHighscoresJson;
    }

    private void Save(HighscoresJson loadedHighscoresJson)
    {
        PlayerPrefs.SetString("TB", JsonUtility.ToJson(loadedHighscoresJson));
        PlayerPrefs.Save();
    }
       
    private void PopulateScoreListings(List<HighscoreEntry> highscoreEntries, GameObject tableType=null)
    {
        
        string rankString;
        int count = 1;
        #region SetTable
        if (tableType == null) 
        { 
            switch(playerInstance.gameState)
            {
                case Player.GameState.FFA:
                    tableType = ffaScrollView;
                    break;
                case Player.GameState.SM:
                    tableType = smScrollView;
                    break;
                case Player.GameState.TB:
                    tableType = tbScrollView;
                    break;
            }
        }
        #endregion

        //if (allListings != null)
        //{
        //    foreach (GameObject listing in allListings)
        //    {
        //        Destroy(listing);
        //    }
        //    allListings.Clear();
        //}
        // Note this list will delete all listings off of al the tables------------------------

        foreach (HighscoreEntry entry in highscoreEntries)
        {
            
            switch (count++)
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
            allListings.Add(tempListing);

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
        public int score = 0;
        public string name = "John";
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
