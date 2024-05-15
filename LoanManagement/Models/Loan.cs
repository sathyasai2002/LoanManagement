using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Models
{
    internal class Loan
    {
        public int LoanId { get; set; }
        public int CustomerId { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanTerm { get; set; } 
        public string LoanType { get; set; } 
        public string LoanStatus { get; set; } 

        public Loan(int loanId, int customerId, decimal principalAmount, decimal interestRate, int loanTerm, string loanType, string loanStatus)
        {
            LoanId = loanId;
            CustomerId = customerId;
            PrincipalAmount = principalAmount;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanType = loanType;
            LoanStatus = loanStatus;
        }

        public Loan(int loanId, int customerId, decimal principalAmount, decimal interestRate, int loanTerm, string loanStatus)
        {
            LoanId = loanId;
            CustomerId = customerId;
            PrincipalAmount = principalAmount;
            InterestRate = interestRate;
            LoanTerm = loanTerm;
            LoanStatus = loanStatus;
        }

        public Loan()
        {
        }

        public override string ToString()
        {
            return $" {LoanId},{CustomerId}, {PrincipalAmount:C}, {InterestRate:P},{LoanTerm},{LoanType},{LoanStatus}";
        }
    }
}
