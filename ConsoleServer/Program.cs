using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ConsoleServer
{
	internal sealed class Program
	{
		const string IP = "127.0.0.1";
		const int PORT = 15000;

		static void Main(string[] args)
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
					Console.WriteLine($"Waiting for connection...");

					using (var client = tcpListener.AcceptTcpClient())
					{
						if (client.Connected)
						{
							Console.WriteLine($"The client is connected");
							Console.WriteLine($"Client remote endpoint: {client.Client.RemoteEndPoint}");

							using (var networkStream = client.GetStream())
							{
								Console.WriteLine($"Waiting for a client message...");

								using (var streamReader = new StreamReader(networkStream))
								{
									var clientMessage = streamReader.ReadLine();

									Console.WriteLine($"Client message: {clientMessage}");
									Console.Write($"Your message: ");

									var serverMessage = Console.ReadLine();

									using (var streamWriter = new StreamWriter(networkStream) { AutoFlush = true })
									{
										streamWriter.WriteLine(serverMessage);
									}
								}
							}

							Console.WriteLine(string.Empty);
						}
						else
							Console.WriteLine($"The client is not connected");
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				tcpListener?.Stop();
			}
		}
	}
}