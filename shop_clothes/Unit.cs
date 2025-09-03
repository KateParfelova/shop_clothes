using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace shop_clothes
{
    public class Unit : IUnit
    {
        private readonly SqlConnection _connection;
        private SqlTransaction? _transaction;
        private bool _disposedValue;
        private readonly string? _dbPath;

        public Unit(string? connectionString = null)
        {
            string masterConnection="";
            if (connectionString is null)
            {
                // тут ты считываешь строки с твоего файла
                //connectionString подключение к твоей бд
                //masterConnection подключение к master бд
            }

            try
            {
                //пытаешься подключиться к бд
                _connection = new SqlConnection(connectionString);
                _connection.Open();
            }
            catch
            {

                //тут ты создаёшь бд
                _connection = new SqlConnection(masterConnection);
                _connection.Open();
                var creationCommand = new SqlCommand
                {
                    CommandText = $@"тут твои sql команды для создания бд и таблиц к ней",
                    Connection = _connection
                };
                creationCommand.ExecuteNonQuery(); //эта хрень выполяет твой sql код
                _connection.Close();
            }
        }

        //public IRepository<User> Users => new Repository<User>(_connection, _transaction);
        public IRepository<User> Users => throw new NotImplementedException();

        public IRepository<User> Products => throw new NotImplementedException();

        public IRepository<Request> Requests => throw new NotImplementedException();

        public IRepository<category> Categories => throw new NotImplementedException();

        public void BeginTransaction() => _transaction = _connection.BeginTransaction();

        public void Commit()
        {
            ArgumentNullException.ThrowIfNull(_transaction, "Transaction is not begin");
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }


        public void Rollback()
        {
            ArgumentNullException.ThrowIfNull(_transaction, "Transaction is not begin");
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _transaction?.Rollback();
                    _transaction?.Dispose();
                    _connection?.Close();
                    _connection?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
