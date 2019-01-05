/// <reference path="../../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../../node_modules/@types/bootstrap-table/index.d.ts" />
/// <reference path="../../node_modules/@types/eonasdan-bootstrap-datetimepicker/index.d.ts" />

namespace vproker {
    export class CreateClient {
        static render() {

            const passportElem = $("#passport");
            passportElem.focusout(() => {
                const passport = passportElem.val();
                const alert = $("#passportAlert");
                if (passport.length) {
                    if (CreateClient.isCorrectPassport(passport)) {

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

                        CreateClient.validatePassport(passport, (isValid) => {
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
            });
        }

        static isCorrectPassport(passport: string) {
            return /^\d{10}$/.test(passport);
        }

        static validatePassport(passport: string, success: (isValid) => void) {
            $.ajax({
                url: "/api/client/validatePassport",
                data: { passport: passport },
                success: (data) => {
                    success(data);
                }
            });
        }
    }
}