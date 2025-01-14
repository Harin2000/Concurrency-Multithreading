using System.Collections.Concurrent;

namespace H2O
{
    internal class H2O1
    {
        static void Main(string[] args)
        {
            var releaseHydrogen = () => { Console.Write("H"); };
            var releaseOxygen = () => { Console.Write("O"); };

            var h2o = new H2O();
            List<Action> actions = new List<Action>();
            for (int i=0; i < 4; ++i)
            {
                actions.Add(() => h2o.Oxygen(releaseOxygen));
                actions.Add(() => h2o.Hydrogen(releaseHydrogen));
                actions.Add(() => h2o.Hydrogen(releaseHydrogen));
            }

            Parallel.ForEach(actions, (action) => action());
        }

        //static Action<Action> Release(int i, H2O h2o)
        //{
        //    if (i % 3 == 0) return h2o.Oxygen;
        //    else return h2o.Hydrogen;
        //}
    }

    public class H2O
    {
        Dictionary<char, int> h2o = new Dictionary<char, int> { { 'H', 0 }, { 'O', 0 } };
        static object SyncRoot = new();

        public H2O()
        {

        }

        public void Hydrogen(Action releaseHydrogen)
        {

            // releaseHydrogen() outputs "H". Do not change or remove this line.
            lock (SyncRoot)
            {
                while (h2o['H'] == 2)
                {
                    //Console.WriteLine("Waiting for H to restore.");
                    Monitor.Wait(SyncRoot);
                }
                releaseHydrogen();
                h2o['H']++;
                if (h2o['O'] == 1 && h2o['H'] == 2) {
                    //Console.WriteLine("Restore triggered.");
                    h2o['O'] = 0;
                    h2o['H'] = 0;
                }
                Monitor.PulseAll(SyncRoot);
            }
        }

        public void Oxygen(Action releaseOxygen)
        {

            // releaseOxygen() outputs "O". Do not change or remove this line.
            lock (SyncRoot)
            {
                while (h2o['O'] == 1)
                {
                    //Console.WriteLine("Waiting for O to restore.");
                    Monitor.Wait(SyncRoot);
                }
                releaseOxygen();
                h2o['O']++;
                if (h2o['O'] == 1 && h2o['H'] == 2)
                {
                    //Console.WriteLine("Restore triggered.");
                    h2o['O'] = 0;
                    h2o['H'] = 0;
                }
                Monitor.PulseAll(SyncRoot);
            }
        }
    }
}
