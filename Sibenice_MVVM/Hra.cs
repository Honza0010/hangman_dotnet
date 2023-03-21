using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Xml.Linq;
using System.Windows;

namespace Sibenice
{
    public class Hra
    {
        public string guessedWord { get; set; }
        public string word { get; private set; }
        private int lengthOfWord { get; set; }
        private string lastWord { get; set; }

        public Hra(string lastWord)
        {
            this.lastWord = lastWord;

            this.word = uploadFromDatabase();
            guessedWord = "";
            if (!this.word.Equals(""))
            {
                lengthOfWord = word.Length;


                for (int i = 0; i < lengthOfWord; i++)
                {
                    guessedWord += "*";
                }
            }
        }

        public bool check(string entered)
        {
            entered = entered.ToLower();
            string help = word.ToLower();
            int enteredLength = entered.Length;
            bool match = false;
            if (enteredLength == 0)
            {
                throw new Exception("Nezadal jsi žádné písmeno");
            }
            if (enteredLength != word.Length && enteredLength != 1)
            {
                throw new Exception("Zadán špatný počet znaků");
            }
            if (entered.Equals(help))
            {
                match = true;
                guessedWord = word;
            }
            else
            {

                for (int i = 0; i < lengthOfWord; i++)
                {
                    if (entered[0] == help[i])
                    {
                        match = true;
                        char[] pom = guessedWord.ToCharArray();
                        pom[i] = entered[0];
                        if (i == 0)
                        {
                            pom[i] = char.ToUpper(pom[i]);
                        }

                        //guessedWord = pom.ToString();
                        guessedWord = string.Concat(pom);
                    }
                }
            }
            return match;
        }
        public bool rightGuessed()
        {
            string help = word.ToLower();
            string help1 = guessedWord.ToLower();
            return help.Equals(help1);
        }

        private string uploadFromDatabase()
        {
            //string server = "localhost";
            //string database = "javasibenice";
            //string username = "root";
            //string password = "";
            //string constring = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + username + ";" + "PASSWORD=" + password + ";";


            string helpWord = "";


            string constring = "";

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings["DatabaseConnection"];

            // If found, return the connection string.
            if (settings != null)
                constring = settings.ConnectionString;

            

            MySqlConnection conn = new MySqlConnection(constring);
            try
            {
                
                conn.Open();
                string query = "SELECT * FROM slova ORDER BY RAND() LIMIT 2";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                //while (reader.Read())
                //{
                //    Console.WriteLine($"{reader["ID"]}, {reader["slovo"]}");
                //    //Console.WriteLine(reader["slovo"]);
                //}
                reader.Read();
                helpWord= reader.GetString(1);
                if(helpWord.Equals(lastWord))
                {
                    reader.Read();
                    helpWord = reader.GetString(1);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                //TODO
                //MessageBox.Show("Nepovedlo se připojit do databáze", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                //throw new Exception("Nejde to");
            }
            finally
            {
                conn.Close();
            }
            return helpWord;
        }

    }
}
