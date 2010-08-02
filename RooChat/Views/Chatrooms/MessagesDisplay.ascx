<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<RooChat.Models.Message>>" %>
<% foreach (var message in Model)
   { %>
    <% Html.RenderPartial("Fetchmessage", message); %>
<% } %>