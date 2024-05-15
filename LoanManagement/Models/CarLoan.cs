using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Models
{
    internal class CarLoan : Loan
    {
        public string CarModel { get; set; }
        public int CarValue { get; set; }

        public CarLoan(int loanId, int customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanStatus, string carModel, int carValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, loanStatus)
        {
            CarModel = carModel;
            CarValue = carValue;
        }

        public override string ToString()
        {
            return base.ToString() + $", Car Model: {CarModel}, Car Value: {CarValue:C}";
        }
    }
}
