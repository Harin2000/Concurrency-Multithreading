namespace RoughConsoleAppWithoutTopLvlStarements
{
    internal class FizzBuzz1
    {
        static void Main(string[] args)
        {
            var printFizz = () => { Console.WriteLine("Fizz"); };
            var printBuzz = () => { Console.WriteLine("Buzz"); };
            var printFizzBuzz = () => { Console.WriteLine("FizzBuzz"); };
            Action<int> printNumber = i => { Console.WriteLine(i); };

            var fb = new FizzBuzz(15);

            Parallel.Invoke(
                () => fb.Fizz(printFizz),
                () => fb.Buzz(printBuzz),
                () => fb.Fizzbuzz(printFizzBuzz),
                () => fb.Number(printNumber)
             );
        }
    }

    public class FizzBuzz
    {
        private int n;

        int m_Next = 1;
        static object SyncRoot = new();
        bool m_Continue = true;
        enum State
        {
            Fizz,
            Buzz,
            FizzBuzz,
            Number
        };
        State m_State = State.Number;

        private void NextCondition()
        {
            ++m_Next;
            if (m_Next > n)
            {
                m_Continue = false;
            }

            if (m_Next % 15 == 0) m_State = State.FizzBuzz;
            else if (m_Next % 3 == 0) m_State = State.Fizz;
            else if (m_Next % 5 == 0) m_State = State.Buzz;
            else m_State = State.Number;

            Monitor.PulseAll(SyncRoot);
        }

        public FizzBuzz(int n)
        {
            this.n = n;
        }

        // printFizz() outputs "fizz".
        public void Fizz(Action printFizz)
        {
            while (true)
            {
                lock (SyncRoot)
                {
                    while (m_Continue && m_State != State.Fizz)
                    {
                        Monitor.Wait(SyncRoot);
                    }
                    if (!m_Continue) return;
                    printFizz();
                    NextCondition();

                }
            }
        }

        // printBuzzz() outputs "buzz".
        public void Buzz(Action printBuzz)
        {
            while (true)
            {
                lock (SyncRoot)
                {
                    while (m_Continue && m_State != State.Buzz)
                    {
                        Monitor.Wait(SyncRoot);
                    }
                    if (!m_Continue) return;
                    printBuzz();
                    NextCondition();
                }
            }
        }

        // printFizzBuzz() outputs "fizzbuzz".
        public void Fizzbuzz(Action printFizzBuzz)
        {
            while (true)
            {
                lock (SyncRoot)
                {
                    while (m_Continue && m_State != State.FizzBuzz)
                    {
                        Monitor.Wait(SyncRoot);
                    }
                    if (!m_Continue) return;
                    printFizzBuzz();
                    NextCondition();
                }
            }
        }

        // printNumber(x) outputs "x", where x is an integer.
        public void Number(Action<int> printNumber)
        {
            while (true)
            {
                lock (SyncRoot)
                {
                    while (m_Continue && m_State != State.Number)
                    {
                        Monitor.Wait(SyncRoot);
                    }
                    if (!m_Continue) return;
                    printNumber(m_Next);
                    NextCondition();
                }
            }
        }
    }
}