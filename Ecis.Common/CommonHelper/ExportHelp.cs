using Ecis.Common.Exceptions;
using System;
using System.IO;
using System.Windows.Forms;

namespace Ecis.Common.CommonHelper
{
    public static class ExportHelp
    {
        public static void DataGridViewToExcel(DataGridView dgv)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "Export Excel File";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName == "")
                return;
            Stream myStream;
            myStream = saveFileDialog.OpenFile();
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
            string str = "";
            try
            {
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    if (!dgv.Columns[i].Visible) continue;
                    if (i > 0)
                    {
                        str += "\t";
                    }
                    str += dgv.Columns[i].HeaderText;
                }
                sw.WriteLine(str);
                for (int j = 0; j < dgv.Rows.Count; j++)
                {
                    string tempStr = "";
                    for (int k = 0; k < dgv.Columns.Count; k++)
                    {
                        if (!dgv.Columns[k].Visible) continue;
                        if (k > 0)
                        {
                            tempStr += "\t";
                        }
                        if (dgv.Rows[j].Cells[k].Value == null)
                            tempStr += string.Empty;
                        else
                            tempStr += dgv.Rows[j].Cells[k].Value.ToString();
                    }
                    sw.WriteLine(tempStr);
                }
                sw.Close();
                myStream.Close();
            }
            catch (Exception ex)
            {
                ExceptUtil.Throw(ErrorCodeContract.ExceptionCode, inner: ex, emType: ErrorMsgType.Info);
            }
            finally
            {
                sw.Close();
                myStream.Close();
            }
        }
    }
}