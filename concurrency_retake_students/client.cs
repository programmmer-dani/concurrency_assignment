using System;
using System.Threading;

namespace Concurrency_retake
{
    public class Client
    {
        // you can add more code below this line
        public Thread thread;
        // you can add more code above this line
        // Please do not alter the variables below
        private int clientId;

        private LinkedList<Order> orderLocation;
        private LinkedList<Portion> pickupPoint;

        public Client(int clientId, LinkedList<Order> orderLocation, LinkedList<Portion> pickupPoint)
        //you can alter the code and the parametersin here
        {
            this.orderLocation = orderLocation;
            this.pickupPoint = pickupPoint;
            this.clientId = clientId;
        }

        public void Start() //you can alter the code in here if needed
        {
            thread = new Thread(Life);
            thread.Start();
        }

        public void Life() //you should only add protections in this method
        {
            Console.WriteLine($"+Client {clientId} is about to place an order.");
            // order placement
            Order order = new();

            lock (orderLocation) { orderLocation.AddFirst(order); }


            Program.orderSemaphore.Release();

            Console.WriteLine($"+Client {clientId} has placed an order.");

            // waiting for the preparation of the order
            Thread.Sleep(new Random().Next(100, 500));


            Portion? tmpmyfood;

            // order pickup from the tray
            Console.WriteLine($"+Client {clientId} is about topick up the order.");

            Program.pickupSemaphore.WaitOne();

            lock (pickupPoint)
            {
                tmpmyfood = pickupPoint.First();
                pickupPoint.RemoveFirst();
            }

            // the order is picked up
            // the client can now leave.

            Console.WriteLine($"+Client {clientId} has finished {tmpmyfood.ToString()}.");
        }
    }
}
