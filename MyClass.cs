namespace SWEN_Game
{
    public class MyClass
    {
        public int Euklid(int x, int y)
        {
            if (x < 0)
            {
                x = -x;
            }

            if (y < 0)
            {
                y = -y;
            }

            if (x == 0)
            {
                return y;
            }

            while (y != 0)
            {
                if (x > y)
                {
                    x = x - y;
                }
                else
                {
                    y = y - x;
                }
            }

            return x;
        }
    }
}
