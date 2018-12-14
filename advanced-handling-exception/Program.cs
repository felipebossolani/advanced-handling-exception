using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;

namespace advanced_handling_exception
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Creator: Felipe Bossolani - fbossolani[at]gmail.com");
            Console.WriteLine(@"Examples based on: http://returnsmart.blogspot.com/2015/07/mcsd-programming-in-c-part-4-70-483.html");
            Console.WriteLine("Choose a Thread Method: ");
            Console.WriteLine("01- FailFast vs Finally");
            Console.WriteLine("02- Simple Custom Exception Class");
            Console.WriteLine("03- ExceptionDispatchInfo Method");
            Console.WriteLine("04- Custom Exception Class - Best Practice");

            int option = 0;
            int.TryParse(Console.ReadLine(), out option);

            switch (option)
            {
                case 1:
                    {
                        FailFast();
                        break;
                    }
                case 2:
                    {
                        SimpleCustomException();
                        break;
                    }
                case 3:
                    {
                        ExceptionDispatchInfoMethod();
                        break;
                    }
                case 4:
                    {
                        CustomClassBestPractices();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid option...");
                        break;
                    }
            }
        }

        private static void CustomClassBestPractices()
        {
            try
            {
                Console.WriteLine("Type the invoice number:");
                string text = Console.ReadLine();
                int i = int.Parse(text);

                throw new InvoiceProcessingException(i, $"Error processing Invoice {i}");
            }
            catch(InvoiceProcessingException ex)
            {
                Console.WriteLine($"InvoiceProcessingException: \n\tInvoiceId: {ex.InvoiceId}\n\tMessage: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected expection: {0}", ex);
            }
        }

        [Serializable]
        public class InvoiceProcessingException : Exception
        {
            private const string HELP_LINK = "http://www.mycompany.com/InvoiceExpectionInformation";
            public int InvoiceId { get; set; }

            public InvoiceProcessingException(int invoiceId)
            {
                InvoiceId = invoiceId;
                this.HelpLink = HELP_LINK;
            }

            public InvoiceProcessingException(int invoiceId, string message) : base(message)
            {
                InvoiceId = invoiceId;
                this.HelpLink = HELP_LINK;
            }

            public InvoiceProcessingException(int invoiceId, string message, Exception innerException) : base(message, innerException)
            {
                InvoiceId = invoiceId;
                this.HelpLink = HELP_LINK;
            }

            protected InvoiceProcessingException(SerializationInfo info, StreamingContext context)
            {
                InvoiceId = (int)info.GetValue("InvoiceId", typeof(int));
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("InvoiceId", InvoiceId, typeof(int));
            }
        }

        private static void ExceptionDispatchInfoMethod()
        {
            ExceptionDispatchInfo possibleException = null;
            
            try
            {
                Console.WriteLine("Type a number:");
                string text = Console.ReadLine();
                int i = int.Parse(text);
            }
            catch(Exception ex)
            {
                possibleException = ExceptionDispatchInfo.Capture(ex);
            }

            if (possibleException != null)
            {
                possibleException.Throw();
            }
            else
            {
                Console.WriteLine("No errors...");
            }
        }

        private static void SimpleCustomException()
        {
            Console.WriteLine("Proccessing monthly salary...");

            try
            {
                ProcessSalary();
            }
            catch(SumException ex)
            {
                throw new WrongSalaryException("Error while processing salary", ex);
            }
        }

        private static decimal ProcessSalary()
        {
            decimal d = 0;
            try
            {
                return 1000M / d;
            }
            catch (Exception ex)
            {
                throw new SumException();
            }
            
        }

        public class SumException : Exception { }
        public class WrongSalaryException : Exception {
            public WrongSalaryException(string message, Exception e) { }
        }

        private static void FailFast()
        {
            Console.WriteLine("Input a number:");
            string s = Console.ReadLine();

            try
            {
                int i = Int32.Parse(s);
                if (i == 10) Environment.FailFast("You typed a special number!");
            }
            finally
            {
                Console.WriteLine("End");
            }
        }
    }
}
