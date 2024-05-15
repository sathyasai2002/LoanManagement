using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Models
{
    internal class HomeLoan: Loan
    {
        public string PropertyAddress { get; set; }
        public int PropertyValue { get; set; }

        public HomeLoan(int loanId, int customer, decimal principalAmount, decimal interestRate, int loanTerm, string loanStatus, string propertyAddress, int propertyValue)
            : base(loanId, customer, principalAmount, interestRate, loanTerm, loanStatus)
        {
            PropertyAddress = propertyAddress;
            PropertyValue = propertyValue;
        }

        public override string ToString()
        {
            return base.ToString() + $", Property Address: {PropertyAddress}, Property Value: {PropertyValue:C}";
        }
    }
}
