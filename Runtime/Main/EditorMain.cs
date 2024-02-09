// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Server;

namespace AccelByte.Core
{
    internal class EditorMain : IPlatformMain
    {
        public void Run()
        {
            ConfigHelper.InitializeOverridenConfig();
            ConfigHelper.InitializeAMSConfig();
        }

        public void Stop()
        {
        }
    }
}