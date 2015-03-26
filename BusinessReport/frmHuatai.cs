using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using DbCommon;
using DataBaseEntities;
using Microsoft.Office.Interop.Excel;
using System.IO;


namespace BusinessReport
{
    public partial class frmHuatai : Form
    {
        public frmHuatai()
        {
            InitializeComponent();
            InitGrid();
            this.pgbShow.Visible = false;
        }

        List<HuataiDetailItem> huataiDetailItem;

        public QueryWay queryWay = QueryWay.按回传日期;

        private void btnSee_Click(object sender, EventArgs e)
        {
            string StartDay = this.dtStartDate.Value.ToString("yyyyMMdd");
            string EndDay = this.dtEndDate.Value.ToString("yyyyMMdd");
            DateTime date = this.dtStartDate.Value.AddDays(40);
            long startD = long.Parse(StartDay);
            long endD = long.Parse(EndDay);
            if (this.dtEndDate.Value > date)
            {
                MessageBox.Show("选择的日期在40天之内");
                return;
            }
            using (IFCenterEntities ctx = DbConnUtils.GetIFCenterEntities())
            {
                huataiDetailItem = GetNewDetail();
                List<HuataiReport> huataiReportList = getHuataiReport(huataiDetailItem);
                this.dgvShow.DataSource = huataiReportList;

            }
        }

        int getDayFromPackName(string packName)
        {
            return Int32.Parse(packName.Substring(8, 8));
        }

