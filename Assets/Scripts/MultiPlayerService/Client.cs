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
    public enum Roles { Undefined, Player1, Player2 };

    public class Client : IDisposable
    {
        private Socket sender { get; set; }
        public Roles Role { get; set; } = Roles.Undefined;
        private bool disposing = false;
        private const string EndOfStreamText = "<EOS>";

        public Client()
        {
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        #region Events
        public event Action OnDisposed;
        public event Action<GameMsg> OnMessageReceived;
        #endregion

        public void StartGame()
        {
            Send(new StartMsg());
        }

        public void Move(float x)
        {
            Send(new MoveMsg() { X = x });
        }
        public void StopMove()
        {
            Send(new StopMoveMsg());
        }

        public void Jump()
        {
            Send(new JumpMsg());
        }
        public void Plant()
        {
            Send(new PlantMsg());
        }
        public void Fire()
        {
            Send(new FireMsg());
        }

        public void SwitchGravity()
        {
            Send(new SwitchGravityMsg());
        }

        public void Ready()
        {
            Send(new ReadyMsg());
        }

        private void Send<T>(T gameMsg) where T : GameMsg
        {
            Send(JsonConvert.SerializeObject(new BaseMessage(gameMsg) { From = Role }));
        }

        private void Send(string response)
        {
            if (!string.IsNullOrWhiteSpace(response))
            {
                byte[] msg = Encoding.UTF8.GetBytes(response + EndOfStreamText);
                sender.Send(msg);
            }
        }

        public bool Start(IPAddress remoteIP)
        {
            if (Role == Roles.Undefined)
                Role = Roles.Player2;

            try
            {
                if (!sender.Connected)
                    sender.Connect(new IPEndPoint(remoteIP, 2022));

                Task.Run(() =>
                {
                    disposing = false;

                    while (!disposing)
                    {
                        try
                        {
                            string data = null;

                            while (!disposing)
                            {
                                var buffer = new byte[1024];
                                int bytesRec = sender.Receive(buffer);
                                data += Encoding.UTF8.GetString(buffer, 0, bytesRec);
                                if (data.IndexOf(EndOfStreamText) > -1)
                                {
                                    data = data.Replace(EndOfStreamText, string.Empty);
                                    break;
                                }
                            }

                            OnReceived(data);
                        }
                        catch (SocketException excp)
                        {
                            if (excp.ErrorCode != 10035)
                                Dispose();
                            break;
                        }
                        catch (Exception)
                        {

                        }
                    }
                });
            }
            catch (Exception)
            {
                Dispose();
            }

            return sender.Connected;
        }

        internal bool Start(Socket handler)
        {
            sender = handler;
            Role = Roles.Player1;

            return Start(IPAddress.Any);
        }

        private void OnReceived(string json)
        {
            BaseMessage msg = json.ToObject<BaseMessage>();

            if (msg.Name == nameof(MoveMsg))
            {
                OnMessageReceived?.Invoke(msg.Data.ToObject<MoveMsg>());
            }
            else if (msg.Name == nameof(StartMsg))
            {
                OnMessageReceived?.Invoke(msg.Data.ToObject<StartMsg>());
            }

        }

        public void Dispose()
        {
            disposing = true;
            try { using (sender) ; } catch { }

            OnDisposed?.Invoke();
        }
    }
}
