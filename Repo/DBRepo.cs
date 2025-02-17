using Microsoft.Data.SqlClient;
using MVCAPP.Models;

namespace MVCAPP.Repo;

public class DBRepo
{
    private readonly IConfiguration _configuration;
    private readonly string connectionStr;

    public DBRepo(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionStr = _configuration.GetConnectionString("DBConn").ToString();
    }

    public List<Employee> Employees()
    {
        List<Employee> list = new List<Employee>();
        using (SqlConnection connection = new SqlConnection(connectionStr))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand("usp_GetAllEmployee", connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //cmd.Parameters.Add(new SqlParameter("@Id",value)
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                var emp = new Employee
                {
                    EmployeeID = Convert.ToInt32(rd["EmployeeID"]),
                    FirstName = rd["FirstName"].ToString(),
                    LastName = rd["LastName"].ToString(),
                    Email = rd["Email"].ToString(),
                    PhoneNumber = rd["PhoneNumber"].ToString(),
                    HireDate = DateOnly.FromDateTime(Convert.ToDateTime(rd["HireDate"])),
                    JobTitle = rd["JobTitle"].ToString(),
                    Department = rd["Department"].ToString(),
                    Salary = Convert.ToInt32(rd["Salary"]),
                };

                list.Add(emp);

            }
            return list;
        }
    }
    
    public bool InsertEmployee(Employee emp)
    {
        using (SqlConnection conn = new SqlConnection(connectionStr))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("usp_InsertEmployee", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@FirstName", emp.FirstName));
            cmd.Parameters.Add(new SqlParameter("@LastName", emp.LastName));
            cmd.Parameters.Add(new SqlParameter("@Email", emp.Email));
            cmd.Parameters.Add(new SqlParameter("@PhoneNumber", emp.PhoneNumber));
            cmd.Parameters.Add(new SqlParameter("@HireDate", emp.HireDate.ToDateTime(TimeOnly.MinValue)));
            cmd.Parameters.Add(new SqlParameter("@JobTitle", emp.JobTitle));
            cmd.Parameters.Add(new SqlParameter("@Department", emp.Department));
            cmd.Parameters.Add(new SqlParameter("@Salary", emp.Salary));

            try
            {
                cmd.ExecuteNonQuery(); // Return the number of rows affected
                return true;
            }
            catch (Exception ex)
            {
                // log the exception
                return false;
                //throw new Exception("Error inserting employee: " + ex.Message, ex);
            }
        }
    }
    public int UpdateEmployee(Employee emp)
    {

        using (SqlConnection conn = new SqlConnection(connectionStr))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("UpdateEmployee", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@EmployeeID", emp.EmployeeID));
            cmd.Parameters.Add(new SqlParameter("@FirstName", emp.FirstName));
            cmd.Parameters.Add(new SqlParameter("@LastName", emp.LastName));
            cmd.Parameters.Add(new SqlParameter("@Email", emp.Email));
            cmd.Parameters.Add(new SqlParameter("@PhoneNumber", emp.PhoneNumber));
            cmd.Parameters.Add(new SqlParameter("@HireDate", emp.HireDate));
            cmd.Parameters.Add(new SqlParameter("@JobTitle", emp.JobTitle));
            cmd.Parameters.Add(new SqlParameter("@Department", emp.Department));
            cmd.Parameters.Add(new SqlParameter("@Salary", emp.Salary));
            try
            {
                var i = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw;
            }
            return -1;
        }

    }
    public Employee GetEmployeeById(int id)
    {
        using (SqlConnection conn = new SqlConnection(connectionStr))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("usp_GetAllEmployee", conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Id", id));
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Employee
                {
                    EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                    FirstName = reader["FirstName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Email = reader["Email"].ToString(),
                    PhoneNumber = reader["PhoneNumber"].ToString(),
                    HireDate = DateOnly.FromDateTime(Convert.ToDateTime(reader["HireDate"])),
                    JobTitle = reader["JobTitle"].ToString(),
                    Department = reader["Department"].ToString(),
                    Salary = Convert.ToInt32(reader["Salary"]),
                };
            }
            return null;
        }
    }
    
    public int DeleteEmployee(int employeeID)
    {
        using (SqlConnection conn = new SqlConnection(connectionStr))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("DeleteEmployee", conn); // Ensure stored procedure name is correct
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@EmployeeID", employeeID));
            return cmd.ExecuteNonQuery(); // Return actual rows affected
        }
    }


}