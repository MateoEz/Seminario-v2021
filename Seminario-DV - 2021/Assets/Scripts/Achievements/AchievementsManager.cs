using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AchievementsManager : MonoBehaviour
{
    [SerializeField] private TextAsset achievementsTextAsset;

    private static AchievementsManager _instance;
    public static AchievementsManager Instance => _instance;
    
    private Achievements _achievements;
    
    private readonly string ACHIEVEMENTS_KEY = "achievements";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            if (!PlayerPrefs.HasKey(ACHIEVEMENTS_KEY))
            {
                PlayerPrefs.SetString(ACHIEVEMENTS_KEY,Regex.Replace(achievementsTextAsset.text,"(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1"));
            }

            _achievements = JsonUtility.FromJson<Achievements>(PlayerPrefs.GetString(ACHIEVEMENTS_KEY));

        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void TrackAchievement(string id)
    {
        if (_achievements.achievements.Length == 0) return;
        
        foreach (var achievement in _achievements.achievements)
        {
            if (achievement.completed) continue;

            if (achievement.id == id)
            {
                achievement.progress++;

                if (achievement.progress >= achievement.expectedValue)
                {
                    achievement.completed = true;
                    
                    AudioMaster.Instance.PlayClip("LOGRO");
                    FindObjectOfType<AchievementPopup>().Setup(achievement.title, achievement.description);
                }
            }
        }
        
        PlayerPrefs.SetString(ACHIEVEMENTS_KEY,JsonUtility.ToJson(_achievements));
    }
}

[System.Serializable]
public class Achievements
{
    public AchievementData[] achievements;
}
