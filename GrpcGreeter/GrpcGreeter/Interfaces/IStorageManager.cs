using dbms_core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter
{
    public interface IStorageManager
    {
        Table GetSelectedTable(string name);

        Database GetDatabaseFromPath(string path);

        void SerialiseDatabase(Database database);

        void SerialiseTable(Table table);
    }
}
