using Common.Tools.RandomGenerator;

namespace Common.Tools
{
    public class InstanceClass
    {
        public InstanceClass()
        {
        }

        public InstanceClass(object arg)
        {
        }

        public string Next(int len = 8)
        {
            return RandomString.NextAlphabet(len);
        }
    }
}