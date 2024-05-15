using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using LoanManagement.Utility;
using LoanManagement.Models;
using System.Diagnostics.Metrics;

namespace LoanManagement.Repository
{
    internal class LoanRepositoryImpl : ILoanRepository
    {
        SqlConnection sqlConnection = null;
        SqlCommand cmd = null;
        public LoanRepositoryImpl()
        {
            cmd = new SqlCommand();
            sqlConnection = new SqlConnection(DbUtil.GetConnectionString());

        }
        public int Applyloan(Loan loan, string LoanType, string propertyAddress = null, int propertyValue = 0, string carModel = null, int carValue = 0)
        {
            int LoanId;

            sqlConnection.Open();
            cmd.CommandText = "Insert into Loan values(@CustomerID, @PricipalAmount, @InterestRate, @LoanTerm, @LoanType, @LoanStatus,)";
            cmd.Parameters.AddWithValue("@CustomerID", loan.CustomerId);
            cmd.Parameters.AddWithValue("@PrincipalAmount", loan.PrincipalAmount);
            cmd.Parameters.AddWithValue("@InterestRate", loan.InterestRate);
            cmd.Parameters.AddWithValue("@LoanTerm", loan.LoanTerm);
            cmd.Parameters.AddWithValue("@LoanType", loan.LoanType);
            cmd.Parameters.AddWithValue("@LoanStatus", loan.LoanStatus);

            if (LoanType.Equals("HomeLoan", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters.AddWithValue("@PropertyAddress", propertyAddress);
                cmd.Parameters.AddWithValue("@PropertyValue", propertyValue);
                cmd.Parameters.AddWithValue("@CarModel", DBNull.Value);
                cmd.Parameters.AddWithValue("@CarValue", 0);
            }
            else if (LoanType.Equals("CarLoan", StringComparison.OrdinalIgnoreCase))
            {
                cmd.Parameters.AddWithValue("@PropertyAddress", DBNull.Value);
                cmd.Parameters.AddWithValue("@PropertyValue", 0);
                cmd.Parameters.AddWithValue("@CarModel", carModel);
                cmd.Parameters.AddWithValue("@CarValue", carValue);
            }

            cmd.Connection = sqlConnection;
            cmd.ExecuteNonQuery();
            LoanId = Convert.ToInt32(cmd.ExecuteScalar());
            return LoanId;
            sqlConnection.Close();
        }

        public new Decimal CalculateInterest(int LoanId)
        {
            sqlConnection.Open();
            int interestAmount=0;
            cmd.CommandText = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId";
            cmd.Parameters.AddWithValue("@LoanId", LoanId);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    decimal principalAmount = reader.GetDecimal(0);
                    decimal interestRate = reader.GetDecimal(1);
                    int loanTerm = reader.GetInt32(2);

                    interestAmount = Convert.ToInt32(principalAmount * interestRate * loanTerm) / 12;
                }
                
            }
            return interestAmount;
            sqlConnection.Close();

        }
        public void LoanStatus(int LoanId)
        {
            sqlConnection.Open();
            cmd.CommandText = "SELECT CustomerId, CreditScore FROM Customer INNER JOIN Loan ON Customer.CustomerId = Loan.CustomerId WHERE LoanId = @LoanId;";
            cmd.Parameters.AddWithValue("@LoanId", LoanId);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    int customerId = reader.GetInt32(0);
                    int creditScore = reader.GetInt32(1);

                    string status = (creditScore > 650) ? "Approved" : "Rejected";

                    string updateQuery = "UPDATE Loan SET LoanStatus = @Status WHERE LoanId = @LoanId;";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, sqlConnection))
                    {
                        updateCmd.Parameters.AddWithValue("@LoanId", LoanId);
                        updateCmd.Parameters.AddWithValue("@Status", status);
                        updateCmd.ExecuteNonQuery();
                    }

                    Console.WriteLine($"Loan with ID {LoanId} is {status}.");
                }
            }

        }
        public Decimal CalculateEMI(int loanId) 
        {
            decimal emi=0;
            sqlConnection.Open();
            cmd.CommandText = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId;";
            cmd.Parameters.AddWithValue("@LoanId", loanId);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                decimal principalAmount = reader.GetDecimal(0);
                decimal interestRate = reader.GetDecimal(1);
                int loanTerm = reader.GetInt32(2);

                decimal monthlyInterestRate = interestRate / 12 / 100;

                emi = (principalAmount * monthlyInterestRate * (decimal)Math.Pow(1 + (double)monthlyInterestRate, loanTerm))
                        / ((decimal)Math.Pow(1 + (double)monthlyInterestRate, loanTerm) - 1);
            }
            return emi;

        }
        public int LoanRepayment(int loanId, decimal amount) 
        {
            sqlConnection.Open();
            int numberOfEmisPaid = 0;
            decimal emi = CalculateEMI(loanId);

            if (amount < emi)
            {
                Console.WriteLine("Amount is less than a single EMI. Payment rejected.");
                return numberOfEmisPaid;
            }
            numberOfEmisPaid = (int)(amount / emi);
            cmd.CommandText = "UPDATE Loan SET NumberOfEmisPaid = NumberOfEmisPaid + @NumberOfEmisPaid WHERE LoanId = @LoanId;";
            cmd.Parameters.AddWithValue("@NumberOfEmisPaid", numberOfEmisPaid);
            cmd.Parameters.AddWithValue("@LoanId", loanId);
            cmd.ExecuteNonQuery();
            Console.WriteLine($"Successfully paid {numberOfEmisPaid} EMIs.");
            return numberOfEmisPaid;
        }
        public List<Loan> GetAllLoans()
        {
            sqlConnection.Open();
            List<Loan> loans = new List<Loan>();
            cmd.CommandText = "SELECT * from Loan";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Loan loan = new Loan
                {
                    LoanId = reader.GetInt32(0),
                    CustomerId = reader.GetInt32(1),
                    PrincipalAmount = reader.GetDecimal(2),
                    InterestRate = reader.GetDecimal(3),
                    LoanTerm = reader.GetInt32(4),
                    LoanType = reader.GetString(5),
                    LoanStatus = reader.GetString(6),
                };

                loans.Add(loan);

            }
            return loans;
        }

        public Loan GetLoanById(int loanId)
        {
            Loan loan = null;
            sqlConnection.Open();
            cmd.CommandText = "SELECT * FROM Loan WHERE LoanId = @LoanId;";
            cmd.Parameters.AddWithValue("@LoanId", loanId);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                loan = new Loan
                {
                    LoanId = reader.GetInt32(0),
                    CustomerId = reader.GetInt32(1),
                    PrincipalAmount = reader.GetDecimal(2),
                    InterestRate = reader.GetDecimal(3),
                    LoanTerm = reader.GetInt32(4),
                    LoanType = reader.GetString(5),
                    LoanStatus = reader.GetString(6),
                    
                };
            }
            return loan;
        }
    }
}
