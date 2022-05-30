/// <reference path="../../node_modules/@types/jquery/index.d.ts" />

namespace vproker {
    export class Client {
        static render() {
            const $downloadClientsReport = $('#downloadClients');
            $downloadClientsReport.click(() => {
                let downloadUrl = "./DownloadClients";
                window.open(downloadUrl);
            });
        }
    }
}