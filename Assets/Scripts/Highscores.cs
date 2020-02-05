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


    private void OnEnable()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        Player.OnPlayerReturnsToMenu += UpdatePermanentTable;

        string loadJson = PlayerPrefs.GetString("highscoretable");
        HighscoresJson loadedHighscoresJson = JsonUtility.FromJson<HighscoresJson>(loadJson);
        highscoreEntryList = loadedHighscoresJson.highscoreEntryList;

        // Sorting 
        for (int i  = 0; i < highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.Count; j++)
            {
                if(highscoreEntryList[j].score > highscoreEntryList[i].score)
                {
                    // Swap
                    HighscoreEntry tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tmp;

                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry newHighscoreEntry = new HighscoreEntry { score = score, name = name };
        string loadJson = PlayerPrefs.GetString("highscoretable");
        HighscoresJson loadedHighscoresJson = JsonUtility.FromJson<HighscoresJson>(loadJson);

        loadedHighscoresJson.highscoreEntryList.Add(newHighscoreEntry);

        string json = JsonUtility.ToJson(loadedHighscoresJson);
        PlayerPrefs.SetString("highscoretable", json);
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

    private class HighscoresJson
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    // Represents a single highscore entry
    [Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
    private void UpdatePermanentTable(Player player) // do we want to have multiple copies of the same name?
    {
        AddHighscoreEntry(player.ScoreCurrent, player.PlayerID);
    }

    private void OnDisable()
    {
        Player.OnPlayerReturnsToMenu -= UpdatePermanentTable;
    }
}
