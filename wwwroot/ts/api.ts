namespace vproker {
    export interface ClientInfo {
        allOrdersNumber: number,
        activeOrdersNumber: number,
        isRegular: boolean
        client: any
    }

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

        static getInfoByPhone(number: string, success: (info: ClientInfo) => void) {
            $.ajax({
                url: "/api/client/getInfoByPhone",
                data: { number: number },
                success: (data) => {
                    success(data as ClientInfo);
                }
            });
        }

    }

    export class ToolApi {
        static getToolInfo(toolId: string, success: (info) => void) {
            $.ajax({
                url: "/api/ToolApi/" + toolId,
                //data: { id: toolId },
                success: (data) => {
                    success(data);
                }
            });
        }
    }
}