using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// DbTableHerper 的摘要描述
/// </summary>
public class DbTableHerper
{
    public DbTableHerper()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    /// <summary>
    /// 取得資料庫中 資料表與資料表描述
    /// </summary>
    /// <param name="constr"></param>
    /// <returns></returns>
    public DataTable GetTableList(string constr)
    {

        string TSQL = @"
                        WITH DescTalbe
                        AS (
                        SELECT *
                        FROM ::fn_listextendedproperty( NULL, 'user', 'dbo', 'table', NULL, NULL, NULL )
                        UNION ALL
                        SELECT *
                        FROM ::fn_listextendedproperty( NULL, 'user', 'dbo', 'view', NULL, NULL, NULL )
                       )
                        SELECT TableDetail.*,
                               DescTalbe.value
                        FROM INFORMATION_SCHEMA.Tables TableDetail
                             LEFT JOIN DescTalbe ON TableDetail.TABLE_NAME = DescTalbe.objname COLLATE Chinese_Taiwan_Stroke_CI_AS
                                                AND name = 'MS_Description'
                        ORDER BY TABLE_TYPE,
                                 TABLE_NAME;";
        return SqlServer.GetSqlDataTable(constr, TSQL);
    }
    /// <summary>
    /// 判斷資料表是否存在
    /// </summary>
    /// <param name="constr"></param>
    /// <param name="table_schema"></param>
    /// <param name="tablename"></param>
    /// <returns></returns>
    public bool CheckTableisexists(string constr, string table_schema, string tablename)
    {
        string TSQL = @"
                        select count(table_name) exist from INFORMATION_SCHEMA.[TABLES]
                        where table_schema = @table_schema and table_name = @tablename
                        ";
        List<Parameters> para = new List<Parameters>();
        para.Add(Parameters.getParameter("@table_schema", SqlDbType.NVarChar, table_schema));
        para.Add(Parameters.getParameter("@tablename", SqlDbType.NVarChar, tablename));
        string bol = SqlServer.GetSqlDataTable(constr, TSQL, para).Rows[0]["exist"].ToString();
        return bol != "0";
    }
    /// <summary>
    /// 更新 單筆 DB 的 單筆 Table描述
    /// </summary>
    public void UpdateDBTableDescription(string constr, string Table_Type, string Table_Schema, string Table_Name, string Table_Desc)
    {
        string TSQL = @"
                IF NOT EXISTS(select * from fn_listextendedproperty (NULL, 'schema', @OWNER, @Type, @TABLE_NAME, NULL, NULL) where name = 'MS_Description')
                BEGIN
                  exec sp_addextendedproperty @name = N'MS_Description'
                                              , @value = @DESC
                                              , @level0type = N'Schema'
                                              , @level0name = @OWNER 
                                              , @level1type = @Type
                                              , @level1name = @Table_Name
                END
                ELSE
                BEGIN
                  exec sp_updateextendedproperty @name = N'MS_Description'
                                              , @value = @DESC
                                              , @level0type = N'Schema'
                                              , @level0name = @OWNER 
                                              , @level1type = @Type
                                              , @level1name = @Table_Name
                END
        ";
        List<Parameters> para = new List<Parameters>();
        para.Add(Parameters.getParameter("@Type", SqlDbType.NVarChar, Table_Type));
        para.Add(Parameters.getParameter("@OWNER", SqlDbType.NVarChar, Table_Schema));
        para.Add(Parameters.getParameter("@Table_Name", SqlDbType.NVarChar, Table_Name));
        para.Add(Parameters.getParameter("@DESC", SqlDbType.NVarChar, Table_Desc));


        SqlServer.SqlInsertUpdateDelete(constr, TSQL, para);

    }
/// <summary>
/// 判斷欄位是否存在
/// </summary>
/// <param name="constr"></param>
/// <param name="table_schema"></param>
/// <param name="table_name"></param>
/// <param name="column_name"></param>
/// <returns></returns>
    public bool CheckTableColumnisexists(string constr, string table_schema, string table_name,string column_name)
    {
        string TSQL = @"
                        select count(objname) exist from fn_listextendedproperty ( NULL, 'user', @table_schema, 'table', @tablename, 'column', @column_name )
                        ";
        List<Parameters> para = new List<Parameters>();
        para.Add(Parameters.getParameter("@table_schema", SqlDbType.NVarChar, table_schema));
        para.Add(Parameters.getParameter("@tablename", SqlDbType.NVarChar, table_name));
        para.Add(Parameters.getParameter("column_name", SqlDbType.NVarChar, column_name));
        string bol = SqlServer.GetSqlDataTable(constr, TSQL, para).Rows[0]["exist"].ToString();
        return bol != "0";
    }

