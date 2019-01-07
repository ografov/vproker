namespace vproker {
    export class ClientApi {
        static validatePassport(passport: string, success: (isValid) => void) {
            $.ajax({
                url: "/api/client/validatePassport",
                data: { passport: passport },
                success: (data) => {
                    success(data);
                }
            });
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
    }
}