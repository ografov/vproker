/// <reference path="../../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../../node_modules/@types/bootstrap-table/index.d.ts" />
/// <reference path="../../node_modules/@types/eonasdan-bootstrap-datetimepicker/index.d.ts" />

namespace vproker {
    export class CreateOrder {
        static render() {
            $("#clientPhoneNumber").focusout(() => {
                const number = $("#clientPhoneNumber").val();
                if (number && number.length) {
                    CreateOrder.getByPhoneInfo(number, (info) => {
                        if (info) {
                            console.log(info);
                            const statHtml = `<div>
                                                <div>Всего заказов - ${info.allOrdersNumber}</div>
                                                <div>Активных заказов - ${info.activeOrdersNumber}</div>
                                                <a href="/client/details/${info.client.id}">О клиенте</a></div>
                                              </div>`;
                            $("#clientInfo").html(statHtml).show();
                            $(`#clientId [value='${info.client.id}']`).attr("selected", "selected");
                            $("#clientId").attr("disabled", "disabled");
                        }
                        else {
                            const statHtml = `<div><a href="/client/create">Добавить Клиента</a></div>`;
                            $("#clientInfo").html(statHtml).show();
                            $("#clientId").removeAttr("disabled");
                        }
                    });
                }
                else {
                    $("#clientInfo").hide().html('');
                    $("#clientId").removeAttr("disabled");
                }
            });
        }

        static getByPhoneInfo(number: string, success: (info) => void) {
            $.ajax({
                url: "/api/client/getByPhoneInfo",
                data: { number: number },
                success: (data) => {
                    success(data);
                }
            });
        }
    }
}