using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcGreeter.DbProviders
{
    public class SqlQueries
    {
        public const string SELECT = "SELECT * FROM {0}";

        public const string CreateDbIfNotExist = @"DECLARE @dbname nvarchar(128)
                                                SET @dbname = N'{0}'

                                                IF (NOT EXISTS (SELECT name 
                                                FROM master.dbo.sysdatabases 
                                                WHERE ('[' + name + ']' = @dbname 
                                                OR name = @dbname)))
                                                BEGIN
	                                                DECLARE @sql NVARCHAR(max) =  'CREATE DATABASE ' + @dbname;
	                                                EXEC(@sql);
                                                END";

        public const string CreateTableIfNotExist = @"DECLARE @tableName nvarchar(128)
                                                    SET @tableName = N'{0}'

                                                    IF (NOT EXISTS (SELECT * 
                                                                     FROM INFORMATION_SCHEMA.TABLES 
                                                                     WHERE TABLE_SCHEMA = 'dbo' 
                                                                     AND  TABLE_NAME = @tableName))
                                                    BEGIN
                                                        DECLARE @sql NVARCHAR(MAX) =  'CREATE TABLE ' + @tableName + ' (id INT IDENTITY(1,1) PRIMARY KEY, data NVARCHAR(MAX))';
	                                                    EXEC(@sql)
                                                    END";

        public const string TruncateTable = @"DECLARE @tableName nvarchar(128)
                                                    SET @tableName = N'{0}'

                                                    IF(EXISTS (SELECT*
                                                                     FROM INFORMATION_SCHEMA.TABLES
                                                                     WHERE TABLE_SCHEMA = 'dbo'
                                                                     AND TABLE_NAME = @tableName))
                                                    BEGIN
                                                        TRUNCATE TABLE {0}
                                                    END";

        public const string Insert = @"INSERT INTO {0}
                                        VALUES {1}";

        public const string DeleteTableIfExist = "DROP TABLE IF EXISTS dbo.{0}";

        public const string DeleteDbIfExist = @"DECLARE @dbname nvarchar(128)
                                                SET @dbname = N'{0}'

                                                IF (EXISTS (SELECT name 
                                                FROM master.dbo.sysdatabases 
                                                WHERE ('[' + name + ']' = @dbname 
                                                OR name = @dbname)))
                                                BEGIN
	                                                DECLARE @kill varchar(8000); SET @kill = '';  
	                                                SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), spid) + ';'  
	                                                FROM master..sysprocesses  
	                                                WHERE dbid = db_id(@dbname)
	                                                EXEC(@kill); 

	                                                DECLARE @sql NVARCHAR(max) =  'DROP DATABASE ' + @dbname;
	                                                EXEC(@sql);
                                                END";

        public const string SelectAllDbsNames = @"SELECT name FROM master.dbo.sysdatabases";
    }
}
