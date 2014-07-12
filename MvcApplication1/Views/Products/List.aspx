<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<DomainModel.Entities.Product>>" %>
<%@ Import Namespace="DomainModel.Entities" %>
<%@ Import Namespace="WebUI.HtmlHelpers" %>

<asp:Content runat="server" ContentPlaceHolderID="TitleContent">
     SportsStore:ala blah
    <%=string.IsNullOrEmpty((string) ViewData["CurrentCategory"])
    ? "all Products"
    : Html.Encode(ViewData["CurrentCategory"])%>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="MainContent">
    <% foreach (var product in Model)
       { %>
     <% Html.RenderPartial("ProductSummary", product); %>
   <% } %>
    <div class="Pager">
    <%=Html.PageLinks((int)ViewData["CurrentPage"],(int)ViewData["TotalPages"], i=>Url.Action("List",new { page = i})) %>
    </div>    
</asp:Content>