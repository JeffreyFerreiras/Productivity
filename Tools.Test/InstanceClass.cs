using Tools.RandomGenerator;

namespace Tools
{
    public class InstanceClass
    {
        public InstanceClass() { }
        public InstanceClass(object arg) { }

        public string Next(int len = 8)
        {
            return RandomString.NextAlphabet(len);
        }
    }
}