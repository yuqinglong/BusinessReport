using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;
using System.IO;


namespace BusinessReport
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        string ifCenterString = System.Configuration.ConfigurationManager.ConnectionStrings["IFCenter"].ConnectionString;

        string mainBackString = System.Configuration.ConfigurationManager.ConnectionStrings["MainBack"].ConnectionString;

        string startIndex = System.Configuration.ConfigurationManager.AppSettings["StartIndex"];

        string savePath = System.Configuration.ConfigurationManager.AppSettings["SavePath"];

        Dictionary<int, int> startIndexList = new Dictionary<int, int>();

        QueryType queryType;

        DataTable dt;

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitData(this.ywlProjectList);
            InitData(this.sxProjectList);
            InitData(this.zjProjectList);

            string[] indexArray = startIndex.Split(',');
            foreach (var item in indexArray)
            {
                int key = Int32.Parse(item.Split(':')[0]);
                int value = Int32.Parse(item.Split(':')[1]);
                startIndexList.Add(key, value);
            }
        }

        private void InitData(CheckedListBox projectList)
        {
            using (SqlConnection paramConnection = new SqlConnection(ifCenterString))
            {
                string querySql = "select  CAST(CID as varchar(10)) + ','+ CAST(CTID as varchar(10)) 'CIDCTID',";
                querySql += "ProjectName from ParamCenter.dbo.ClientProjectList where ShowInMonitor = 1 ";
                querySql += "order by ProjectName";

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(querySql, paramConnection);
                da.Fill(dt);

                projectList.DataSource = dt;
                projectList.DisplayMember = "ProjectName";
                projectList.ValueMember = "CIDCTID";
                projectList.ClearSelected();
            }

            int day = DateTime.Now.Day;

            this.ywlSDT.Value = DateTime.Now.AddDays(-day + 1);
            this.sxSDT.Value = DateTime.Now.AddDays(-day + 1);
            this.zjSDT.Value = DateTime.Now.AddDays(-day + 1);

        }

        //导出到Excel
        private void Export_Click(object sender, EventArgs e)
        {
            if (dt != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel文件(*.xlsx)|*.xlsx";

                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                string fileName = string.Empty;

                if (this.queryType == QueryType.业务量查询)
                {
                    fileName = "业务量报表-" + this.ywlProjectList.Text + this.ywlSDT.Value.ToString("yyyyMMdd") + "-" + this.ywlEDT.Value.ToString("dd") + ".xlsx";
                    ExcelOperation.SaveToExcel(dt, fileName, "业务量");
                }
                else if (this.queryType == QueryType.时效查询)
                {
                    fileName = "时效报表-" + this.sxProjectList.Text + this.sxSDT.Value.ToString("yyyyMMdd") + "-" + this.sxEDT.Value.ToString("dd") + ".xlsx";
                    ExcelOperation.SaveToExcel(dt, fileName, "时效");
                }
                else if (this.queryType == QueryType.字节查询)
                {
                    fileName = "字节报表-" + this.zjProjectList.Text + this.zjSDT.Value.ToString("yyyyMMdd") + "-" + this.zjEDT.Value.ToString("dd") + ".xlsx";
                    ExcelOperation.SaveToExcel(dt, fileName, "字节");
                }

                MessageBox.Show("导出成功！");
            }
        }
        //退出
        private void Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //业务量查询
        private void ywlBtnQuery_Click(object sender, EventArgs e)
        {
            this.queryType = QueryType.业务量查询;
            BtnQuery(this.ywlProjectList, this.ywlRdReceive, this.ywlRdBack, this.ywlRdPack, this.ywlSDT, this.ywlEDT, this.ywlDgv);
        }

        //时效查询
        private void sxBtnQuery_Click(object sender, EventArgs e)
        {
            this.queryType = QueryType.时效查询;
            BtnQuery(this.sxProjectList, this.sxRdReceive, this.sxRdPack, this.sxRdBack, this.sxSDT, this.sxEDT, this.sxDgv);
        }
        //字节查询
        private void zjBtnQuery_Click(object sender, EventArgs e)
        {
            this.queryType = QueryType.字节查询;
            BtnQuery(this.zjProjectList, null, null, null, this.zjSDT, this.zjEDT, this.zjDgv);
        }

        private void BtnQuery(CheckedListBox projectList, RadioButton rdReceive, RadioButton rdBack, RadioButton rdPack, DateTimePicker sdt, DateTimePicker edt, DataGridView dgv)
        {
            if (projectList.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择客户！");
                return;
            }

            string startDay = sdt.Value.ToString("yyyyMMdd");

            string endDay = edt.Value.ToString("yyyyMMdd");

            if (queryType == QueryType.业务量查询 || queryType == QueryType.时效查询)
            {
                if (!rdReceive.Checked && !rdPack.Checked && !rdBack.Checked)
                {
                    MessageBox.Show("请选择日期取值方式！");
                    return;
                }

                int cid = Int32.Parse(projectList.SelectedValue.ToString().Split(',')[0]);

                int ctid = Int32.Parse(projectList.SelectedValue.ToString().Split(',')[1]);

                if (ctid == 24)
                {
                    frmHuatai frm = new frmHuatai();

                    if (rdReceive.Checked)
                    {
                        frm.queryWay = QueryWay.按接收日期;
                    }
                    else if (rdBack.Checked)
                    {
                        frm.queryWay = QueryWay.按回传日期;
                    }
                    else if (rdPack.Checked)
                    {
                        frm.queryWay = QueryWay.按包名日期;
                    }

                    frm.WindowState = FormWindowState.Maximized;
                    frm.ShowDialog();
                }
                else
                {
                    int index = -1;

                    if (startIndexList.ContainsKey(ctid))
                    {
                        index = startIndexList[ctid];
                    }

                    queryYwl(rdReceive, rdBack, rdPack, cid, ctid, index, startDay, endDay, queryType, dgv);
                }
            }
            else if (queryType == QueryType.字节查询)
            {

                List<string> list = new List<string>();
                for (int i = 0; i < projectList.Items.Count; i++)
                {
                    if (projectList.GetItemChecked(i))
                    {
                        projectList.SetSelected(i, true);
                        list.Add(projectList.SelectedValue.ToString());
                    }
                }

                queryZJ(list, startDay, endDay);
            }
        }

        private void queryYwl(RadioButton rdReceive, RadioButton rdBack, RadioButton rdPack, int cid, int ctid, int index, string startDay, string endDay, QueryType queryType, DataGridView dgv)
        {
            string querySql = string.Empty;

            if (queryType == QueryType.业务量查询)
            {
                #region 业务量查询
                if (ctid == 42)
                {
                    #region 邮储
                    querySql += "select  cid '客户号',ctid '业务号',dateTime '日期',packageCount '包数',clasifyByte '分类字节',";
                    querySql += "fixByte '定光标字节',blankByte '空白字节',webByte '外网字节',entryByte '内网字节',compareByte '核对字节',";
                    querySql += "qcByte '成品字节',finalByte '输出字节' from ReportCenter.dbo.DailyYCReport with(nolock)  ";
                    querySql += "where cid=@cid and ctid=@ctid and  {0} >= @startDay and {0} <= @endDay  ";


                    if (rdReceive.Checked)
                    {
                        querySql = string.Format(querySql, "dateTime");
                    }
                    else
                    {
                        MessageBox.Show("邮储客户只能根据接收日期进行统计！");
                        return;
                    }
                    #endregion
                }
                else
                {
                    #region 非邮储
                    querySql += "select {0} '业务日期',OrganCode '机构代码',BranchCode '分支代码',";
                    querySql += "BelongType '结算类型',sum(CaseCount) '业务量',sum(ByteCount) '字节数' from Client_WorkCount  ";
                    querySql += "with(nolock)  where cid=@cid and ctid=@ctid and  {0} >= @startDay and {0} <= @endDay ";
                    querySql += "group by {0},OrganCode,BranchCode,BelongType order by {0}";

                    if (rdReceive.Checked)
                    {
                        querySql = string.Format(querySql, "DownLoadDay");
                    }
                    else if (rdBack.Checked)
                    {
                        querySql = string.Format(querySql, "PassbackDay");
                    }
                    else if (rdPack.Checked)
                    {
                        if (index > 0)
                        {
                            querySql = string.Format(querySql, "cast(substring(packageName,{0},8) as int)");
                            querySql = string.Format(querySql, index);
                        }
                        else
                        {
                            MessageBox.Show("该客户无法从批次名称获取日期，请选择其它日期取值方式。");
                            return;
                        }
                    }
                    #endregion
                }
                #endregion
            }
            else if (queryType == QueryType.时效查询)
            {
                #region 时效查询
                querySql += "select dbo.ConvertToDate({0}) '业务日期',PackageName '批次名称',CaseCount '份数',";
                querySql += "CaseType '类型名称',dbo.ConvertToDate(CreateTime) '开始接收时间',dbo.ConvertToDate(DownLoadTime) '接收结束时间',";
                querySql += "dbo.ConvertToDate(PassbackTime) '开始回传时间',dbo.ConvertToDate(PassbackTime) '回传结束时间' from DownLoad ";
                querySql += "with(nolock)  where cid=@cid and ctid=@ctid and  {0} >= @startDay and {0} <= @endDay";

                if (rdReceive.Checked)
                {
                    querySql = string.Format(querySql, "DownLoadDay");
                }
                else if (rdBack.Checked)
                {
                    querySql = string.Format(querySql, "PassbackDay");
                }
                else if (rdPack.Checked)
                {
                    if (index > 0)
                    {
                        querySql = string.Format(querySql, "substring(packageName,{0},8)");
                        querySql = string.Format(querySql, index);
                    }
                    else
                    {
                        MessageBox.Show("该客户无法从批次名称获取日期，请选择其它日期取值方式。");
                        return;
                    }
                }
                #endregion
            }
            else if (queryType == QueryType.字节查询)
            {
                #region 字节查询
                querySql += "select A.客户号,A.作业号,A.作业名称,A.定光标字节数,B.空白判断字节数,C.录入字节数,E.成品字节数,F.输出字节数 ";
                querySql += "from  (select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '定光标字节数' ";
                querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport] with(nolock)";
                querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 30";
                querySql += "group by cid,ctid) A,  ";
                querySql += "(select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '空白判断字节数' ";
                querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport]  with(nolock) ";
                querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 50 ";
                querySql += "group by cid,ctid) B,   ";
                querySql += "(select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '录入字节数'  ";
                querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport]  with(nolock) ";
                querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 90 ";
                querySql += "group by cid,ctid) C,   ";
                querySql += "(select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '核对字节数'  ";
                querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport]  with(nolock) ";
                querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 130 ";
                querySql += "group by cid,ctid) D,   ";
                querySql += "(select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '成品字节数'  ";
                querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport]  with(nolock) ";
                querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 160 ";
                querySql += "group by cid,ctid) E,   ";
                querySql += "(select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '输出字节数'  ";
                querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport]  with(nolock) ";
                querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 170 ";
                querySql += "group by cid,ctid) F   ";
                querySql += "where A.客户号 = B.客户号 and A.作业号 = B.作业号 ";
                querySql += "  and B.客户号 = C.客户号 and B.作业号 = C.作业号 ";
                querySql += "  and C.客户号 = D.客户号 and C.作业号 = D.作业号 ";
                querySql += "  and D.客户号 = E.客户号 and D.作业号 = E.作业号 ";
                querySql += "  and E.客户号 = F.客户号 and E.作业号 = F.作业号 ";
                #endregion
            }

            #region 执行查询
            dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(ifCenterString))
            {
                SqlCommand command = new SqlCommand(querySql, connection);

                if (queryType != QueryType.字节查询)
                {
                    command.Parameters.AddWithValue("@cid", cid);
                    command.Parameters.AddWithValue("@ctid", ctid);
                }

                command.Parameters.AddWithValue("@startDay", startDay);
                command.Parameters.AddWithValue("@endDay", endDay);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
            }

            dgv.DataSource = dt;
            #endregion
        }

        private void queryZJ(List<string> CIDCTIDList, string startDay, string endDay)
        {
            string querySql = string.Empty;

            querySql += "select A.客户号,A.作业号,A.作业名称,A.定光标字节数,B.空白判断字节数,C.录入字节数,E.成品字节数,F.授权字节数 ";
            querySql += "from  (select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '定光标字节数' ";
            querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport] with(nolock)";
            querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 30  and (";

            foreach (var item in CIDCTIDList)
            {
                querySql += "(cid={0} and ctid = {1}) or ";
                querySql = string.Format(querySql, item.Split(',')[0], item.Split(',')[1]);
            }
            querySql = querySql.Substring(0, querySql.LastIndexOf("or")) + ") ";

            querySql += "group by cid,ctid) A,  ";
            querySql += "(select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '空白判断字节数' ";
            querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport]  with(nolock) ";
            querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 50  and (";

            foreach (var item in CIDCTIDList)
            {
                querySql += "(cid={0} and ctid = {1}) or ";
                querySql = string.Format(querySql, item.Split(',')[0], item.Split(',')[1]);
            }
            querySql = querySql.Substring(0, querySql.LastIndexOf("or")) + ") ";

            querySql += "group by cid,ctid) B,   ";
            querySql += "(select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '录入字节数'  ";
            querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport]  with(nolock) ";
            querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 90  and (";

            foreach (var item in CIDCTIDList)
            {
                querySql += "(cid={0} and ctid = {1}) or ";
                querySql = string.Format(querySql, item.Split(',')[0], item.Split(',')[1]);
            }
            querySql = querySql.Substring(0, querySql.LastIndexOf("or")) + ") ";

            querySql += "group by cid,ctid) C,   ";
            querySql += "(select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '核对字节数'  ";
            querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport]  with(nolock) ";
            querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 130  and (";

            foreach (var item in CIDCTIDList)
            {
                querySql += "(cid={0} and ctid = {1}) or ";
                querySql = string.Format(querySql, item.Split(',')[0], item.Split(',')[1]);
            }
            querySql = querySql.Substring(0, querySql.LastIndexOf("or")) + ") ";

            querySql += "group by cid,ctid) D,   ";
            querySql += "(select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '成品字节数'  ";
            querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport]  with(nolock) ";
            querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 160 ";
            querySql += "group by cid,ctid) E,   ";
            querySql += "(select cid '客户号',ctid '作业号',dbo.GetProjectName(cid,ctid) '作业名称',sum(ByteCount) '授权字节数'  ";
            querySql += "from  ReportCenter.[dbo].[DailyWorkLoadReport]  with(nolock) ";
            querySql += "where CreateDay >=@startDay  and CreateDay <= @endDay and FlowNo = 170  and (";

            foreach (var item in CIDCTIDList)
            {
                querySql += "(cid={0} and ctid = {1}) or ";
                querySql = string.Format(querySql, item.Split(',')[0], item.Split(',')[1]);
            }
            querySql = querySql.Substring(0, querySql.LastIndexOf("or")) + ") ";

            querySql += "group by cid,ctid) F   ";
            querySql += "where A.客户号 = B.客户号 and A.作业号 = B.作业号 ";
            querySql += "  and B.客户号 = C.客户号 and B.作业号 = C.作业号 ";
            querySql += "  and C.客户号 = D.客户号 and C.作业号 = D.作业号 ";
            querySql += "  and D.客户号 = E.客户号 and D.作业号 = E.作业号 ";
            querySql += "  and E.客户号 = F.客户号 and E.作业号 = F.作业号 ";


            dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(ifCenterString))
            {
                SqlCommand command = new SqlCommand(querySql, connection);

                command.Parameters.AddWithValue("@startDay", startDay);
                command.Parameters.AddWithValue("@endDay", endDay);

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
            }

            this.zjDgv.DataSource = dt;
        }

        private void ckCheckAll_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox ck = sender as CheckBox;

            if (ck != null)
            {
                if (ck.CheckState == CheckState.Checked)
                {
                    for (int i = 0; i < this.zjProjectList.Items.Count; i++)
                    {
                        this.zjProjectList.SetItemChecked(i, true);
                    }
                }
                else
                {
                    for (int i = 0; i < this.zjProjectList.Items.Count; i++)
                    {
                        this.zjProjectList.SetItemChecked(i, false);
                    }
                }
            }
        }

        private void ywlProjectList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox cklData = sender as CheckedListBox;
            this.sxProjectList.SetItemCheckState(e.Index, CheckState.Checked);
            this.zjProjectList.SetItemCheckState(e.Index, CheckState.Checked);
            this.sxProjectList.SetSelected(e.Index, true);
            this.zjProjectList.SetSelected(e.Index, true);

            if (cklData.CheckedItems.Count > 0)
            {
                for (int i = 0; i < cklData.Items.Count; i++)
                {
                    if (i != e.Index)
                    {
                        cklData.SetItemChecked(i, false);
                    }

                }
            }
        }

        private void sxProjectList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox cklData = sender as CheckedListBox;

            if (cklData.CheckedItems.Count > 0)
            {
                for (int i = 0; i < cklData.Items.Count; i++)
                {
                    if (i != e.Index)
                    {
                        cklData.SetItemChecked(i, false);
                    }

                }
            }
        }

        private void zjProjectList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox cklData = sender as CheckedListBox;

            if (cklData.CheckedItems.Count > 0)
            {
                for (int i = 0; i < cklData.Items.Count; i++)
                {
                    if (i != e.Index)
                    {
                        cklData.SetItemChecked(i, false);
                    }

                }
            }
        }
    }
    public enum QueryType
    {
        业务量查询 = 1,
        时效查询 = 2,
        字节查询 = 3
    }

    public enum QueryWay
    {
        按接收日期 = 1,
        按回传日期 = 2,
        按包名日期 = 3
    }

}
