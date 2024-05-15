using LoanManagement.Models;
using LoanManagement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Services
{
    internal class LoanServices
    {
        public LoanServices() { }
        public void ApplyLoan()
        {
            ILoanRepository loanRepository = new LoanRepositoryImpl();
            Customer newCustomer = new Customer();
            Loan loan = new Loan();

            Console.Write("Enter customer name:");
            newCustomer.Name = Console.ReadLine();
            Console.Write("Enter customer email id:");
            newCustomer.EmailAddress = Console.ReadLine();
            Console.Write("Enter the phone number");
            newCustomer.PhoneNumber = Console.ReadLine();
            Console.Write("Enter the address");
            newCustomer.Address = Console.ReadLine();
            Console.Write("Enter the credit score");
            newCustomer.CreditScore = Convert.ToInt32(Console.ReadLine());

            loan.CustomerId = newCustomer.CustomerId;

            Console.Write("Enter the principal amount:");
            loan.PrincipalAmount = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Enter the interest rate");
            loan.InterestRate = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Enter the loan term in months");
            loan.LoanTerm = int.Parse(Console.ReadLine());
            Console.Write("Enter the loan type:(CarLoan/HomeLoan)");
            loan.LoanType = Console.ReadLine();



            if (loan.LoanType.Equals("CarLoan", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Enter the car model:");
                string carModel = Console.ReadLine();
                Console.Write("Enter the car value:");
                int carValue = Convert.ToInt32(Console.ReadLine());
                loan.LoanId = loanRepository.Applyloan(loan, "CarLoan",  carModel,  carValue);
            }
            else if (loan.LoanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Enter the property address:");
                string propertyAddress = Console.ReadLine();
                Console.Write("Enter the property value:");
                int propertyValue = Convert.ToInt32(Console.ReadLine());
                loan.LoanId = loanRepository.Applyloan(loan, "HomeLoan",  propertyAddress,  propertyValue);
            }

            Console.WriteLine("Do you want to confirm the loan application? (Yes/No)");
            string confirmation = Console.ReadLine().ToLower();
            if (confirmation.Equals("yes"))
            {
                loan.LoanStatus = "Pending";
                Console.WriteLine("Loan application stored in database with status pending.");
            }
            else
            {
                Console.WriteLine("Loan application cancelled.");
            }
        }


    }
}
