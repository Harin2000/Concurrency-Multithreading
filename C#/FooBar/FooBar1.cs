using System.Threading.Channels;

namespace FooBar
{
    internal class FooBar1
    {
        static void Main(string[] args)
        {
            var printFoo = () => { Console.WriteLine("foo"); };
            var printBar = () => { Console.WriteLine("bar"); };

            var foobar = new FooBar(5);

            Parallel.Invoke(
                () => foobar.Foo(printFoo),
                () => foobar.Bar(printBar)
                );
        }
    }

    public class FooBar
    {
        private int n;
        static object SyncRoot = new();
        int mInt = 0;

        public FooBar(int n)
        {
            this.n = n;
        }

        public void Foo(Action printFoo)
        {

            for (int i = 0; i < n; i++)
            {

                // printFoo() outputs "foo". Do not change or remove this line.
                lock (SyncRoot)
                {
                    while (mInt % 2 != 0) {
                        Monitor.Wait(SyncRoot);
                    }
                    printFoo();
                    ++mInt;
                    Monitor.Pulse(SyncRoot);
                }
            }
        }

        public void Bar(Action printBar)
        {

            for (int i = 0; i < n; i++)
            {

                // printBar() outputs "bar". Do not change or remove this line.
                lock (SyncRoot)
                {
                    while (mInt % 2 == 0)
                    {
                        Monitor.Wait(SyncRoot);
                    }
                    printBar();
                    ++mInt;
                    Monitor.Pulse(SyncRoot);
                }
            }
        }
    }
}
