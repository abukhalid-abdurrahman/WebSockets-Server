using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WebSockets
{
    class Program
    {
        static void Main(string[] args)
        {
            // Данные для ответа пользователю
            string responseString = "<p>Successfully conected to server!<p>";
            byte[] responseData = Encoding.UTF8.GetBytes(responseString);

            // Создание сервера по адресу 127.0.0.1:80
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
            // Запуск сервера
            server.Start();

            Console.WriteLine("Server started at 127.0.0.1 on port 80...");

            try
            {
                while (true)
                {
                    // Создание пользователя который подключится к серверу
                    TcpClient client = server.AcceptTcpClient();
                    // IP-адрес пользователя
                    string clientIP = ((IPEndPoint)(client.Client.RemoteEndPoint)).Address.ToString();

                    Console.WriteLine($"Client {clientIP} connected to server...");

                    // Получение канала соединения (handshake)
                    NetworkStream stream = client.GetStream();
                    // Запиь данных для ответа пользователю в соединенный канал
                    stream.Write(responseData, 0, responseData.Length);

                    Console.WriteLine($"Response to client {clientIP}: {responseData}");

                    // Закрываем соединения
                    stream.Close();
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"500 Internal Server Error\r\n{ex.Message}");
            }
            finally
            {
                // Отключение сервера
                if (server != null)
                    server.Stop();
            }
        }
    }
}