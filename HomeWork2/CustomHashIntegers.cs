using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork2
{
    public abstract class CustomHashInt
    {
        public int x;
        public abstract override int GetHashCode();
    }

    public class ConstHashInt : CustomHashInt
    {
        public override int GetHashCode() => 42;
    }

    public class SelfHashInt : CustomHashInt
    {
        public override int GetHashCode() => x;
    }

    public class SimpleHashInt : CustomHashInt
    {
        public override int GetHashCode() => ((x >> 16) ^ 16) * 0x45b9f3b;
    }

    public class ComplexHashInt : CustomHashInt
    {
        public override int GetHashCode() => 101 * ((x >> 24) + 101 * ((x >> 16) + 101 * (x >> 8))) + x;
    }
}
