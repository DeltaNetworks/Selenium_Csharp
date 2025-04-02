using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AutoFram.UIReport
{
    public static class Report
    {
        private static readonly object padlock = new object();
        private static readonly object padlock1 = new object();
        private static readonly object padlock2 = new object();
        private static int create_file = 0;
        private static int create_template = 0;
        private static string reportFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Documents\SeleniumReports\";
        public static string reportSubFolder = reportFolder + DateTime.Now.ToString("yyyyMMddHHmmss");
        private static string reportFileTitle = reportSubFolder + @"\report.html";
        public static string reportLog = reportSubFolder + @"\reportLog.txt";
        public static string reportFileException = reportSubFolder + @"\log.txt";
        public static string screenShotLocation = reportSubFolder + @"\";
        public static string currentMachineName = getMachineName();
        public static string QAmachine = "HH8VMM";
        public static string currentUserName = getUserName();
        public static string QAuser = "octop";
        public static int testCounter = 0;
        public static int setupteardown = 0;
        public static Stopwatch totalwatch = Stopwatch.StartNew();
        public static int threadcounter = 0;
        public static bool backfinish = false;
        private static string totalInfoToMail = "";
        public static void CreateReportFolder()
        {
            //Random rm = new Random();
            //int num = rm.Next(1, 4);
            //Thread.Sleep(num * 1000);
            if (create_file == 0)
            {
                lock (padlock)
                {
                    create_file++;
                    //if (!Directory.Exists(reportFolder))
                    //{
                    //    Directory.CreateDirectory(reportFolder);
                    //}
                    if (!Directory.Exists(reportSubFolder))
                    {
                        Directory.CreateDirectory(reportSubFolder);
                        WriteTestLog($"created: {reportSubFolder} | log file is created: {reportFileTitle}");
                        //SaveReportToFile(reportFileTitle, template);
                    }
                }
            }
        }
        public static void CreateTemplate()
        {
            Random rm = new Random();
            int num = rm.Next(1, 5);
            Thread.Sleep(num * 1000);
            if (create_template == 0)
            {
                lock (padlock1)
                {
                    create_template++;
                    List<string> template = new List<string>();
                    template.Add("<!DOCTYPE html><html><head><style>*{ font-family: sans-serif; }");
                    template.Add(".content-table { width: 95%;border-collapse: collapse;margin: 25px 0;");
                    template.Add("font-size: 0.9em;min-width: 400px;border-radius: 5px 5px 0 0;");
                    template.Add("overflow: hidden;box-shadow: 0 0 20px rgba(0, 0, 0, 0.15); }");
                    template.Add(".content-table thead tr {");
                    template.Add("background-color: #157bac;color: #ffffff;text-align: left;");
                    template.Add("font-weight: bold; }");
                    template.Add(".content-table th,.content-table td { padding: 12px 15px; }");
                    template.Add(".content-table tbody tr { border-bottom: 1px solid #dddddd; }");
                    template.Add(".content-table tbody tr:nth-of-type(even) { background-color: #f3f3f3; }");
                    template.Add(".content-table tbody tr:last-of-type { border-bottom: 2px solid #157bac; }");
                    template.Add(".content-table tbody tr.active-row { font-weight: bold;color: #009879; }");
                    template.Add("</style></head><body>");
                    template.Add("<h2>Automation UI Tests Report</h2>");
                    template.Add($"<h3>Total Test:&nbsp;&nbsp;22&nbsp;&nbsp;| Pass:&nbsp;&nbsp;22&nbsp;&nbsp;| Failed: 22</h3>");
                    template.Add("<table class=\"content-table\">");
                    template.Add("<thead><tr><th>Test #</th><th>Component</th><th>Test Name</th><th>Client</th><th>Browser</th><th>Start Time</th><th>Duration</th><th>ScreenShot</th><th>Result</th><th>Reason</th></tr></thead>");
                    template.Add("<tbody>");
                    template.Add("</tbody></table></body></html>");
                    template.Add("Tests:|Fail:)");
                    SaveReportToFile(reportFileTitle, template);
                }
            }
        }
        public static void SaveTestCase(string num, string component, string name, string client, string browser, string starttime, string duration, string sshot_file, string result, string excpDetails, string testhasdata, string failreason)
        {
            string exceptionDetails = null;
            string line = null;
            if (result.Contains("Fail"))
            {
                string reasoncolor = "";
                if (failreason.Length > 3)
                    reasoncolor = $"<td style=\"background-color:Tomato;\">{failreason}</td>";
                exceptionDetails = "Test Num: " + num + ", component: " + component + ", name: " + name + ", browser: " + browser + ", start time: " + starttime + ", exception: " + excpDetails;
                //SaveLogToFile(reportFileException, exceptionDetails + Environment.NewLine);
                WriteTestLog(exceptionDetails);
                line = ($"<tr><td>{num}</td><td>{component}</td><td>{name}</td><td>{client}</td><td>{browser}</td><td>{starttime}</td><td>{duration}</td><td><a href=\"{sshot_file}\" target=\"_blank\">Click</a></td><td style=\"background-color:Tomato;\">{result}</td>{reasoncolor}</tr>");
            }
            if (result.Contains("Pass"))
                line = ($"<tr><td>{num}</td><td>{component}</td><td>{name}</td><td>{client}</td><td>{browser}</td><td>{starttime}</td><td>{duration}</td><td></td><td style=\"background-color: #4ac05a;\">{result}</td><td></td></tr>");
            if (testhasdata == "No Data" || testhasdata == "Warning")
            {
                result = "No Data";
                line = ($"<tr><td>{num}</td><td>{component}</td><td>{name}</td><td>{client}</td><td>{browser}</td><td>{starttime}</td><td>{duration}</td><td><a href=\"{sshot_file}\" target=\"_blank\">Click</a></td><td style=\"background-color: #fab948;\">{result}</td><td></td></tr>");
            }
            //if (testhasdata == "Warning")
            //{
            //    result = "No Data";
            //    line = ($"<tr><td>{num}</td><td>{component}</td><td>{name}</td><td>{client}</td><td>{browser}</td><td>{starttime}</td><td>{duration}</td><td><a href=\"{sshot_file}\" target=\"_blank\">Click</a></td><td style=\"background-color: #fab948;\">{result}</td></tr>");
            //}
            SaveTestCaseToReportFile(line);
            //SaveTestCaseToReportFile(line);
        }

        private static void SaveTestCaseToReportFile(string line)
        {
            List<string> allLines = ReadReportFile();
            if (line.Contains("Fail") || line.Contains("No Data"))
                allLines.Insert(18, line);
            else
                allLines.Insert(allLines.Count - 1, line);
            allLines = RemoveDuplicate(allLines);
            SaveReportToFile(reportFileTitle, allLines);
        }
        private static List<string> RemoveDuplicate(List<string> testlist)
        {
            if (testlist.Count > 17)
            {
                for (int i = 18; i < testlist.Count; i++)
                {
                    if (!testlist[i].Contains("<tr><td>"))
                        testlist.RemoveAt(i);
                }
                testlist.Add("</tbody></table></body></html>");
            }
            return testlist;
        }

        public static void SetTotalsSummary(string env, string totalTime)
        {
            SetReportOrder();
            Report.WriteTestLog("START SET-TOTAL-SUMMARY");
            List<string> allLines = ReadReportFile();
            #region total calculations
            string username = Environment.UserName;
            int total = 0;
            int totalPass = 0;
            int totalFail = 0;
            int totalNoData = 0;
            foreach (string line in allLines)
            {
                if (line.StartsWith("<tr><td>"))
                    total++;
                if (line.StartsWith("<tr><td>") && line.Contains("Pass"))
                    totalPass++;
                if (line.StartsWith("<tr><td>") && line.Contains("Fail"))
                    totalFail++;
                if (line.StartsWith("<tr><td>") && line.Contains("No Data"))
                    totalNoData++;
            }
            int totalPassPerc = 0;
            int totalFailPerc = 0;
            int totalNoDataPerc = 0;
            if (totalPass > 0)
                totalPassPerc = (totalPass * 100) / total;
            if (totalFail > 0)
                totalFailPerc = (totalFail * 100) / total;
            if (totalNoData > 0)
                totalNoDataPerc = (totalNoData * 100) / total;
            string buildnum = "";
            if (currentMachineName.Contains("HH8VM"))
            {
                string path = @"C:\Octopus\Applications\Qa-Manual\ForeFrontUITests";
                if (Directory.Exists(path))
                {
                    WriteTestLog($"SET-TOTAL-SUMMARY | QA-MACHINE | START GET LAST BUILD NUM | {path}");
                    DirectoryInfo di = new DirectoryInfo(path);
                    string currentDirectory = di.FullName;
                    WriteTestLog($"SET-TOTAL-SUMMARY | directory fullname: {currentDirectory}");
                    DirectoryInfo[] subfolders = di.GetDirectories();
                    string buildfolder = "";
                    if (subfolders.Length > 0)
                    {
                        WriteTestLog($"SET-TOTAL-SUMMARY | HAS SUB-FOLDERS");
                        foreach (DirectoryInfo folder in subfolders)
                            WriteTestLog($"SET-TOTAL-SUMMARY: FOUND: {folder}");
                        string expecteddate = DateTime.Now.ToString("yyyyMMdd");
                        string expecteddate1 = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                        foreach (DirectoryInfo folder in subfolders)
                        {
                            string actualdate = folder.LastAccessTime.ToString("yyyyMMdd");
                            if (expecteddate.Contains(actualdate) || expecteddate1.Contains(actualdate))
                            {
                                buildfolder = folder.Name;
                                break;
                            }
                        }
                        WriteTestLog($"SET-TOTAL-SUMMARY | TAKE BUILD FOLDER: {buildfolder}");
                        if (buildfolder.Contains("-"))
                        {

                            string[] dirarray = buildfolder.Split('-');
                            buildnum = dirarray[1];
                            if (buildnum.Contains("_"))
                            {
                                buildnum = buildnum.Split('_')[0];
                            }
                        }
                        WriteTestLog($"SET-TOTAL-SUMMARY | BUILD NUM: {buildnum}");
                    }
                }
            }
            #endregion
            string totalInfo = $"<h3>Tests:&nbsp;&nbsp;{total}&nbsp;&nbsp;| Pass:&nbsp;&nbsp;{totalPassPerc}%&nbsp;({totalPass})&nbsp;| Failed: {totalFailPerc}%&nbsp;({totalFail})&nbsp| No Data: {totalNoDataPerc}%&nbsp;({totalNoData})&nbsp|&nbsp; Date:&nbsp;{DateTime.Now.ToShortDateString()} &nbsp;&nbsp;|&nbsp;Total Time: {totalTime}&nbsp;&nbsp;|&nbsp; Env: {env}&nbsp;&nbsp;|&nbsp;Build: {buildnum}&nbsp;&nbsp;|&nbsp;Machine: {currentMachineName}&nbsp;&nbsp;|&nbsp;User: {username}</h3>";
            allLines[14] = totalInfo;
            totalInfoToMail = $"Tests:{total}|Pass:{totalPassPerc}% ({totalPass})|Fail:{totalFailPerc}% ({totalFail})|No Data:{totalNoDataPerc}% ({totalNoData})|Date:{DateTime.Now.ToShortDateString()}|Time: {totalTime}|Env: {env}|Build: {buildnum}";
            SaveReportToFile(reportFileTitle, allLines, totalInfoToMail);
        }

        public static void SetReportOrder()
        {
            List<string> templines = new List<string>();
            List<string> allLines = ReadReportFile();
            if (allLines.Count > 21)
            {
                try
                {
                    for (int i = 19; i < allLines.Count - 1; i++)
                    {
                        if (allLines[i].Contains("No Data") || allLines[i].Contains("Fail"))
                        {
                            templines.Add(allLines[i]);
                        }
                    }
                    for (int j = 19; j < allLines.Count - 1; j++)
                    {
                        if (allLines[j].Contains("Pass"))
                        {
                            templines.Add(allLines[j]);
                        }
                        if (allLines[j].StartsWith("Tests:") && allLines[j].Contains("|Fail:"))
                            allLines[j].Remove(j, 1);
                    }
                }
                catch
                {
                }
            }
            if (templines.Count > 1)
            {
                int row_count = allLines.Count - 19;
                allLines.RemoveRange(19, row_count);
                foreach (string line in templines)
                {
                    if (line.StartsWith("Tests:") && line.Contains("|Fail:"))
                        continue;
                    allLines.Add(line);
                }
                allLines.Add("</tbody></table></body></html>");
                SaveReportToFile(reportFileTitle, allLines, "");
            }
        }
        public static void SendReportByEmail()
        {
            //string mailaddress = "DL-Cappitech-QA-Frontend@spglobal.com";

            //SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
            //MailMessage mmsg = new MailMessage("dev@cappitech.com", mailaddress, "Automation UI Test Report", "New Test Report is Attached.");
            //mmsg.Attachments.Add(new Attachment(reportFileTitle));
            //smtp.EnableSsl = true;
            //var credentials = SecretsManager.Get().GetMailCredentials("dev");
            //smtp.Credentials = new System.Net.NetworkCredential(credentials.User, credentials.Password);
            //smtp.Send(mmsg);
            //WriteTestLog($"***** mail send: {mailaddress}");
            //Thread.Sleep(1000);
        }

        private static void SaveReportToFile(string filepath, List<string> details, string totaltomail = null)
        {
            lock (padlock)
            {
                File.WriteAllLines(filepath, details);
                if (totaltomail != null && totaltomail.Length > 10)
                {
                    string[] tempreport = File.ReadAllLines(filepath);
                    if (!tempreport[tempreport.Length - 1].Contains("|No Data:"))
                    {
                        WriteTestLog("report is not contain => total-to-mail => adding it now.");
                        // if totaltomail is not exist - add it
                        details.Add(totaltomail);
                        try
                        {
                            File.WriteAllLines(filepath, details);
                        }
                        catch (Exception ex)
                        {
                            WriteTestLog("##10TOTALTOMAIL NOT EXIST - IS FAILED TO WRITE. =>" + ex.ToString());
                        }
                    }
                    else
                    {
                        WriteTestLog("report is contains => total-to-mail => updating value");
                        // if totaltomail is exist - update it
                        details[details.Count - 1] = totaltomail;
                        try
                        {
                            File.WriteAllLines(filepath, details);
                        }
                        catch (Exception ex)
                        {
                            WriteTestLog("##20 TOTALTOMAIL EXIST - IS FAILED TO UPDATE. =>" + ex.ToString());
                        }
                    }
                }
            }
        }

        public static string getMachineName()
        {
            string machineName = Environment.MachineName;
            return machineName;
        }

        public static string getUserName()
        {
            string userName = Environment.UserName;
            return userName;
        }

        public static int ManageTestCounter()
        {
            lock (padlock)
            {
                testCounter++;
            }
            return testCounter;
        }

        public static void WriteTestLog(string textline, string guid = null)
        {
            lock (padlock)
            {
                if (!File.Exists(reportLog))
                {
                    try
                    {
                        File.AppendAllText(reportLog, DateTime.Now.ToString() + "log file created." + Environment.NewLine);
                    }
                    catch { }
                }
                if (guid == null)
                {
                    File.AppendAllText(reportLog, DateTime.Now.ToString() + $" | {textline}" + Environment.NewLine);
                }
                else
                    try
                    {
                        File.AppendAllText(reportLog, DateTime.Now.ToString() + $" |[{guid}] {textline}" + Environment.NewLine);
                    }
                    catch { }
            }
        }
        public static void WriteTestLog1(string textline)
        {
            string reportLog1 = reportSubFolder + @"\reportLog1.txt";
            lock (padlock)
            {
                File.AppendAllText(reportLog1, DateTime.Now.ToString() + $" | {textline}" + Environment.NewLine);
            }
        }

        public static string SaveReadResult(string result, string testguid, string operation)
        {
            WriteTestLog($"RESULT | test is ==> {result}.", testguid);
            WriteTestLog($"RESULT | TRY SAVE RESULT", testguid);
            string filename = "";
            string writeresult = "";
            lock (padlock2)
            {
                if (Directory.Exists(reportSubFolder))
                {
                    if (operation == "save")
                    {
                        try
                        {
                            filename = reportSubFolder + @"\result_save.txt";
                            File.AppendAllText(filename, result + "," + testguid + Environment.NewLine);  // if pass then write the testguid
                        }
                        catch { }
                    }
                    if (operation == "read")
                    {
                        try
                        {
                            WriteTestLog($"RESULT | TRY READ RESULT.", testguid);
                            filename = reportSubFolder + @"\result_save.txt";
                            string[] lines = File.ReadAllLines(filename);
                            foreach (string line in lines)
                            {
                                if (line.Contains(testguid) && line.Contains("Pass"))
                                {
                                    writeresult = "Pass";
                                    break;
                                }
                                if (line.Contains(testguid) && line.Contains("Fail"))
                                {
                                    writeresult = "Fail";
                                    break;
                                }
                            }
                            WriteTestLog($"RESULT | READ RESULT ==> {writeresult}", testguid);
                            return writeresult;
                        }
                        catch { }
                    }
                }
            }
            WriteTestLog($"RESULT | CREATE FILE: {filename} | PASS.", testguid);
            return filename;
        }

        private static List<string> ReadReportFile()
        {
            List<string> allLines = null;
            lock (padlock)
            {
                allLines = File.ReadAllLines(reportFileTitle).ToList();
            }
            return allLines;
        }
        public static void ManageThreadCounter()
        {
            lock (padlock)
            {
                threadcounter++;
            }
        }
    }
}
