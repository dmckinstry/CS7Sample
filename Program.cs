using System;
using System.Collections.Concurrent;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoOutVariables(new Point());

            int i = 5;
            DemoPatterns(i, new Circle());

            DemoTuplesDeconstructionFunctions(i);

            DemoLiteralImprovements();

            DemoExpressionBodiedMembers();
        }

        //-------------------------------------------------------
        // Out Variables
        public static void DemoOutVariables(Point p)
        {
            //int x, y; // have to "predeclare"
            //p.GetCoordinates(out x, out y);
            p.GetCoordinates(out var x, out var y);

            Console.WriteLine($"({x}, {y})");
        }

        //-------------------------------------------------------
        // Patterns
        public static void DemoPatterns(object o, object shape)
        {
            if (o is int i || (o is string s && int.TryParse(s, out i))) { /* use i */ };

            switch (shape)
            {
                case Circle c:
                    Console.WriteLine($"circle with radius {c.Radius}");
                    break;

                case Rectangle sq when (sq.Length == sq.Height):
                    Console.WriteLine($"{sq.Length} x {sq.Height} square");
                    break;

                case Rectangle r:
                    Console.WriteLine($"{r.Length} x {r.Height} rectangle");
                    break;

                default:
                    Console.WriteLine("<unknown shape>");
                    break;

                case null:
                    throw new ArgumentNullException(nameof(shape));
            }
        }

        //-------------------------------------------------------
        // Tuples, Deconstruction and internal functions
        public static void DemoTuplesDeconstructionFunctions(int id)
        {
            // Calling a Tuples functions
            var names = LookupName(id);
            Console.WriteLine($"found {names.first} {names.middle} {names.last}.");

            // Calling with Deconstruction
            var (fname, _, lname) = LookupName(id);
            Console.WriteLine($"found {fname} {lname}.");

            // Internal function with a tuple return type.... (via NuGet)
            (string first, string middle, string last) LookupName(int i) // tuple return type
            {
                // retrieve first, middle and last from data storage
                string first = "John", middle = "Qunicy", last = "Adams";
                return (first, middle, last); // tuple literal
            }
        }

        //-------------------------------------------------------
        // Literals
        public static void DemoLiteralImprovements()
        {
            var oneMillion = 1_000_000;
            var bits = 0b1111_0000_0000_1101;

            Console.WriteLine($"{oneMillion} Cheerios is still a lot of {bits}");
        }

        //-------------------------------------------------------
        // Ref returns and locals 
        public static void DemoRefReturnsAndLocals()
        {
            // Simple internal function to look for a match in an array and return ref
            ref int Find(int number, int[] numbers)
            {
                for (int i = 0; i < numbers.Length; i++)
                {
                    if (numbers[i] == number)
                    {
                        return ref numbers[i]; // return the storage location, not the value
                    }
                }
                throw new IndexOutOfRangeException($"{nameof(number)} not found");
            }

            // Create an array and change the 5th element from "7" to "9"
            int[] array = { 1, 15, -39, 0, 7, 14, -12 };
            Console.Write($"The value at position 5 was {array[4]}...");

            ref int place = ref Find(7, array); // aliases 7's place in the array
            place = 9; // replaces 7 with 9 in the array

            Console.WriteLine($" but has been changed to {array[4]}.");
        }

        //-------------------------------------------------------
        // Expression Bodied members (from C#6 and C#7)
        public static void DemoExpressionBodiedMembers()
        {
            // Before Expression Bodied Members
            int DoubleInCs5(int i)
            {
                return i * 2;
            }

            // As introduced in C# 6
            int DoubleInCs6(int i) => i * 2;

            // Review what Expression Bodied means
            Console.WriteLine($"Double using old method: {DoubleInCs5(1)}");
            Console.WriteLine($"Double using old method: {DoubleInCs6(2)}");

            // Now referencing Expression Bodied in constructors, finalizers and accessors
            var p = new Person("Dave");
            Console.WriteLine($"{p.Name}'s not here, man.");
        }

        class Person
        {
            private static ConcurrentDictionary<Guid, string> names = new ConcurrentDictionary<Guid, string>();
            private Guid id = Guid.NewGuid();

            // Expression Bodied in constructors and finalizers
            public Person(string name) => names.TryAdd(id, name); // constructors
            ~Person() => names.TryRemove(id, out _);              // finalizers

            // Expression Bodied in Accessors
            public string Name
            {
                get => names[id];                                 // getters
                set => names[id] = value;                         // setters
            }
        }

    }
}