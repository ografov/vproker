/// <reference path="../../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../../node_modules/@types/bootstrap-table/index.d.ts" />
/// <reference path="../../node_modules/@types/eonasdan-bootstrap-datetimepicker/index.d.ts" />

namespace vproker {
    export class History {
        static render() {
            const $table = $('#table'),
                $ok = $('#ok');
            $ok.click(function () {
                $table.bootstrapTable('refresh');
            });

            $('#startPicker').datetimepicker({
                locale: 'ru'
            });
            $('#endPicker').datetimepicker({
                locale: 'ru'
            });

            const $clear = $('#clear');
            $clear.click(() => {
                $('#toolbar').find('input[name]').each(function (i, elem) {
                    $(elem).val('');
                });
                const startPicker = $('#startPicker').data("DateTimePicker");
                if (startPicker) {
                    startPicker.clear();
                }
                const endPicker = $('#endPicker').data("DateTimePicker");
                if (endPicker) {
                    endPicker.clear();
                }

                $table.bootstrapTable('refresh');
            });

            // download
            const $download = $('#download');
            $download.click(() => {
                const params = History.queryParams();
                let downloadUrl = "./DownloadHistory?";
                for (const key in params) {
                    downloadUrl += `${key}=${params[key]}&`; 
                }
                window.open(downloadUrl); 
            });
        }

        static queryParams() {
            const params = {};
            $('#toolbar').find('input[name]').each(function () {
                params[$(this).attr('name')] = $(this).val();
            });

            const startPicker = $('#startPicker').data("DateTimePicker");
            if (startPicker) {
                params['start'] = startPicker.date() != null && startPicker.date().format('MM/DD/YYYY');
            }
            const endPicker = $('#endPicker').data("DateTimePicker");
            if (endPicker) {
                params['end'] = endPicker.date() != null && endPicker.date().format('MM/DD/YYYY');
            }

            console.log('toolbar params: ', params);
            return params;
        }

        static idToActionsFormatter(value) {

            var editBnt = '<a class="edit" href="/Order/Edit/' + value + '" title="Редактировать" style="margin: 3px"><i class="glyphicon glyphicon-edit"></i></a>';
            var detailsBtn = '<a class="info-sign" href="/Order/Details/' + value + '" title="Просмотр" style="margin: 3px"><i class="glyphicon glyphicon-info-sign"></i></a>';
            var deleteBtn = '<a class="edit" href="/Order/Delete/' + value + '" title="Удалить" style="margin: 3px"><i class="glyphicon glyphicon-remove"></i></a>';

            return editBnt + detailsBtn + deleteBtn;
        }

        static dateTimeFormatter(value) {
            var date = new Date(value + 'Z');
            return '<span title="' + date + '">' + date.toLocaleString('ru-RU') + '</span>';
        }
    }
}