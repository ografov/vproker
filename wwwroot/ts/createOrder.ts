/// <reference path="../../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../../node_modules/@types/bootstrap-table/index.d.ts" />
/// <reference path="../../node_modules/@types/eonasdan-bootstrap-datetimepicker/index.d.ts" />

namespace vproker {
    export class CreateOrder {
        static render() {
            $("#clientPhoneNumber").focusout(() => {
                const number = $("#clientPhoneNumber").val();
                if (number && number.length) {
                    CreateOrder.getByPhoneInfo(number, (stat) => {
                        console.log(stat);
                        const statHtml = `<div>Всего заказов - ${stat.all}</div><div>Активных заказов - ${stat.active}</div>`;
                        $("#clientInfo").html(statHtml).show();
                    });
                }
                else {
                    $("#clientInfo").hide().html('');
                }
            });
        }

        static getByPhoneInfo(number: string, success: (info) => void) {
            $.ajax({
                url: "/api/order/getByPhoneInfo",
                data: { number: number },
                success: (data) => {
                    success(data);
                }
            });
        }
        static getOrdersByPhone(number: string,  success: (orders) => void) {
            $.ajax({
                url: "/api/order/getByPhone",
                data: { number: number },
                success: (data) => {
                    success(data);
                }
            });
        }
    }
}