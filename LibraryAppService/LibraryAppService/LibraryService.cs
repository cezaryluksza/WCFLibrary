using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppService
{
    public class LibraryService : ILibraryService
    {
        private static string ConStr = "Data Source=DESKTOP-CIU5GDV;Initial Catalog=Library;Integrated Security=True";
        public List<Book> GetListOfBooks()
        {
            List<Book> LB = new List<Book>();
            SqlConnection con = new SqlConnection(ConStr);
            SqlCommand cmd = new SqlCommand("Select * from Books", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Book b = new Book();
                b.ISBN = dr[0].ToString();
                b.BookTitle = dr[1].ToString();
                b.Publication = dr[2].ToString();
                b.Pages = int.Parse(dr[3].ToString());
                b.Series = dr[5].ToString();
                LB.Add(b);
            }
            dr.Close();
            con.Close();
            Console.WriteLine("Data downloaded.");
            return LB;
        }
        public int VerifyLogin(string login, string password)
        {
            int permission = 0;
            SqlConnection con = new SqlConnection(ConStr);
            SqlCommand cmd = new SqlCommand("select Permission from [User] where LoginName = '"+login+"' and PWDCOMPARE('"+password+"',PasswordHash) = 1", con);
            //SqlParameter param1 = new SqlParameter("@login", login);
            //SqlParameter param2 = new SqlParameter("@password", password);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                permission = int.Parse(dr[0].ToString());
            }
            dr.Close();
            con.Close();
            Console.WriteLine("Logged in as "+login);
            return permission;
        }
        public List<Libraries> BooksInLibraries(string ISBN)
        {
            List<Libraries> LL = new List<Libraries>();
            SqlConnection con = new SqlConnection(ConStr);
            SqlCommand cmd = new SqlCommand("select * from tf_BooksInLibraries('"+ISBN+"')", con);
            SqlParameter param1 = new SqlParameter("@ISBN", ISBN);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Libraries l = new Libraries();
                l.LibraryId = int.Parse(dr[0].ToString());
                l.ISBN = dr[1].ToString();
                l.Quantity = int.Parse(dr[2].ToString());
                l.LibraryName = dr[3].ToString();
                l.Street = dr[4].ToString();
                l.NumberBuilding = dr[5].ToString();
                l.City = dr[6].ToString();
                l.Country = dr[7].ToString();
                LL.Add(l);
            }
            dr.Close();
            con.Close();
            Console.WriteLine("Books downloaded.");
            return LL;
        }
        public bool Request(string login, string ISBN, int LibraryId)
        {
            int MemberId = GetMemberId(login);
            if (MemberId >= 0)
            {
                SqlConnection con = new SqlConnection(ConStr);
                SqlCommand cmd = new SqlCommand("insert into MemberRequest (MemberId,ISBN,DateRequested) values (" + MemberId + ", '" + ISBN + "',getdate())", con);
                //SqlParameter param1 = new SqlParameter("@MemberId", MemberId);
                //SqlParameter param2 = new SqlParameter("@ISBN", ISBN);
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result < 0)
                {
                    //throw new Exception("Something went wrong while inserting data.");
                    con.Close();
                    return false;
                }
                else
                {
                    SqlCommand cmd3 = new SqlCommand("update BooksAtLibraries set QuantityInStock = QuantityInStock - 1 where ISBN = '"+ISBN+"' and LibraryId = " + LibraryId, con);
                    //SqlParameter param4 = new SqlParameter("@LibraryId", LibraryId);
                    int result2 = cmd3.ExecuteNonQuery();
                    if (result < 0)
                    {
                        con.Close();
                        //throw new Exception("Something went wrong while updating data.");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Request success.");
                        con.Close();
                        return true;
                    }
                }
            }
            else
            {
                //throw new Exception("Something went wrong while getting user data.");
                return false;
            }
        }
        public List<Request> ShowRequests(string login)
        {
            int MemberId = GetMemberId(login);
            List<Request> RL = new List<Request>();
            SqlConnection con = new SqlConnection(ConStr);
            SqlCommand cmd = new SqlCommand("select * from tf_GetRequests("+ MemberId +")", con);
            //SqlParameter param1 = new SqlParameter("@MemberId", MemberId);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Request r = new Request();

                r.RequestId = int.Parse(dr[0].ToString());
                r.MemberId = int.Parse(dr[1].ToString());
                r.ISBN = dr[2].ToString();
                r.DateRequested = DateTime.Parse(dr[3].ToString());
                if (dr[4].ToString() == null)
                {
                    r.DateLocated = DateTime.Parse(dr[4].ToString());
                }
                r.PBook.BookTitle = dr[5].ToString();
                r.PBook.Publication = dr[6].ToString();
                r.PBook.Pages = int.Parse(dr[7].ToString());
                r.PBook.Series = dr[8].ToString();
                RL.Add(r);
            }
            dr.Close();
            con.Close();
            Console.WriteLine("Requests for "+login+" downloaded.");
            return RL;
        }

        public List<PersonalData> GetPersonalData(string login)
        {
            List<PersonalData> PDL = new List<PersonalData>();
            SqlConnection con = new SqlConnection(ConStr);
            SqlCommand cmd = new SqlCommand("select * from tf_GetMemberData('"+login+"')", con);
            //SqlParameter param1 = new SqlParameter("@login", login);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                PersonalData PD = new PersonalData();
                PD.MemberId = int.Parse(dr[0].ToString());
                PD.FirstName = dr[1].ToString();
                PD.LastName = dr[2].ToString();
                PD.DateOfBirth = DateTime.Parse(dr[3].ToString());
                PD.PhoneNumber = dr[4].ToString();
                PD.Email = dr[5].ToString();
                PD.Street = dr[6].ToString();
                PD.NumberBuilding = dr[7].ToString();
                PD.City = dr[8].ToString();
                PDL.Add(PD);
            }
            dr.Close();
            con.Close();
            Console.WriteLine("Personal data for " + login + " downloaded.");
            return PDL;
        }

        public static int GetMemberId(string login)
        {
            int id = -1;
            SqlConnection con = new SqlConnection(ConStr);
            SqlCommand cmd = new SqlCommand("select MemberId from [User] where [User].LoginName = '"+login+"'", con);
            //SqlParameter param1 = new SqlParameter("@login", login);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                id = int.Parse(dr[0].ToString());
            }
            dr.Close();
            con.Close();
            return id;
        }
    }
}
