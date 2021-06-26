# SteamLeaderboard
# 何ができる？
・steamのランキングボードをゲーム内に表示できる

<img width="992" alt="スクリーンショット 2021-06-26 10 45 14" src="https://user-images.githubusercontent.com/6568021/123498569-ba471f00-d66b-11eb-93da-1671bf48b431.png">


# 前提
・[Steamworks.NET](https://steamworks.github.io)を導入している.  
・Steamのログインは成功している.  

# 参考
・[こちら](https://kan-kikuchi.hatenablog.com/entry/Steam_Ranking)を読んでランキングや実績の実装について理解していると楽

# ランキングボードの手順
・[]()からパッケージを取得してインポート  
・StudioShimazu>PrefabsのSteamLeaderboardPanelを表示したいシーンのCanvasの子にする  
・以下のコードを表示したいところで実行すればOK  

```csharp
SteamLeaderboard.instance.ShowLeaderboards();
```

