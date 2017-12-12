namespace Tools
{
    public class InstanceClass
    {
        public string Next(int len = 8)
        {
            return RandomString.NextAlphabet(len);
        }
    }
}