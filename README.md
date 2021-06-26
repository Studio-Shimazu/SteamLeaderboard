# SteamLeaderboard

前提
・[Steamworks.NET](https://steamworks.github.io)を導入している.
・Steamのログインは成功している

参考
・[こちら](https://kan-kikuchi.hatenablog.com/entry/Steam_Ranking)を読んでランキングや実績の実装について理解していると楽

ランキングボードの手順
・[]()からパッケージを取得してインポート
・StudioShimazu>PrefabsのSteamLeaderboardPanelを表示したいシーンのCanvasの子にする
・以下のコードを表示したいところで実行すればOK

```csharp
SteamLeaderboard.instance.ShowLeaderboards();
```

