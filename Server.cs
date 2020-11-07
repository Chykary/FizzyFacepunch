﻿using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.FizzySteam
{
    public class Server : Common
    {
        private event Action<int> OnConnected;
        private event Action<int, byte[], int> OnReceivedData;
        private event Action<int> OnDisconnected;
        private event Action<int, Exception> OnReceivedError;

        private BidirectionalDictionary<SteamId, int> steamToMirrorIds;
        private int maxConnections;
        private int nextConnectionID;

        public static Server CreateServer(FizzyFacepunch transport, int maxConnections)
        {            
            Server s = new Server(transport, maxConnections);

            s.OnConnected += (id) => transport.OnServerConnected.Invoke(id);
            s.OnDisconnected += (id) => transport.OnServerDisconnected.Invoke(id);
            s.OnReceivedData += (id, data, channel) => transport.OnServerDataReceived.Invoke(id, new ArraySegment<byte>(data), channel);
            s.OnReceivedError += (id, exception) => transport.OnServerError.Invoke(id, exception);

            SteamNetworking.OnP2PSessionRequest = (steamid) =>
            {
                Debug.Log($"Incoming request from SteamId {steamid}.");
                SteamNetworking.AcceptP2PSessionWithUser(steamid);
            };

            if (!SteamClient.IsValid)
            {
                Debug.LogError("SteamWorks not initialized.");
            }

            return s;
        }

        private Server(FizzyFacepunch transport, int maxConnections) : base(transport)
        {
            this.maxConnections = maxConnections;
            steamToMirrorIds = new BidirectionalDictionary<SteamId, int>();
            nextConnectionID = 1;
        }

        protected override void OnNewConnection(SteamId id) => SteamNetworking.AcceptP2PSessionWithUser(id);

        protected override void OnReceiveInternalData(InternalMessages type, SteamId clientSteamID)
        {
            switch (type)
            {
                case InternalMessages.CONNECT:
                    if (steamToMirrorIds.Count >= maxConnections)
                    {
                        SendInternal(clientSteamID, InternalMessages.DISCONNECT);
                        return;
                    }

                    SendInternal(clientSteamID, InternalMessages.ACCEPT_CONNECT);

                    int connectionId = nextConnectionID++;
                    steamToMirrorIds.Add(clientSteamID, connectionId);
                    OnConnected.Invoke(connectionId);
                    Debug.Log($"Client with SteamID {clientSteamID} connected. Assigning connection id {connectionId}");
                    break;
                case InternalMessages.DISCONNECT:
                    if (steamToMirrorIds.TryGetValue(clientSteamID, out int connId))
                    {
                        OnDisconnected.Invoke(connId);                        
                        CloseP2PSessionWithUser(clientSteamID);
                        steamToMirrorIds.Remove(clientSteamID);
                        Debug.Log($"Client with SteamID {clientSteamID} disconnected.");
                    }
                    else
                    {
                        OnReceivedError.Invoke(-1, new Exception("ERROR Unknown SteamID"));
                    }

                    break;
                default:
                    Debug.Log("Received unknown message type");
                    break;
            }
        }

        protected override void OnReceiveData(byte[] data, SteamId clientSteamID, int channel)
        {
            if (steamToMirrorIds.TryGetValue(clientSteamID, out int connectionId))
            {
                OnReceivedData.Invoke(connectionId, data, channel);
            }
            else
            {
                CloseP2PSessionWithUser(clientSteamID);
                Debug.LogError("Data received from steam client thats not known " + clientSteamID);
                OnReceivedError.Invoke(-1, new Exception("ERROR Unknown SteamID"));
            }
        }

        public bool Disconnect(int connectionId)
        {
            if (steamToMirrorIds.TryGetValue(connectionId, out SteamId steamID))
            {
                SendInternal(steamID, InternalMessages.DISCONNECT);

                return true;
            }
            else
            {
                Debug.LogWarning("Trying to disconnect unknown connection id: " + connectionId);
                return false;
            }
        }

        public void Shutdown()
        {
            foreach (KeyValuePair<SteamId, int> client in steamToMirrorIds)
            {
                Disconnect(client.Value);
                WaitForClose(client.Key);
            }

            SteamNetworking.OnP2PSessionRequest = null;
            Dispose();
        }


        public void SendAll(int connectionId, byte[] data, int channelId)
        {
            if (steamToMirrorIds.TryGetValue(connectionId, out SteamId steamId))
            {
                Send(steamId, data, channelId);
            }
            else
            {
                Debug.LogError("Trying to send on unknown connection: " + connectionId);
                OnReceivedError.Invoke(connectionId, new Exception("ERROR Unknown Connection"));
            }
        }

        public string ServerGetClientAddress(int connectionId)
        {
            if (steamToMirrorIds.TryGetValue(connectionId, out SteamId steamId))
            {
                return steamId.ToString();
            }
            else
            {
                Debug.LogError("Trying to get info on unknown connection: " + connectionId);
                OnReceivedError.Invoke(connectionId, new Exception("ERROR Unknown Connection"));
                return string.Empty;
            }
        }

        protected override void OnConnectionFailed(SteamId remoteId)
        {
            int connectionId = steamToMirrorIds.TryGetValue(remoteId, out int connId) ? connId : nextConnectionID++;
            OnDisconnected.Invoke(connectionId);
        }
    }
}