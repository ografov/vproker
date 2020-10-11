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

            $("#selectTool").change(() => {
                CreateOrder.showToolInfo();
            });

            CreateOrder.checkPhoneNumber();
            //CreateOrder.checkPassport();
        };

        private static showToolInfo() {
            const id = $("#selectTool").children("option:selected").val();
            vproker.ToolApi.getToolInfo(id, (info) => {
                const $toolInfo = $("#toolInfo");

                if (info) {
                    const statHtml = `<div>
                                    <div>Сутки - ${info.dayPrice}, Смена - ${info.workShiftPrice}</div>
                                    <div>Час - ${info.hourPrice}, Залог - ${info.pledge}</div>
                                    <a href="/tool/details/${info.id}">Подробнее</a></div>
                                  </div>`;
                    $toolInfo.html(statHtml).show();
                }
            });
        }

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
                    //const intervalId = setInterval(function () {
                    //    alert.fadeToggle(500);
                    //    //alert.animate({
                    //    //    opacity: 0
                    //    //}, 500);
                    //    //alert.animate({
                    //    //    opacity: 1
                    //    //}, 500);
                    //}, 500);

                    vproker.ClientApi.validatePassport(passport, (isValid) => {
                        //clearInterval(intervalId);
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

        private static showClientInfo(info: ClientInfo) {
            const $clientName = $("#clientName");
            const $clientInfo = $("#clientInfo");
            const $passport = $("#passport");

            if (info) {
                console.log(info);
                const regularClientDiv = `<div>Постоянный клиент                
                        <span style="vertical-align: top;" title="Постоянный клиент">
                            <svg width="2em" height="2em" viewBox="0 0 16 16" class="bi bi-person-check-fill" fill="green" xmlns="http://www.w3.org/2000/svg">
                                <path fill-rule="evenodd" d="M1 14s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1H1zm5-6a3 3 0 1 0 0-6 3 3 0 0 0 0 6zm9.854-2.854a.5.5 0 0 1 0 .708l-3 3a.5.5 0 0 1-.708 0l-1.5-1.5a.5.5 0 0 1 .708-.708L12.5 7.793l2.646-2.647a.5.5 0 0 1 .708 0z" />
                            </svg>
                        </span></div>`;
                const statHtml = `<div>
                                    <div>Всего заказов - ${info.allOrdersNumber}</div>
                                    <div>Активных заказов - ${info.activeOrdersNumber}</div>
                                    ${info.isRegular ? regularClientDiv : ''}
                                    <a href="/client/details/${info.client.id}">Подробнее</a></div>
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