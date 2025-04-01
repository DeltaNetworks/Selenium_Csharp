using AutoFram.UIReport;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoFram.DataTest
{
    public class GlobalData
    {
        public string url_zap = "https://www.zap.co.il/";
        public string url_ronen_ytube = "https://www.youtube.com/";
        public string url_google = "https://www.google.com/";
        public List<string> youtube_list = new List<string>() { "https://www.youtube.com/watch?v=pntnac9O3H4", "https://www.youtube.com/watch?v=0rzO2baQ0l8" };
        public string url_finviz = "https://finviz.com/";
        public string url_TradingView = "https://www.tradingview.com/chart";

        public int[] sleeptimes = new int[] { 1800, 2000, 2200, 2400, 2500, 2600, 2800, 3000, 3300, 3500 };
        public enum FileInclude { Eligibility, EMIR, MiFID, Timeliness, Transactions_20, Accuracy }
        public enum Language { English, Japanese }
        public enum UserName { psava, psavaAuto, avaAutoStaff1, avaAutoOperator1, thinkmarkets, fortrade, tmAutoStaff1, tmAutoStaff2, tmAutoOperator1, tmAutoOperator2, tmviewAuto1, noaustest_view, swissquotes_CY, cchang, ronen, test332_automation, test332_automation_SSO }
        public List<string> DateFilterTransactions = new List<string> { "Last 15 days", "Last Month", "This Month", "This Week", "This Year", "Last 30 days" };
        public List<string> DateFilterInsights = new List<string> { "This Quarter", "Last 6 months", "Last Quarter", "Last 12 months", "This Year" };
        public List<string> TestDetails = new List<string>();
        public List<string> TestFailDetails = new List<string>();
        public List<string> TestExceptionDetails = new List<string>();
        private List<string> ValidAssetClass = new List<string> { "Commodity", "Credit", "Equity", "FX", "Interest Rate", "Margin Lending", "Other" };
        private List<string> ValidInstrumentType = new List<string> { "CD", "Forward", "Future", "Option", "Other" };
        private List<string> ValidCommodityBase = new List<string> { "Agricultural", "Energy", "Environmental", "Exotic" };
        private List<string> ValidCommodityDetails = new List<string> { "CO", "EL", "IE", "NG", "OI", "OT" };
        private List<string> ValidRegulation = new List<string> { "ASIC", "Canada", "CFTC", "EMIR", "MAS", "SEC" };
        private List<string> ValidBestExType = new List<string> { "BBO", "Slippage", "Latency", "High Low" };
        public List<string> RowBlueColor = new List<string>() { "106", "160", "249" };         // blue color
                                                                                               //public List<string> RowOrangeColor = new List<string>() { "242", "164", "101" };       // orange color
        public List<string> RowBlackWhiteColor07 = new List<string>() { "20", "20", "20", "1" }; // black and white color
        public List<string> RowYellowColor = new List<string>() { "236", "239", "90" };        // yellow color
        public List<string> RowBlackWhiteColor09 = new List<string>() { "20", "20", "20" }; // black and white color
        public List<string> RowHighLightColor = new List<string> { "0", "0", "0", "0" }; // highlight color
        public string ARMcolor = "178,128,182";
        public readonly List<string> BestExSpreadColorsNegativeTest = new List<string>() { "(106, 160, 249)", "(239, 102, 90)" };
        public readonly List<string> BestExSpreadColorsPositiveTest = new List<string>() { "(239, 102, 90)", "(106, 160, 249)" };
        public readonly List<string> ReconTRFilterStatusObjects = new List<string>() { "Discrepancy", "Missing From Source", "Missing From Endpoint", "Matched" };
        public readonly List<string> ReconTRFilterStatusGridObjects = new List<string>() { "Discrepancy", "MissingFromSource", "MissingFromEndpoint", "Matched" };
        public string msgyouhavenopermission = "You do not have permission";
        public string msgyoucannotperform = "You cannot perform this action";
        public string NoResults = "No results found matching";
        public string thereisalreadyrecord = "There is already a record or pending record matching given data";
        public List<string> TimelinessBucketRegular = new List<string>() { "Total Reported", "More Than a Month", "Up to a Month", "Up to a Week", "On Time", "Up to a Week", "Average Late" };
        public List<string> TimelinessBucketNOA = new List<string>() { "Total Reported", "Up to a Week", "On Time", "Up to a Week", "Same Day", "Average Late" };
        public List<string> PairingMatchingBucket = new List<string>() { "Total", "Unpaired", "Paired", "Never Pairable", "Matched" };
        public readonly string BarStatusAcceptedGrinColor = "Accepted";
        public readonly List<string> TransactionsFilterRegulations = new List<string>() { "ASIC", "EMIR", "MiFID" };
        public readonly List<string> ReportLevelGridObjects = new List<string>() { "ASIC", "Canada", "CFTC", "EMIR", "MAS", "SEC", "SFTR" };
        public readonly List<string> TransactionTabGridColumnsObjects = new List<string>() { "Date", "Regulation", "Message", "Report Level", "Asset Class", "Product", "UPI","Ticket", "UTI", "Reporting Party", "Non Reporting Party", "QTY", "Status", "Reject", "Cappitech Assist", "Details",
             "Filename", "Reporting Date", "Pairing Status" };
        public readonly List<string> InsightTabGridColumnsObjects = new List<string>() { "Date", "Regulation", "Message", "Report Level", "Asset Class", "Product", "UPI", "Ticket", "UTI", "Reporting Party", "Non Reporting Party", "QTY" };
        public readonly List<string> InsightEligibilityASICGridColumnsObjects = new List<string>() { "Date", "Message", "Report Level", "Asset Class", "Product", "UPI", "Ticket", "UTI", "Reporting Party", "Non Reporting Party", "Quantity", "Status", "Reason", "Details", "Filename", "Reporting Date" };
        public readonly List<string> InsightAccuracyMiFIDGridColumnsObjects = new List<string>() { "Date", "Regulation", "Message", "Report Level", "Asset Class", "Product", "UPI", "Ticket", "UTI", "Reporting Party", "Non Reporting Party", "Quantity", "Status", "Reject", "Details", "Aging", "Filename", "Reporting Date" };
        public readonly List<string> InsightAccuracyBucketObjects = new List<string>() { "Reported", "Rejected", "Resolved", "Accepted", "Internally Identified", "Avg. Aging" };
        public readonly List<string> InsightTimelinessGridColumnsObjects = new List<string>() { "Date", "Regulation", "Message", "Report Level", "Asset Class", "Product", "UPI", "Ticket", "UTI", "Reporting Party", "Non Reporting Party", "Quantity", "Days Late", "Reporting Date" };
        public readonly List<string> MainFilterAssetClassObjects = new List<string>() { "Commodity", "Equity", "FX" };
        public readonly List<string> AssetClassGridObjects = new List<string>() { "CO", "EQ", "FX" };
        public readonly List<string> TransactionTabReportLevelObjects = new List<string>() { "Commodity", "Equity", "FX" };
        public readonly List<string> TransactionTabReportLevelGridObjects = new List<string>() { "Commodity", "Equity", "FX" };
        public readonly List<string> TransactionReportLevelFilter = new List<string>() { "Transactions", "Positions", "PPD", "Trade State" };
        public readonly List<string> TransactionReportLevelFilterGridObjects = new List<string>() { "Transaction", "Position", "PPD", "Trade State" };
        public readonly List<string> Transaction_MessageType_Filter_Obj = new List<string>() { "Collateral", "New", "Valuation" };
        public readonly List<string> Transaction_Status_Filter_Obj = new List<string>() { "Submitted", "Rejected", "Accepted" };
        public readonly List<string> DataM_Transaction_MsgType = new List<string>() { "PositionComponent", "New", "Modified" };
        private readonly List<string> DownloadFiles_To_Delete = new List<string> { "reconfiles-", "Dtcc", "_Commodities_", "_Equity_", "crdownload" };
        public readonly List<string> avatrade_users = new List<string>() { "avaAutoStaff1", "avaAutoOperator1", "avaview" };
        public readonly List<string> test332_users = new List<string>() { "test332_operator" };
        public readonly List<string> ihs_users = new List<string>() { "yael", "sagi", "ayelet" };
        public readonly string api_request = "requestWillBeSent";
        public readonly string api_response = "responseReceived";
        public readonly string api_replay_address = "/api/v2/data/claim/exceptions/replay";
        public readonly string api_upload_address = "/api/v2/clientdata/upload";
        public readonly string api_exit_address = "/api/v2/data/claim/transactions/exit";
        public readonly string api_error_address = "api/v2/data/claim/transactions/error";
        public readonly string api_comment_assign_address = "/api/v2/comments-and-assignments/";
        public readonly string api_uti_amend_address = "/api/v2/data/utiamend/";
        public readonly string PendingApproval = "Pending Approval";
        public readonly string DataM_Filter_AllOther = "All other";
        public readonly string DataM_Filter_Executing = "Executing";
        public readonly string DataM_Execute = "Execute";
        public readonly string Edit = "Edit";
        public readonly string DataM_Filter_PendingExecution = "Pending Execution";
        public int InitTimeOut = 500;
        public string driverDirectory = "C:\\SeleniumDrivers\\";
        public readonly string DataM_CommentsAndAssign = "Comment / Assign";
        public readonly string DataM_Error = "Error";
        public readonly string DataM_Exit = "Exit";
        public readonly string ContinueBtn = "Continue";
        public readonly string DataM_CommentIconGreenColor = "#A5DB6F";
        public readonly string DataM_ErrorActionMsg = "You are about to request an Error action";
        public readonly string DataM_ExitActionMsg = "You are about to request an Exit action";
        public readonly string DataM_ExecutingSuccessMsg = "Your execution was successful";
        public readonly string DataM_status_all_colorLocator = "//div[@role]//*[contains(@color,'status-all')]";

        //public string cacheDirectory = "";
        public string localStorageForCache = "C:/temp/AutoTests/";

        internal TestSettings TestSettings { get; }
        public IConfigurationRoot Config { get; }
        public string GenRandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] rnd = new byte[1];
            while (builder.Length < size)
            {
                rng.GetBytes(rnd);
                char c = (char)rnd[0];
                if (Char.IsLetter(c) && rnd[0] < 127)
                {
                    builder.Append(c.ToString());
                }
            }
            return builder.ToString();
        }
        public int GenRandomNumber(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        public List<string> CreateDataInstrumentList(int size, string operation = "insert", string currntuser = "")
        {
            List<string> valuelist = new List<string>();
            for (int i = 0; i < size; i++)
            {
                if (size < 44 && i == 14)
                {
                    valuelist.Add(GenRandomNumber(30, 2000).ToString());
                    continue;
                }
                if (size == 44 && i == 15)
                {
                    valuelist.Add(GenRandomNumber(30, 2000).ToString());
                    continue;
                }
                valuelist.Add(GenRandomString(6));
            }
            valuelist[1] = GenRandomString(12);
            if (size > 3 && size < 9 || size >= 43)  // => fix data for fields validations
            {
                valuelist[1] = "HHHH" + GenRandomString(8);   //isin
                valuelist[3] = ValidAssetClass[GenRandomNumber(0, ValidAssetClass.Count - 1)];  // asset class
                valuelist[4] = ValidInstrumentType[GenRandomNumber(0, ValidInstrumentType.Count - 1)]; // instrument type
                valuelist[5] = "COMMODITY:AGRICULTURAL:DAIRY:EXOTIC";      // product taxonomy
            }
            if (size == 8)
            {
                valuelist[5] = GenRandomString(10);
                valuelist[6] = "Energy";
                valuelist[7] = "EL";
            }
            if (size > 40)  // => fix data for fields validations
            {
                valuelist[7] = "Energy";//ValidCommodityBase[GenRandomNumber(0, ValidCommodityBase.Count - 1)];
                valuelist[8] = ValidCommodityDetails[GenRandomNumber(0, ValidCommodityDetails.Count - 1)];
                valuelist[13] = GenRandomNumber(1000, 9999).ToString();
                valuelist[12] = "Units";
                valuelist[13] = GenRandomNumber(100, 999).ToString();
                valuelist[14] = "Units";
                valuelist[16] = "Cash";
                valuelist[20] = "Mark to market";
                valuelist[21] = "Yes";
                valuelist[22] = "Yes";
                valuelist[23] = "Yes";
                valuelist[24] = "Yes";
                valuelist[25] = "Non-confirmed";
                valuelist[28] = GenRandomString(5);
                valuelist[38] = GenRandomString(22);
            }
            if (operation == "update")
                valuelist.RemoveAt(0);
            if (currntuser.Contains("Operator"))
                valuelist[0] += "OPRT";
            return valuelist;
        }
        public List<string> CreateDataListCounterParty(int size, string operation = "insert")
        {
            List<string> valuelist = new List<string>();
            for (int i = 0; i < size; i++)
            {
                valuelist.Add(GenRandomString(6));
            }
            valuelist[0] = ValidRegulation[GenRandomNumber(0, ValidRegulation.Count - 1)];
            valuelist[1] = "No";
            if (valuelist.Count > 4)
            {
                valuelist[2] = "CANN11111" + GenRandomNumber(10000, 1000000);
                valuelist[3] = "CLC";
                valuelist[6] = "Financial Counterparty";
            }

            if (valuelist.Count >= 17)
            {
                valuelist[16] = "2138005LTLTVZ4WMEX25";
            }
            return valuelist;
        }
        public List<string> CreateDataListReportingParty(int size)
        {
            List<string> valuelist = new List<string>();
            for (int i = 0; i < size; i++)
            {
                valuelist.Add(GenRandomString(6));
            }
            valuelist[0] = ValidRegulation[GenRandomNumber(0, ValidRegulation.Count - 1)];
            valuelist[1] = "11111" + GenRandomNumber(1000, 100000);
            valuelist[2] = "RANN" + GenRandomString(4) + GenRandomNumber(1000, 100000);
            if (valuelist.Count > 5)
            {
                valuelist[4] = GenRandomString(2);
                valuelist[5] = "Financial Counterparty";
                valuelist[7] = "ETF";
                valuelist[8] = "Agent";
                valuelist[9] = "No";
                valuelist[10] = "No";
                valuelist[12] = GenRandomNumber(10000, 300000).ToString();
                valuelist[14] = GenRandomNumber(10000, 300000).ToString();
            }

            if (valuelist.Count >= 17)
            {
                valuelist[16] = "2138005LTLTVZ4WMEX25";
            }
            return valuelist;
        }
        public List<string> CreateDataListBestExPolicy(int size)
        {
            List<string> valuelist = new List<string>();
            for (int i = 0; i < size; i++)
            {
                valuelist.Add(GenRandomString(6));
            }
            valuelist[0] = "BESTEXPc" + GenRandomString(6) + GenRandomNumber(1000, 100000);
            valuelist[1] = ValidBestExType[GenRandomNumber(0, 3)];
            if (size > 3)
            {
                valuelist[2] = ValidAssetClass[GenRandomNumber(0, ValidAssetClass.Count - 1)];
                valuelist[5] = GenRandomNumber(0, 500).ToString();
                valuelist[6] = GenRandomNumber(0, 500).ToString();
                valuelist[7] = GenRandomNumber(0, 5500).ToString();
                valuelist[9] = GenRandomNumber(0, 1500).ToString();
            }
            return valuelist;
        }
        public string GetCurrentMonthYear()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public string DeleteAllSpaces(string value)
        {
            return value.Replace(" ", "");
        }
        public string GetCurrentTestClassName(string value)
        {
            string[] clssname = new string[1];
            string clsname = value;
            if (value.Contains("."))
                clssname = value.Split('.');
            if (clssname.Length > 3)
                clsname = clssname[3];
            if (clsname.Contains('+'))
                clsname = clsname.Split('+')[0];
            if (value.Contains("Transactions_") && !value.Contains("DataM"))
                clsname = "Transactions-Tab";
            if (value.Contains("Insight"))
                clsname = "Insights";
            if (value.Contains("Pairing"))
                clsname = "Recon";
            if (value.Contains("Export"))
                clsname = "Export";
            if (value.Contains("BestExTab"))
                clsname = "BestExTab";
            if (value.Contains("Instrument"))
                clsname = "Instruments";
            if (value.Contains("CounterParty"))
                clsname = "Counter-Party";
            if (value.Contains("ReportingParty"))
                clsname = "Reporting-Party";
            if (value.Contains("BestExPolicy"))
                clsname = "BestExPolicy";
            if (value.Contains("DataM_Transaction"))
                clsname = "DataM_Transaction";
            if (value.Contains("Audit") && value.Contains("Instrument"))
                clsname = "Audit-Instrument";
            if (value.Contains("Audit") && value.Contains("BestExPolicy"))
                clsname = "Audit-BestExPolicy";
            if (value.Contains("Audit") && value.Contains("CounterParty"))
                clsname = "Audit-CounterParty";
            if (value.Contains("Audit") && value.Contains("DataM_Transaction"))
                clsname = "Audit-Transaction";
            if (value.Contains("Audit") && value.Contains("ReportingParty"))
                clsname = "Audit-ReportingParty";
            if (value.Contains("Recon"))
                clsname = "Recon";
            if (value.Contains("SSO_"))
                clsname = "SSO";
            return clsname;
        }
        public string GetCurrentTestName(string value)
        {
            string testName = null;
            string[] tstname = value.Split('"');
            testName = tstname[0].Replace('(', ' ');
            if (testName.Contains("DataM_"))
                testName = testName.Remove(0, 6);
            return testName;
        }
        public string GetShortDate(int adddays = 0)
        {
            if (adddays == 0)
                return DateTime.Now.ToString("yyyy-MM-dd");
            else
                return DateTime.Now.AddDays(adddays).ToString("yyyy-MM-dd");
        }
        public string SplitAndGetBarsDate(string value)
        {
            string expected = value;
            if (value.Length >= 10)
            {
                value = value.Split('\r')[0];  // 30 sep 2022
                                               //string expected = value.Split(' ')[1];
                                               //string expected = " " + value.Split(' ')[2];
                expected = value.Remove(value.Length - 5);
            }
            return expected;
        }
        public string GetLoginUserName(string value)
        {
            if (value == "cchang")
                return "cchang@rjobrien.com";
            if (value == "test332_automation_SSO")
                return "test332_automation_SSO@spglobal.com";
            return value;
        }
        public string GetTestTime(string duration)
        {
            if (duration.Length > 8)
                duration = duration.Remove(8);
            duration = duration.Remove(0, 3);
            return duration;
        }
        public string ShortString(string value, int amount)
        {
            Report.WriteTestLog("short string lenght.");
            if (value.Length > 35)
                return value = value.Remove(amount);
            else
                return value;
        }
        public void Delete_BeforeTestStart()
        {
            string UserPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string[] files_list = Directory.GetFiles(UserPath);
            foreach (string file in files_list)
            {
                if (file.ToLower().Contains("statistics_20") || file.ToLower().Contains("reconfiles-") || file.Contains("2023-")
                    || file.ToLower().Contains("crdownload") || file.ToLower().Contains(".tmp") || file.Contains("reportingparties_")
                    || file.Contains("counterparties_") || file.ToLower().Contains("bestexpolicies_") || file.ToLower().Contains("transaction")
                    || file.ToLower().Contains("instruments_") || file.Contains("exceptions_") || file.ToLower().Contains("pairingandmatching_20")
                    || file.ToLower().Contains("eligibility_20") || file.ToLower().Contains("statistics_20") || file.ToLower().Contains("reports_20")
                    || file.ToLower().Contains("accuracy_20") || file.ToLower().Contains("timeliness_20"))
                {
                    FileInfo fileinf = new FileInfo(file);
                    fileinf.Delete();
                    UIReport.Report.WriteTestLog($"FILE: DOWNLOAD FILE DELETED BEFORE TEST: {file}");
                }
            }

            string[] dir_array = Directory.GetDirectories(UserPath);
            foreach (string dir in dir_array)
            {
                if (dir.Contains("Dtcc") || dir.Contains("reconfiles-") || dir.Contains("exceptions_") || dir.Contains("reportingparties_")
                    || dir.Contains("PairingAndMatching_20") || dir.Contains("FE6") || dir.Contains("Eligibility_") || dir.Contains("Accuracy_")
                    || dir.Contains("Timeliness_20"))
                {
                    try
                    {
                        Directory.Delete(dir, true);
                        UIReport.Report.WriteTestLog($"FOLDER: DOWNLOAD FOLDER DELETED BEFORE TEST: {dir}");
                    }
                    catch (Exception ex)
                    {
                        UIReport.Report.WriteTestLog($"FOLDER: fail delete before test. | {ex.Message}");
                    }
                }
            }
        }
        public List<string> GetFileName(string[] rowdata)
        {
            List<string> data = new List<string>();
            data.Add(rowdata[0]);
            for (int i = 0; i < rowdata.Length; i++)
            {
                if (rowdata[i].ToLower().Contains("csv"))
                {
                    data.Add(rowdata[i]);
                    break;
                }
            }
            return data;
        }
        public string GetDayOfWeek()
        {
            return DateTime.Now.DayOfWeek.ToString().ToLower();
        }
    }
}
