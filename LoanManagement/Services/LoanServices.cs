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

           /* Console.Write("Enter customer name:");
            newCustomer.Name = Console.ReadLine();
            Console.Write("Enter customer email id:");
            newCustomer.EmailAddress = Console.ReadLine();
            Console.Write("Enter the phone number");
            newCustomer.PhoneNumber = Console.ReadLine();
            Console.Write("Enter the address");
            newCustomer.Address = Console.ReadLine();
            Console.Write("Enter the credit score");
            newCustomer.CreditScore = Convert.ToInt32(Console.ReadLine());*/

            //loan.CustomerId = newCustomer.CustomerId;
            Console.Write("Enter customer Id:");
            loan.CustomerId = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the principal amount:");
            loan.PrincipalAmount = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Enter the interest rate");
            loan.InterestRate = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Enter the loan term in months");
            loan.LoanTerm = int.Parse(Console.ReadLine());
            Console.Write("Enter the loan type:(CarLoan/HomeLoan)");
            loan.LoanType = Console.ReadLine();
            loan.LoanStatus = "Pending";


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
                
                Console.WriteLine("Loan application stored in database with status pending.");
            }
            else
            {
                Console.WriteLine("Loan application cancelled.");
            }
        }

        public Decimal CalculateInterest() 
        {

            ILoanRepository loanRepository = new LoanRepositoryImpl();

            Console.WriteLine("Enter Loan Id");
            int loanId = Convert.ToInt32(Console.ReadLine());

            decimal interestAmount = loanRepository.CalculateInterest(loanId);
            Console.WriteLine($"Interest amount for loan ID {loanId} is:{interestAmount}");

            return 0;

        }

        public void LoanStatus()
        {
            ILoanRepository loanRepository = new LoanRepositoryImpl();
            Console.WriteLine("Enter Loan Id");
            int loanId = Convert.ToInt32(Console.ReadLine());

            String Status = loanRepository.LoanStatus(loanId);
            Console.WriteLine($"Interest amount for loan ID {loanId} is:{Status}");
        }

        public Decimal CalculateEMI()
        {
            ILoanRepository loanRepository = new LoanRepositoryImpl();
            Console.WriteLine("Enter Loan Id");
            int loanId = Convert.ToInt32(Console.ReadLine());

            Decimal EMI = loanRepository.CalculateEMI(loanId);
            Console.WriteLine($"EMI FOR the {loanId} is:{EMI}");
            return 0;

        }

        public void GetLoanById()
        {
            ILoanRepository loanRepository = new LoanRepositoryImpl();
            Console.WriteLine("Enter the Loan-ID:");
            int loanId = Convert.ToInt32(Console.ReadLine());

            List<Loan> loans = loanRepository.GetLoanById(loanId);

            if (loans != null && loans.Any())
            {
                Console.WriteLine("Loans for User using UserID " + loanId);
                foreach (var loan in loans)
                {
                    Console.WriteLine($"Loan ID: {loan.LoanId}, Principal Amount: {loan.PrincipalAmount}, Interest Rate: {loan.InterestRate}, Loan Term: {loan.LoanTerm}");
                }
            }



    }
}
