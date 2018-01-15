<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DataBaseConnectionSetting.aspx.cs" Inherits="DataBaseConnectionSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        const dark = "img/darkbulb.png";
        const light = "img/lightbulb.png";
        $(function () {
            //按鈕事件註冊，事件包含對應按鈕Textbox檢核與連線資訊確認訊息
            $(".test").click(function () {
                var action = $(this).context.id.replace("btn", "");
                if (CheckTextBoxNull(action)) {
                    alert("請確認" + (action == "From" ? "來源" : "匯入") + "欄位不得為空");

                } else {
                    var data = JSON.stringify($("#div" + action + " :input").serializeArray());
                    CheckConnection(data, action);
                }
            });
        });
        function CheckConnection(datalist, action) {
            //確認伺服器連線資訊，使用ploading遮罩在ajax執行過程，並透過service取得資料庫連線狀態，在ajax done時解除遮罩
            //將ajax return 給when做listen，確保when在ajax執行完成後再執行done的動作
            $('.mainTainer').ploading({
                action: 'show',
                spinner: 'wave'
            })
            return $.ajax({
                url: 'WebService/DBVaildService.ashx',
                type: "POST",
                dataType: 'text',
                data: { array: datalist },
                success: function (msg) {
                    console.log(msg);
                    alert((action == "From" ? "來源" : "匯入") + "資料庫" + (msg == "Success" ? "連線成功" : "連線失敗")); //連線測試訊息
                    $("#" + action + "bulb").attr("src", (msg == "Success" ? light : dark));                               //連線測試狀態圖
                },
                error: function (xhr, ajaxOptions, thrownError) {                                                          //當連線失敗時，將連線測試結果清空，連線測試狀態圖恢復到未連結
                    $("#" + action + "bulb").attr("src", dark);

                }
            }).done(function () {                                                                                          //關閉遮罩
                $('.mainTainer').ploading({
                    action: 'hide'
                })
            });
        }

        function CheckTextBoxNull(action) {
            ///檢查TextBox有沒有空值，若action有值則檢查單邊，若無值則所有都檢查
            let hasNull = false;
            $((action ? "input[id^='txt" + action + "']" : "input[type=text] , input[type=password]")).each(function () {
                if ($(this).val().trim() == '')
                    hasNull = true;
            });
            return hasNull;
        }
        function ExecConnectionTestAndVerify() {
            //在submit時，先檢核所有欄位是否有空值，降低連線的測試時間
            let result = true;
            if (CheckTextBoxNull()) {
                alert("請確認所有欄位不得為空");
                return result;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid mainTainer">

        <div class="row">
            <div class="col-md-5" id="divFrom">
                <h5 class="form-signin-heading"><img id="Frombulb" src="img/darkbulb.png" />要來源的資料庫</h5>
                <label for="txtFromSource" class="sr-only">From Connection Source</label>
                <asp:TextBox ID="txtFromSource" ClientIDMode="Static" runat="server" class="form-control" placeholder="IP or Domain"  autofocus=""></asp:TextBox>
                <label for="txtFromDbname" class="sr-only">Password</label>
                <asp:TextBox ID="txtFromDbname" ClientIDMode="Static" runat="server" class="form-control" placeholder="Dbname"  autofocus=""></asp:TextBox>
                <label for="txtFromUser" class="sr-only">Password</label>
                <asp:TextBox ID="txtFromUser" ClientIDMode="Static" runat="server" class="form-control" placeholder="User name"  autofocus=""></asp:TextBox>
                <asp:TextBox ID="txtFromPassword" ClientIDMode="Static" runat="server" TextMode="Password" class="form-control" placeholder="Password" ></asp:TextBox>
                <button class="btn btn-md btn-primary btn-block test" id="btnFrom" type="button">連線測試</button>
            </div>
            <div class="col-md-2 text-center " style="vertical-align: central">
                <img style="width: 55px; height: 55px; display: block; margin: auto;" src="img/arrow.jpg" />
            </div>
            <div class="col-md-5" id="divTo">
                <h5 class="form-signin-heading"><img id="Tobulb" src="img/darkbulb.png" />要匯入的資料庫</h5>
                <label for="txtToSource" class="sr-only">From Connection Source</label>
                <asp:TextBox ID="txtToSource" ClientIDMode="Static" runat="server" class="form-control" placeholder="IP or Domain"  autofocus=""></asp:TextBox>
                <label for="txtToDbname" class="sr-only">Password</label>
                <asp:TextBox ID="txtToDbname" ClientIDMode="Static" runat="server" class="form-control" placeholder="Dbname"  autofocus=""></asp:TextBox>
                <label for="txtToUser" class="sr-only">Password</label>
                <asp:TextBox ID="txtToUser" ClientIDMode="Static" runat="server" class="form-control" placeholder="User name"  autofocus=""></asp:TextBox>
                <asp:TextBox ID="txtToPassword" ClientIDMode="Static" runat="server" TextMode="Password" class="form-control" placeholder="Password" ></asp:TextBox>
                <button class="btn btn-md btn-primary btn-block test" id="btnTo" type="button">連線測試</button>
            </div>

        </div>
        <div class="row text-center">
            <div class="col-md-12">
                <br />
                <asp:Button ID="btnSubmit" OnClientClick="return ExecConnectionTestAndVerify();" OnClick="btnSubmit_Click" runat="server" Text="開始設定" class="btn btn-md btn-primary btn-block" />
            </div>
        </div>

    </div>

</asp:Content>

