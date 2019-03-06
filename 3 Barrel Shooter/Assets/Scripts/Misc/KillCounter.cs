using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    // Move these to another script
    private Dictionary<string, int> killDict = new Dictionary<string, int>();
    private Text killfeed;
    private LevelManager levelManager;
    private GameObject canvas;

    public void InitKillCounter(LevelManager lm, GameObject c)
    {
        levelManager = lm;
        canvas = c;
        killfeed = canvas.transform.GetChild(3).gameObject.GetComponent<Text>();
        InitKillDict();
        CreateScoreCounters();
    }


    private void InitKillDict()
    {
        foreach (var player in levelManager.GetPlayerList())
        {
            killDict[player.GetComponent<PlayerInfo>().GetPlayerName()] = 0;

        }
    }

    public List<string> GetWinner()
    {
        int maxKills = 0;
        List<string> topKiller = new List<string>();
        foreach (KeyValuePair<string, int> kv in killDict)
        {
            if (kv.Value == maxKills)
            {
                topKiller.Add(kv.Key);
            }
            else if (kv.Value > maxKills)
            {
                topKiller.Clear();
                maxKills = kv.Value;
                topKiller.Add(kv.Key);
                
            }
            
        }
        return topKiller;
    }
    public Dictionary<string, int> GetKillDict()
    {
        return killDict;
    }


    public void CreateScoreCounters()
    {

    }


    public void addKill(string killed, string killedBy)
    {
        killDict[killedBy] += 1;
        updateKillFeed(killed, killedBy);
        canvas.GetComponent<KillTracker>().updateCanvasElements(killDict);
    }


    private void updateKillFeed(string killed, string killedBy)
    {
        killfeed.text = killedBy + " --> " + killed;
        StartCoroutine("WaitTilNextKill", 5);

    }


    IEnumerator WaitTilNextKill(int n)
    {
        yield return new WaitForSeconds(n);
        killfeed.text = "";
    }
}
