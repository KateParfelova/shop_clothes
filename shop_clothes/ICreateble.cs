using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop_clothes
{
    public interface ICreateble<T>
    {
        abstract static T Create(SqlDataReader reader);

        public int Id { get; set; }
    }

}
