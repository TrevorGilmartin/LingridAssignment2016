using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using GameServerConsole;
using Microsoft.Xna.Framework.Graphics;

namespace LingrenGame
{


    public class GamePlayer
    {
        string playerID;
        public string gamertag = string.Empty;
        public Vector2 position = new Vector2();
        NetClient _client;
        PlayerData _playerDataPacket;
        bool joined;

        public string ImageName = string.Empty;

        public GamePlayer(NetClient client, string GamerTag, string ImgName, string playerid, Vector2 StartPos)
        {
            // Created as a reult of a joined message
            position = StartPos;
            gamertag = GamerTag;
            playerID = playerid;
            ImageName = ImgName;

        }

        public GamePlayer(NetClient client, Guid playerid, string GamerTag, string ImgName, Vector2 StartPos)
        {

            position = StartPos;
            playerID = playerid.ToString();
            gamertag = GamerTag;
            ImageName = ImgName;
            _client = client;

            // consruct a join player packet and serialise it
            _playerDataPacket = new PlayerData("Join", "Cornealious", ImageName, PlayerID, StartPos.X, StartPos.Y);
            sendMessage(_playerDataPacket);
        }

        // Game Player Method
        private void sendMessage(PlayerData _playerDataPacket)
        {
            string json = JsonConvert.SerializeObject(_playerDataPacket);
            NetOutgoingMessage sendMsg =
               _client.CreateMessage();
            sendMsg.Write(json);
            _client.SendMessage(sendMsg, NetDeliveryMethod.ReliableOrdered);
        }

        public void Move(Vector2 delta)
        {
            Position += delta;
            _playerDataPacket.X = Position.X;
            _playerDataPacket.Y = Position.Y;
            _playerDataPacket = new PlayerData("Move", gamertag,ImageName, PlayerID, Position.X, Position.Y);
            sendMessage(_playerDataPacket);
        }

        public PlayerData PlayerDataPacket
        {
            get
            {
                return _playerDataPacket;
            }

            set
            {
                _playerDataPacket = value;
                position.X = value.X;
                position.Y = value.Y;
            }
        }

        public string PlayerID
        {
            get
            {
                return playerID;
            }

            set
            {
                playerID = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public bool Joined
        {
            get
            {
                return joined;
            }

            set
            {
                joined = value;
            }
        }

        public void ChangePosition(Vector2 newPosition)
        {
            position = newPosition;

        }
    }
}
