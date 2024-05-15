using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagement.Models;

namespace LoanManagement.Repository
{
    internal interface ILoanRepository 
    {
        public int Applyloan(Loan loan ,string LoanType,string PropertyAddress = null,int PropertyValue = 0,string CarModel = null, int CarValue = 0);
        public new Decimal CalculateInterest(int LoanId);
        public String LoanStatus(int LoanId);
        public decimal CalculateEMI(int loanId);
        public int LoanRepayment(int loanId, decimal amount);
        public List<Loan> GetAllLoans();
        public void GetLoanById(int loanId);

    }
}
