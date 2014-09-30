<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UCCadCamFiles.ascx.cs" Inherits="UCCadCamFiles" %>
<asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False" 
    CellPadding="4" ForeColor="#333333" GridLines="None" 
    
    onrowcommand="gvFiles_RowCommand" onrowdatabound="gvFiles_RowDataBound">
    <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbFolderItem" CommandName="OpenFolder" CommandArgument='<%# Eval("Name") %>'></asp:LinkButton>
                <asp:Literal runat="server" ID="ltlFileItem"></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>

    </Columns>
   
</asp:GridView>