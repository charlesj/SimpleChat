<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<SimpleChat.Models.Participant>>" %>
<% foreach( var p in Model ) { %>
    <div class="participant"><%= p.Name %></div>
<% } %>