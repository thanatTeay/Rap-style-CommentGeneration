using SoundCapture;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTwitchCapture
{
    class TheTool
    {
        static public MainWindow Sys = null;


        //only Numeric Textbox
        public static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }

        //If blank get 0
        public static int getInt(System.Windows.Controls.TextBox txt)
        {
            int i = 0;
            try { i = int.Parse(txt.Text); }
            catch { }
            return i;
        }

        public static void removeSpace(System.Windows.Controls.TextBox tb)
        {
            tb.Text = tb.Text.Replace(" ", string.Empty);
            tb.Select(tb.Text.Length, 0);//move cursor
        }

        //If blank get 0
        public static int getInt(System.Windows.Controls.ComboBox cb)
        {
            int i = 0;
            try
            {
                i = int.Parse(cb.SelectedItem.ToString());
            }
            catch { }
            return i;
        }

        //If blank get 0
        public static double getDb(System.Windows.Controls.ComboBox cb)
        {
            double d = 0;
            try
            {
                d = double.Parse(cb.Text);
            }
            catch { }
            return d;
        }

        //If blank get 0
        public static int getInt(string txt)
        {
            int i = 0;
            try { i = int.Parse(txt); }
            catch { }
            return i;
        }

        //If blank get 0
        public static double getDouble(System.Windows.Controls.TextBox txt)
        {
            double d = 0;
            try { d = double.Parse(txt.Text); }
            catch { }
            return d;
        }

        //If blank get 0
        public static double getDouble(string txt)
        {
            double d = 0;
            try { d = double.Parse(txt); }
            catch { }
            return d;
        }

        public static double adjustRange(double d, double min, double max)
        {
            if (d < min) { d = min; }
            else if (d > max) { d = max; }
            return d;
        }

        static public void openFileLocation(string fullPath)
        {
            try
            {
                string path = Path.GetDirectoryName(fullPath);
                Process.Start(path);
            }
            catch { }
        }

        static public void delFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch { }
        }

        static public Boolean checkPathExist(string s)
        {
            return File.Exists(s);
        }

        //=============================================================================================
        //===================== prepare ===================================================

        static public string getFileName_byPath(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        static public string getExtension_byPath(string path)
        {
            return Path.GetExtension(path);
        }

        static public string getDirectory_byPath(string path)
        {
            return Path.GetDirectoryName(path);
        }

        static public string getPath_withoutExtension(string path)
        {
            return Path.GetDirectoryName(path);
        }

        //______.exe >> ______
        static public string getFilePathExcludeExtension_byPath(string path)
        {
            if (path == "") { return ""; }
            return TheTool.getDirectory_byPath(path) + @"\" + TheTool.getFileName_byPath(path);
        }

        //0 : 1 >> 01  if s = "0"
        static public string getTxt_Numeric_FillBy(string txt, int unit, string s)
        {
            for (int i = txt.Length; i < unit; i++)
            {
                txt = s + txt;
            }
            return txt;
        }


        //(i,2,"0")
        //0 : 1 >> 01
        static public string getTxt_Numeric_FillBy(int txt, int unit, string s)
        {
            return getTxt_Numeric_FillBy(txt.ToString(), unit, s);
        }

        //2 : 0.1 >> 0.10
        static public string getTxt_NumericDigit_FillBy0(string txt, int digit)
        {
            int length = txt.Length;
            int length_expect = 2 + digit;
            for (int i = length; i < length_expect; i++)
            {
                txt += "0";
            }
            return txt;
        }

        //(1,3) >> 001
        static public string getTxt_preFillBy0(string txt, int unit)
        {
            string s = "";
            int length = txt.Length;
            while (length < unit) { s += "0"; length++; }
            s += txt;
            return s;
        }

        static public void folder_CreateIfMissing(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        static public List<string> folder_explore(string rootPath)
        {
            List<string> list_filePath = new List<string>();
            try
            {
                DirectoryInfo root = new DirectoryInfo(rootPath);
                FileInfo[] Files = root.GetFiles("*.*");
                foreach (FileInfo f in Files) { list_filePath.Add(f.Name); }
            }
            catch (Exception ex) { Sys.showError(ex); }
            return list_filePath;
        }

        // d1/d2
        public static string writeDetectSummary(int total_d1, int total_d2)
        {
            double temp = total_d1 * 100;
            temp = temp / total_d2;
            return total_d1 + " _ " + Math.Round(temp, 2) + "%";
        }

        public static string writeDetectPercent(int total_d1, int total_d2)
        {
            double temp = total_d1 * 100;
            temp = temp / total_d2;
            return total_d1 + " / " + total_d2 + " _ " + Math.Round(temp, 2) + "%";
        }

        public static string calTimeTotal(int time)
        {
            string timeStr = "";
            if (time > 60)
            {
                timeStr += "/";
                int h = 0; int m = 0; int s = 0;
                //
                while (time >= 3600) { h++; time -= 3600; }
                while (time >= 60) { m++; time -= 60; }
                s = time;
                //

                if (h > 0) { timeStr += " " + h + "h"; }
                if (m > 0) { timeStr += " " + m + "m"; }
                if (s > 0) { timeStr += " " + s + "s"; }
            }
            return timeStr;
        }
       

        //=========================================================================
        //=========================================================================
        static public Boolean export_dataTable_to_CSV(string pathSave, DataTable dataTable)
        {
            try
            {
                StreamWriter sw = new StreamWriter(pathSave, false);// Create the CSV
                //--- Write header ---
                string header = "";
                int colCount = dataTable.Columns.Count;
                for (int i = 0; i < colCount; i++)
                {
                    header += dataTable.Columns[i];
                    if (i < colCount - 1)
                    {
                        header += ",";
                    }
                }
                sw.Write(header); sw.Write(sw.NewLine);
                //--------------------------------------------------
                // Now write all the rows.
                foreach (DataRow r in dataTable.Rows)
                {
                    for (int i = 0; i < colCount; i++)
                    {
                        if (!Convert.IsDBNull(r[i]))
                        {
                            sw.Write(TheTool.string_replaceChar(r[i].ToString(), ",", " "));
                        }
                        if (i < colCount - 1)
                        {
                            sw.Write(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
                //
                return true;
            }
            catch { return false; }
        }

        static public int CSV_countCol(string path)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                string[] cols = lines[0].Split(',');
                return cols.Count();
            }
            catch { return 0; }
        }

        //return list;
        static public void read_File(
            ref List<string> list, string path, Boolean readHeader)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (i == 0 && readHeader == false) { }
                    else { list.Add(lines[i]); }
                }
            }
            catch { }
        }

        //===========================================
        static public List<string> concatFile_OWS(List<string> filePath_list, Boolean withHeader, int skip)
        {
            List<string> concat_txt = new List<string>();
            string filename;
            int i = 0;
            foreach (string file_path in filePath_list)
            {
                filename = TheTool.getFileName_byPath(file_path);
                if (i == 0)
                {
                    TheTool.spec_read_File_toConcat(ref concat_txt, file_path, skip, withHeader, filename);
                }
                else
                {
                    TheTool.spec_read_File_toConcat(ref concat_txt, file_path, skip, false, filename);
                }
                i++;
            }
            return concat_txt;
        }

        static public List<string> concatFile(List<string> filePath_list, Boolean withHeader, int skip)
        {
            List<string> concat_txt = new List<string>();
            string filename;
            int i = 0;
            foreach (string file_path in filePath_list)
            {
                filename = TheTool.getFileName_byPath(file_path);
                if (i == 0)
                {
                    read_File_toConcat(ref concat_txt, file_path, skip, true, filename);
                }
                else
                {
                    read_File_toConcat(ref concat_txt, file_path, skip, false, filename);
                }
                i++;
            }
            return concat_txt;
        }

        //return list;
        static public void read_File_toConcat(
            ref List<string> list, string path
            , int skipRow, Boolean firstFile, string fileName)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (i == 0 && firstFile == true)
                    {
                        list.Add("file," + lines[i]);
                    }
                    else if (i < skipRow) { }
                    else { list.Add(fileName + "," + lines[i]); }
                }
            }
            catch { }
        }


        //specific fileName
        static public void spec_read_File_toConcat(
            ref List<string> list, string path
            , int skipRow, Boolean firstFile, string fileName)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (i == 0 && firstFile == true)
                    {
                        list.Add("file,user," + lines[i]);
                    }
                    else if (i < skipRow) { }
                    else
                    {
                        list.Add(
                            fileName + ","
                            + spec_getUser_fromFileName(fileName) + ","
                            + lines[i]);
                    }
                }
            }
            catch { }
        }

        //File A01_xxxxxx  >> get A01
        static public string spec_getUser_fromFileName(string fileName)
        {
            return fileName.Split('_')[0];
        }


        static public string filePath = "";
        static public Microsoft.Win32.OpenFileDialog dialog;

        static public Nullable<bool> openFileDialog_01(Boolean multiFile, string type, string initialPath)
        {
            dialog = new Microsoft.Win32.OpenFileDialog();
            if (initialPath != "") { dialog.InitialDirectory = initialPath; }
            if (multiFile) { dialog.Multiselect = true; }
            //
            dialog.DefaultExt = type;
            if (type == ".arff")
            {
                dialog.Filter = "Arff data files (*.arff) |*.arff";
            }
            else if (type == ".xml")
            {
                dialog.Filter = "XML map files (*.xml) |*.xml";
            }
            else if (type == ".txt")
            {
                dialog.Filter = "Text files (*.txt) |*.txt";
            }
            else if (type == ".*")
            {
                dialog.Filter = "All files (*.*) |*.*";
            }
            else
            {
                dialog.Filter = "CSV basic (*.csv) |*.csv";
            }
            //
            return dialog.ShowDialog();
        }


        static public Nullable<bool> openFileDialog(Boolean multiFile, string type, string initialPath, string ext)
        {
            dialog = new Microsoft.Win32.OpenFileDialog();
            if (initialPath != "") { dialog.InitialDirectory = initialPath; }
            if (multiFile) { dialog.Multiselect = true; }
            //
            dialog.DefaultExt = type;
            dialog.Filter = ext;
            //
            return dialog.ShowDialog();
        }

        static public void copy_File(string path_origin, string path_save)
        {
            try
            {
                FileInfo file = new FileInfo(path_origin);
                file.CopyTo(path_save);
            }
            catch { }
        }


        //==============================================================
        public static double[] intArr_divideAll(int[] dividend, int[] divisor)
        {
            double[] d = new double[dividend.Count()];
            for (int i = 0; i < dividend.Count(); i++)
            {
                if (divisor[i] != 0) { d[i] = (double)dividend[i] / divisor[i]; }
            }
            return d;
        }

        public static double[] intArr_divideAll(int[] arr, int divisor)
        {
            double[] d = new double[arr.Count()];
            for (int i = 0; i < arr.Count(); i++)
            {
                d[i] = (double)arr[i] / divisor;
            }
            return d;
        }

        public static string intArr_getListString(int[] arr)
        {
            string s = "";
            for (int i = 0; i < arr.Count(); i++)
            {
                if (i > 0) { s += ","; }
                s += arr[i];
            }
            return s;
        }

        public static string doubleArr_getListString(double[] arr)
        {
            string s = "";
            for (int i = 0; i < arr.Count(); i++)
            {
                if (i > 0) { s += ","; }
                s += arr[i];
            }
            return s;
        }


        //index is row number, not id
        public static double dataTable_getAverage(DataTable dt, List<int> indices, string col_name)
        {
            List<double> sum_value = new List<double>();
            try
            {
                foreach (int i in indices)
                {
                    sum_value.Add(TheTool.getDouble(dt.Rows[i][col_name].ToString()));
                }
            }
            catch (Exception ex) { Sys.showError(ex); }
            return sum_value.Average();
        }

        public static double dataTable_getAverage(DataTable dt, string col_name)
        {
            List<double> avg_value = new List<double>();
            //try
            //{
            foreach (DataRow dr in dt.Rows)
            {
                avg_value.Add(TheTool.getDouble(dr[col_name].ToString()));
            }
            return avg_value.Average();
            //}
            //catch (Exception ex)
            //{
            //    TheSys.showError("ERROR (AVG on Column) : col=" + col_name + " count = " + dt.Columns.Count);
            //    TheSys.showError(ex);
            //    return 0;
            //}
        }

        public static double calNorm(double[] d)
        {
            double norm = 0;
            try
            {
                for (int i = 0; i < d.Count(); i++)
                {
                    norm += Math.Pow(d[i], 2);
                }
                norm = Math.Sqrt(norm);
            }
            catch (Exception e) { Sys.showError("Err: [calNorm()] " + e.ToString(), true); }
            return norm;
        }

        public static double calEuclidian(double[] d1, double[] d2)
        {
            double eucli = 0;
            try
            {
                if (d1.Count() == d2.Count())
                {
                    for (int i = 0; i < d1.Count(); i++)
                    {
                        eucli += Math.Pow(d1[i] - d2[i], 2);
                    }
                }
                eucli = Math.Sqrt(eucli);
            }
            catch (Exception e) { Sys.showError("Err: [calEuclidian_2Joint()] " + e.ToString(), true); }
            return eucli;
        }


        //Replace "a" by "b"
        static public string string_replaceChar(string txt, string a, string b)
        {
            return txt.Replace(a, b);
        }

        public static void SortData(List<List<double>> data)
        {
            foreach (List<double> row in data) { row.Sort(); }
        }

        ////Intput row<col> 
        static public List<string> getListString(List<List<double>> list_old)
        {
            List<string> list_new = new List<string>();
            try
            {
                foreach (List<double> row in list_old)
                {
                    string line = ""; int c = 0;
                    foreach (double col in row)
                    {
                        if (c > 0) { line += ","; }
                        line += col;
                        c++;
                    }
                    list_new.Add(line);
                }
            }
            catch (Exception ex) { Sys.showError(ex); }
            return list_new;
        }

        static public void exportCSV_orTXT(string file_path, List<string> list, Boolean popUp_ifFinish)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(file_path))
                {
                    foreach (string s in list)
                    {
                        file.WriteLine(s);
                    }
                }
                if (popUp_ifFinish == true) { System.Windows.MessageBox.Show(@"Save to '" + file_path + "'", "Export CSV"); }
            }
            catch (Exception ex)
            {
                Sys.showError("CSV: unsuccessful export " + file_path);
                Sys.showError(ex);
            };
            //WebClient webC = new WebClient();
            //Uri newUri = new Uri("http://localhost:49268/dosomething.aspx");
            //webC.UploadStringAsync(newUri, string.Empty);
        }

        static public void exportCSV_orTXT(string file_path, string s, Boolean popUp_ifFinish)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(file_path))
                {
                    file.WriteLine(s);
                }
                if (popUp_ifFinish == true) { System.Windows.MessageBox.Show(@"Save to '" + file_path + "'", "Export CSV"); }
            }
            catch (Exception ex)
            {
                Sys.showError("CSV: unsuccessful export " + file_path);
                Sys.showError(ex);
            };
        }

        static public void exportCSV_orTXT(string file_path, List<int> list_int, Boolean popUp_ifFinish)
        {
            List<string> list = new List<string>();
            foreach (int i in list_int) { list.Add(i.ToString()); }
            exportCSV_orTXT(file_path, list, popUp_ifFinish);
        }

        static public List<string> read_File_getListString(string path)
        {
            try
            {
                var logFile = File.ReadAllLines(path);
                return new List<string>(logFile);
            }
            catch (Exception ex) { Sys.showError(ex); return new List<string> { }; }
        }

        //static public double[][] read_File_getArrayArray(string path, Boolean skipFirstRow)
        //{
        //    List<List<double>> list_list = read_File_getListListDouble(path,skipFirstRow);
        //    try
        //    {
        //        int row = list_list.Count();
        //        int col = list_list.First().Count();
        //        double[][] array_array = new double[row][];
        //        int r = 0;
        //        foreach (List<double> row_l in list_list)
        //        {
        //            array_array[r] = row_l.ToArray();
        //        }
        //        return array_array;
        //    }
        //    catch { }
        //    return new double[0][];
        //}

        //row<col>
        static public List<List<double>> read_File_getListListDouble(string path, Boolean skipFirstRow)
        {
            List<List<double>> col_list = new List<List<double>>();
            try
            {
                var logFile = File.ReadAllLines(path);
                int r = 0;
                foreach (string s in logFile)
                {
                    if (r > 0 || !skipFirstRow)
                    {
                        List<double> row_list = new List<double>();
                        string[] cell = TheTool.splitText(s, ",");
                        for (int i_c = 0; i_c < cell.Count(); i_c++)
                        {
                            row_list.Add(TheTool.getDouble(cell[i_c]));
                        }
                        col_list.Add(row_list);
                    }
                    r++;
                }
            }
            catch { }
            return col_list;
        }

        static public string read_File_getFirstLine(string path)
        {
            try
            {
                return File.ReadLines(path).First();
            }
            catch { return ""; }
        }

        static public string read_File_get1String(string path)
        {
            string txt = "";
            try
            {
                List<string> stringList = TheTool.read_File_getListString(path);
                foreach (string s in stringList)
                {
                    txt += s;
                }
            }
            catch { }
            return txt;
        }

        static public string convertBoolean_Str(Boolean b, string T, string F)
        {
            if (b == true) { return T; }
            else { return F; }
        }

        static public string convertBoolean_01(Boolean b)
        {
            if (b == true) { return "1"; }
            else { return "0"; }
        }

        static public int convertBoolean_01int(Boolean b)
        {
            if (b == true) { return 1; }
            else { return 0; }
        }

        static public Boolean convert01_Boolean(string txt)
        {
            if (txt == "1") { return true; }
            else { return false; }
        }

        static public String convert01_Boolean(int txt)
        {
            if (txt == 1) { return "true"; }
            else { return "false"; }
        }

        static public Boolean writeFile(List<string> allLine, string fileURL, Boolean popUp_ifFinish)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileURL))
                {
                    foreach (string line in allLine)
                    {
                        file.WriteLine(line);
                    }
                }
                if (popUp_ifFinish == true) { System.Windows.MessageBox.Show(@"Save: " + fileURL); }
                return true;
            }
            catch (Exception ex)
            {
                Sys.showError("Write File Error: " + fileURL);
                Sys.showError(ex);
                return false;
            }
        }


        public static double calAVG(List<double> list)
        {
            try { return list.Skip(0).Take(list.Count).Average(); }
            catch { return 0; }
        }

        public static string[] getFilePath_inFolder(string folderPath, Boolean nameonly)
        {
            try
            {
                if (nameonly)
                {
                    string[] files = Directory.GetFiles(folderPath, "*.xml", SearchOption.TopDirectoryOnly);
                    List<string> files_nm = new List<string>();
                    foreach (string a in files)
                    {
                        files_nm.Add(TheTool.getFileName_byPath(a));
                    }
                    return files_nm.ToArray();
                }
                else { return Directory.GetFiles(folderPath, "*.xml", SearchOption.TopDirectoryOnly); }
            }
            catch { return new string[] { }; }
        }

        public static string[] splitText(string s, string splt_txt)
        {
            try
            {
                return s.Split(new string[] { splt_txt }, StringSplitOptions.None);
            }
            catch { return new string[0]; }
        }

        public static List<int> splitText_getListInt(string s, string splt_txt)
        {
            List<int> list_int = new List<int>();
            foreach (string s0 in splitText(s, splt_txt)) { list_int.Add(getInt(s0)); }
            return list_int;
        }

        public static double getNormalized(double v, double min, double max)
        {
            double r = max - min;
            if (r > 0)
            {
                v = v - min;
                v = v / r; return v;
            }
            else { return 0; }
        }

        //Output has no header : double[row, col]
        public static double[,] getArrayDouble_fromDataTable(DataTable dt)
        {
            var numRows = dt.Rows.Count;
            var numCols = dt.Columns.Count;
            double[,] arr = new double[numRows, numCols];
            for (int i_c = 0; i_c < numCols; i_c++)
            {
                for (int i_r = 0; i_r < numRows; i_r++)
                {
                    arr[i_r, i_c] = TheTool.getDouble(dt.Rows[i_r][i_c].ToString());
                }
            }
            return arr;
        }

        //Output has no header : double[row, col]
        public static double[,] getArrayDouble_fromDataList(List<String> data, Boolean input_hasHeader)
        {
            if (input_hasHeader) { data.RemoveAt(0); }
            var numRows = data.Count();
            var numCols = TheTool.splitText(data.First(), ",").Count();
            double[,] arr = new double[numRows, numCols];
            int i_r = 0;
            foreach (string str in data)
            {
                string[] cell = TheTool.splitText(str, ",");
                for (int i_c = 0; i_c < cell.Count(); i_c++)
                {
                    arr[i_r, i_c] = TheTool.getDouble(cell[i_c]);
                }
                i_r++;
            }
            return arr;
        }

        public static String string_remove(String s, String removeTxt)
        {
            return s.Replace(removeTxt, string.Empty);
        }

        public static String string_Tab(int count)
        {
            String txt = "";
            for (int i = 0; i < count; i++) { txt += "\t"; }
            return txt;
        }

        public static Boolean stringExist_inList(String txt, List<String> list)
        {
            Boolean exist = false;
            foreach (String s in list) { if (String.Equals(s, txt)) { exist = true; } }
            return exist;
        }

        public static void sortComboBox(System.Windows.Controls.ComboBox combo)
        {
            combo.Items.SortDescriptions.Add(
                new System.ComponentModel.SortDescription("",
                System.ComponentModel.ListSortDirection.Ascending));
            combo.SelectedIndex = 0;
        }

        public static int[] array_increaseIndex(int[] oldArray, int inceaseSize)
        {
            int[] newArray = new int[oldArray.Count() + inceaseSize];
            for (int i = 0; i < oldArray.Count(); i++)
            {
                newArray[i] = oldArray[i];
            }
            return newArray;
        }

        public static int[] array_addIndex(int[] oldArray, int value)
        {
            int[] newArray = array_increaseIndex(oldArray, 1);
            newArray[oldArray.Count()] = value;
            return newArray;
        }

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public static List<T> list_Copy<T>(List<T> original)
        {
            List<T> new_list = new List<T>();
            new_list.AddRange(original);
            return new_list;
        }

        public static List<T> list_CutAt<T>(List<T> original, int lastID)
        {
            List<T> new_list = new List<T>();
            int i = 0;
            foreach (T o in original)
            {
                new_list.Add(o);
                i++;
                if (i == lastID) { break; }
            }
            return new_list;
        }

        //select row base on "time" in list
        public static List<T> list_SelectRow<T>(List<T> list_original, List<int> selectedRow)
        {
            List<T> list_new = new List<T>();
            int i = 0;
            foreach (T r in list_original)
            {
                if (selectedRow.Contains(i))
                {
                    list_new.Add(r);
                }
                i++;
            }
            return list_new;
        }

        public static string math_getOptReversion(string origin, Boolean reverse)
        {
            if (reverse)
            {
                if (origin == ">") { return "<="; }
                else if (origin == ">=") { return "<"; }
                else if (origin == "<=") { return ">"; }
                else if (origin == "<") { return ">="; }
                else if (origin == "!=") { return "="; }
                else { return "!="; }
            }
            else { return origin; }
        }

        public static Boolean checkTimePass(int moreThan, DateTime begin, DateTime now)
        {
            Boolean r = false;
            TimeSpan span = now.Subtract(begin);
            int timechange = (int)span.TotalMilliseconds;
            if (timechange > moreThan) { r = true; }
            return r;
        }

        public static List<List<double>> ListList_SwitchColumnToRow(List<List<double>> data)
        {
            try
            {
                var tmp = new List<List<double>>();
                int row = 0;
                int col = 0;
                for (col = 0; col < data[0].Count; col++)
                {
                    var tmp_list = new List<double>();
                    for (row = 0; row < data.Count; row++)
                    {
                        tmp_list.Add(data[row][col]);
                    }
                    tmp.Add(tmp_list);
                }

                return (tmp);
            }
            catch (Exception ex) { Sys.showError(ex); return new List<List<double>>(); }
        }

        //remove column : Input must have header (output have header)
        public static List<String> data_cropCol(List<String> data, string col_first, string col_last)
        {
            List<String> result = new List<String>();
            try
            {
                String[] cell;
                //---- seek First & Last -------------------------------------
                cell = TheTool.splitText(data.First(), ",");
                int i_first = 0; int i_last = cell.Count() - 1;
                int stage = 0;
                for (int i = 0; i < cell.Count(); i++)
                {
                    if (stage == 0)
                    {
                        if (col_first == "" || cell[i] == col_first) { i_first = i; stage = 1; }
                    }
                    else
                    {
                        if (col_last == "") { break; }
                        else if (cell[i] == col_last) { i_last = i; break; }
                    }
                }
                //---- Data -------------------------------------
                foreach (String str in data)
                {
                    string s = "";
                    cell = TheTool.splitText(str, ",");
                    for (int i = i_first; i <= i_last; i++)
                    {
                        s += cell[i];
                        if (i != i_last) { s += ","; }
                    }
                    result.Add(s);
                }
            }
            catch { }
            //catch (Exception ex) { TheSys.showError(ex.ToString()); }
            return result;//New Table
        }

        //=============================================================================
        //===== Data Table ============================================================

        //remove column by column name (e.g. "p_jump", "")
        public static DataTable dataTable_cropCol(DataTable dt, string col_first, string col_last)
        {
            DataTable dt2 = dt.Copy();
            string nm;
            int stage = 0;
            foreach (DataColumn column in dt.Columns)
            {
                nm = column.ColumnName;
                if (stage == 0 && (col_first == "" || nm == col_first)) { stage = 1; }
                if (stage != 1) { dt2.Columns.Remove(nm); }//Remove
                if (stage == 1 && nm == col_last) { stage = 2; }
            }
            return dt2;//New Table
        }

        //remove column by column index
        public static DataTable dataTable_cropCol(DataTable dt, int col_first, int col_last)
        {
            DataTable dt2 = dt.Copy();
            string nm;
            int i_col = 0;
            foreach (DataColumn column in dt.Columns)
            {
                nm = column.ColumnName;
                if (i_col < col_first || (i_col > col_last && col_last != 0)) { dt2.Columns.Remove(nm); }
                i_col++;
            }
            return dt2;//New Table
        }

        //for Double Table, round to Integer (x-digit)
        public static void dataTable_roundValue(DataTable dt, int digit)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    dr[dc] = Math.Round(TheTool.getDouble(dr[dc].ToString()), digit);
                }
            }
        }

        //id on first coluumn (select Range)
        public static DataTable dataTable_selectRow_byId(DataTable dt, int start, int end)
        {
            DataTable dt2 = dt.Clone();
            foreach (DataRow dr in dt.Rows)
            {
                int i = TheTool.getInt(dr[0].ToString());
                if (i >= start && i <= end) { dt2.Rows.Add(dr.ItemArray); }
            }
            return dt2;
        }

        public static DataTable dataTable_selectFirstRow(List<DataTable> dt_list)
        {
            DataTable dt2 = dt_list.First().Clone();
            foreach (DataTable dt in dt_list) { dt2.Rows.Add(dt.Rows[0]); }
            return dt2;
        }

        public static DataTable dataTable_selectFirstRow_byId(DataTable dt)
        {
            DataTable dt2 = dt.Clone();
            dt2.Rows.Add(dt.Rows[0]);
            return dt2;
        }

        public static DataTable dataTable_selectRow_byIndex(DataTable dt, int start, int end)
        {
            DataTable dt2 = dt.Clone();
            int i = 0;
            if (end < start)
            {
                int s = end; int e = start;
                start = s; end = e;
            }
            foreach (DataRow dr in dt.Rows)
            {
                if (i >= start && i <= end) { dt2.Rows.Add(dr.ItemArray); }
                i++;
            }
            return dt2;
        }

        //Table: row 1 = Min, row 2 = Max
        public static DataTable dataTable_getMaxMinTable(DataTable dt)
        {

            DataTable dt2 = dt.Clone();
            dt2.Rows.Add(dt2.NewRow());
            dt2.Rows.Add(dt2.NewRow());
            try
            {
                int col_i = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    double min = 0;
                    double max = 0;
                    int row_i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        double v = TheTool.getDouble(dr[dc].ToString());
                        if (row_i == 0) { min = v; max = v; }
                        else if (v < min) { min = v; }
                        else if (v > max) { max = v; }
                        row_i++;
                    }
                    dt2.Rows[0][col_i] = min;
                    dt2.Rows[1][col_i] = max;
                    col_i++;
                }
            }
            catch (Exception ex) { Sys.showError(ex.ToString()); }
            return dt2;
        }

        public static double dataTable_getData_ifColumnExist_double(DataRow row, DataTable dt, string col)
        {
            return getDouble(dataTable_getData_ifColumnExist(row, dt, col));
        }

        public static int dataTable_getData_ifColumnExist_int(DataRow row, DataTable dt, string col)
        {
            return getInt(dataTable_getData_ifColumnExist(row, dt, col));
        }

        public static string dataTable_getData_ifColumnExist(DataRow row, DataTable dt, string col)
        {
            string s = "";
            if (dataTable_checkIfColumnExist(dt, col)) { s = row[col].ToString(); }
            return s;
        }

        public static Boolean dataTable_checkIfColumnExist(DataTable dt, string colName)
        {
            DataColumnCollection columns = dt.Columns;
            if (columns.Contains(colName)) { return true; }
            else { return false; }
        }

        public static DataTable dataTable_MinMaxNormalization(DataTable dt_data, DataTable dt_mm)
        {
            DataTable dt_normal = dt_data.Copy();
            List<String> result = new List<String>();
            try
            {
                foreach (DataColumn col in dt_normal.Columns)
                {
                    String col_name = col.ColumnName;
                    if (dataTable_checkIfColumnExist(dt_mm, col_name))
                    {
                        double min = TheTool.getDouble(dt_mm.Rows[0][col].ToString());
                        double max = TheTool.getDouble(dt_mm.Rows[1][col].ToString());
                        double range = max - min;
                        foreach (DataRow row in dt_normal.Rows)
                        {
                            if (range != 0)
                            {
                                double v = TheTool.getDouble(row[col].ToString());
                                v = (v - min) / range;
                                row[col] = v;
                            }
                            else { row[col] = 0; }
                        }
                    }
                }
            }
            catch (Exception ex) { Sys.showError("Normal: " + ex.ToString()); }
            return dt_normal;
        }

        //discritize (0,1) MMNormalized Range to 10 Partition
        public static DataTable dataTable_discritize10Partition(DataTable dt)
        {
            DataTable dt2 = dt.Copy();
            try
            {
                foreach (DataColumn dc in dt2.Columns)
                {
                    foreach (DataRow dr in dt2.Rows)
                    {
                        double v = TheTool.getDouble(dr[dc].ToString());
                        v *= 10;
                        if (v > 10) { v = 10; }
                        else if (v < 0) { v = 0; }
                        else { v = Math.Floor(v); }
                        dr[dc] = v;
                    }
                }
            }
            catch (Exception ex) { Sys.showError(ex); }
            return dt2;
        }

        //range is positive
        public static DataTable dataTable_partitize(DataTable dt, double range)
        {
            DataTable dt2 = dt.Copy();
            try
            {
                int i_c = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    int i_r = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        double v = TheTool.getDouble(dr[dc].ToString());
                        dt2.Rows[i_r][i_c] = getPartition(v, range);
                        i_r++;
                    }
                    i_c++;
                }
            }
            catch (Exception ex) { Sys.showError(ex); }
            return dt2;
        }

        public static DataTable dataTable_removeCol(DataTable dt, List<string> col_list)
        {
            DataTable dt2 = dt.Copy();
            try
            {
                foreach (string col in col_list)
                {
                    if (dt.Columns.Contains(col))
                    {
                        dt2.Columns.Remove(col);
                    }
                }
            }
            catch (Exception ex) { Sys.showError(ex); }
            return dt2;
        }

        //sort by column 1 then 2, 3, 4, ....
        public static DataTable dataTable_fullSort(DataTable dt)
        {
            try
            {
                int col_count = 0;
                String col_name = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (col_count > 0) { col_name += ", "; }
                    col_name += dc.ColumnName;
                    col_count++;
                }
                DataView dv = dt.DefaultView;
                dv.Sort = col_name;
                //TheSys.showError(col_name);
                return dv.ToTable();
            }
            catch (Exception ex) { Sys.showError(ex); return dt; }
        }

        public static DataTable dataTable_sort(DataTable dt, string colName, Boolean desc)
        {
            if (desc) { dt.DefaultView.Sort = colName + " DESC"; }
            else { dt.DefaultView.Sort = colName + " ASC"; }
            return dt.DefaultView.ToTable();
        }

        public static List<String> dataTable_getColList(DataTable dt)
        {
            List<string> col_list = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                col_list.Add(dc.ColumnName);
            }
            return col_list;
        }

        //Select By Id, not index
        //key pose must start from 0
        public static List<DataTable> dataTable_split(DataTable dt, List<int[]> keypose_list)
        {
            List<DataTable> dt_list = new List<DataTable>();
            foreach (int[] keypose in keypose_list)
            {
                if (keypose.Count() > 1 && keypose[1] > keypose[0] && keypose[0] > 0)
                {
                    dt_list.Add(TheTool.dataTable_selectRow_byId(dt, keypose[0], keypose[1]));
                }
            }
            return dt_list;
        }

        //--------------------------------------------------------------------------

        public static int getPartition(double dividend, double range)
        {
            //double temp = ((dividend - (dividend % divisor)) / divisor);
            //return (int) temp;
            //int i = 0;
            //if (dividend > 0) { for (double v = dividend; v >= range; v -= range) { i++; } }
            //else if (dividend < 0) { for (double v = dividend; v <= range; v += range) { i--; } }
            //return i;
            return (int)Math.Round(dividend / range);
        }

        //never used
        public static string string_cropText(string full, string start, string end)
        {
            string[] split1 = splitText(full, start);
            string[] split2 = splitText(split1[0], end);
            return split2[0];
        }

        public static double LinearRegression_Cal_Correlation(DataTable dt, string str1, string str2)
        {
            List<double> str2Value = new List<double>();
            List<double> str1Value = new List<double>();

            DataColumn str1Col = null;
            DataColumn str2Col = null;
            foreach (DataColumn column in dt.Columns)
            {
                if (column.ColumnName == str1)
                    str1Col = column;
                if (column.ColumnName == str2)
                    str2Col = column;
            }
            foreach (DataRow row in dt.Rows)
            {
                str2Value.Add(double.Parse(row[str2Col].ToString()));
                str1Value.Add(double.Parse(row[str1Col].ToString()));
            }
            return (LinearRegression(str1Value.ToArray(), str2Value.ToArray(), 0, str2Value.Count));
        }

        public static double LinearRegression(double[] xVals, double[] yVals, int inclusiveStart, int exclusiveEnd)
        {

            double rsquared = 0f;
            double yintercept = 0f;
            double slope = 0f;

            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double ssX = 0;
            double ssY = 0;
            double sumCodeviates = 0;
            double sCo = 0;
            double count = exclusiveEnd - inclusiveStart;

            for (int ctr = inclusiveStart; ctr < exclusiveEnd; ctr++)
            {
                double x = xVals[ctr];
                double y = yVals[ctr];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }
            ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            ssY = sumOfYSq - ((sumOfY * sumOfY) / count);
            double RNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            double RDenom = (count * sumOfXSq - (sumOfX * sumOfX))
             * (count * sumOfYSq - (sumOfY * sumOfY));
            sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            double meanX = sumOfX / count;
            double meanY = sumOfY / count;
            double dblR = RNumerator / Math.Sqrt(RDenom);
            rsquared = dblR * dblR;
            yintercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;

            return (rsquared);
        }

        public static string getString_fromList(List<int> i_list, string join_str)
        {
            string txt = "";
            if (i_list.Count > 0)
            {
                int c = 0;
                foreach (int i in i_list)
                {
                    if (c > 0)
                    {
                        txt += join_str;
                    }
                    txt += i;
                    c++;
                }
            }
            return txt;
        }

        public static string getString_fromList(List<string> i_list, string join_str)
        {
            string txt = "";
            if (i_list.Count > 0)
            {
                int c = 0;
                foreach (string i in i_list)
                {
                    if (c > 0)
                    {
                        txt += join_str;
                    }
                    txt += i;
                    c++;
                }
            }
            return txt;
        }

        //calSpherical() for double[] is exist
        public static double calSpherical(double[] target, double[] origin, Boolean Azimuthal)
        {
            double output = 0;
            try
            {
                if (target != origin)
                {
                    double x = target[0] - origin[0];
                    double y = target[1] - origin[1];
                    double z = target[2] - origin[2];
                    double r = Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2);
                    r = Math.Sqrt(r);
                    if (Azimuthal)
                    {
                        output = Math.Atan2(x, z);
                        output = TheTool.RadianToDegree(output);
                    }
                    else
                    {
                        output = Math.Acos(y / r);
                        output = TheTool.RadianToDegree(output);
                    }
                }
            }
            catch (Exception ex) { Sys.showError(ex); }
            return output;
        }

        //---------------------------------------


        //=========================================================================================

        public static string printText(List<string> s_list, string txt_joint)
        {
            string s = "";
            double a = 0;
            foreach (string s0 in s_list)
            {
                if (a > 0) { s += txt_joint; }
                s += s0;
                a++;
            }
            return s;
        }

        public static string printText(List<int> list_data)
        { return printText(list_data, ","); }

        public static string printText(List<int> list_data, string txt_join)
        {
            string output = ""; int i = 0;
            foreach (int d in list_data)
            {
                if (i > 0) { output += txt_join; }
                output += d;
                i++;
            }
            return output;
        }

        public static string printText(int[] list_data, string txt_join)
        {
            string output = ""; int i = 0;
            foreach (int d in list_data)
            {
                if (i > 0) { output += txt_join; }
                output += d;
                i++;
            }
            return output;
        }

        public static string printText(double[] list_data, string txt_join)
        {
            string output = ""; int i = 0;
            foreach (double d in list_data)
            {
                if (i > 0) { output += txt_join; }
                output += d;
                i++;
            }
            return output;
        }


        public static string printText(List<double> list_data, string txt_join)
        {
            return printText(list_data, txt_join, -1);
        }

        public static string printText(List<double> i_list, string txt_joint, int deci)
        {
            string s = "";
            double a = 0;
            foreach (double d in i_list)
            {
                if (a > 0) { s += txt_joint; }
                if (deci < 0) { s += d; }
                else { s += TheTool.math_roundE(d, deci); }
                a++;
            }
            return s;
        }

        public static List<string> printTextList(List<int[]> i_list)
        {
            List<string> output = new List<string>();
            foreach (int[] i in i_list) { output.Add(printText(i, ",")); }
            return output;
        }

        public static List<string> printTextLIst(List<List<int>> i_list)
        {
            List<string> output = new List<string>();
            for (int i = 0; i < i_list.Count; i++) { output.Add(printText(i_list[i])); }
            return output;
        }

        public static List<string> printTextLIst(List<List<int>> i_list, string indexPrefix, int digit)
        {
            List<string> output = new List<string>();
            for (int i = 0; i < i_list.Count; i++) { output.Add(getTxt_Numeric_FillBy(i.ToString(), digit, " ") + ": " + printText(i_list[i])); }
            return output;
        }
        //=============================================================

        public static void list_initialize(List<double> list, int size)
        {
            for (int i = 0; i < size; i++) { list.Add(0); }
        }


        public static void list_initialize(List<List<double>> list, int size)
        {
            for (int i = 0; i < size; i++) { list.Add(new List<double>()); }
        }

        public static List<int[]> listArray_sort(List<int[]> list, int col_id)
        {
            return list.OrderBy(x => x[col_id]).ToList();
        }

        public static double divide(double dividend, double divisor)
        {
            if (divisor == 0) { return 0; }
            else { return dividend / divisor; }
        }

        //http://stackoverflow.com/questions/4642687/given-start-point-angles-in-each-rotational-axis-and-a-direction-calculate-end
        public static double[] getPosition_originAngleDistance(double[] origin, double distance, double angleZ, double angleY)
        {
            angleZ = angleZ * Math.PI / 180;//deg to Radian
            angleY = angleY * Math.PI / 180;//deg to Radian
            double[] output = new double[3];
            output[0] = origin[0] + distance * Math.Cos(angleY) * Math.Sin(angleZ);
            output[1] = origin[1] + distance * Math.Sin(angleY);
            output[2] = origin[2] + distance * Math.Cos(angleY) * Math.Sin(angleZ);
            return output;
        }

        public static double getNorm(double a, double b)
        {
            return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }

        ////Row x Col
        //public static double[][] matrix_multiplication_NxN(double[][] M1, double[][] M2, int n)
        //{
        //    double[][] output = new double[n][];
        //    for (int i = 0; i < n; i++)
        //    {
        //        output[i][] = new double[]{ M1[0][0] *  M2[0][0], M1[0][0] *  M2[0][0]};

        //    }
        //    return output;
        //}
        public static void Matrix_print(double[,] M, int round)
        {
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    Sys.showError(Math.Round(M[i, j], round) + "_", false);
                }
                Sys.showError("");
            }
            Sys.showError("");
        }

        public static double[,] Matrix_Multiply(double[,] A, double[,] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            double temp = 0;
            double[,] kHasil = new double[rA, cB];
            if (cA != rB)
            {
                Console.WriteLine("matrik can't be multiplied !!");
            }
            else
            {
                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += A[i, k] * B[k, j];
                        }
                        kHasil[i, j] = temp;
                    }
                }
            }
            return kHasil;
        }

        //(6.123233995736766e-17, 2) to 6.12e-17
        public static string math_roundE(double d, int digit)
        {
            string format = "0.";//e.g. "0.000E0"
            for (int i = 0; i < digit; i++) { format += "0"; }
            format += "E0";
            return d.ToString(format);
        }

        public static int list_IndexOfMaximum(List<double> list)
        {
            return list.IndexOf(list.Max());
        }

        public static int list_IndexOfMinimum(List<double> list)
        {
            return list.IndexOf(list.Min());
        }

        public static void writeFile(string filePath, List<string> txt, Boolean overwrite)
        {
            if (overwrite)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                {
                    foreach (string s in txt) { file.WriteLine(s); }
                }
            }
            else
            {
                if (!File.Exists(filePath)) { TheTool.exportCSV_orTXT(filePath, new List<string>(), false); }
                TextWriter tsw = new StreamWriter(filePath, true);
                foreach (string t in txt) { tsw.WriteLine(t); }
                tsw.Close();
            }
        }

        public static void writeFile(string filePath, string txt, Boolean overwrite)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                if (overwrite)
                {
                    if (!File.Exists(filePath)) { TheTool.exportCSV_orTXT(filePath, new List<string>(), false); }
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
                    {
                        file.WriteLine(txt);
                    }
                }
                else
                {
                    if (!File.Exists(filePath)) { TheTool.exportCSV_orTXT(filePath, new List<string>(), false); }
                    TextWriter tsw = new StreamWriter(filePath, true);
                    tsw.WriteLine(txt);
                    tsw.Close();
                }
            }
            catch (Exception ex) { Sys.showError(ex); }
        }


        //static public Boolean writeFile(string data, string fileURL, Boolean popUp_ifFinish)
        //{
        //    List<String> list_s = new List<String>();
        //    list_s.Add(data);
        //    return writeFile(list_s, fileURL, popUp_ifFinish);
        //}


        public static void openFile(string file_path)
        {
            try { System.Diagnostics.Process.Start(file_path); }
            catch (Exception ex) { Sys.showError(ex); }
        }

        //Input: 1,24,34
        public static string textComma_Sort(string input, Boolean noDuplicate)
        {
            string output = "";
            List<int> list_i = new List<int>();
            foreach (string s in splitText(input, ",")) { list_i.Add(getInt(s)); }
            list_i.Sort();
            int index = 0;
            int last = -9999;
            foreach (int i in list_i)
            {
                if (!noDuplicate || i != last)
                {
                    if (index > 0) { output += ","; }
                    output += i; index++;
                    last = i;
                }
            }
            return output;
        }

        //Input: 1,24,34
        public static string textComma_DelLastItem(string input)
        {
            string output = "";
            string[] k = splitText(input, ",");
            for (int i = 0; i < k.Count() - 1; i++)
            {
                if (i > 0) { output += ","; }
                output += k[i];
            }
            return output;
        }

        public static List<int> listInt_selectUnmatch(List<int> all, List<int> match)
        {
            List<int> output = new List<int>();
            foreach (int i in all) { if (!match.Contains(i)) { output.Add(i); } }
            return output;
        }

        public static List<int> listInt_getReducedList(List<int> origin, List<int> remove)
        {
            List<int> output = new List<int>();
            output.AddRange(origin);
            foreach (int i in remove) { output.Remove(i); }
            return output;
        }

        //add addInt, e.g., {1,2}, into all List<int> in listListInt
        public static void listListInt_addListInt(ref List<List<int>> listListInt, List<int> addInt, Boolean sort)
        {
            foreach (List<int> listInt in listListInt)
            {
                listInt.AddRange(addInt);
                if (sort) { listInt.Sort(); }
            }
        }

        //"@attribute 'carbon' real" >> carbon
        //getTextBetween("@attribute 'carbon' real"," '","' ");
        public static string getTextBetween(string input, char a, char b)
        {
            return input.Split(a, b)[1];
        }

        public static void list_subtract(List<int> list, List<int> remove)
        {
            list.RemoveAll(i => remove.Contains(i));
        }

        public static Random rand = new Random(DateTime.Now.Millisecond);

        //Can be called 1 at a time, else duplicated
        // 1 <= month < 13
        public static int getRandInt(int ge, int l)
        {
            return rand.Next(ge, l);  // 1 <= month < 13
        }

        public static string getTime()
        {
            return DateTime.Now.ToString("MMdd_HHmmss");
        }

        public static string getTime2()
        {
            return DateTime.Now.ToString("HH:mm:ss");

        }
        //get a - b
        public static List<int> getList_subtracted(List<int> a, List<int> b)
        {
            var r = a.Except(b);
            return r.ToList();
        }

        ////getTxt_format(9.2125, 3) to 9.212
        //public static string getTxt_format(string s, int digit)
        //{
        //    return String.Format("{0:G" + digit + "}", s);
        //}

        //take List of probability
        public static double calEntropy(List<double> list_p)
        {
            double d = 0;
            foreach (double p in list_p) { if (p > 0) { d += p * Math.Log(p); } }
            return -d;
        }

        //"notepad"
        public static Boolean checkIfProcessIsRunning(string name)
        {
            Process[] pname = Process.GetProcessesByName(name);
            if (pname.Length == 0) { return false; }
            else { return true; }
        }


        public static double getSum(double[] da)
        {
            double s = 0;
            foreach (double d in da) { s += d; }
            return s;
        }

        public static void list_randomSelection()
        {
            //List<string> list = new List<string>() { "aaa", "bbb", "ccc", "ddd" };
            //int l = list.Count;
            //Random r = new Random();
            //int num = r.Next(l);
            //Sys.showError(list[num]);
        }


        static public void file_CreateIfMissing(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
                TextWriter tw = new StreamWriter(path);//dispose to unlock
                tw.Close();
            }
        }
    }

}
