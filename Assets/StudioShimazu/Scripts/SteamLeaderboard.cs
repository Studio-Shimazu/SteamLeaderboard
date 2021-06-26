using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamLeaderboard : MonoBehaviour
{
    [SerializeField] Transform top10Parent = default;
    SteamLeaderObj[] top10;
    [SerializeField] SteamLeaderObj myRankObj = default;
    [SerializeField] GameObject leaderboardObj = default;
    public static SteamLeaderboard instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
        // leaderboardObj.SetActive(false);
    }

    private void Init()
    {
        top10 = top10Parent.GetComponentsInChildren<SteamLeaderObj>();
    }

    public void OnReload()
    {
        ShowLeaderboards();
    }

    public void ShowLeaderboards()
    {
        if (SteamManager.Initialized)
        {
            Debug.Log($"Steamの初期化成功");
            FindLeaderboard();
        }
        else
        {
            Debug.LogWarning("Steamの初期化失敗");
        }
    }

    private void FindLeaderboard()
    {
        Debug.Log("リーダーボードの検索開始");
        CallResult<LeaderboardFindResult_t>.Create().Set(SteamUserStats.FindLeaderboard("ScoreRanking"), DownloadMyEntry);
        CallResult<LeaderboardFindResult_t>.Create().Set(SteamUserStats.FindLeaderboard("ScoreRanking"), DownloadEntries);        
    }


    private void DownloadEntries(LeaderboardFindResult_t result, bool failure)
    {
        //リーダーボードが見つかったか判定
        if (failure || result.m_bLeaderboardFound == 0)
        {
            Debug.LogWarning("リーダーボードが見つかりませんでした");
            return;
        }

        //取得する情報
        var call = SteamUserStats.DownloadLeaderboardEntries(
          result.m_hSteamLeaderboard, //送信するリーダーボード 
          ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, //順位を取得する範囲
          0, //取得する順位の一番上
          9 //取得する順位の一番下
        );

        //順位の取得送信
        Debug.Log("順位の取得開始");
        CallResult<LeaderboardScoresDownloaded_t>.Create().Set(call, OnDownloadEntries);
    }

    //順位の取得完了
    private void OnDownloadEntries(LeaderboardScoresDownloaded_t result, bool failure)
    {
        //順位の取得が上手くいったか判定
        if (failure)
        {
            Debug.LogWarning("順位の取得が失敗しました");
            for (int i = 0; i < 10; i++)
            {
                top10[i].Set("-", "-", "-");
            }
            return;
        }
        Debug.Log($"順位の取得完了");


        //登録されている順位の個数を確認
        Debug.Log($"取得した順位の個数 : {result.m_cEntryCount}");

        //各順位の情報を確認
        for (int i = 0; i < 10; i++)
        {
            if (result.m_cEntryCount <= i)
            {
                top10[i].Set("-","-","-");
                continue;
            }
            //各順位の情報を取得
            LeaderboardEntry_t leaderboardEntry;
            SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, i, out leaderboardEntry, new int[0], 0);
            top10[i].Set(leaderboardEntry.m_nGlobalRank.ToString(), leaderboardEntry.m_nScore.ToString(), SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser));
        }

        leaderboardObj.SetActive(true);
    }

    void DownloadMyEntry(LeaderboardFindResult_t result, bool failure)
    {
        CSteamID[] targetIDs = { SteamUser.GetSteamID() };//取得したい情報のIDに、自分のIDを設定
        var call = SteamUserStats.DownloadLeaderboardEntriesForUsers(
          result.m_hSteamLeaderboard, //送信するリーダーボード 
          targetIDs,       //取得したいUserのID
          targetIDs.Length //取得したい情報の個数
        );
        CallResult<LeaderboardScoresDownloaded_t>.Create().Set(call, OnDownloadMyEntry);
    }
    void OnDownloadMyEntry(LeaderboardScoresDownloaded_t result, bool failure)
    {
        //順位の取得が上手くいったか判定
        if (failure)
        {
            myRankObj.Set("-", "-", "-");
            Debug.LogWarning("順位の取得が失敗しました");
            return;
        }
        Debug.Log($"順位の取得完了");

        LeaderboardEntry_t leaderboardEntry;
        SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, 0, out leaderboardEntry, new int[0], 0);
        myRankObj.Set(leaderboardEntry.m_nGlobalRank.ToString(), leaderboardEntry.m_nScore.ToString(), SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser));
    }

    public void OnClose()
    {
        leaderboardObj.SetActive(false);
    }
}
