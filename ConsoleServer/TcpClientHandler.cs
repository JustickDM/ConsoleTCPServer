using System;
using System.IO;
using System.Net.Sockets;

namespace ConsoleServer
{
	internal sealed class TcpClientHandler
	{
		private readonly TcpClient _tcpClient;
		
		public TcpClientHandler(TcpClient tcpClient) => _tcpClient = tcpClient;

		public void Process()
		{
            try
            {
				using var networkStream = _tcpClient.GetStream();
				using var streamReader = new BinaryReader(networkStream);
				using var streamWriter = new BinaryWriter(networkStream);

				var clientMessage = streamReader.ReadString();

				Console.WriteLine($"Client {_tcpClient.Client.RemoteEndPoint}: {clientMessage}");

				streamWriter.Write(clientMessage);
				streamWriter.Flush();
			}
			catch (Exception ex)
            {
				Console.WriteLine(ex.Message);
				Loger.WriteException(ex);
            }
			finally
			{
				_tcpClient.Close();
			}
        }
	}
}
