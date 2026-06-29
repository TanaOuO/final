<%@ Page Title="首頁" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SchoolWebApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1 class="display-4">歡迎使用校務系統</h1>
        <p class="lead">請選擇您要查詢的功能：</p>
        <hr class="my-4" />

        <a href="StudentQuery.aspx" class="btn btn-primary btn-lg" role="button">學生查詢</a>
        <a href="TeacherQuery.aspx" class="btn btn-success btn-lg" role="button">教師查詢</a>
    </div>
</asp:Content>