        private List<HuataiDetailItem> GetNewDetail()
        {
            string StartDay = this.dtStartDate.Value.ToString("yyyyMMdd");
            string EndDay = this.dtEndDate.Value.ToString("yyyyMMdd");
            DateTime date = this.dtStartDate.Value.AddDays(40);
            long startD = long.Parse(StartDay);
            long endD = long.Parse(EndDay);


            List<HuataiDetailItem> detailList = new List<HuataiDetailItem>();

            using (IFCenterEntities ctx = DbCommon.DbConnUtils.GetIFCenterEntities())
            {
                List<DownLoad> downList = null;

                if (queryWay == QueryWay.按接收日期)
                {
                    downList = (from dd in ctx.DownLoads
                                where dd.CID == 2005 && dd.CTID == 24 &&
                                dd.DownLoadDay >= startD && dd.DownLoadDay <= endD
                                && dd.Status == 40 && dd.ErrorStatus == -1
                                select dd).ToList();
                }
                else if (queryWay == QueryWay.按回传日期)
                {
                    downList = (from dd in ctx.DownLoads
                                where dd.CID == 2005 && dd.CTID == 24 &&
                                dd.PassbackDay >= startD && dd.PassbackDay <= endD
                                && dd.Status == 40 && dd.ErrorStatus == -1
                                select dd).ToList();
                }
                else if (queryWay == QueryWay.按包名日期)
                {
                    string temp = ctx.Connection.ConnectionString;

                    int startIndex = temp.IndexOf("\"")+1;
                    int endIndex = temp.LastIndexOf("\"");
                    int len = endIndex - startIndex;
                    string connString = temp.Substring(startIndex,len);

                    var db = new Common.Database(connString, true);

                    string sql = "select * from downLoad where cid = 2005 and ctid = 24 ";
                    sql += " and cast(substring(PackageName,9,8) as int) >= " + startD;
                    sql += " and cast(substring(PackageName,9,8) as int) <=" + endD;
                    sql += " and  status =40 and errorStatus=-1 ";

                    downList = db.Query<DownLoad>(sql).ToList();

  
                }





                for (int i = 0; i < downList.Count; i++)
                {
                    this.lblEndDate.Text = i.ToString();
                    System.Windows.Forms.Application.DoEvents();
                    DownLoad downLoad = downList[i];
                    List<int> caseIdList = (from ff in ctx.FinalProducts
                                            where ff.DWID == downLoad.DWID && ff.CaseId != -1
                                            select ff.CaseId).ToList();
                    string mainConn = "Data Source=10.144.141.147;Initial Catalog=MainBack;User ID=sa;Password=Yz74%?8acfqd^6;Persist Security Info=True;";
                    var maindb = new Common.Database(mainConn, true);


                    for (int j = 0; j < caseIdList.Count; j++)
                    {
                        int caseId = caseIdList[j];
                        BK_CaseList CaseList;
                        List<PageList> pageList;
                        CaseList = maindb.FirstOrDefault<BK_CaseList>(@"select * from CaseList   WITH(NOLOCK)
                                                                       WHERE CaseId=" + caseId);
                        if (CaseList != null)
                        {
                            string subConn = "Data Source=10.144.141.147;Initial Catalog=" + CaseList.BackupDbName + ";User ID=sa;Password=Yz74%?8acfqd^6;Persist Security Info=True;";
                            var db07 = new Common.Database(subConn, true);
                            pageList = db07.Query<PageList>(@"select *   from PageList   WITH(NOLOCK)
                                                              WHERE CaseId=" + caseId).ToList();

                            Dictionary<string, string> dic = new Dictionary<string, string>();
                            foreach (var item in pageList)
                            {
                                dic[item.CaseType] = item.CaseType;
                            }
                            foreach (var caseType in dic.Keys)
                            {
                                HuataiDetailItem item = new HuataiDetailItem();
                                item.DWID = downLoad.DWID.ToString();
                                item.DownLoadDay = downLoad.DownLoadDay.ToString();
                                item.CID = downLoad.CID.ToString();
                                item.CTID = downLoad.CTID.ToString();
                                item.PassbackDay = downLoad.PassbackDay.ToString();
                                item.CreateTime = downLoad.CreateTime.ToString();
                                item.DownLoadTime = downLoad.DownLoadTime.ToString();
                                item.PassbackTime = downLoad.PassbackTime.ToString();
                                item.CaseName = CaseList.CaseName;
                                item.CaseType = caseType;
                                item.CaseTypeCnName = GetCaseTypeCnName(item.CaseType);
                                item.PackageName = downLoad.PackageName;
                                item.CaseCount = 1;
                                string useTime = GetUseTime(downLoad.CreateTime, downLoad.PassbackTime);
                                item.UseTime = useTime;
                                detailList.Add(item);
                            }
                        }
                    }
                }
            }
            return detailList;

        }

        private List<HuataiReport> getHuataiReport(List<HuataiDetailItem> huataiDetailItem)
        {
            List<HuataiReport> reportList = new List<HuataiReport>();
            for (int i = 0; i <= (dtEndDate.Value - dtStartDate.Value).TotalDays; i++)
            {
                HuataiReport item = new HuataiReport();
                item.DownLoadDay = dtStartDate.Value.AddDays(i).ToString("yyyyMMdd").Insert(4, "/").Insert(7, "/");
                int curDay = int.Parse(dtStartDate.Value.AddDays(i).ToString("yyyyMMdd"));
                string strCurDay = curDay.ToString();
                item.mainType705Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA000705") == true
                                         select dd.CaseCount).Sum().ToString();
                item.mainType904Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA000904") == true
                                         select dd.CaseCount).Sum().ToString();
                item.mainType1101Count = (from dd in huataiDetailItem
                                          where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001101") == true
                                          select dd.CaseCount).Sum().ToString();
                item.mainType1401Count = (from dd in huataiDetailItem
                                          where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001401") == true
                                          select dd.CaseCount).Sum().ToString();
                item.aipType1001Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001001") == true
                                         select dd.CaseCount).Sum().ToString();
                item.aipType1002Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001002") == true
                                         select dd.CaseCount).Sum().ToString();
                item.aipType1003Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001003") == true
                                         select dd.CaseCount).Sum().ToString();
                item.aipType1004Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001004") == true
                                         select dd.CaseCount).Sum().ToString();
                item.aipType1005Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001005") == true
                                         select dd.CaseCount).Sum().ToString();
                item.chargeType603Count = (from dd in huataiDetailItem
                                           where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001009") == true
                                           select dd.CaseCount).Sum().ToString();
                item.applyType709Count = (from dd in huataiDetailItem
                                          where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA000709") == true
                                          select dd.CaseCount).Sum().ToString();
                item.applyType902Count = (from dd in huataiDetailItem
                                          where dd.PassbackDay == strCurDay && (dd.CaseType.Contains("AIA000902") == true ||
                                          dd.CaseType.Contains("AIA001311") == true)
                                          select dd.CaseCount).Sum().ToString();
                //item.applyType822Count = (from dd in huataiDetailItem
                //                          where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001311") == true
                //                          select dd.CaseCount).Sum().ToString();
                item.riskType901Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA010901") == true
                                         select dd.CaseCount).Sum().ToString();
                item.tbqrsType905Count = (from dd in huataiDetailItem
                                          where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA000905") == true
                                          select dd.CaseCount).Sum().ToString();
                item.familyType1006Count = (from dd in huataiDetailItem
                                            where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001006") == true
                                            select dd.CaseCount).Sum().ToString();
                item.familyType1102Count = (from dd in huataiDetailItem
                                            where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001102") == true
                                            select dd.CaseCount).Sum().ToString();
                item.bkaType1003Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("BKA001003") == true
                                         select dd.CaseCount).Sum().ToString();
                item.bkaType1004Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("BKA001004") == true
                                         select dd.CaseCount).Sum().ToString();
                item.bkaType1101Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("BKA001101") == true
                                         select dd.CaseCount).Sum().ToString();
                item.bkaType1102Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("BKA001102") == true
                                         select dd.CaseCount).Sum().ToString();
                item.bkaType1105Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("BKA001105") == true
                                         select dd.CaseCount).Sum().ToString();
                item.bkaType51101Count = (from dd in huataiDetailItem
                                          where dd.PassbackDay == strCurDay && dd.CaseType.Contains("BKA051101") == true
                                          select dd.CaseCount).Sum().ToString();
                item.bkaType51401Count = (from dd in huataiDetailItem
                                          where dd.PassbackDay == strCurDay && dd.CaseType.Contains("BKA051401") == true
                                          select dd.CaseCount).Sum().ToString();
                item.bkaType1005Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("BKA001005") == true
                                         select dd.CaseCount).Sum().ToString();
                item.bkaType1401Count = (from dd in huataiDetailItem
                                         where dd.PassbackDay == strCurDay && dd.CaseType.Contains("BKA001401") == true
                                         select dd.CaseCount).Sum().ToString();
                item.familyType1201Count = (from dd in huataiDetailItem
                                            where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001201") == true
                                            select dd.CaseCount).Sum().ToString();
                item.familyType1301Count = (from dd in huataiDetailItem
                                            where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001301") == true
                                            select dd.CaseCount).Sum().ToString();
                item.familyType1307Count = (from dd in huataiDetailItem
                                            where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001307") == true
                                            select dd.CaseCount).Sum().ToString();
                item.applyType204Count = (from dd in huataiDetailItem
                                          where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA001204") == true
                                          select dd.CaseCount).Sum().ToString();
                item.familyType0706Count = (from dd in huataiDetailItem
                                            where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA000706") == true
                                            select dd.CaseCount).Sum().ToString();
                item.familyType0604Count = (from dd in huataiDetailItem
                                            where dd.PassbackDay == strCurDay && dd.CaseType.Contains("AIA000604") == true
                                            select dd.CaseCount).Sum().ToString();
                item.Total = (from dd in huataiDetailItem
                              where dd.PassbackDay == strCurDay
                              select dd.CaseCount).Sum().ToString();
                reportList.Add(item);
            }
            HuataiReport total = new HuataiReport();
            total.DownLoadDay = "总计";
            total.mainType705Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("AIA000705") == true
                                      select dd.CaseCount).Sum().ToString();
            total.mainType904Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("AIA000904") == true
                                      select dd.CaseCount).Sum().ToString();
            total.mainType1101Count = (from dd in huataiDetailItem
                                       where dd.CaseType.Contains("AIA001101") == true
                                       select dd.CaseCount).Sum().ToString();
            total.mainType1401Count = (from dd in huataiDetailItem
                                       where dd.CaseType.Contains("AIA001401") == true
                                       select dd.CaseCount).Sum().ToString();
            total.aipType1001Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("AIA001001") == true
                                      select dd.CaseCount).Sum().ToString();
            total.aipType1002Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("AIA001002") == true
                                      select dd.CaseCount).Sum().ToString();
            total.aipType1003Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("AIA001003") == true
                                      select dd.CaseCount).Sum().ToString();
            total.aipType1004Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("AIA001004") == true
                                      select dd.CaseCount).Sum().ToString();
            total.aipType1005Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("AIA001005") == true
                                      select dd.CaseCount).Sum().ToString();
            total.chargeType603Count = (from dd in huataiDetailItem
                                        where dd.CaseType.Contains("AIA001009") == true
                                        select dd.CaseCount).Sum().ToString();
            total.applyType709Count = (from dd in huataiDetailItem
                                       where dd.CaseType.Contains("AIA000709") == true
                                       select dd.CaseCount).Sum().ToString();
            total.applyType902Count = (from dd in huataiDetailItem
                                       where dd.CaseType.Contains("AIA000902") == true ||
                                       dd.CaseType.Contains("AIA001311") == true
                                       select dd.CaseCount).Sum().ToString();
            //total.applyType822Count = (from dd in huataiDetailItem
            //                           where dd.CaseType.Contains("AIA001311") == true
            //                           select dd.CaseCount).Sum().ToString();
            total.riskType901Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("AIA010901") == true
                                      select dd.CaseCount).Sum().ToString();
            total.tbqrsType905Count = (from dd in huataiDetailItem
                                       where dd.CaseType.Contains("AIA000905") == true
                                       select dd.CaseCount).Sum().ToString();
            total.familyType1006Count = (from dd in huataiDetailItem
                                         where dd.CaseType.Contains("AIA001006") == true
                                         select dd.CaseCount).Sum().ToString();
            total.familyType1102Count = (from dd in huataiDetailItem
                                         where dd.CaseType.Contains("AIA001102") == true
                                         select dd.CaseCount).Sum().ToString();
            total.bkaType1003Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("BKA001003") == true
                                      select dd.CaseCount).Sum().ToString();
            total.bkaType1004Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("BKA001004") == true
                                      select dd.CaseCount).Sum().ToString();
            total.bkaType1101Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("BKA001101") == true
                                      select dd.CaseCount).Sum().ToString();
            total.bkaType1102Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("BKA001102") == true
                                      select dd.CaseCount).Sum().ToString();
            total.bkaType1105Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("BKA001105") == true
                                      select dd.CaseCount).Sum().ToString();
            total.bkaType51101Count = (from dd in huataiDetailItem
                                       where dd.CaseType.Contains("BKA051101") == true
                                       select dd.CaseCount).Sum().ToString();
            total.bkaType51401Count = (from dd in huataiDetailItem
                                       where dd.CaseType.Contains("BKA051401") == true
                                       select dd.CaseCount).Sum().ToString();
            total.bkaType1005Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("BKA001005") == true
                                      select dd.CaseCount).Sum().ToString();
            total.bkaType1401Count = (from dd in huataiDetailItem
                                      where dd.CaseType.Contains("BKA001401") == true
                                      select dd.CaseCount).Sum().ToString();
            total.familyType1201Count = (from dd in huataiDetailItem
                                         where dd.CaseType.Contains("AIA001201") == true
                                         select dd.CaseCount).Sum().ToString();
            total.familyType1301Count = (from dd in huataiDetailItem
                                         where dd.CaseType.Contains("AIA001301") == true
                                         select dd.CaseCount).Sum().ToString();
            total.familyType1307Count = (from dd in huataiDetailItem
                                         where dd.CaseType.Contains("AIA001307") == true
                                         select dd.CaseCount).Sum().ToString();
            total.applyType204Count = (from dd in huataiDetailItem
                                       where dd.CaseType.Contains("AIA001204") == true
                                       select dd.CaseCount).Sum().ToString();
            total.familyType0706Count = (from dd in huataiDetailItem
                                         where dd.CaseType.Contains("AIA000706") == true
                                         select dd.CaseCount).Sum().ToString();
            total.familyType0604Count = (from dd in huataiDetailItem
                                         where dd.CaseType.Contains("AIA000604") == true
                                         select dd.CaseCount).Sum().ToString();
            total.Total = (from dd in huataiDetailItem
                           select dd.CaseCount).Sum().ToString();
            reportList.Add(total);

            return reportList;
        }

        private List<HuataiDetailItem> GetHuataiDetailItem(List<DownLoad> downList, List<HuataiFinalItem> huataiFinalItem)
        {
            List<HuataiDetailItem> itemList = new List<HuataiDetailItem>();
            for (int i = 0; i < downList.Count; i++)
            {
                DownLoad downLoad = downList[i];
                string str = downLoad.DWID.ToString();
                List<HuataiFinalItem> huataiFinalTemp = (from dd in huataiFinalItem
                                                         where dd.DWID == str
                                                         select dd).ToList();
                for (int j = 0; j < huataiFinalTemp.Count; j++)
                {
                    HuataiDetailItem item = new HuataiDetailItem();
                    item.DWID = downLoad.DWID.ToString();
                    item.DownLoadDay = downLoad.DownLoadDay.ToString();
                    item.CID = downLoad.CID.ToString();
                    item.CTID = downLoad.CTID.ToString();
                    item.PassbackDay = downLoad.PassbackDay.ToString();
                    item.CreateTime = downLoad.CreateTime.ToString();
                    item.DownLoadTime = downLoad.DownLoadTime.ToString();
                    item.PassbackTime = downLoad.PassbackTime.ToString();
                    item.CaseName = huataiFinalTemp[j].FileName;
                    item.CaseType = huataiFinalTemp[j].CaseType;
                    item.CaseTypeCnName = GetCaseTypeCnName(item.CaseType);
                    item.PackageName = downLoad.PackageName;
                    item.CaseCount = 1;
                    string useTime = GetUseTime(downLoad.CreateTime, downLoad.PassbackTime);
                    item.UseTime = useTime;
                    itemList.Add(item);
                }
            }
            return itemList;
        }

        private string GetCaseTypeCnName(string caseType)
        {
            string type = "";
            if (caseType.Contains("AIA000705") == true ||
                caseType.Contains("AIA000904") == true ||
                caseType.Contains("AIA001101") == true ||
                caseType.Contains("AIA001401") == true)
            {
                type = "个险单证";
            }
            else if (caseType.Contains("AIA001001") == true ||
                        caseType.Contains("AIA001002") == true ||
                        caseType.Contains("AIA001003") == true ||
                        caseType.Contains("AIA001004") == true ||
                        caseType.Contains("AIA001005") == true)
            {
                type = "意外险投保单";
            }
            else if (caseType.Contains("AIA000603") == true ||
                        caseType.Contains("AIA001009") == true)
            {
                type = "转账协议书";
            }
            else if (caseType.Contains("AIA000709") == true ||
                     caseType.Contains("AIA000902") == true ||
                     caseType.Contains("AIA001204") == true ||
                caseType.Contains("AIA001311") == true)
            {
                type = "补充投保事项申请表";
            }
            else if (caseType.Contains("AIA010901") == true)
            {
                type = "投保提示";
            }
            else if (caseType.Contains("AIA000905") == true)
            {
                type = "投保单确认书";
            }
            else if (caseType.Contains("AIA001006") == true ||
                     caseType.Contains("AIA001102") == true)
            {
                type = "家庭投保单";
            }
            else if (caseType.Contains("BKA001003") == true ||
                       caseType.Contains("BKA001004") == true ||
                       caseType.Contains("BKA001101") == true ||
                       caseType.Contains("BKA001102") == true ||
                       caseType.Contains("BKA001105") == true ||
                       caseType.Contains("BKA051101") == true ||
                       caseType.Contains("BKA051401") == true ||
                       caseType.Contains("BKA001401") == true)
            {
                type = "银代单证";
            }
            else if (caseType.Contains("BKA001005") == true)
            {
                type = "人身保险投保单";
            }
            else if (caseType.Contains("AIA001201") == true)
            {
                type = "爱心家庭综合意外伤害保险";
            }
            else if (caseType.Contains("AIA001301") == true ||
                caseType.Contains("AIA001307") == true)
            {
                type = "爱心家庭综合意外伤害保险";
            }
            return type;
        }

        public class HuataiDetailItem
        {
            public string DWID { get; set; }
            public string CID { get; set; }
            public string CTID { get; set; }
            public string DownLoadDay { get; set; }
            public string PassbackDay { get; set; }
            public string CreateTime { get; set; }
            public string DownLoadTime { get; set; }
            public string PassbackTime { get; set; }
            public string CaseName { get; set; }
            public string CaseType { get; set; }
            public string CaseTypeCnName { get; set; }
            public string PackageName { get; set; }
            public int CaseCount { get; set; }
            public string UseTime { get; set; }
        }

        public class HuataiFinalItem
        {
            public string DWID { get; set; }
            public string FileName { get; set; }
            public string CaseType { get; set; }
        }

        public class HuataiReport
        {
            public string DownLoadDay { get; set; }
            //个险单证
            public string mainType705Count { get; set; }   //华泰人身投保单第五联(第一种)-AIA000705
            public string mainType904Count { get; set; } //华泰人身投保单(第三种)新-AIA000904
            public string mainType1101Count { get; set; }   //华泰人身投保单(第五种)-AIA001101
            public string mainType1401Count { get; set; }   //华泰人身投保单(第五种)-AIA001401

            //意外险投保单
            public string aipType1001Count { get; set; }    //华泰爱心综合意外A款投保单(新)-AIA001001
            public string aipType1002Count { get; set; } //华泰爱心综合意外A款投保单(新)-AIA001002
            public string aipType1003Count { get; set; }    //华泰爱心综合意外A款投保单(新)-AIA001003
            public string aipType1004Count { get; set; }  //华泰爱心综合意外A款投保单(新)-AIA001004
            public string aipType1005Count { get; set; }   //华泰投保单-AIA001005


            //转账协议书
            public string chargeType603Count { get; set; } //华泰保险费自动转账付款协议书-AIA000603

            //补充投保事项申请表
            public string applyType709Count { get; set; }  //华泰补充投保事项申请表-AIA000709
            public string applyType902Count { get; set; } //华泰补充投保事项-AIA000902
            public string applyType204Count { get; set; }  //华泰补充投保事项-AIA001204
            //public string applyType822Count { get; set; } //华泰补充投保事项-AIA001311
            //投保提示
            public string riskType901Count { get; set; }  //投保提示-AIA010901

            //投保单确认书
            public string tbqrsType905Count { get; set; }   //投保单确认书-AIA000905

            //家庭投保单
            public string familyType1006Count { get; set; } //爱心家庭综合意外伤害保险投保单-AIA001006
            public string familyType1102Count { get; set; } //爱心家庭综合意外伤害保险投保单-AIA001102

            //银代单证
            public string bkaType1003Count { get; set; } // 华泰银险投保单(第十种)-BKA001003
            public string bkaType1004Count { get; set; } //华泰银险投保单(第十一种)-BKA001004
            public string bkaType1101Count { get; set; } // 华泰银险投保单(第十二种)-BKA001101
            public string bkaType1102Count { get; set; } //华泰银险投保单(第十三种)-BKA001102
            public string bkaType1105Count { get; set; } //华泰银行保险投保单(第十四种)-BKA001105
            public string bkaType51101Count { get; set; } //华泰银险投保单(第十五种)-BKA051101
            public string bkaType51401Count { get; set; }
            public string bkaType1401Count { get; set; } //华泰银险投保单()-BKA001401

            //BKA001005
            public string bkaType1005Count { get; set; } //人身保险投保单-BKA001005
            //AIA001201
            public string familyType1201Count { get; set; }//爱心家庭综合意外伤害保险-AIA001201
            //AIA001301
            public string familyType1301Count { get; set; }//爱心家庭综合意外伤害保险-AIA001301
            public string familyType1307Count { get; set; }

            public string familyType0706Count { get; set; }
            public string familyType0604Count { get; set; }

            public string Total { get; set; }
        }

        private void InitGrid()
        {
            //this.dgvShow.ReadOnly = true;
            this.dgvShow.AutoGenerateColumns = false;
            this.dgvShow.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvShow.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvShow.AllowUserToAddRows = false;
            this.dgvShow.RowHeadersVisible = false;
            //this.dgvShow.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvShow.AllowUserToOrderColumns = false;

        }

        private string GetUseTime(long createTime, long passbackTime)
        {
            string preateTimeString = createTime.ToString();
            preateTimeString = preateTimeString.Insert(12, ":");
            preateTimeString = preateTimeString.Insert(10, ":");
            preateTimeString = preateTimeString.Insert(8, " ");
            preateTimeString = preateTimeString.Insert(6, "-");
            preateTimeString = preateTimeString.Insert(4, "-");

            string passbackTimeString = passbackTime.ToString();
            passbackTimeString = passbackTimeString.Insert(12, ":");
            passbackTimeString = passbackTimeString.Insert(10, ":");
            passbackTimeString = passbackTimeString.Insert(8, " ");
            passbackTimeString = passbackTimeString.Insert(6, "-");
            passbackTimeString = passbackTimeString.Insert(4, "-");

            DateTime startTime = DateTime.Parse(preateTimeString.ToString());
            DateTime passPackTime = DateTime.Parse(passbackTimeString.ToString());
            TimeSpan span = passPackTime - startTime;
            string str = string.Format("{0}:{1}:{2}", span.Hours, span.Minutes, span.Seconds);
            return str;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataGridviewShowToExcel(this.dgvShow, true, huataiDetailItem);

        }

        private bool DataGridviewShowToExcel(DataGridView dgv, bool isShowExcle, List<HuataiDetailItem> huataiDetailItem)
        {
            if (dgv.Rows.Count == 0)
                return false;
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
            string path = Path.Combine(System.Windows.Forms.Application.StartupPath, @"Template/华泰人寿统计表.xlsx");
            Microsoft.Office.Interop.Excel._Workbook workbook = workbooks.Add(path);
            Worksheet mySheet = workbook.Sheets[1] as Worksheet; //第一个sheet页
            excel.Visible = false;
            for (int i = 0; i < dgv.RowCount; i++)
            {
                for (int j = 0; j < dgv.ColumnCount; j++)
                {
                    mySheet.Cells[i + 4, j + 1] = "'" + dgv[j, i].Value.ToString();
                }
            }
            mySheet = workbook.Sheets[2] as Worksheet;
            this.pgbShow.Minimum = 0;
            this.pgbShow.Maximum = huataiDetailItem.Count;
            this.pgbShow.Visible = true;
            for (int i = 0; i < huataiDetailItem.Count; i++)
            {
                HuataiDetailItem item = huataiDetailItem[i];
                //业务日期
                mySheet.Cells[i + 2, 0 + 1] = item.DownLoadDay.Insert(4, "/").Insert(7, "/");
                //批次名称
                mySheet.Cells[i + 2, 0 + 2] = "'" + item.PackageName;
                //投保单号
                mySheet.Cells[i + 2, 0 + 3] = "'" + item.CaseName;
                // 份数	
                mySheet.Cells[i + 2, 0 + 4] = "'" + item.CaseCount;
                // 单证名称 	
                mySheet.Cells[i + 2, 0 + 5] = "'" + TransCaseType(item.CaseType);
                // 类型	
                mySheet.Cells[i + 2, 0 + 6] = "'" + item.CaseTypeCnName;
                // 开始接收时间	
                mySheet.Cells[i + 2, 0 + 7] = "'" + TransFormat(item.DownLoadTime);
                // 接收结束时间	
                mySheet.Cells[i + 2, 0 + 8] = "'" + TransFormat(item.DownLoadTime);
                // 开始回传时间	
                mySheet.Cells[i + 2, 0 + 9] = "'" + TransFormat(item.PassbackTime);
                // 回传结束时间	
                mySheet.Cells[i + 2, 0 + 10] = "'" + TransFormat(item.PassbackTime);
                // 处理时间
                mySheet.Cells[i + 2, 0 + 11] = item.UseTime;

                this.pgbShow.Value = i;
            }
            this.pgbShow.Visible = false;
            excel.Visible = isShowExcle;
            return true;
        }

        private string TransCaseType(string p)
        {
            string str = "";
            switch (p)
            {
                case "HuataiAIA000705": str = "AIA000705"; break;
                case "HuataiAIA000904": str = "AIA000904"; break;
                case "HuataiAIA001101": str = "AIA001101"; break;
                case "HuataiAIA001001": str = "AIA001001"; break;
                case "HuataiAIA001002": str = "AIA001002"; break;
                case "HuataiAIA001003": str = "AIA001003"; break;
                case "HuataiAIA001004": str = "AIA001004"; break;
                case "HuataiAIA001005": str = "AIA001005"; break;
                case "HuataiAIA001311": str = "822"; break;
                case "HuataiAIA001307": str = "AIA001307"; break;
                case "HuataiAIA001401": str = "AIA001401"; break;
                case "HuataiAIA001401New": str = "AIA001401"; break;
                case "HuataiAIA001009": str = "811"; break;
                case "HuataiAIA000709": str = "818"; break;
                case "HuataiAIA000902": str = "822"; break;
                case "HuataiAIA010901": str = "813"; break;
                case "HuataiAIA000905": str = "823"; break;
                case "HuataiAIA001006": str = "AIA001006"; break;
                case "HuataiAIA001102": str = "AIA001102"; break;
                case "HuataiBKA001003": str = "BKA001003"; break;
                case "HuataiBKA001004": str = "BKA001004"; break;
                case "HuataiBKA001101": str = "BKA001101"; break;
                case "HuataiBKA001102": str = "BKA001102"; break;
                case "HuataiBKA001105": str = "BKA001105"; break;
                case "HuataiBKA051101": str = "BKA051101"; break;
                case "HuataiBKA051401": str = "BKA051401"; break;
                case "HuataiBKA001005": str = "BKA001005"; break;
                case "HuataiBKA001401": str = "BKA001401"; break;
                case "HuataiBKA001401New": str = "BKA001401"; break;
                case "HuataiAIA001201": str = "AIA001201"; break;
                case "HuataiAIA001301": str = "AIA001301"; break;
                case "HuataiAIA001204": str = "838"; break;
                case "HuataiAIA000706": str = "819"; break;
                case "HuataiAIA000604": str = "810"; break;
            }
            return str;
        }

        private string TransFormat(string str)
        {
            if (str != null)
            {
                string strTemp = str;
                str = str.Insert(12, ":");
                str = str.Insert(10, ":");
                str = str.Insert(8, " ");
                str = str.Insert(6, "-");
                str = str.Insert(4, "-");
            }
            return str;
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            string StartDay = this.dtStartDate.Value.ToString("yyyyMMdd");
            string EndDay = this.dtEndDate.Value.ToString("yyyyMMdd");
            DateTime date = this.dtStartDate.Value.AddDays(40);
            long startD = long.Parse(StartDay);
            long endD = long.Parse(EndDay);
            if (this.dtEndDate.Value > date)
            {
                MessageBox.Show("选择的日期在40天之内");
                return;
            }
            string conn = "Data Source=10.144.141.147;Initial Catalog=BackupCenter201305;User ID=sa;Password=Yz74%?8acfqd^6;Persist Security Info=True;";
            string conn07 = "Data Source=10.144.141.147;Initial Catalog=BackupCenter201307;User ID=sa;Password=Yz74%?8acfqd^6;Persist Security Info=True;";
            var db = new Common.Database(conn, true);
            var db07 = new Common.Database(conn07, true);
            List<HuataiDetailItem> detailList = new List<HuataiDetailItem>();

            using (IFCenterEntities ctx = DbCommon.DbConnUtils.GetIFCenterEntities())
            {
                List<DownLoad> downList = (from dd in ctx.DownLoads
                                           where dd.CID == 2005 && dd.CTID == 24 &&
                                                 dd.PassbackDay >= startD && dd.PassbackDay <= endD
                                                 && dd.Status == 40 && dd.ErrorStatus == -1
                                           select dd).ToList();
                for (int i = 0; i < downList.Count; i++)
                {
                    this.lblEndDate.Text = i.ToString();
                    System.Windows.Forms.Application.DoEvents();
                    DownLoad downLoad = downList[i];
                    List<int> caseIdList = (from ff in ctx.FinalProducts
                                            where ff.DWID == downLoad.DWID && ff.CaseId != -1
                                            select ff.CaseId).ToList();
                    for (int j = 0; j < caseIdList.Count; j++)
                    {
                        int caseId = caseIdList[j];
                        CaseList CaseList;
                        List<PageList> pageList;
                        CaseList = db.FirstOrDefault<CaseList>(@"select * from CaseList   WITH(NOLOCK)
                                                            WHERE CaseId=" + caseId);
                        if (CaseList != null)
                        {
                            pageList = db.Query<PageList>(@"select *   from PageList   WITH(NOLOCK)
                                                            WHERE CaseId=" + caseId).ToList();
                        }
                        else
                        {
                            CaseList = db07.FirstOrDefault<CaseList>(@"select * from CaseList   WITH(NOLOCK)
                                                            WHERE CaseId=" + caseId);
                            pageList = db07.Query<PageList>(@"select *   from PageList   WITH(NOLOCK)
                                                            WHERE CaseId=" + caseId).ToList();
                        }
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        foreach (var item in pageList)
                        {
                            dic[item.CaseType] = item.CaseType;
                        }
                        foreach (var caseType in dic.Keys)
                        {
                            HuataiDetailItem item = new HuataiDetailItem();
                            item.DWID = downLoad.DWID.ToString();
                            item.DownLoadDay = downLoad.DownLoadDay.ToString();
                            item.CID = downLoad.CID.ToString();
                            item.CTID = downLoad.CTID.ToString();
                            item.PassbackDay = downLoad.PassbackDay.ToString();
                            item.CreateTime = downLoad.CreateTime.ToString();
                            item.DownLoadTime = downLoad.DownLoadTime.ToString();
                            item.PassbackTime = downLoad.PassbackTime.ToString();
                            item.CaseName = CaseList.CaseName;
                            item.CaseType = caseType;
                            item.CaseTypeCnName = GetCaseTypeCnName(item.CaseType);
                            item.PackageName = downLoad.PackageName;
                            item.CaseCount = 1;
                            string useTime = GetUseTime(downLoad.CreateTime, downLoad.PassbackTime);
                            item.UseTime = useTime;
                            detailList.Add(item);
                        }

                    }
                }
            }
            using (ReportCenterEntities ctx = DbCommon.DbConnUtils.GetReportCenterEntities())
            {
                for (int i = 0; i < detailList.Count; i++)
                {

                }
            }
        }


    }
}
