using LoanManagement.Services;
using LoanManagement.Repository;

namespace LoanManagement
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LoneApp loneApp = new LoneApp();
            loneApp.Menu();
            /*ILoanRepository loanRepository = new LoanRepositoryImpl();
            loanRepository.CalculateEMI(7);*/
        }
    }
}
