using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WST.IService;
using WST.Service;
using WST.Model;


namespace WST.WindowsService
{
    public partial class NoticeEndTimeService : ServiceBase
    {
        public IUserService userService = new UserService();
        public NoticeEndTimeService()
        {
            InitializeComponent();
        }

        //  定义计时器  24小时扫描一次
        private System.Timers.Timer timer = new System.Timers.Timer(60*1000 * 24);
        protected override void OnStart(string[] args)
        {
            //使用Elapsed事件，其中timer_Elapsed就是你需要处理的事情
            timer.Elapsed += new System.Timers.ElapsedEventHandler(StatScan);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            // TODO:  在此处添加代码以执行停止服务所需的关闭操作。
            timer.Enabled = false;
        }
        //  统计扫描信息
        private void StatScan(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                DateTime noticeStartTime = DateTime.Now.AddDays(7);
                DateTime noticeEndTime = DateTime.Now.AddDays(8);
                var userList = userService.GetList(x => x.EndTime.HasValue && x.EndTime > noticeStartTime && x.EndTime < noticeEndTime);
                if (userList != null && userList.Count > 0)
                {

                }

            }
            catch (Exception)
            {
            }
        }
    }
}
