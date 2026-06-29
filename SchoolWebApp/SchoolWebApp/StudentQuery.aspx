<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentQuery.aspx.cs" Inherits="SchoolWebApp.StudentQuery" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>學生選課查詢</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            學號：
            <asp:TextBox ID="TextBoxStudentID" runat="server" Width="150px"></asp:TextBox>
            <asp:Button ID="btnQuery" runat="server" Text="查詢" OnClick="btnQuery_Click" />
        </div>

        <h3>學生資料</h3>
        <asp:GridView ID="GridViewStudent" runat="server" AutoGenerateColumns="true" EmptyDataText="查無資料"
                      DataSourceID="SqlDataSourceStudent" />

        <h3>選課資料</h3>
        <asp:GridView ID="GridViewCourse" runat="server" AutoGenerateColumns="true" EmptyDataText="查無資料"
                      DataSourceID="SqlDataSourceCourse" />

        <!-- 學生資料 SqlDataSource -->
        <asp:SqlDataSource ID="SqlDataSourceStudent" runat="server"
            ConnectionString="<%$ ConnectionStrings:SchoolDB_Group1ConnectionString %>"
            SelectCommand="SELECT StudentNumber, StudentName, StudentGender FROM Student WHERE StudentNumber = @StudentNumber">
            <SelectParameters>
                <asp:ControlParameter Name="StudentNumber" ControlID="TextBoxStudentID" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>

        <!-- 選課資料 SqlDataSource（透過 StudentNumber 關聯 Student 表） -->
        <asp:SqlDataSource ID="SqlDataSourceCourse" runat="server"
            ConnectionString="<%$ ConnectionStrings:SchoolDB_Group1ConnectionString %>"
            SelectCommand="
                SELECT s.StudentNumber, c.CourseCode, c.CourseName, c.Credit 
                FROM StudentCourse sc
                INNER JOIN Student s ON sc.StudentID = s.StudentID
                INNER JOIN Course c ON sc.CourseCode = c.CourseCode
                WHERE s.StudentNumber = @StudentNumber">
            <SelectParameters>
                <asp:ControlParameter Name="StudentNumber" ControlID="TextBoxStudentID" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
