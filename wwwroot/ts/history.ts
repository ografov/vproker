/// <reference path="../../node_modules/@types/jquery/index.d.ts" />
/// <reference path="../../node_modules/@types/bootstrap-table/index.d.ts" />
/// <reference path="../../node_modules/@types/eonasdan-bootstrap-datetimepicker/index.d.ts" />

namespace vproker {
    export class History {
        static render() {
            var $table = $('#table'),
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
        }

        static queryParams() {
            var params = {};
            $('#toolbar').find('input[name]').each(function () {
                params[$(this).attr('name')] = $(this).val();
            });
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