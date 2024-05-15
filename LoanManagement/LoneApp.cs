using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagement.Services;

namespace LoanManagement
{
    internal class LoneApp
    {
        
        
        public void Menu()
        {
            LoanServices loanServices = new LoanServices();
            while (true)
            {
                Console.WriteLine("Loan Management System");
                Console.WriteLine("1. Apply for a loan");
                Console.WriteLine("2. Calculate Interest");
                Console.WriteLine("3. Loan Status");
                Console.WriteLine("4. Calculate EMI");
                Console.WriteLine("5. Loan Repayment");
                Console.WriteLine("6. Get All Loans");
                Console.WriteLine("7. Get loan By Id");

                Console.WriteLine("8. Exit");
                Console.WriteLine("Enter your choice:");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        loanServices.ApplyLoan();
                        break;
                    case "2":
                        loanServices.CalculateInterest();
                        return;
                    case "3":
                        loanServices.LoanStatus();
                        return;
                    case "4":
                        loanServices.CalculateEMI();
                        return;
                    case "7":
                        loanServices.GetLoanById();
                        return;
                    
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

    }
}
