// Copyright (c) 2018 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    internal class SDKConfigArgs
    {
        [DataMember] public string Namespace;
        [DataMember] public bool? UsePlayerPrefs;
        [DataMember] public bool? EnableDebugLog;
        [DataMember] public string DebugLogFilter;
        [DataMember] public string BaseUrl;
        [DataMember] public string IamServerUrl;
        [DataMember] public string PlatformServerUrl;
        [DataMember] public string BasicServerUrl;
        [DataMember] public string LobbyServerUrl;
        [DataMember] public string CloudStorageServerUrl;
        [DataMember] public string GameProfileServerUrl;
        [DataMember] public string StatisticServerUrl;
        [DataMember] public string QosManagerServerUrl;
        [DataMember] public string AgreementServerUrl;
        [DataMember] public string LeaderboardServerUrl;
        [DataMember] public string CloudSaveServerUrl;
        [DataMember] public string GameTelemetryServerUrl;
        [DataMember] public string AchievementServerUrl;
        [DataMember] public string UGCServerUrl;
        [DataMember] public string ReportingServerUrl;
        [DataMember] public string SeasonPassServerUrl;
        [DataMember] public string SessionBrowserServerUrl;
        [DataMember] public string SessionServerUrl;
        [DataMember] public string MatchmakingV2ServerUrl;
        [DataMember] public bool? UseTurnManager;
        [DataMember] public string TurnManagerServerUrl;
        [DataMember] public string TurnServerHost;
        [DataMember] public string TurnServerPort;
        [DataMember] public string TurnServerPassword;
        [DataMember] public string TurnServerSecret;
        [DataMember] public string TurnServerUsername;
        [DataMember] public int? PeerMonitorIntervalMs;
        [DataMember] public int? PeerMonitorTimeoutMs;
        [DataMember] public int? HostCheckTimeoutInSeconds;
        [DataMember] public string GroupServerUrl;
        [DataMember] public string ChatServerWsUrl;
        [DataMember] public string ChatServerUrl;
        [DataMember] public string GdprServerUrl;
        [DataMember] public string RedirectUri;
        [DataMember] public string AppId;
        [DataMember] public string PublisherNamespace;
        [DataMember] public string CustomerName;
        [DataMember] public bool? EnableAuthHandshake;
        [DataMember] public int? MaximumCacheSize;
        [DataMember] public int? MaximumCacheLifeTime;
        [DataMember] public bool? EnablePresenceBroadcastEvent;
        [DataMember] public int? PresenceBroadcastEventInterval;
        [DataMember] public int? PresenceBroadcastEventGameState;
        [DataMember] public string PresenceBroadcastEventGameStateDescription;
        [DataMember] public bool? EnablePreDefinedEvent;
        [DataMember] public bool? EnableClientAnalyticsEvent;
        [DataMember] public float? ClientAnalyticsEventInterval;
        [DataMember] public string DSHubServerUrl;
        [DataMember] public string DSMControllerServerUrl;
        [DataMember] public string MatchmakingServerUrl;
        [DataMember] public string AMSServerUrl;
        [DataMember] public int? AMSHeartbeatInterval;
        [DataMember] public int? AMSPort;
        [DataMember] public bool? EnableAmsServerQos;
        [DataMember] public string StatsDServerUrl;
        [DataMember] public int? StatsDServerPort;
        [DataMember] public int? StatsDMetricInterval;
        [DataMember] public string DsId;
        [DataMember] public bool? OverrideServiceUrl;
    }

    [DataContract, Preserve]
    internal class MultiSDKConfigsArgs
    {
        [DataMember] public SDKConfigArgs Development;
        [DataMember] public SDKConfigArgs Certification;
        [DataMember] public SDKConfigArgs Production;
        [DataMember] public SDKConfigArgs Default;
    }
}
