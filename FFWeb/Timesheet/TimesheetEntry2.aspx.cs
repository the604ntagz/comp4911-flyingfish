﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FFLib;

public partial class Timesheet_TimesheetEntry2 : System.Web.UI.Page
{
    FlyingFishClassesDataContext ff = new FlyingFishClassesDataContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        // verifies if the user has logged in. If the
        try
        {
            var qry = (from o in ff.EmployeeMemberships
                       join emp in ff.aspnet_Users on o.userId equals emp.UserId
                       where emp.UserName == User.Identity.Name
                       select o.empId).Single();

            Session["CurEmpId"] = qry.ToString();
            
            System.DateTime currentDate = System.DateTime.Now;
        
            Session["CurrentDate"] = currentDate;

        }
        catch
        {
            Response.Redirect("~/Login.aspx");
        }

    }
    protected void btnCreateNewRecord_Click(object sender, EventArgs e)
    {

        try
        {
            var qry = (from emp in ff.Employees
                       where emp.empId == Convert.ToInt32(Session["CurEmpId"])
                       select emp.approver).Single();


            TimesheetHeader tmp = new TimesheetHeader()
            {

                tsDate = Convert.ToDateTime(Session["CurrentDate"]),
                empId = Convert.ToInt32(Session["CurEmpId"]),
                status = "SAVED",
                approvedBy = qry,
                comments = "try for header"

            };


            var qry2 = from th in ff.TimesheetHeaders
                       where th.empId == tmp.empId && th.tsDate == tmp.tsDate
                       select th;

            if (qry2 == null)
            {
                ff.TimesheetHeaders.InsertOnSubmit(tmp);
                ff.SubmitChanges();
            }

            TimesheetEntry tmp2 = new TimesheetEntry()
            {
                tsDate = Convert.ToDateTime(Session["CurrentDate"]),
                empId = Convert.ToInt32(Session["CurEmpId"]),
                projId = Convert.ToInt32(tbProjectID.Text),
                wpId = tbWpID.Text,
                sun = Convert.ToInt32(tbSun.Text),
                sat = Convert.ToInt32(tbSat.Text),
                fri = Convert.ToInt32(tbFri.Text),
                thu = Convert.ToInt32(tbThu.Text),
                wed = Convert.ToInt32(tbWed.Text),
                tue = Convert.ToInt32(tbTue.Text),
                mon = Convert.ToInt32(tbMon.Text),
                notes = tbNote.Text

            };

            ff.TimesheetEntries.InsertOnSubmit(tmp2);
            ff.SubmitChanges();

            Label1.Text = "Sumbit has been saved.";

            tbFri.Text = String.Empty;
            tbMon.Text = String.Empty;
            tbNote.Text = String.Empty;
            tbProjectID.Text = String.Empty;
            tbSat.Text = String.Empty;
            tbSun.Text = String.Empty;
            tbThu.Text = String.Empty;
            tbTue.Text = String.Empty;
            tbWed.Text = String.Empty;
            tbWpID.Text = String.Empty;

        }
        catch (Exception myException)
        {
            Label1.Text = myException.Message;

        }
        

    }
}