using Microsoft.Data.Sqlite;
using NLog;
using System;
using System.IO;

namespace assets_manager.Helpers;

public class SqLiteHelper
{
    private readonly string? app_data_path = Config.AppPathConst.AppDataDirectory;
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    private SqliteConnectionStringBuilder? dbConnectionstr;
    private SqliteDataReader? dbDataReader;
    private SqliteCommand? dbCommand;

    public SqliteConnection? dbConnection;

    public SqLiteHelper() {
        if (app_data_path == null) {
            logger.Error("app_data_path is null");
            return;
        }
        if (!Directory.Exists(app_data_path)) {
            Directory.CreateDirectory(app_data_path);
        }

        try
        {
            logger.Info("SqliteConnection");
            dbConnection = new SqliteConnection();

            dbConnectionstr = new SqliteConnectionStringBuilder();
            dbConnectionstr.DataSource = $"{app_data_path}/assets_config.db";
            dbConnection.ConnectionString = dbConnectionstr.ToString();
            dbConnection.Open();
        }
        catch (Exception e)
        {
            logger.Error(e.ToString());

        }

    }

    /// <summary>
    /// 执行SQL命令
    /// </summary>
    /// <returns>The query.</returns>
    /// <param name="queryString">SQL命令字符串</param>
    public SqliteDataReader ExecuteQuery(string queryString)
    {
        try
        {
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = queryString;       //设置SQL语句
            dbDataReader = dbCommand.ExecuteReader();
        }
        catch (Exception e)
        {
            logger.Error(e.ToString());
        }

        return dbDataReader;
    }

    /// <summary>
    /// 关闭数据库连接
    /// </summary>
    public void CloseConnection()
    {
        //销毁Command
        if (dbCommand != null)
        {
            dbCommand.Cancel();
        }
        dbCommand = null;
        //销毁Reader
        if (dbDataReader != null)
        {
            dbDataReader.Close();
        }
        dbDataReader = null;
        //销毁Connection
        if (dbConnection != null)
        {
            dbConnection.Close();
        }
        dbConnection = null;

    }

    /// <summary>
    /// 读取整张数据表
    /// </summary>
    /// <returns>The full table.</returns>
    /// <param name="tableName">数据表名称</param>
    public SqliteDataReader ReadFullTable(string tableName)
    {
        string queryString = "SELECT * FROM " + tableName;  //获取所有可用的字段
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 向指定数据表中插入数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="values">插入的数值</param>
    public SqliteDataReader InsertValues(string tableName, string[] values, string col_list = "(key, value)")
    {
        //获取数据表中字段数目
        int fieldCount = ReadFullTable(tableName).FieldCount;
        //当插入的数据长度不等于字段数目时引发异常
        if (values.Length + 1 != fieldCount)
        {
            logger.Error("values.Length!=fieldCount");
            throw new SqliteException("values.Length!=fieldCount", 0);
        }
        string queryString = "INSERT INTO " + tableName + $"{col_list}" + " VALUES (" + "'" + values[0] + "'";
        for (int i = 1; i < values.Length; i++)
        {
            queryString += ", " + "'" + values[i] + "'";
        }
        queryString += " )";
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 更新指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    /// <param name="key">关键字</param>
    /// <param name="value">关键字对应的值</param>
    /// <param name="operation">运算符：=,<,>,...，默认“=”</param>
    public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key, string value, string operation)
    {
        // operation="=";  //默认
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length)
        {
            logger.Error("colNames.Length!=colValues.Length");
            throw new SqliteException("colNames.Length!=colValues.Length", 0);
        }
        string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + "'" + colValues[0] + "'";

        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += ", " + colNames[i] + "=" + "'" + colValues[i] + "'";
        }
        queryString += " WHERE " + key + operation + "'" + value + "'";

        return ExecuteQuery(queryString);
    }


    /// <summary>
    /// 更新指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    /// <param name="key1">关键字</param>
    /// <param name="value1">关键字对应的值</param>
    /// <param name="operation">运算符：=,<,>,...，默认“=”</param>
    public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key1, string value1, string operation, string key2, string value2)
    {
        // operation="=";  //默认
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length)
        {
            logger.Error("colNames.Length!=colValues.Length");
            throw new SqliteException("colNames.Length!=colValues.Length", 0);
        }
        string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + "'" + colValues[0] + "'";

        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += ", " + colNames[i] + "=" + "'" + colValues[i] + "'";
        }
        //表中已经设置成int类型的不需要再次添加‘单引号’，而字符串类型的数据需要进行添加‘单引号’
        queryString += " WHERE " + key1 + operation + "'" + value1 + "'" + "OR " + key2 + operation + "'" + value2 + "'";

        return ExecuteQuery(queryString);
    }


    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    public SqliteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] colValues, string[] operations)
    {
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
        {
            logger.Error("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length", 0);
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + "'" + colValues[0] + "'";
        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += "OR " + colNames[i] + operations[0] + "'" + colValues[i] + "'";
        }
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 删除指定数据表内的数据
    /// </summary>
    /// <returns>The values.</returns>
    /// <param name="tableName">数据表名称</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colValues">字段名对应的数据</param>
    public SqliteDataReader DeleteValuesAND(string tableName, string[] colNames, string[] colValues, string[] operations)
    {
        //当字段名称和字段数值不对应时引发异常
        if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
        {
            logger.Error("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            throw new SqliteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length", 0);
        }

        string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + "'" + colValues[0] + "'";

        for (int i = 1; i < colValues.Length; i++)
        {
            queryString += " AND " + colNames[i] + operations[i] + "'" + colValues[i] + "'";
        }
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// 创建数据表
    /// </summary> +
    /// <returns>The table.</returns>
    /// <param name="tableName">数据表名</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colTypes">字段名类型</param>
    public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
    {
        string queryString = "CREATE TABLE IF NOT EXISTS " + tableName + "( " + colNames[0] + " " + colTypes[0];
        for (int i = 1; i < colNames.Length; i++)
        {
            queryString += ", " + colNames[i] + " " + colTypes[i];
        }
        queryString += "  ) ";
        return ExecuteQuery(queryString);
    }

    /// <summary>
    /// Reads the table.
    /// </summary>
    /// <returns>The table.</returns>
    /// <param name="tableName">Table name.</param>
    /// <param name="items">Items.</param>
    /// <param name="colNames">Col names.</param>
    /// <param name="operations">Operations.</param>
    /// <param name="colValues">Col values.</param>
    public SqliteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues)
    {
        string queryString = "SELECT " + items[0];
        for (int i = 1; i < items.Length; i++)
        {
            queryString += ", " + items[i];
        }
        queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
        for (int i = 0; i < colNames.Length; i++)
        {
            queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
        }
        return ExecuteQuery(queryString);
    }


}
