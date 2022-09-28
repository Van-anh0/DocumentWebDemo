<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentUI.aspx.cs" Inherits="DocumentWebDemo.pages.DocumentUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <h2>Quản lý thông tin sản phẩm</h2>
    <div class="card">
        <div class="card-header">
            <div class="form-row">
                <div class="col-md-1">
                    <asp:DropDownList CssClass="custom-select" AutoPostBack="true" ID="dlPageNumber" runat="server" OnSelectedIndexChanged="dlPageNumber_SelectedIndexChanged">
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>40</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                        <asp:ListItem>150</asp:ListItem>
                        <asp:ListItem>200</asp:ListItem>
                        <asp:ListItem>250</asp:ListItem>
                        <asp:ListItem>300</asp:ListItem>
                        <asp:ListItem>350</asp:ListItem>
                        <asp:ListItem>400</asp:ListItem>
                        <asp:ListItem>450</asp:ListItem>
                        <asp:ListItem>500</asp:ListItem>
                        <asp:ListItem>1000</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <input type="text" class="form-control" id="txtKeyword" placeholder="Keyword" runat="server">
                </div>
                <div class="cod-md-2">
                    <asp:Button ID="btTim" runat="server" CssClass="btn btn-primary" Text="Tìm" OnClick="btTim_Click" />
                </div>
                <div class="cod-md-2">
                    &nbsp;<asp:Button ID="btThemMoi" CssClass="btn btn-primary" runat="server" Text="Thêm mới" OnClick="btThemMoi_Click" />
                </div>
                <div class="cod-md-2">
                    &nbsp;<asp:Button ID="btXoa" runat="server" CssClass="btn btn-primary" Text="Xóa" OnClientClick="return confirm('Bạn có muốn xóa không?')" OnClick="btXoa_Click" />
                </div>
            </div>
        </div>
        <div class="card-body">
            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>

            <!-- Controls -->
            <asp:Panel ID="pnControls" runat="server" Visible="false">
                <div class="form-group">
                    <div class="form-row">
                        <div class="col-md-2">
                            <input type="text" class="form-control" id="txtID" readonly runat="server" placeholder="ID">
                        </div>
                        <div class="col-md-3">
                            <input type="text" class="form-control" id="txtName" runat="server" placeholder="Ten san pham">
                        </div>
                        <div class="col-6">
                            <input type="text" class="form-control" id="txtPrice" runat="server" placeholder="Gia san pham">
                        </div>
                        <div class="col-6">
                            <asp:DropDownList ID="drProductType" CssClass="custom-select mr-sm-2" runat="server"></asp:DropDownList>
                        </div>
                        <div class="col-6">
                            <input type="text" class="form-control" id="txtDescription" runat="server" placeholder="Mo ta san pham">
                        </div>

                    </div>
                </div>



                <div class="form-group">
                    <asp:Button ID="btXoaTrang" runat="server" Text="Xóa trắng" CssClass="btn btn-primary" OnClick="btXoaTrang_Click" />
                    <asp:Button ID="btLuu" runat="server" Text="Lưu" CssClass="btn btn-primary" OnClick="btLuu_Click" />
                    <asp:Button ID="btImport" runat="server" Text="Import" CssClass="btn btn-primary" OnClick="btImport_Click" />
                    <asp:Button ID="btThoat" runat="server" Text="Thoát" CssClass="btn btn-primary" OnClick="btThoat_Click" />
                </div>
            </asp:Panel>

            <!-- Tables -->
            <%--<asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>--%>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnTable" runat="server">
                        <div class="table-responsive">
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>
                                            <input id="selectAll" type="checkbox"><label for='selectAll'></label></th>
                                        <th>Edit</th>
                                        <th>Tên sản phẩm</th>
                                        <th>Loại sản phẩm</th>
                                        <th>Giá</th>
                                        <th>Mô tả</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%  foreach (var item in ls)
                                        { %>
                                    <tr>
                                        <td style="width: 40px; text-align: center">
                                            <input name='cbID' value='<%=item.ID %>' type='checkbox' /></td>
                                        <td style="width: 50px">
                                            <a style="text-align: center" href="?idEdit=<%=item.ID %>" class="btn btn-primary">Edit</a>
                                        </td>
                                        <td><%=item.Title %></td>
                                        <td><%=item.Path %></td>
                                        <td><%=item.Content %></td>
                                        <td><%=item.DocumentTypeID %></td>
                                    </tr>
                                    <% } %>
                                </tbody>
                            </table>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <div class="card-footer text-right">
            <asp:Panel ID="pnPhanTrang" runat="server">
                <div class="form-row">
                    <div class="col-auto">
                        <asp:Button ID="btTruoc" runat="server" Text="Trước" class="btn btn-dark" OnClick="btPhanTrang_Click" />
                    </div>
                    <div class="col-auto">
                        <asp:HiddenField ID="hPageIndex" runat="server" />
                        <asp:HiddenField ID="hTotalRows" runat="server" />
                        <asp:HiddenField ID="hPageSize" runat="server" />
                        <asp:Panel ID="pnButton" runat="server"></asp:Panel>

                    </div>
                    <div class="col-auto">
                        <asp:Button ID="btSau" runat="server" Text="Sau" class="btn  btn-dark" OnClick="btPhanTrang_Click" />
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>

    <script>
        $("#selectAll").click(function () {
            $("input[type=checkbox]").prop('checked', $(this).prop('checked'));

        });
    </script>


</asp:Content>


