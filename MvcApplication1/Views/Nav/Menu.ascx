<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.List<WebUI.Controllers.NavLink>>" %>

<% foreach (var link in Model)
   {%>
       <a href="<%= Url.RouteUrl(link.RouteValues) %>"
           class="<%=link.IsSelected ? "selected" : "" %>">
           <%=link.Text%>
       </a>
   <%} %>