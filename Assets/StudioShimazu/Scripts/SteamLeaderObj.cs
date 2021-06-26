using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SteamLeaderObj : MonoBehaviour
{
    [SerializeField] Text rankText = default;
    [SerializeField] Text scoreText = default;
    [SerializeField] Text nameText = default;

    public void Set(string rank, string score, string userName)
    {
        rankText.text = rank;
        scoreText.text = score;
        nameText.text = userName;
    }
}
