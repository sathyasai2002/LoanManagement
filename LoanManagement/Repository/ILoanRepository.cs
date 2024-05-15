﻿using System;
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
        public decimal CalculateInterest(int LoanId);
        public void LoanStatus(int LoanId);
        public decimal CalculateEMI(int loanId);
        public int LoanRepayment(int loanId, decimal amount);
        public List<Loan> GetAllLoans();
        public Loan GetLoanById(int loanId);

    }
}
