using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Multiplayer
{
    public class MultiplayerService
    {
        public Client Opponent { get; set; }

        public event Action<GameMsg> OnGameEventReceived;

        public MultiplayerService()
        {

        }

        public void Join(IPAddress iPAddress)
        {
            Opponent = new Client();
            Opponent.OnMessageReceived += (e) => OnGameEventReceived?.Invoke(e);

            Opponent.Start(iPAddress);
        }

        public void Host()
        {
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, 2022));
            listener.Listen(10);

            Task.Run(() =>
            {
                bool disconnected = false;
                while (!disconnected)
                {
                    var handler = listener.Accept();

                    Task.Run(() =>
                   {
                       try
                       {
                           Opponent = new Client();

                           Opponent.OnMessageReceived += (e) => OnGameEventReceived?.Invoke(e);

                           Opponent.OnDisposed += () =>
                           {
                               disconnected = true;
                               Opponent = null;
                           };

                           Opponent.Start(handler);

                       }
                       catch (Exception)
                       { }
                   });
                }
            });
        }
    }
}
