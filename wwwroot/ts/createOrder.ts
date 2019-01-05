/// <reference path="../../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../../node_modules/@types/bootstrap-table/index.d.ts" />
/// <reference path="../../node_modules/@types/eonasdan-bootstrap-datetimepicker/index.d.ts" />

namespace vproker {
    export class CreateOrder {
        static render() {
            $("#clientPhoneNumber").focusout(() => {
                const number = $("#clientPhoneNumber").val();
                if (number && number.length) {
                    CreateOrder.getInfoByPhone(number, (info) => {
                        CreateOrder.showClientInfo(info);
                    });
                }
                else {
                    $("#clientInfo").hide().html('');
                    //$("#clientId").removeAttr("disabled");
                }
            });

            $("#clientId").change(() => {
                const selectedClientId = $("#clientId option:selected").val();
                CreateOrder.getInfoById(selectedClientId, (info) => {
                    CreateOrder.showClientInfo(info);
                });
            });
        };

        private static showClientInfo(info) {
            if (info) {
                console.log(info);
                const statHtml = `<div>
                                    <div>Всего заказов - ${info.allOrdersNumber}</div>
                                    <div>Активных заказов - ${info.activeOrdersNumber}</div>
                                    <a href="/client/details/${info.client.id}">О клиенте</a></div>
                                  </div>`;
                $("#clientInfo").html(statHtml).show();
                $(`#clientId [value='${info.client.id}']`).attr("selected", "selected");
                //$("#clientId").attr("disabled", "disabled");
            }
            else {
                const statHtml = `<div><a href="/client/create">Добавить Клиента</a></div>`;
                $("#clientInfo").html(statHtml).show();
                //$("#clientId").removeAttr("disabled");
            }
        }

        static getInfoByPhone(number: string, success: (info) => void) {
            $.ajax({
                url: "/api/client/getInfoByPhone",
                data: { number: number },
                success: (data) => {
                    success(data);
                }
            });
        }

        static getInfoById(clientId: string, success: (info) => void) {
                $.ajax({
                    url: "/api/client/getInfoById",
                    data: { id: clientId },
                    success: (data) => {
                        success(data);
                    }
                });
        }
    }
}