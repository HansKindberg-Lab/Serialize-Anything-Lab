<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Default" %>
<%@ Import Namespace="System.Reflection" %>
<!DOCTYPE html>
<html lang="en">
	<head>
		<title>Default</title>
		<style>
			form fieldset fieldset {
				margin-bottom: 10px;
			}
		</style>
	</head>
	<body>
		<form action="#">
			<fieldset>
				<legend>Select</legend>
				<fieldset>
					<legend>Include static fields</legend>
					<input<%= this.IncludeStaticFields ? " checked=\"checked\"" : string.Empty %> id="include-static-fields-true" name="<%= this.IncludeStaticFieldsKey %>" value="true" type="radio" />
					<label for="include-static-fields-true">True</label>
					<br />
					<input<%= !this.IncludeStaticFields ? " checked=\"checked\"" : string.Empty %> id="include-static-fields-false" name="<%= this.IncludeStaticFieldsKey %>" value="false" type="radio" />
					<label for="include-static-fields-false">False</label>
				</fieldset>
				<div class="button-area">
					<button type="submit">Visa</button>
				</div>
			</fieldset>
		</form>
		<p><strong>Number of fiels for the type "<%= this.InstanceType %>: </strong><%= this.NumberOfFields %></p>
		<asp:Repeater id="FieldRepeater" DataSource="<%# this.Fields %>" runat="server">
			<HeaderTemplate>
				<ul>
			</HeaderTemplate>
			<ItemTemplate>
				<li>
					<span><strong>Name: </strong><%# ((FieldInfo) Container.DataItem).Name %></span>
					<br />
					<span><strong>Declaring-type: </strong><%# ((FieldInfo) Container.DataItem).DeclaringType %></span>
				</li>
			</ItemTemplate>
			<FooterTemplate>
				</ul>
			</FooterTemplate>
		</asp:Repeater>
	</body>
</html>