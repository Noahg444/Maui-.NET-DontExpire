using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SQLite;
using System.Reflection;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;
using System.Linq;


namespace DontExpireFinal;

public class DB
{
    private static string DBName = "log.db";
    public static SQLiteConnection conn;
    public static void OpenConnection()
    {
        string libFolder = FileSystem.AppDataDirectory;
        string fname = System.IO.Path.Combine(libFolder, DBName);
        conn = new SQLiteConnection(fname);
    }
    public static void CreateTable()
    {
        OpenConnection();
        conn.CreateTable<Food>();
    }


}
