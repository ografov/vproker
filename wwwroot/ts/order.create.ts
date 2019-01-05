/// <reference path="../../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../../node_modules/@types/bootstrap-table/index.d.ts" />
/// <reference path="../../node_modules/@types/eonasdan-bootstrap-datetimepicker/index.d.ts" />

namespace vproker {
    export class CreateOrder {
        static render() {
            $("#phoneNumber").focusout(() => {
                CreateOrder.checkPhoneNumber();
            });

            $("#passport").focusout(() => {
                CreateOrder.checkPassport();
            });

            CreateOrder.checkPhoneNumber();
            //CreateOrder.checkPassport();
        };

        private static checkPhoneNumber() {
            const number = $("#phoneNumber").val();
            if (number && number.length) {
                vproker.ClientApi.getInfoByPhone(number, (info) => {
                    CreateOrder.showClientInfo(info);
                });
            }
            else {
                $("#clientInfo").hide().html('');
            }
        }

        private static checkPassport() {
            const passportElem = $("#passport");
            const passport = passportElem.val();
            const alert = $("#passportAlert");
            if (passport.length) {
                if (CreateOrder.isCorrectPassport(passport)) {

                    // animate progress
                    alert.removeClass('alert-danger').removeClass('alert-success').addClass('alert alert-warning');
                    alert.html('Проверяем...');
                    const intervalId = setInterval(function () {
                        alert.fadeToggle(500);
                        //alert.animate({
                        //    opacity: 0
                        //}, 500);
                        //alert.animate({
                        //    opacity: 1
                        //}, 500);
                    }, 500);

                    vproker.ClientApi.validatePassport(passport, (isValid) => {
                        clearInterval(intervalId);
                        const message = `Паспорт ${isValid ? "действителен" : "не действителен!"}`;
                        alert.html(message).show();
                        alert.removeClass('alert-warning').addClass(isValid ? 'alert-success' : 'alert-danger');
                    });
                }
                else {
                    alert.html('Паспортные данные введены не верно').addClass('alert-danger').show();
                }
            }
            else {
                $("#passportAlert").hide().empty();
            }
        }

        private static showClientInfo(info) {
            const $clientName = $("#clientName");
            const $clientInfo = $("#clientInfo");
            const $passport = $("#passport");

            if (info) {
                console.log(info);
                const statHtml = `<div>
                                    <div>Всего заказов - ${info.allOrdersNumber}</div>
                                    <div>Активных заказов - ${info.activeOrdersNumber}</div>
                                    <a href="/client/details/${info.client.id}">О клиенте</a></div>
                                  </div>`;
                $clientInfo.html(statHtml).show();
                $(`#clientId [value='${info.client.id}']`).attr("selected", "selected");
                //$("#clientId").attr("disabled", "disabled");

                if (info.client.name) {
                    $clientName.val(info.client.name).attr("disabled", "disabled");
                }
                if (info.client.passport) {
                    // to not disable to be sent and validated on server
                    $passport.val(info.client.passport); //.attr("disabled", "disabled");
                    CreateOrder.checkPassport();
                }
            }
            else {
                const statHtml = `<div><a href="/client/create">Добавить Клиента</a></div>`;
                $clientInfo.html(statHtml).show();
                $clientName.val("").removeAttr("disabled");
                $passport.val(""); //.removeAttr("disabled");
            }
        }

        static isCorrectPassport(passport: string) {
            return /^\d{10}$/.test(passport);
        }
    }
}