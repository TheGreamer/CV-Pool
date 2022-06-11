using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CvPool.Classes
{
    public class Utilities
    {
        public static void GroupBoxProcess(GroupBox groupBox = null, VisibilityProcess processType = VisibilityProcess.ShowSingle)
        {
            for (int i = 0; i < CvPool.groupBoxes.Length; i++)
            {
                CvPool.groupBoxes[i].Visible = false;

                if (processType == VisibilityProcess.SetDocks)
                    CvPool.groupBoxes[i].Dock = DockStyle.Fill;
            }

            if (processType == VisibilityProcess.ShowSingle)
                groupBox.Visible = true;
        }

        public static void ClearValues(object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                switch (values[i])
                {
                    case ComboBox: (values[i] as ComboBox).SelectedIndex = -1; break;
                    case TextBox: (values[i] as TextBox).Text = string.Empty; break;
                    case RichTextBox: (values[i] as RichTextBox).Text = string.Empty; break;
                    case DateTimePicker: (values[i] as DateTimePicker).Value = DateTime.Now; break;
                    case NumericUpDown: (values[i] as NumericUpDown).Value = 0; break;
                    case DataGridView: (values[i] as DataGridView).Columns.Clear(); break;
                    case PictureBox: (values[i] as PictureBox).BackgroundImage = null; break;
                    case ListView: (values[i] as ListView).Items.Clear(); break;
                    case string: values[i] = string.Empty; break;
                    default: break;
                }
            }
        }

        public static void ToggleControlsEnabledState(Control[] controls, bool enable)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                controls[i].Enabled = enable;
            }
        }

        public static void ToggleControlsVisibleState(Component[] controls, bool visible, Component controlToHide = null, List<object> messageItems = null)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i] is ToolStripItem)
                    (controls[i] as ToolStripItem).Visible = visible;
            }

            if (controlToHide != null)
                if (controlToHide is ToolStripItem)
                    (controlToHide as ToolStripItem).Visible = false;

            if (messageItems != null)
                MessageBox.Show(messageItems[0].ToString(), messageItems[1].ToString(), (MessageBoxButtons)messageItems[2], (MessageBoxIcon)messageItems[3]);
        }

        public static void SetFilterColors(DataGridView dataGridView, Control[] controls, Color? backColor = null, Color? foreColor = null)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    for (int l = 0; l < controls.Length; l++)
                    {
                        DataGridViewCell cell = dataGridView.Rows[i].Cells[j];

                        if (cell.Value.ToString() == controls[l].Text)
                        {
                            cell.Style.BackColor = backColor ?? Color.LawnGreen;
                            cell.Style.ForeColor = foreColor ?? Color.DarkGreen;
                        }
                    }
                }
            }
        }

        public static void ClearSelectionFromDGV(DataGridView dataGridView) => dataGridView.ClearSelection();

        public static void SaveImage(string image, string folder, bool checkExistence = true)
        {
            if (checkExistence)
            {
                string fullPath = Path.Combine(folder, Path.GetFileName(image));
                if (!File.Exists(fullPath))
                    File.Copy(image, fullPath, true);
            }
            else
            {
                File.Copy(image, Path.Combine(folder, Path.GetFileName(image)), true);
            }
        }

        public static void ReplaceInFile(string filePath, string searchText, string replaceText)
        {
            StreamReader reader = new(filePath);
            string content = reader.ReadToEnd();
            reader.Close();

            content = Regex.Replace(content, searchText, replaceText);

            StreamWriter writer = new(filePath);
            writer.Write(content);
            writer.Close();
        }

        public static void SendMail(string receiverMailAddress, string messageSubject, string messageBody, bool isHtml = true)
        {
            string senderMailAddress = Encoding.ASCII.GetString(GetBytesFromBinary(File.ReadAllText($"{CvPool.textFolder}\\E-Mail.txt")));
            string senderMailPassword = Encoding.ASCII.GetString(GetBytesFromBinary(File.ReadAllText($"{CvPool.textFolder}\\Password.txt")));

            MailMessage message = new(senderMailAddress, receiverMailAddress)
            {
                Subject = messageSubject,
                Body = messageBody,
                IsBodyHtml = isHtml
            };

            SmtpClient client = new()
            {
                Port = 587,
                EnableSsl = true,
                Host = "smtp-mail.outlook.com",
                Credentials = new NetworkCredential(senderMailAddress, senderMailPassword)
            };

            try
            {
                client.Send(message);
            }
            catch
            {
                MessageBox.Show($"E-Posta gönderilirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static byte[] GetBytesFromBinary(string binary)
        {
            List<byte> list = new();

            for (int i = 0; i < binary.Length; i += 8)
            {
                list.Add(Convert.ToByte(binary.Substring(i, 8), 2));
            }

            return list.ToArray();
        }
    }
}