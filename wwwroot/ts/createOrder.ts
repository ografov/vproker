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

            const passportElem = $("#clientPassport");
            passportElem.focusout(() => {
                const passport = passportElem.val();
                const alert = $("#passportAlert");
                if (passport.length) {
                    if (CreateOrder.isCorrectPassport(passport)) {
                        CreateOrder.validatePassport(passport, (isValid) => {
                            const message = `Паспорт ${isValid ? "действителен" : "не действителен!"}`;
                            alert.html(message).show();
                            alert.attr('class', 'alert');
                            alert.addClass(isValid ? 'alert-success' : 'alert-danger');
                        });
                    }
                    else {
                        alert.html('Паспортные данные введены не верно').addClass('alert-danger').show();
                    }
                }
                else {
                    $("#passportAlert").hide().empty();
                }
            });
        }

        static isCorrectPassport(passport: string) {
            return /^\d{10}$/.test(passport);
        }

        static validatePassport(passport: string, success: (isValid) => void) {
            $.ajax({
                url: "/api/order/validatePassport",
                data: { passport: passport },
                success: (data) => {
                    success(data);
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