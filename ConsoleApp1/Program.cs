using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {

            string? name = null;
            //   var myName = name!.ToString(); // Null Forgiving Operator   

            //int i = int.MaxValue;

            //    int k = checked(i + 1);
            //    Console.WriteLine(k);

            //IDisplay impObj = new Imp();
            //impObj.Print();
            //impObj.Display();

            Imp impObj = new Imp();

            //impObj.Calculate(5, 6);

            DateTime date = DateTime.Now.Date;
            for(int i=15;i<=135;i=i+15)
            {
                Console.WriteLine(date.AddMinutes(i).ToString("{hh:mm}"));
            }

           // Console.WriteLine(DateTime.Now.Date);
        }
    }

    interface IDisplay
    {
        public void Print()
        {
            Console.WriteLine("Print1");
        }

        public void Display()
        {
            Console.WriteLine("Print1");
        }
    }

    class Imp:IDisplay
    {
        public void Print()
        {
            Console.WriteLine("Print2");
        }

        public int Calculate(int x,int y,out int result)
        {
            result = x + y;
            return result;
        }
    }

   


}
