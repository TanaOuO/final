<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeacherQuery.aspx.cs" Inherits="SchoolWebApp.TeacherQuery" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>教師開課查詢</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            教師編號：
            <asp:TextBox ID="TextBoxEmpID" runat="server" Width="150px"></asp:TextBox>
            <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" />
        </div>

        <h3>教師資料</h3>
        <asp:GridView ID="GridViewTeacher" runat="server" AutoGenerateColumns="true" EmptyDataText="查無教師資料" DataSourceID="SqlDataSourceTeacher"/>

        <h3>開課資料</h3>
        <asp:GridView ID="GridViewCourses" runat="server" AutoGenerateColumns="true" EmptyDataText="查無開課資料" DataSourceID="SqlDataSourceCourses"/>

        <!-- 教師資料 SqlDataSource -->
        <asp:SqlDataSource ID="SqlDataSourceTeacher" runat="server"
            ConnectionString="<%$ ConnectionStrings:SchoolDB_Group1ConnectionString %>"
            SelectCommand="SELECT EmpID, EmpName, EmpDept, EmpTitleID FROM Employee WHERE EmpID = @EmpID">
            <SelectParameters>
                <asp:ControlParameter Name="EmpID" ControlID="TextBoxEmpID" PropertyName="Text" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>

        <!-- 教師開課資料 SqlDataSource -->
        <asp:SqlDataSource ID="SqlDataSourceCourses" runat="server"
            ConnectionString="<%$ ConnectionStrings:SchoolDB_Group1ConnectionString %>"
            SelectCommand="SELECT CourseCode, CourseName, Credit FROM Course WHERE TeacherEmpID = @EmpID">
            <SelectParameters>
                <asp:ControlParameter Name="EmpID" ControlID="TextBoxEmpID" PropertyName="Text" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
