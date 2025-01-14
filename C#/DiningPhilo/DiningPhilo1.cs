using System.Collections.Concurrent;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiningPhilo
{
    internal class DiningPhilo1
    {
        static void Main(string[] args)
        {
            ThreadLocal<int> threadLocal = new ThreadLocal<int>();

            var pickLeftFork = () => Console.WriteLine($"{threadLocal.Value} picks left fork.");
            var pickRightFork = () => Console.WriteLine($"{threadLocal.Value} picks right fork.");
            var eat = () => { 
                Console.WriteLine($"{threadLocal.Value} eats.");
                Thread.Sleep(1000);
            };
            var putLeftFork = () => Console.WriteLine($"{threadLocal.Value} puts left fork.");
            var putRightFork = () => Console.WriteLine($"{threadLocal.Value} puts left fork.");

            var diningPhilo = new DiningPhilosophers();

            List<Action> wantsToEatList = new List<Action>();
            for (int i = 0; i < 3; ++i)
            {
                //for(int philo = 0; philo < 5; ++philo)
                //{
                wantsToEatList.Add(() =>
                {
                    threadLocal.Value = 0;
                    diningPhilo.WantsToEat(0, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork);
                });
                wantsToEatList.Add(() =>
                {
                    threadLocal.Value = 1;
                    diningPhilo.WantsToEat(1, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork);
                });
                wantsToEatList.Add(() =>
                {
                    threadLocal.Value = 2;
                    diningPhilo.WantsToEat(2, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork);
                });
                wantsToEatList.Add(() =>
                {
                    threadLocal.Value = 3;
                    diningPhilo.WantsToEat(3, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork);
                });
                wantsToEatList.Add(() =>
                {
                    threadLocal.Value = 4;
                    diningPhilo.WantsToEat(4, pickLeftFork, pickRightFork, eat, putLeftFork, putRightFork);
                });
                //}
            }

            Parallel.Invoke(wantsToEatList.ToArray());
        }

    }

    public class DiningPhilosophers
    {
        Dictionary<int, bool> UsingFork;
        static object SyncRoot = new();
        public DiningPhilosophers()
        {
            UsingFork = new Dictionary<int, bool>();
            UsingFork[0] = false;
            UsingFork[1] = false;
            UsingFork[2] = false;
            UsingFork[3] = false;
            UsingFork[4] = false;
        }

        public void WantsToEat(int philosopher,
                        Action pickLeftFork,
                        Action pickRightFork,
                        Action eat,
                        Action putLeftFork,
                        Action putRightFork)
        {
            int leftFork = philosopher;
            int rightFork = ((philosopher + 1) % 5);

            lock (SyncRoot)
            {
                while (UsingFork[leftFork] || UsingFork[rightFork])
                {
                    Monitor.Wait(SyncRoot);
                }
                pickLeftFork();
                pickRightFork();
                eat();
                putLeftFork();
                putRightFork();
                UsingFork[leftFork] = false;
                UsingFork[rightFork] = false;
                Monitor.PulseAll(SyncRoot);
            }
        }
    }
}