    /// <summary>
    /// 取得資料表欄位清單
    /// </summary>
    /// <param name="constr"></param>
    /// <param name="Table_Schema"></param>
    /// <param name="Table_Name"></param>
    /// <returns></returns>
    public DataTable GetColumnList(string constr, string Table_Schema, string Table_Name)
    {
        /*
         先移除VIEW的處理

         */
        string TSQL = @"
                       SELECT @OWNER as TABLE_SCHEMA, @TABLE_NAME as TABLE_NAME,*
                         FROM ::fn_listextendedproperty( NULL, 'user', @OWNER, 'table', @TABLE_NAME, 'column', NULL )
                        WHERE name = 'MS_Description';

";
        List<Parameters> para = new List<Parameters>();
        para.Add(Parameters.getParameter("@OWNER", SqlDbType.NVarChar, Table_Schema));
        para.Add(Parameters.getParameter("@TABLE_NAME", SqlDbType.NVarChar, Table_Name));

        return SqlServer.GetSqlDataTable(constr, TSQL, para);
    }
    /// <summary>
    /// 更新單筆資料欄位描述
    /// </summary>
    /// <param name="constr"></param>
    /// <param name="Table_Schema"></param>
    /// <param name="Table_Name"></param>
    /// <param name="Coulmn_Name"></param>
    /// <param name="Column_Desc"></param>
    public void UpdateDBTableColumnDescription(string constr, string Table_Schema, string Table_Name, string Coulmn_Name, string Column_Desc)
    {
        string TSQL = @"
                IF NOT EXISTS(select * from fn_listextendedproperty(NULL, 'user', 'dbo', 'table', @TABLE_NAME, 'column',NULL) where name = 'MS_Description')
                BEGIN
                  exec sp_addextendedproperty @name = N'MS_Description'
                                              , @value = @DESC
                                              , @level0type = N'Schema'
                                              , @level0name = @OWNER 
                                              , @level1type = 'TABLE'
                                              , @level1name = @Table_Name
                                              , @level2type = N'Column'
                                              , @level2name = @Coulmn_Name;  
                END
                ELSE
                BEGIN
                  exec sp_updateextendedproperty @name = N'MS_Description'
                                              , @value = @DESC
                                              , @level0type = N'Schema'
                                              , @level0name = @OWNER 
                                              , @level1type = 'TABLE'
                                              , @level1name = @Table_Name
                                              , @level2type = N'Column'
                                              , @level2name = @Coulmn_Name;  
                
                END
        ";
        List<Parameters> para = new List<Parameters>();
        para.Add(Parameters.getParameter("@OWNER", SqlDbType.NVarChar, Table_Schema));
        para.Add(Parameters.getParameter("@Table_Name", SqlDbType.NVarChar, Table_Name));
        para.Add(Parameters.getParameter("@Coulmn_Name", SqlDbType.NVarChar, Coulmn_Name));

        para.Add(Parameters.getParameter("@DESC", SqlDbType.NVarChar, Column_Desc));


        SqlServer.SqlInsertUpdateDelete(constr, TSQL, para);

    }
}