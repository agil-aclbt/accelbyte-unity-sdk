// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using UnityEngine;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("com.accelbyte.UnitySDKEditor")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("com.accelbyte.UnitySDKSteam")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("com.accelbyte.UnitySDKPS4")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("com.accelbyte.UnitySDKPS5")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("com.accelbyte.UnitySDKGameCore")]
namespace AccelByte.Core
{
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
    using PlatformMain = WindowsMain;
#elif (UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX) && UNITY_SERVER
    using PlatformMain = LinuxServerMain;
#elif !UNITY_EDITOR && UNITY_PS4
    using PlatformMain = PS4Main;
#elif !UNITY_EDITOR && UNITY_PS5
    using PlatformMain = PS5Main;
#elif !UNITY_EDITOR && UNITY_GAMECORE
    using PlatformMain = GameCoreMain;
#elif UNITY_EDITOR
    using PlatformMain = EditorMain;
#else
    using PlatformMain = NullMain;
#endif
    public static class AccelByteSDKMain
    {
        internal static PlatformMain Main;
        private static bool isMainInitialized = false;

        private static IAccelByteGameThreadSignaller gameThreadSignaller;

        private static System.Action<float> onGameUpdate;

        internal static System.Action<float> OnGameUpdate
        {
            get
            {
                CheckMainThreadSignallerAlive();
                return onGameUpdate;
            }
            set
            {
                CheckMainThreadSignallerAlive();
                onGameUpdate = value;
            }
        }

        internal static System.Action OnSDKStopped;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void StartAccelByteSDK()
        {
            AccelByteDebug.LogVerbose("AccelByte API Initialized!");

            if (Main == null)
            {
                Main = new PlatformMain();
            }

            onGameUpdate = null;

            ExecuteBootstraps();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += EditorApplicationPlayyModeStateChanged;
#endif
            Application.quitting += ApplicationQuitting;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LateStartAccelByteSDK()
        {
            if (isMainInitialized)
            {
                return;
            }

            Main.Run();
            isMainInitialized = true;
        }

        private static void StopSDK()
        {
            OnSDKStopped?.Invoke();
            EnvrionmentBootstrap.Stop();
            ClientAnaylticsBootstrap.Stop();
            SdkInterfaceBootstrap.Stop();
            DetachGameUpdateSignaller();

            Main.Stop();
            isMainInitialized = false;

            OnSDKStopped = null;
        }

        private static void ExecuteBootstraps()
        {
            EnvrionmentBootstrap.Execute();
            ClientAnaylticsBootstrap.Execute();
            FlightIDBootstrap.Execute();
            SdkInterfaceBootstrap.Execute();
        }

#if UNITY_EDITOR
        private static void EditorApplicationPlayyModeStateChanged(UnityEditor.PlayModeStateChange newState)
        {
            if(newState == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                UnityEditor.EditorApplication.playModeStateChanged -= EditorApplicationPlayyModeStateChanged;
                StopSDK();
            }
        }
#endif

        public static void AttachGameUpdateSignaller(IAccelByteGameThreadSignaller newSignaller)
        {
            if(gameThreadSignaller != null)
            {
                gameThreadSignaller.GameThreadSignal -= OnGameThreadUpdate;
            }

            gameThreadSignaller = newSignaller;

            if (gameThreadSignaller != null)
            {
                gameThreadSignaller.GameThreadSignal -= OnGameThreadUpdate;
                gameThreadSignaller.GameThreadSignal += OnGameThreadUpdate;
            }
            else
            {
                AccelByteDebug.LogWarning("AccelByte update signaller set to null.");
            }
        }

        public static void DetachGameUpdateSignaller()
        {
            if (gameThreadSignaller != null)
            {
                gameThreadSignaller.GameThreadSignal -= OnGameThreadUpdate;
            }

            gameThreadSignaller = null;
        }

        private static void CheckMainThreadSignallerAlive()
        {
            if(gameThreadSignaller == null)
            {
                GameObject signallerGameObject = Utils.AccelByteGameObject.GetOrCreateGameObject();
                var accelByteSignaller = signallerGameObject.GetComponent<AccelByteGameThreadSignaller>();
                if(accelByteSignaller == null)
                {
                    accelByteSignaller = signallerGameObject.AddComponent<AccelByteGameThreadSignaller>();
                }
                AttachGameUpdateSignaller(accelByteSignaller);
            }
        }

        internal static void AddGameUpdateListener(System.Action<float> newListener, bool checkThreadSignallerIsAlive = true)
        {
            if(checkThreadSignallerIsAlive)
            {
                CheckMainThreadSignallerAlive();
            }
            onGameUpdate += newListener;
        }

        internal static void RemoveGameUpdateListener(System.Action<float> removedListener)
        {
            onGameUpdate -= removedListener;
        }

        private static void OnGameThreadUpdate(float deltaTime)
        {
            onGameUpdate?.Invoke(deltaTime);
        }

        private static void ApplicationQuitting()
        {
            StopSDK();
        }
    }
}
