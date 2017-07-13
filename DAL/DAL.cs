using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.ComponentModel;
using Entities;

namespace DAL
{
    public class Dal
    {
        string query = "";
        SqlCommand cmd;
        static string connstring= @"Data Source=MASTERPC\MSSQLEXPRESSS;Initial Catalog=Assignment6;Integrated Security=True";
        SqlConnection conn = new SqlConnection(connstring);
        public bool IsValid(user obj)
        {
            conn.Open();
            query = "select * from  [dbo].[User] where Login ='" + obj.Login + "'and Password='"+obj.Password+"'";
            cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                conn.Close();
                return true;
               

            }
            else
            {
                conn.Close();
                return false;
                
            }
           
        }
        public bool IsAdmin(user obj)
        {
            conn.Open();
            query = "select * from  [dbo].[User] where IsAdmin=1 and Login ='" + obj.Login + "'and Password='" + obj.Password + "'";
            cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                conn.Close();
                return true;


            }
            else
            {
                conn.Close();
                return false;

            }
        }
        public bool SignUp(user obj)
        {
           
            conn.Open();
            query = "select * from  [dbo].[User] where Login ='" + obj.Login + "'";
             cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            
            if(reader.Read())
            {
                return false ;
               
            }
              conn.Close();
            conn.Open();
             query = "insert into [dbo].[User] (Login,Name,Password,CreatedOn,IsActive,PicUrl,IsAdmin)values('" + obj.Login + "','" + obj.Name + "','" + obj.Password + "','" + DateTime.Now + "',1,'"+obj.PicUrl+"',0)";
            cmd = new SqlCommand(query, conn);
            int num = cmd.ExecuteNonQuery();
            conn.Close();
            if (num > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<product> Load()
        {
            //user obj = new user();
            List < product > productlist= new List<product>();
            conn.Open();
            query = "select Name,TypeName,Price,Description,PicUrl, ProductId from  Product ,Type where Product.TypeId=Type.TypeId and IsActive=1 ";
            cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                product obj = new product();
               
                obj.Name = reader[0].ToString();
                obj.Price = (double)reader[2];
                obj.Description = reader[3].ToString();
                obj.picture = reader[4].ToString();
                obj.temp = reader[1].ToString();
                obj.ProductId=Convert.ToInt32( reader[5]);
                productlist.Add(obj);


            }
            return productlist;
        }
        public List<product> Loadforadmin(int adminid)
        {
            //user obj = new user();
            List<product> productlist = new List<product>();
            conn.Open();
            query = "select Name,TypeName,Price,Description,PicUrl, ProductId from  Product ,Type where Product.TypeId=Type.TypeId and UpdatedBy=" + adminid + " and IsActive=1  ";
            cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                product obj = new product();

                obj.Name = reader[0].ToString();
                obj.Price = (double)reader[2];
                obj.Description = reader[3].ToString();
                obj.picture = reader[4].ToString();
                obj.temp = reader[1].ToString();
                obj.ProductId = Convert.ToInt32(reader[5]);
                productlist.Add(obj);


            }
            return productlist;
        }

        public bool AddProducttobatabse(product p, int adminid)
        {
            try{
                conn.Open();
               
                query = "insert into Product (Name,TypeId,Price,Description,PicURL,UpdatedOn,UpdatedBy,IsActive)values('" + p.Name + "'," + p.TypeId + "," + p.Price + ",'" + p.Description + "','" + p.picture + "','" + DateTime.Now + "'," + adminid + ",1)";
                cmd = new SqlCommand(query, conn);
              int rowaffected=  cmd.ExecuteNonQuery();
              conn.Close();
                if(rowaffected>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        public List<type> GetList()
        {

            List<type> mylist = new List<type>();
           
            query = "select TypeId,TypeName from Type";
            conn.Open();
            cmd = new SqlCommand( query,conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                type item = new type();
                item.TypeId = Convert.ToInt32(reader[0]);
                item.TypeName= reader[1].ToString();
                mylist.Add(item);

            }
            conn.Close();
                return mylist;
        }
        public int GetUserId(user user1)
        {
            try
            {
                int id=0;
             conn.Open();
                query = "select UserId from [dbo].[User] where Login='" + user1.Login + "'";
                cmd = new SqlCommand(query, conn);
                SqlDataReader reader =cmd.ExecuteReader();
                if(reader.Read())
                {
                    id=Convert.ToInt32(reader[0]);
                }
                conn.Close();
                return id;
               
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool UpdateAdmin(user obj,string admin)
        {
            
            try
            {
                conn.Open();
                cmd = new SqlCommand("update [dbo].[User] set Name='" + obj.Name + "',Password='" + obj.Password + "',PicUrl='" +obj.PicUrl+ "',CreatedOn='" + DateTime.Now + "',IsActive=1,IsAdmin=1 where Login='" + admin + "'",conn);
                int recordaffected = cmd.ExecuteNonQuery();
                if (recordaffected> 0)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool UpdateUser(user obj, string user)
        {
            
            try
            {
                conn.Open();
                cmd = new SqlCommand("update [dbo].[User] set Name='" + obj.Name + "',Password='" + obj.Password + "',PicUrl='" + obj.PicUrl + "',CreatedOn='" + DateTime.Now + "',IsActive=1,IsAdmin=0 where Login='" + user + "'", conn);
                int recordaffected = cmd.ExecuteNonQuery();
                if (recordaffected > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public user getuser(string login)
        {
            user obj = new user();
            try
            {
                conn.Open();
                cmd = new SqlCommand("select * from [dbo].[User] where Login='" + login + "'", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    obj.Login = reader[1].ToString();
                    obj.Name = reader[2].ToString();
                    obj.Password = reader[3].ToString();
                    obj.PicUrl = reader[6].ToString();
                }
                conn.Close();
                return obj;
               
            }
            catch(Exception ex)
            {
                return null;

            }
           
        }
        public product GetProductById(int id)
        {
            try
            {
                product p = new product();
                conn.Open();
                cmd = new SqlCommand("select ProductId,Name,Price,Description,TypeName ,PicUrl from Product,Type where ProductId=" + id + " and Product.TypeId=Type.TypeId", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    p.ProductId = (int)reader[0];
                    p.Name = reader[1].ToString();
                    p.Price = (double)reader[2];
                    p.Description = reader[3].ToString();
                    p.temp = reader[4].ToString();
                    p.picture = reader[5].ToString();
                    
                }
                conn.Close();
                return p;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool UpdateProduct(product obj, int productid,int adminid )
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand("update Product set Name='" + obj.Name + "',TypeId=" + obj.TypeId + ",Price=" + obj.Price + ",Description='" + obj.Description + "',PicUrl='" + obj.picture + "',UpdatedOn='" + DateTime.Now + "',UpdatedBy=" + adminid + ",IsActive=1 where ProductId=" + productid + "", conn);
                int recef = cmd.ExecuteNonQuery();
                if (recef > 0)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public bool DeleteProduct(int id, int adminid)
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand("update Product  set IsActive=0 where ProductId=" + id + "and UpdatedBy=" + adminid + "", conn);
                int recef = cmd.ExecuteNonQuery();
                conn.Close();
                if (recef > 0)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        // Assignment6 Part/Function

        public bool Addcommentt(int Productid, int Userid, string comment)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                try
                {
                    conn.Open();
                    String query = String.Format("insert into Comments(ProductId,UserId,CommentString)values({0},{1},'{2}')", Productid, Userid, comment);
                    cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }

}

        public List<comment> Getcomments()
        {
            List<comment> commentlist = new List<comment>();
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                query = "select  PicUrl,Name,CommentString ,ProductId from [dbo].[User],[dbo].[Comments] where [dbo].[User].UserId= [dbo].[Comments].UserId  order by CommentsId  DESC ";
                cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    comment obj = new comment();
                    obj.Image = reader[0].ToString();
                    obj.name = reader[1].ToString();
                    obj.commentstring = reader[2].ToString();
                    obj.pid = Convert.ToInt32(reader[3]);
                    commentlist.Add(obj);
                }
            }
            return commentlist;
        }
        public List<comment> GetAllcomments(int id)
        {
            List<comment> commentlist = new List<comment>();
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                query = "select PicUrl,Name,CommentString ,ProductId from [dbo].[User],[dbo].[Comments] where ProductId="+id+" and [User].UserId= [dbo].[Comments].UserId ";
                cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comment obj = new comment();
                    obj.Image = reader[0].ToString();
                    obj.name = reader[1].ToString();
                    obj.commentstring = reader[2].ToString();
                    obj.pid = Convert.ToInt32(reader[3]);
                    commentlist.Add(obj);
                }
            }
            return commentlist;
        }
        public bool Addtofavorit(int Productid, int Userid)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                try
                {
                    conn.Open();
                    String query = String.Format("insert into [dbo].[Favourite](ProductId,UserId)values({0},{1})", Productid, Userid);
                    cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();

                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        public List<product> ViewFavorite(int userid)
        {
            List<product> productlist = new List<product>();
            conn.Open();
            query = "select Name,TypeName,Price,Description,PicUrl from  Product ,[dbo].[Favorite],Type where Product.TypeId=Type.TypeId and IsActive=1 and [dbo].[Product].ProductId=[dbo].[Favorite].ProductId and [dbo].[Favorite].UserId="+userid+" ";
            cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                product obj = new product();

                obj.Name = reader[0].ToString();
                obj.Price = (double)reader[2];
                obj.Description = reader[3].ToString();
                obj.picture = reader[4].ToString();
                obj.temp = reader[1].ToString();
                productlist.Add(obj);


            }
            return productlist;
        }
        public List<product> SearchResult(string pname)
        {
            List<product> productlist = new List<product>();
            conn.Open();
            query = "select Name,TypeName,Price,Description,PicUrl from  Product ,Type where Product.TypeId=Type.TypeId and IsActive=1 and Name='"+pname+"'";
            cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                product obj = new product();

                obj.Name = reader[0].ToString();
                obj.Price = (double)reader[2];
                obj.Description = reader[3].ToString();
                obj.picture = reader[4].ToString();
                obj.temp = reader[1].ToString();
                productlist.Add(obj);


            }
            return productlist;
        }
        public bool RateProduct(int pid, int op,int  tp)
        {
            using (SqlConnection conn=new SqlConnection(connstring))
            {
                try
                {
                    conn.Open();
                    query=String.Format("insert into Ratings (ProductId,TotalPoints,ObtainedPoints)values({0},{1},{2})",pid,tp,op);
                    cmd = new SqlCommand(query, conn);
                   if( cmd.ExecuteNonQuery()>0)
                   {
                       return true;
                   }
                }
                catch(Exception ex)
                {
                    return false;
                }
            }

            return true;

        }
        public List<rate> GetAllRatings()
        {
            List<rate> ratings = new List<rate>();
            using(SqlConnection conn=new SqlConnection(connstring))
            {
                try
                {
                    conn.Open();
                    query = "select * from Ratings";
                    cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        rate obj = new rate();
                        obj.ProductId = Convert.ToInt32(reader[1]);
                        obj.TotalPoint = Convert.ToInt32(reader[2]);
                        obj.ObtainedPoint = Convert.ToInt32(reader[3]);
                        ratings.Add(obj);
                    }
                }
                catch(Exception ex)
                {

                }
            }






            return ratings;
        }    
          }
    }
   
    

