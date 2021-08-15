using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleServer
{
	internal sealed class Program
	{
		const string IP = "192.168.0.12";
		const int PORT = 21;

		private static void Main(string[] args)
		{
			Console.Title = "Server";

			var tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse(IP), PORT));

			tcpListener.Start();

			Console.WriteLine($"The server is running");
			Console.WriteLine($"Local endpoint: {tcpListener.LocalEndpoint}");
			Console.WriteLine($"Protocol type: {tcpListener.Server.ProtocolType}");
			Console.WriteLine(string.Empty);

			try
			{
				while (true)
				{
					var tcpClient = tcpListener.AcceptTcpClient();

					if (tcpClient.Connected)
					{
						Console.WriteLine($"Client {tcpClient.Client.RemoteEndPoint} is connected");

						var tcpClientHandler = new TcpClientHandler(tcpClient);
						var tcpClientThread = new Thread(new ThreadStart(tcpClientHandler.Process));

						tcpClientThread.Start();
					}
					else
						Console.WriteLine($"The client is not connected");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Loger.WriteException(ex);
			}
			finally
			{
				tcpListener?.Stop();
			}
		}
	}
}