using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscores : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;
    HighscoresJson loadedHighscoresJson;

    private void Awake()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);
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

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
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
        
        loadedHighscoresJson = JsonUtility.FromJson<HighscoresJson>(PlayerPrefs.GetString("highscoretable"));
        return loadedHighscoresJson;
    }

    private void Save(HighscoresJson loadedHighscoresJson)
    {
        PlayerPrefs.SetString("highscoretable", JsonUtility.ToJson(loadedHighscoresJson));
        PlayerPrefs.Save();
    }



    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 25f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;
            case 1:
                rankString = "1ST"; break;
            case 2:
                rankString = "2ND"; break;
            case 3:
                rankString = "3RD"; break;
        }
        entryTransform.Find("posText").GetComponent<Text>().text = rankString;

        int score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
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
