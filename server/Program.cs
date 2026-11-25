//Console.WriteLine("Hello from Server!");
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

//DEVO CREARE UN NUOVO THREAD, (COME SE USASSI 2 CLIENT)

namespace Server
{
    // Server TCP minimale: accetta un client alla volta e risponde a ogni messaggio.
    class Program
    {
        const int PORTA = 5000;

        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, PORTA);
            listener.Start();
            Console.WriteLine("Server In ascolto sulla porta " + PORTA + "...");

            while (true)
            {
                // Bloccante: attende che un client si connetta
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connesso: " + client.Client.RemoteEndPoint);
                Thread thread = new Thread(() => EseguiLavoro(client));
                thread.Start(); // Avvia il thread
                //thread.Join(); // Stiamo dicendo al thread di aspettare, non ha molto senso...
                Console.WriteLine("Client disconnesso.");
            }
        }
        static void EseguiLavoro(TcpClient client)
        {
            using (client)
            {
                Console.WriteLine("Lavoro in esecuzione su un nuovo thread.");
                using (NetworkStream stream = client.GetStream())
                    {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                            {
                                writer.AutoFlush = true;

                                string messaggio;
                                // ReadLine restituisce null quando il client chiude la connessione
                                while ((messaggio = reader.ReadLine()) != null)
                                {
                                    Console.WriteLine("Ricevuto: " + messaggio);
                                    writer.WriteLine("ECO: " + messaggio);

                                    if (messaggio.ToUpper() == "FINE")
                                    {
                                        break;
                                    }

                                }
                            }
                        }
                    }
            }
            
        }
    }
    
}