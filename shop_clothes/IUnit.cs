using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace shop_clothes
{
    public interface IUnit : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();


        IRepository<User> Users { get; }
        IRepository<User> Products { get; }
        IRepository<category> Categories { get; }
        IRepository<Request> Requests { get; }

    }

}
