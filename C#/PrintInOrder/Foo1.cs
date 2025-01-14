using System.Runtime.InteropServices;

namespace PrintInOrder
{
    internal class Foo1
    {
        static void Main(string[] args)
        {
            var printFirst = () => { Console.WriteLine("first"); };
            var printSecond = () => { Console.WriteLine("second"); };
            var printThird = () => { Console.WriteLine("third"); };

            var foo = new Foo();
            Parallel.Invoke(
                () => foo.First(printFirst),
                () => foo.Second(printSecond),
                () => foo.Third(printThird)
            );
        }
    }

    public class Foo
    {
        static object SyncRoot = new();
        int m_Next = 1;
        public Foo()
        {

        }

        public void First(Action printFirst)
        {

            // printFirst() outputs "first". Do not change or remove this line.
            lock (SyncRoot)
            {
                printFirst();
                ++m_Next;
                Monitor.PulseAll(SyncRoot);
            }
        }

        public void Second(Action printSecond)
        {

            // printSecond() outputs "second". Do not change or remove this line.
            lock (SyncRoot) {
                while(m_Next < 2)
                {
                    Monitor.Wait(SyncRoot);
                }
                printSecond();
                ++m_Next;
                Monitor.PulseAll(SyncRoot);
            }
        }

        public void Third(Action printThird)
        {

            // printThird() outputs "third". Do not change or remove this line.
            lock (SyncRoot) {
                while(m_Next < 3)
                {
                    Monitor.Wait(SyncRoot);
                }
                printThird();
                ++m_Next;
            }
        }
    }
}
