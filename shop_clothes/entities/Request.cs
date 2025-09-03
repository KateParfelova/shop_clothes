using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace shop_clothes
{
    public class Request: ICreateble<Request>
    {
        [Key]
        public int Id { get; set; }

        public int IdUser { get; set; }
        public int IdProducts { get; set; }

        public static Request Create(SqlDataReader reader)
        {
            return new Request()
            {
                Id = (int)reader.GetValue(0),
                IdUser = (int)reader.GetValue(1),
                IdProducts = (int)reader.GetValue(2)
            };
        }
    }
}
