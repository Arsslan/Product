using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class rate
    {
        public int ProductId { get; set; }
        public int TotalPoint { get; set; }
        public int ObtainedPoint { get;set;}
    }
    
    public class comment
    {
        public int pid { get; set; }
        public string Image { get;set;}
        public string name{ get; set; }
       // public int uid { get; set; }
        public string commentstring { get; set; }
    }
        public class product
        {
            public int ProductId { get; set; }
            public String Name { get; set; }
            public int TypeId { get; set; }
            public double Price { get; set; }
            public String Description { get; set; }
            public String picture { get; set; }
            public DateTime UpdatedOn { get; set; }
            public int UpdatedBy { get; set; }
            public int IsActive { get; set; }
            public string temp { get; set; }
            public double rate { get;set; }
        }
        public class type
        {
            public int TypeId { get; set; }
            public String TypeName { get; set; }
        }
        public class user 
        {
            public int UserId { get; set; }
            public String Name { get; set; }
            public String Login { get; set; }
            public String Password { get; set; }
            public String CreatedOn { get; set; }
            public String PicUrl{ get; set; }
            public int IsActive { get; set; }
            public int IsAdmin { get; set; }

            public string pic { get; set; }
        }
    }

