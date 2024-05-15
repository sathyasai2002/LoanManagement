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
            
            sqlConnection = new SqlConnection(DbUtil.GetConnectionString());
            cmd = new SqlCommand();


        }
        public int Applyloan(Loan loan, string LoanType, string propertyAddress = null, int propertyValue = 0, string carModel = null, int carValue = 0)
        {
            int LoanId;

            sqlConnection.Open();
            cmd.CommandText = "Insert into Loan values(@CustomerID, @PrincipalAmount, @InterestRate, @LoanTerm, @LoanType, @LoanStatus,@PropertyAddress,@PropertyValue,@CarModel,@CarValue)";
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

        

        public Decimal CalculateInterest(int loanId)
        {
            Decimal interestAmount = 0;

            
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandText = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanID = @LoanId";
                cmd.Parameters.AddWithValue("@LoanId", loanId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    Decimal principalAmount = reader.GetDecimal(0);
                    Decimal interestRate = reader.GetDecimal(1);
                    int loanTerm = reader.GetInt32(2);


                     interestAmount = (principalAmount * interestRate * loanTerm) / 12;
                }
            return interestAmount;

           
           
        }



        public String LoanStatus(int LoanId)
        {
            
            sqlConnection.Open();
            cmd.Connection = sqlConnection;
            String status = " ";
            cmd.CommandText = "SELECT CustomerId, CreditScore FROM Customer INNER JOIN Loan ON Customer.CustomerId = Loan.CustomerId WHERE LoanId = @LoanId;";
            cmd.Parameters.AddWithValue("@LoanId", LoanId);
            var reader = cmd.ExecuteReader();

            
                if (reader.Read())
                {
                    int customerId = reader.GetInt32(0);
                    int creditScore = reader.GetInt32(1);

                     status = (creditScore > 650) ? "Approved" : "Rejected";

                    cmd.CommandText = "UPDATE Loan SET LoanStatus = @Status WHERE LoanId = @LoanId;";
                    
                        cmd.Parameters.AddWithValue("@LoanId", LoanId);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.ExecuteNonQuery();
                    

                    
                }
            

            sqlConnection.Close();
            return status;

        }









        public Decimal CalculateEMI(int loanId) 
        {
            Decimal emi=0;
            Decimal monthlyInterest = 0;
            sqlConnection.Open();
            cmd.Connection = sqlConnection;
            cmd.CommandText = "SELECT PrincipalAmount, InterestRate, LoanTerm FROM Loan WHERE LoanId = @LoanId;";
            cmd.Parameters.AddWithValue("@LoanId", loanId);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Loan loan = new Loan();

                loan.PrincipalAmount = (Decimal)reader["PrincipalAmount"];
                    loan.InterestRate = (Decimal)reader["interestrate"];
                    loan.LoanTerm = (int)reader["LoanTerm"];
                

                monthlyInterest = loan.InterestRate / 12 / 100;

                emi = (loan.PrincipalAmount * monthlyInterest * (decimal)Math.Pow(1 + (double)monthlyInterest, loan.LoanTerm))
                        / ((decimal)Math.Pow(1 + (double)monthlyInterest, loan.LoanTerm) - 1);



            }
            sqlConnection.Close();
            return emi;
            

        }

        


        public int LoanRepayment(int loanId, decimal amount) 
        {
            sqlConnection.Open();
            cmd.Connection = sqlConnection;
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
            sqlConnection.Close();
            return numberOfEmisPaid;
            
        }
        public List<Loan> GetAllLoans()
        {
            sqlConnection.Open();
            cmd.Connection=sqlConnection;
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

        public void GetLoanById(int loanId)
        {
            List<Loan> loans = new List<Loan>();
            cmd.Connection = sqlConnection;
            cmd.CommandText = "SELECT * FROM Loan where loanId=@loanId";
            cmd.Parameters.AddWithValue("@loanId", loanId);
            sqlConnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Loan loan = new Loan
                {
                    LoanId = (int)reader["LoanId"],
                    PrincipalAmount = (Decimal)reader["PrincipalAmount"],
                    InterestRate = (Decimal)reader["InterestRate"],
                    LoanTerm = (int)reader["LoanTerm"],
                    LoanType = reader["LoanType"].ToString(),
                    LoanStatus = reader["LoanStatus"].ToString()
                };
                loans.Add(loan);
            }
            sqlConnection.Close();

            return ;
        }
    }
 }


