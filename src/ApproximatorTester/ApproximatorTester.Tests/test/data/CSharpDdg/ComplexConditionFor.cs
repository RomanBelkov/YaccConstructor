﻿{caret}namespace N
{
    class Program
    {
        static void Main(string[] args)
        {
            string query = "select";
            for (int i = 0; i < 10 && i > 0 || query.IsNormalized(); i++)
            {
                query += " field";
            }
            Program.ExecuteImmediate(query);
        }

        static void ExecuteImmediate(string query)
        {
            
        }
    }
}