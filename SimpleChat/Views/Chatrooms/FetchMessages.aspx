<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<List<SimpleChat.Models.Message>>" %>
<% Html.RenderPartial("MessagesDisplay", Model); %>