

namespace EcomProject.Service
{
    public interface IBooksService
    {
       void LoadFileIntoCache();
    }

    public class BooksService : IBooksService
    {
        private void SearchBookInFile(string key)
        {

        }

        public void LoadFileIntoCache()
        {
          //  string filePath = @"searchFile.txt";

            // Specify the full path to the text file on the D: drive
            string filePath = @"D:\path\to\your\file.txt";



            // Use StreamReader to read the contents of the file
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            /*  try
              {
                   DataTable datatable = new DataTable();
                   string fileName = @"D:\searchFile.txt";

                   string filePath = System.IO.Path.GetFullPath("D:/searchFile.txt");

                   StreamReader streamreader = new StreamReader(filePath);

                  /*
                   char[] delimiter = new char[] { '\t' };
                   string[] columnheaders = new string[] { "WorkKey", "Edition", "Rating", "Date" };// streamreader.ReadLine().Split(delimiter);
                   foreach (string columnheader in columnheaders)
                   {
                       datatable.Columns.Add(columnheader); // I've added the column headers here.
                   }

                   while (streamreader.Peek() > 0)
                   {
                       DataRow datarow = datatable.NewRow();
                       datarow.ItemArray = streamreader.ReadLine().Split(delimiter);
                       datatable.Rows.Add(datarow);
                   }

              }
              catch (Exception ex)
              {

              }*/
            /* foreach (DataRow row in datatable.Rows)
             {
                 Console.WriteLine("----Row No: " + datatable.Rows.IndexOf(row) + "----");

                 foreach (DataColumn column in datatable.Columns)
                 {
                     //check what columns you need
                     if (column.ColumnName == "Column2" ||
                         column.ColumnName == "Column12" ||
                         column.ColumnName == "Column45")
                     {
                         Console.Write(column.ColumnName);
                         Console.Write(" ");
                         Console.WriteLine(row[column]);
                     }
                 }
             }*/
        }
    }
}
