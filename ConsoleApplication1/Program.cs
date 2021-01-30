using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Program
    {
        static int port = 8005;
        static void Main(string[] args)
        {
            int counterS = 0;
            string buttonName, buttonContent;


            TcpListener serverSocket = new TcpListener(port);
            TcpClient clientSocket1 = default(TcpClient);
            TcpClient clientsocket2 = default(TcpClient);
            serverSocket.Start();
            Console.WriteLine("server waiting for clients......");
  
            //connection client1
            clientSocket1 = serverSocket.AcceptTcpClient();
            counterS++;
            Console.WriteLine("The client " + counterS + " connected");
            NetworkStream streamC1 = clientSocket1.GetStream();
            BinaryWriter writer1 = new BinaryWriter(streamC1);
            BinaryReader reader1 = new BinaryReader(streamC1);
            writer1.Write("Hello Player You are X, please wait for another player\n");
            //writer1.Flush();
            //streamC1.Flush();
            Console.WriteLine("send server to client1 ......");
         
            //connection client2
            clientsocket2 = serverSocket.AcceptTcpClient();
            counterS++;
            Console.WriteLine("The client " + counterS + " connected");
            NetworkStream streamC2 = clientsocket2.GetStream();
            BinaryWriter writer2 = new BinaryWriter(streamC2);
            BinaryReader reader2 = new BinaryReader(streamC2);
            writer1.Write("player 2 has been connected press continue to play, good luck");
            //writer1.Flush();
            //streamC1.Flush();
            writer2.Write("Hello player You are O, please enter continue");
            writer2.Write("good luck");
            //writer2.Flush();
            //streamC2.Flush();
            Console.WriteLine("send server to client2 ......");


            while (true)
            {
               
                buttonName = reader1.ReadString();
                Console.WriteLine("from c1 buttonName= " + buttonName);
                buttonContent = reader1.ReadString();
                Console.WriteLine("from c1 buttonContent= " + buttonContent);
                writer2.Write(buttonName);
                Console.WriteLine("Server send to C2 buttonName = " + buttonName);
                writer2.Write(buttonContent);
                Console.WriteLine("Server send to C2 buttonContent = " + buttonContent);

              
                buttonName = reader2.ReadString();
                Console.WriteLine("from C2 buttonName= " + buttonName);
                buttonContent = reader2.ReadString();
                Console.WriteLine("from C2 buttonContent= " + buttonContent);

                writer1.Write(buttonName);
                Console.WriteLine("Server send to C1 buttonName = " + buttonName);
                writer1.Write(buttonContent);
                Console.WriteLine("Server send to C1 buttonContent = " + buttonContent);


            }

            








        }
    }
}
