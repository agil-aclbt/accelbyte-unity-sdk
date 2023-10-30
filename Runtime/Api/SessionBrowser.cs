// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using System;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Allow user to create, read, update, delete, and join a game session.
    /// Also allow to manage other players involved in the session
    /// </summary>
    public class SessionBrowser : WrapperBase
    {
        private readonly SessionBrowserApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal SessionBrowser(SessionBrowserApi inApi, UserSession inSession, CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "api parameter can not be null.");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner parameter can not be null");

            this.api = inApi;
            this.session = inSession;
            this.coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Create a game session then inform the AccelByte service and allow other players to query this session
        /// </summary>
        /// <param name="createRequest">Specification of the session</param>
        /// <param name="callback">Return the created session info</param>
        /// <returns></returns>
        public void CreateGameSession(SessionBrowserCreateRequest createRequest, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateGameSession(createRequest, cb => 
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.SessionCreate);
                    }
                    HandleCallback(cb, callback);
                }));
        }

        /// <summary>
        /// Update a game session to modify the player amount in the session.
        /// </summary>
        /// <param name="sessionId">Targeted session's ID</param>
        /// <param name="updateRequest">The request to update the session</param>
        /// <param name="callback">Return the updated session info</param>
        /// <returns></returns>
        public void UpdateGameSession(string sessionId, SessionBrowserUpdateSessionRequest updateRequest, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateGameSession(sessionId, updateRequest, callback));
        }

        /// <summary>
        /// Remove a session
        /// </summary>
        /// <param name="sessionId">Targeted session's ID</param>
        /// <param name="callback">Return the removed session info</param>
        /// <returns></returns>
        public void RemoveGameSession(string sessionId, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.RemoveGameSession(sessionId, callback));
        }

        /// <summary>
        /// Query available game sessions
        /// </summary>
        /// <param name="filter">Specification to query sessions</param>
        /// <param name="callback">Return the query result</param>
        /// <returns></returns>
        public void GetGameSessions(SessionBrowserQuerySessionFilter filter, ResultCallback<SessionBrowserGetResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGameSessions(filter, callback));
        }

        /// <summary>
        /// Query available game sessions
        /// </summary>
        /// <param name="userIds">List of user id</param>
        /// <param name="callback">Return the query result</param>
        /// <returns></returns>
        public void GetGameSessionsByUserIds(string[] userIds, ResultCallback<SessionBrowserGetByUserIdsResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGameSessionsByUserIds(userIds, callback));
        }

        /// <summary>
        /// Get a specific game sessions
        /// </summary>
        /// <param name="sessionId">Targeted session's ID</param>
        /// <param name="callback">Return the session info</param>
        /// <returns></returns>
        public void GetGameSession(string sessionId, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGameSession(sessionId, callback));
        }

        /// <summary>
        /// Add player to game session
        /// </summary>
        /// <param name="sessionId">Targeted session's ID</param>
        /// <param name="targetedUserId"></param>
        /// <param name="asSpectator">Indicates as spectator</param>
        /// <param name="callback">Return the addition result</param>
        /// <returns></returns>
        public void RegisterPlayer(string sessionId, string targetedUserId, bool asSpectator, ResultCallback<SessionBrowserAddPlayerResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.RegisterPlayer(sessionId, targetedUserId, asSpectator, callback));
        }

        /// <summary>
        /// Remove player from game session
        /// </summary>
        /// <param name="sessionId">Targeted session's ID</param>
        /// <param name="callback">Return the removal result</param>
        /// <returns></returns>
        public void UnregisterPlayer(string sessionId, string targetedUserId, ResultCallback<SessionBrowserRemovePlayerResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UnregisterPlayer(sessionId, targetedUserId, callback));
        }

        /// <summary>
        /// Query recent player by user ID
        /// </summary>
        /// <param name="targetedUserId"></param>
        /// <param name="offset">Starting position of a query</param>
        /// <param name="limit">The amount of data that will be returned from this query action</param>
        /// <param name="callback">Return an array of the recent player's data</param>
        public void GetRecentPlayer(string targetedUserId, uint offset, uint limit, ResultCallback<SessionBrowserRecentPlayerGetResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(targetedUserId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(targetedUserId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetRecentPlayer(targetedUserId, offset, limit, callback));
        }

        /// <summary>
        /// Join the specified session by session ID. Possible the game required a password to join
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="password"></param>
        /// <param name="callback">Return the specifed session info </param>
        /// <returns></returns>
        public void JoinSession(string sessionId, string password, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(sessionId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetSessionIdInvalidMessage(sessionId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.JoinSession(sessionId, password, cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        SendPredefinedEvent(cb.Value, EventMode.SessionJoin);
                    }
                    HandleCallback(cb, callback);
                }));
        }

        #region PredefinedEvents

        private PredefinedEventScheduler predefinedEventScheduler;

        /// <summary>
        /// Set predefined event scheduler to the wrapper
        /// </summary>
        /// <param name="predefinedEventScheduler">Predefined event scheduler object reference</param>
        internal void SetPredefinedEventScheduler(ref PredefinedEventScheduler predefinedEventScheduler)
        {
            this.predefinedEventScheduler = predefinedEventScheduler;
        }

        private enum EventMode
        {
            SessionCreate,
            SessionJoin
        }

        private IAccelByteTelemetryPayload CreatePayload<T>(T result, EventMode eventMode)
        {
            IAccelByteTelemetryPayload payload = null;

            switch (eventMode)
            {
                case EventMode.SessionCreate:
                    SessionBrowserData sessionCreateResult = result as SessionBrowserData;
                    payload = new PredefinedGameSessionV1CreatedPayload(sessionCreateResult.user_id, sessionCreateResult.session_id);
                    break;

                case EventMode.SessionJoin:
                    SessionBrowserData sessionJoinResult = result as SessionBrowserData;
                    payload = new PredefinedGameSessionV1JoinedPayload(sessionJoinResult.user_id, sessionJoinResult.session_id);
                    break;
            }

            return payload;
        }

        private void SendPredefinedEvent<T>(T result, EventMode eventMode)
        {
            if (predefinedEventScheduler == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload(result, eventMode);

            if (payload == null)
            {
                return;
            }

            var predefinedEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(predefinedEvent, null);
        }

        private void HandleCallback<T>(Result<T> result, ResultCallback<T> callback)
        {
            if (result.IsError)
            {
                callback.TryError(result.Error);
                return;
            }

            callback.Try(result);
        }

        #endregion
    }
}