<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<RooChat.Models.Message>>" %>
<% Html.RenderPartial("MessagesDisplay", Model); %>