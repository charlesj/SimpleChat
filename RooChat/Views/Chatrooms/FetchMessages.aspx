<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<RooChat.Views.MessagesViewModel>" %>
<script language="javascript" type="text/javascript">
    last_id=<%= Model.LastId %>;
</script>
<% Html.RenderPartial("MessagesDisplay", Model); %>