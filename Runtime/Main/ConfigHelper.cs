// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Server;

namespace AccelByte.Core
{
    internal static class ConfigHelper
    {
        public static void InitializeAMSConfig()
        {
            string amsServerUrl = null;
            if (!string.IsNullOrEmpty(AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSServerUrl))
            {
                amsServerUrl = AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSServerUrl;
            }
            else
            {
                amsServerUrl = GetCommandLineArg(ServerAMS.CommandLineAMSWatchdogUrlId);
                if (!string.IsNullOrEmpty(amsServerUrl))
                {
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Certification.AMSServerUrl = amsServerUrl;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSServerUrl = amsServerUrl;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Development.AMSServerUrl = amsServerUrl;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Production.AMSServerUrl = amsServerUrl;
                }
            }

            int heartbeatInterval = 0;
            if (AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSHeartbeatInterval.HasValue)
            {
                heartbeatInterval = AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSHeartbeatInterval.Value;
            }
            else
            {
                string amsHeartbeatInterval = GetCommandLineArg(ServerAMS.CommandLineAMSHeartbeatId);
                if (!string.IsNullOrEmpty(amsHeartbeatInterval))
                {
                    if (int.TryParse(amsHeartbeatInterval, out heartbeatInterval))
                    {
                        AccelByteSDK.OverrideConfigs.SDKConfigOverride.Certification.AMSHeartbeatInterval = heartbeatInterval;
                        AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSHeartbeatInterval = heartbeatInterval;
                        AccelByteSDK.OverrideConfigs.SDKConfigOverride.Development.AMSHeartbeatInterval = heartbeatInterval;
                        AccelByteSDK.OverrideConfigs.SDKConfigOverride.Production.AMSHeartbeatInterval = heartbeatInterval;
                    }
                }
            }

            string dsId = null;
            if (!string.IsNullOrEmpty(AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.DsId))
            {
                dsId = AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.DsId;
            }
            else
            {
                dsId = GetCommandLineArg(DedicatedServer.CommandLineDsId);
                if (!string.IsNullOrEmpty(dsId))
                {
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.DsId = dsId;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Certification.DsId = dsId;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Development.DsId = dsId;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Production.DsId = dsId;
                }
            }

            if (!string.IsNullOrEmpty(dsId))
            {
                if (string.IsNullOrEmpty(amsServerUrl))
                {
                    amsServerUrl = ServerAMS.DefaultServerUrl;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Certification.AMSServerUrl = amsServerUrl;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSServerUrl = amsServerUrl;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Development.AMSServerUrl = amsServerUrl;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Production.AMSServerUrl = amsServerUrl;
                }

                if (heartbeatInterval == 0)
                {
                    heartbeatInterval = ServerAMS.DefaulHeatbeatSeconds;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Certification.AMSHeartbeatInterval = heartbeatInterval;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default.AMSHeartbeatInterval = heartbeatInterval;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Development.AMSHeartbeatInterval = heartbeatInterval;
                    AccelByteSDK.OverrideConfigs.SDKConfigOverride.Production.AMSHeartbeatInterval = heartbeatInterval;
                }

                ServerAMS ams = AccelByteSDK.GetServerRegistry().GetAMS(autoCreate: false);
                if (ams != null)
                {
                    ams.Disconnect();
                    AccelByteSDK.GetServerRegistry().SetAMS(null);
                }
                AutoConnectAMS();
            }
        }

        public static void InitializeOverridenConfig()
        {
            var argsConfigParser = new Utils.CustomConfigParser();
            Models.MultiSDKConfigsArgs configArgs = argsConfigParser.ParseSDKConfigFromArgs();
            if (configArgs != null)
            {
                AccelByteSDK.OverrideConfigs.SDKConfigOverride = configArgs;
            }
            else
            {
                AccelByteSDK.OverrideConfigs.SDKConfigOverride = new Models.MultiSDKConfigsArgs();
                AccelByteSDK.OverrideConfigs.SDKConfigOverride.Certification = new Models.SDKConfigArgs();
                AccelByteSDK.OverrideConfigs.SDKConfigOverride.Default = new Models.SDKConfigArgs();
                AccelByteSDK.OverrideConfigs.SDKConfigOverride.Development = new Models.SDKConfigArgs();
                AccelByteSDK.OverrideConfigs.SDKConfigOverride.Production = new Models.SDKConfigArgs();
            }

            Models.MultiOAuthConfigs oAuthArgs = argsConfigParser.ParseOAuthConfigFromArgs();
            if (oAuthArgs != null)
            {
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride = oAuthArgs;
            }
            else
            {
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride = new Models.MultiOAuthConfigs();
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride.Certification = new Models.OAuthConfig();
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride.Default = new Models.OAuthConfig();
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride.Development = new Models.OAuthConfig();
                AccelByteSDK.OverrideConfigs.OAuthConfigOverride.Production = new Models.OAuthConfig();
            }
        }

        private static string GetCommandLineArg(string arg)
        {
            return Utils.CommandLineArgs.GetArg(arg);
        }

        private static void AutoConnectAMS()
        {
            AccelByteSDK.GetServerRegistry().GetAMS(autoCreate: true, autoConnect: true);
        }
    }
}