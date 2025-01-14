namespace ZeroEvenOdd
{
    internal class ZeroEvenOdd1
    {
        static void Main(string[] args)
        {
            Action<int> printNumber = (i) => { Console.WriteLine(i); };
            var zeroEvenOdd = new ZeroEvenOdd(15);

            Parallel.Invoke(
                () => zeroEvenOdd.Odd(printNumber),
                () => zeroEvenOdd.Even(printNumber),
                () => zeroEvenOdd.Zero(printNumber));
        }
    }

    // TODO: Here, the soln worked even if lock was outside and while loop was inside,
    // in FizzBuzz1, it was opposite, which one is better? - I think this one.
    public class ZeroEvenOdd
    {
        private int n;
        enum State
        {
            Zero,
            Even,
            Odd
        }
        bool m_Continue = true;
        State m_CurrentState = State.Zero;
        int i = 1;
        static object SyncRoot = new();
        private void NextCondition()
        {
            if (m_CurrentState == State.Zero)
            {
                if (i % 2 == 0)
                {
                    m_CurrentState = State.Even;
                }
                else m_CurrentState = State.Odd;
            }
            else
            {
                m_CurrentState = State.Zero;
                ++i;
                if (i > n) m_Continue = false;
            }
            Monitor.PulseAll(SyncRoot);
        }

        public ZeroEvenOdd(int n)
        {
            this.n = n;
        }

        // printNumber(x) outputs "x", where x is an integer.
        public void Zero(Action<int> printNumber)
        {
            lock (SyncRoot)
            {
                while (true)
                {
                    while (m_Continue && m_CurrentState != State.Zero)
                    {
                        Monitor.Wait(SyncRoot);
                    }
                    if (!m_Continue)
                    {
                        Monitor.PulseAll(SyncRoot);
                        return;
                    }
                    printNumber(0);
                    NextCondition();
                }
            }
        }

        public void Even(Action<int> printNumber)
        {
            lock (SyncRoot)
            {
                while (true)
                {
                    while (m_Continue && m_CurrentState != State.Even)
                    {
                        Monitor.Wait(SyncRoot);
                    }
                    if (!m_Continue)
                    {
                        Monitor.PulseAll(SyncRoot);
                        return;
                    }
                    printNumber(i);
                    NextCondition();
                }
            }
        }

        public void Odd(Action<int> printNumber)
        {
            lock (SyncRoot)
            {
                while (true)
                {
                    while (m_Continue && m_CurrentState != State.Odd)
                    {
                        Monitor.Wait(SyncRoot);
                    }
                    if (!m_Continue)
                    {
                        Monitor.PulseAll(SyncRoot);
                        return;
                    }
                    printNumber(i);
                    NextCondition();
                }
            }
        }
    }
}
